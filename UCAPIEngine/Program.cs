using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using APILA.Model;
using APILA.CRM;
using System.Diagnostics;
using System.Configuration;
using System.Threading;

namespace UCAPIEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            var username = args[0];
            var pwd = args[1];
            var usercrm = args[2];

            //var username = "PTAplikasiLintasarta_2011";
            //var pwd = "9835973208";
            //var usercrm = "agen11";

            Console.WriteLine("UCAPIEngine Running...");
            //var obj = GetLogin(username, pwd);
            var proc = new Process();
            proc.StartInfo.FileName = getPathExe();
            proc.StartInfo.Arguments = username + (char)32 + pwd + (char)32 + usercrm;
            //proc.StartInfo.CreateNoWindow = false;
            //proc.StartInfo.UseShellExecute = false;
            proc.Start();
            //ThreadStart ths = new ThreadStart(() => proc.Start());
            //Thread th = new Thread(ths);
            //th.Start();
            proc.Close();

            Environment.Exit(0);
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

        private static string getPathExe()
        {
            return Convert.ToString(ConfigurationManager.AppSettings["pathExe"]);
        }
    }
}
