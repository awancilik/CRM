using APILA.Model;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace APILA.CRM
{
    public class Authentication
    {
        public LoginCRMResponse Login(UserUC user)
        {
            var result = string.Empty;
            var client = new RestClient("https://myistraplus.isatmobex.indosat.com");
            var request = new RestRequest("/restletrouter/v1/service/Login", Method.POST);

            var auth = "Basic " + Base64Encode("ENDUSER:" + user.UserName.Trim() + ":" + user.Password.Trim());

            request.AddHeader("X-Application", "myCrm");
            request.AddHeader("Authorization", auth);
            request.AddHeader("Content-Type", "application/json");

            IRestResponse response = client.Execute(request);

            var obj = new LoginCRMResponse();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                obj.XApplication = response.Headers[1].Value.ToString();
                obj.SetCookie = response.Headers[11].Value.ToString().Split(';')[0];
            }

            return obj;
        }

        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }    
    }
}
