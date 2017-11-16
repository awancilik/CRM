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
    public class UserInfo : ReadOnlyBase<UserInfo>
    {
        public static readonly PropertyInfo<string> UserNameProperty = RegisterProperty<string>(c => c.UserName);
        public string UserName
        {
            get { return GetProperty(UserNameProperty); }
            private set { LoadProperty(UserNameProperty, value); }
        }

        public static UserInfo GetUserInfo(string UserName)
        {
            UserInfo info = DataPortal.Fetch<UserInfo>(UserName);
            if (info.UserName == string.Empty)
            {
                return null;
            }
            return info;
        }

        public static bool isUsernameExist(string userName)
        {
            UserInfo info = DataPortal.Fetch<UserInfo>(userName);
            bool result = true;
            if (info.UserName == string.Empty)
            {
                result = false;
                return result;
            }
            return result;
        }

        private void DataPortal_Fetch(string username)
        {
            using (DatabaseManager ctx = new DatabaseManager())
            {
                MySqlCommand cm = ctx.CreateSelectCommand();
                cm.CommandText = "SELECT UserName FROM aspnetusers WHERE username=@user;";
                cm.Parameters.AddWithValue("@user", username);
                SafeDataReader dr = ctx.Read(cm);
                while (dr.Read())
                {
                    DataPortal_Fetch(dr);
                }
            }
        }

        private void DataPortal_Fetch(SafeDataReader dr)
        {
            LoadProperty(UserNameProperty, dr.GetString(0));
        }
    }
}
