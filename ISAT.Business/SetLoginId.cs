using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ISAT.Business
{

    public class SetLoginId
    {

        public static void UpdateUser(string username, string terminalid)
        {
            using (MySqlConnection cn = new MySqlConnection(Database.ConnectionString))
            {
                cn.Open();
                MySqlCommand cm = cn.CreateCommand();
                cm.CommandText = "UPDATE aspnetusers SET TerminalId=@tid WHERE Username=@user";
                cm.Parameters.AddWithValue("@user", username);
                cm.Parameters.AddWithValue("@tid", terminalid);
                cm.ExecuteNonQuery();
                cn.Close();
            }
        }

        public static string GetTerminalId(string username)
        {
            string terminalid = string.Empty;
            using (MySqlConnection cn = new MySqlConnection(Database.ConnectionString))
            {
                cn.Open();
                MySqlCommand cm = cn.CreateCommand();
                cm.CommandText = "SELECT terminalId FROM aspnetusers WHERE Username=@user";
                cm.Parameters.AddWithValue("@user", username);
                MySql.Data.MySqlClient.MySqlDataReader dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    terminalid = dr.GetString(0);
                }
                cn.Close();
            }
            return terminalid;
        }
    }
}
