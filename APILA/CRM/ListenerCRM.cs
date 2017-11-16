using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace APILA.CRM
{
    public class ListenerCRM
    {
        public string CreateListener(string xapplication, string listenerName)
        {
            var content = string.Empty;
            var client = new RestClient("https://myistraplus.isatmobex.indosat.com");
            var request = new RestRequest("/restletrouter/v1/service/EventListener/bean", Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("x-application", xapplication);
            request.AddParameter("application/json", "{\n\t\"name\":\""+ listenerName + "\"\n}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                content = response.Content;
            }

            return content;
        }
    }
}
