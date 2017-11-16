using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APILA.CRM
{
    public class Database
    {
        public static string GetConnection
        {
            get
            {
                return ConfigurationManager.AppSettings["Data"];
            }
        }
    }
}
