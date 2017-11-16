using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.WebSockets;
using System.Threading;
using RestSharp;
using System.Net;
using Newtonsoft.Json;
using APILA.Model;
using Newtonsoft.Json.Linq;

namespace APILA.CRM
{
    public class ConnectWebSocket
    {
        private static object consoleLock = new object();
        private const int sendChunkSize = 256;
        private const int receiveChunkSize = 1024;
        private const bool verbose = true;
        private static readonly TimeSpan delay = TimeSpan.FromMilliseconds(1000);
        private static RestClient client = new RestClient("https://myistraplus.isatmobex.indosat.com");

        public static async Task<ListenerContent> Connect(string uri, string valcookie, string xapp, string listenerName)
        {
            ClientWebSocket webSocket = null;
            var objListener = new ListenerContent();
            try
            {
                webSocket = new ClientWebSocket();
                webSocket.Options.SetRequestHeader("Cookie", valcookie);
                await webSocket.ConnectAsync(new Uri(uri), CancellationToken.None);
                var terminalListener = CreateListener(xapp, listenerName);
                objListener = JsonConvert.DeserializeObject<ListenerContent>(terminalListener);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: {0}", ex);
            }
            finally
            {
                if (webSocket != null)
                    webSocket.Dispose();

                lock (consoleLock)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("WebSocket Get Terminal closed.");
                    Console.ResetColor();
                }
            }
            return objListener;
        }

        public static async Task<ListenerContent> ConnectCall(string uri, string valcookie, string xapp, string listenerName, string terminalId, string userCrm)
        {
            ClientWebSocket webSocket = null;
            var objListener = new ListenerContent();
            try
            {
                webSocket = new ClientWebSocket();
                webSocket.Options.SetRequestHeader("Cookie", valcookie);
                await webSocket.ConnectAsync(new Uri(uri), CancellationToken.None);
                var terminalListener = CreateListener(xapp, listenerName);
                RetrieveCurrentCalls(xapp, listenerName, terminalId);
                objListener = JsonConvert.DeserializeObject<ListenerContent>(terminalListener);
                await Task.WhenAll(Receive(webSocket, terminalId, userCrm), Send(webSocket));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in receive - {0}", ex.Message);
            }
            finally
            {
                if (webSocket != null)
                    webSocket.Dispose();

                lock (consoleLock)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("WebSocket closed.");
                    Console.ResetColor();
                }
            }
            return objListener;
        }

        private static async Task Send(ClientWebSocket webSocket)
        {
            var random = new Random();
            var ping = "ping";
            byte[] buffer = Encoding.UTF8.GetBytes(ping);

            while (webSocket.State == WebSocketState.Open)
            {
                //random.NextBytes(buffer);
                await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Binary, false, CancellationToken.None);
                LogStatus(false, buffer, buffer.Length, "", "");
                await Task.Delay(delay);
            }
        }

        private static async Task Receive(ClientWebSocket webSocket, string terminalId, string userCrm)
        {
            byte[] buffer = new byte[receiveChunkSize];
            CancellationTokenSource Cts = new CancellationTokenSource();
            CancellationToken Ct = Cts.Token;

            try
            {
                while (webSocket.State == WebSocketState.Open)
                {
                    var ts = new TimeSpan(12, 0, 0);
                    Cts.CancelAfter(ts);
                    var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), Ct);
                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                    }
                    else
                    {
                        LogStatus(true, buffer, result.Count, terminalId, userCrm);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                //This is never thrown.
                Console.WriteLine("Operation cancelled.");
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in receive - {0}", ex.Message);
            }
        }

        private static void LogStatus(bool receiving, byte[] buffer, int length, string terminalId, string userCrm)
        {
            lock (consoleLock)
            {
                //Console.ForegroundColor = receiving ? ConsoleColor.Green : ConsoleColor.Gray;
                //Console.WriteLine("{0} {1} bytes... ", receiving ? "Received" : "Sent", length);

                if (verbose)
                {
                    string result = Encoding.UTF8.GetString(buffer, 0, length);
                    if (result != "ping" && result.Contains("listenerName"))
                    {
                        try
                        {
                            Console.WriteLine(terminalId);
                            Console.WriteLine();
                            Console.WriteLine(result);
                            JObject o = JObject.Parse(result);
                            string _event = (string)o.SelectToken("item[0].event");
                            string callId = (string)o.SelectToken("item[0].callId");
                            string incoming = (string)o.SelectToken("item[0].incoming");
                            string type = (string)o.SelectToken("item[0].type");
                            string acdId = (string)o.SelectToken("item[0].acdId");
                            string displayNumber = (string)o.SelectToken("item[0].displayNumber");
                            string displayLabel = (string)o.SelectToken("item[0].displayLabel");
                            string displayNumberE164 = (string)o.SelectToken("item[0].displayNumberE164");

                            var objCall = new CRMCall();
                            objCall._event = _event;
                            objCall.callId = callId;
                            objCall.incoming = incoming;
                            objCall.type = type;
                            objCall.acdId = acdId;
                            objCall.displayNumber = displayNumber;
                            objCall.displayLabel = displayLabel;
                            objCall.displayNumberE164 = displayNumberE164;
                            objCall.terminalId = terminalId;
                            objCall.userCrm = userCrm;

                            if (_event == "callRinging")
                            {
                                InsertCall.Insert(objCall);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                    else
                    {
                        Console.WriteLine(result);
                    }
                }
                //Console.ResetColor();
            }
        }

        public static string CreateListener(string xapplication, string listenerName)
        {
            var content = string.Empty;
            var request = new RestRequest("/restletrouter/v1/service/EventListener/bean", Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("x-application", xapplication);
            request.AddParameter("application/json", "{\n\t\"name\":\"" + listenerName + "\"\n}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                content = response.Content;
            }

            return content;
        }

        public static List<TerminalContent> GetTerminals(string xapp, string listenerName)
        {
            var request = new RestRequest("/restletrouter/v1/crm/GetTerminals", Method.GET);
            var objTerminal = new List<TerminalContent>();
            request.AddParameter("listenerName", listenerName, ParameterType.UrlSegment);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("x-application", xapp);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = response.Content;
                objTerminal = JsonConvert.DeserializeObject<List<TerminalContent>>(result);
            }

            return objTerminal;
        }

        public static string RetrieveCurrentCalls(string xapp, string listenerName, string terminalId)
        {
            var url = string.Format("/restletrouter/v1/crm/CallLine?listenerName={0}&terminalId={1}",
                                listenerName, terminalId);
            var request = new RestRequest(url, Method.GET);
            var objTerminal = new List<Call>();
            var result = string.Empty;
            request.AddHeader("content-type", "application/json");
            request.AddHeader("x-application", xapp);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                Console.WriteLine("RetrieveCurrentCalls OK");
                result = response.Content;
                objTerminal = JsonConvert.DeserializeObject<List<Call>>(result);
            }

            return result;
        }

        public static string PlaceCall(string xapp, string terminalId, string destination)
        {
            var url = string.Format("/restletrouter/v1/crm/PlaceCall?terminalId={0}&destination={1}",
                                terminalId, destination);
            var request = new RestRequest(url, Method.GET);
            var objTerminal = new List<Call>();
            var result = string.Empty;
            request.AddHeader("content-type", "application/json");
            request.AddHeader("x-application", xapp);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                result = response.Content;
                objTerminal = JsonConvert.DeserializeObject<List<Call>>(result);
            }

            return result;
        }
    }
}
