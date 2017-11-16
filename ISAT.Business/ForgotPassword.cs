using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Csla;
using Csla.Data;
using Csla.Security;
using Csla.Serialization;
using MySql.Data.MySqlClient;


namespace ISAT.Business
{
    [Serializable]
    public class ForgotPassword : CommandBase<ForgotPassword>
    {
        public static readonly PropertyInfo<string> UserNameProperty = RegisterProperty<string>(c => c.UserName);
        public string UserName
        {
            get { return ReadProperty(UserNameProperty); }
            private set { LoadProperty(UserNameProperty, value); }
        }

        public static readonly PropertyInfo<string> PasswordhashProperty = RegisterProperty<string>(c => c.Passwordhash);
        public string Passwordhash
        {
            get { return ReadProperty(PasswordhashProperty); }
            private set { LoadProperty(PasswordhashProperty, value); }
        }

        public static readonly PropertyInfo<string> SecurityStampProperty = RegisterProperty<string>(c => c.SecurityStamp);
        public string SecurityStamp
        {
            get { return ReadProperty(SecurityStampProperty); }
            private set { LoadProperty(SecurityStampProperty, value); }
        }

        public static void Reset(string userName, string passwordhash, string securityStamp)
        {
            ForgotPassword cmd = new ForgotPassword { UserName = userName, Passwordhash = passwordhash , SecurityStamp = securityStamp };
            cmd = DataPortal.Execute<ForgotPassword>(cmd);
        }

        private void DataPortal_Fetch(int userName)
        {
            bool exists = false;
            using (MySqlConnection cn = new MySqlConnection(Database.ConnectionString))
            {
                MySqlCommand cm = cn.CreateCommand();
                cm.CommandType = System.Data.CommandType.Text;
                cm.CommandText = @"SELECT COUNT(*) FROM aspnetusers WHERE username=@user;";
                cm.Parameters.AddWithValue("@user", UserName);
                exists = (int)cm.ExecuteScalar() > 0;
            }
        }

        protected override void DataPortal_Execute()
        {
            using (MySqlConnection cn = new MySqlConnection(Database.ConnectionString))
            {
                cn.Open();
                MySqlCommand cm = cn.CreateCommand();
                cm = cn.CreateCommand();
                cm.CommandType = System.Data.CommandType.Text;
                cm.CommandText = @"UPDATE aspnetusers SET PasswordHash=@pwd, SecurityStamp=@stamp WHERE username=@user;";
                cm.Parameters.AddWithValue("@pwd", Passwordhash);
                cm.Parameters.AddWithValue("@stamp", SecurityStamp);
                cm.Parameters.AddWithValue("@user", UserName);
                cm.ExecuteNonQuery();

                cn.Close();
            }
        }

        private void DataPortal_Fetch(SafeDataReader dr)
        {
            LoadProperty(UserNameProperty, dr.GetString(0));
        }
    }
}
