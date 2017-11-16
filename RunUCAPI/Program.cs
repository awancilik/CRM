using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APILA.General;
using APILA.CRM;
using APILA.Model;
using System.Threading;
using System.Net;

namespace RunUCAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            var username = args[0];//"PTAplikasiLintasarta_2010";
            var pwd = args[1]; //"4361919631";
            var usercrm = args[2]; //"agen10";


            //var username = "PTAplikasiLintasarta_2010";
            //var pwd = "4361919631";
            //var usercrm = "agen10";

            var obj = GetLogin(username, pwd);

            Console.WriteLine("RunUCAPI...");

            Console.WriteLine("X-Application = {0}", obj.XApplication);
            Console.WriteLine();
            Console.WriteLine("Cookie = {0}", obj.SetCookie);
            Console.WriteLine();

            var terminalListener = ConnectWebSocket.Connect("wss://myistraplus.isatmobex.indosat.com/restletrouter/ws-service/myCrm", obj.SetCookie.Split(';')[0], obj.XApplication, "terminalListener").Result;

            var hasil = ConnectWebSocket.GetTerminals(obj.XApplication, terminalListener.name);

            Console.WriteLine();

            Console.WriteLine("TerminalId = {0}", hasil[2].terminalId);
            ISAT.Business.SetLoginId.UpdateUser(username, hasil[2].terminalId);
            Console.WriteLine();

            var callListener = ConnectWebSocket.ConnectCall("wss://myistraplus.isatmobex.indosat.com/restletrouter/ws-service/myCrm",
                                obj.SetCookie.Split(';')[0], obj.XApplication, "callListener", hasil[2].terminalId, usercrm).Result;

            Console.ReadLine();
        }

        public static LoginCRMResponse GetLogin(string userName, string password)
        {
            var auth = new Authentication();
            var obj = new UserUC();
            obj.UserName = userName;
            obj.Password = password;
            var log = auth.Login(obj);
            return log;
        }
    }
}
