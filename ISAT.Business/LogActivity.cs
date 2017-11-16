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
    public class LogActivity :BusinessBase<LogActivity>
    {

        #region Property


        public static readonly PropertyInfo<int> LogIdProperty = RegisterProperty<int>(c => c.LogId);
        public int LogId
        {
            get { return GetProperty(LogIdProperty); }
            set { SetProperty(LogIdProperty, value); }
        }


        public static readonly PropertyInfo<string> ControllerNameProperty = RegisterProperty<string>(c => c.ControllerName);
        public string ControllerName
        {
            get { return GetProperty(ControllerNameProperty); }
            set { SetProperty(ControllerNameProperty, value); }
        }


        public static readonly PropertyInfo<string> ActionNameProperty = RegisterProperty<string>(c => c.ActionName);
        public string ActionName
        {
            get { return GetProperty(ActionNameProperty); }
            set { SetProperty(ActionNameProperty, value); }
        }


        public static readonly PropertyInfo<string> LogDataProperty = RegisterProperty<string>(c => c.LogData);
        public string LogData
        {
            get { return GetProperty(LogDataProperty); }
            set { SetProperty(LogDataProperty, value); }
        }


        public static readonly PropertyInfo<string> CreatedByProperty = RegisterProperty<string>(c => c.CreatedBy);
        public string CreatedBy
        {
            get { return GetProperty(CreatedByProperty); }
            set { SetProperty(CreatedByProperty, value); }
        }


        public static readonly PropertyInfo<DateTime> CreatedDateProperty = RegisterProperty<DateTime>(c => c.CreatedDate);
        public DateTime CreatedDate
        {
            get { return GetProperty(CreatedDateProperty); }
            set { SetProperty(CreatedDateProperty, value); }
        }



        #endregion


        #region Factory Method

        public static LogActivity NewLogActivity()
        {
            return DataPortal.Create<LogActivity>();
        }

        public static LogActivity GetLogActivity(int LogId)
        {
            return DataPortal.Fetch<LogActivity>(LogId);
        }

        public static LogActivity GetLogActivity(SafeDataReader dr)
        {
            return DataPortal.Fetch<LogActivity>(dr);
        }

        public static void DeleteLogActivity(int LogId)
        {
            DataPortal.Delete<LogActivity>(LogId);
        }

        private LogActivity()
        {

        }

        #endregion


        #region Data Access

        private void DataPortal_Fetch(int id)
        {
            using (MySqlConnection cn = new MySqlConnection(Database.ConnectionString))
            {
                cn.Open();
                MySqlCommand cm = cn.CreateCommand();
                cm.CommandText = "SELECT LogId, ControllerName, ActionName, LogData, CreatedBy, CreatedDate FROM logactivity WHERE LogId=@LogId";
                cm.Parameters.AddWithValue("@LogId", id);
                SafeDataReader dr = new SafeDataReader(cm.ExecuteReader());
                while (dr.Read())
                {
                    DataPortal_Fetch(dr);
                }
                cn.Close();
            }
        }

        protected override void DataPortal_Insert()
        {
            using (MySqlConnection cn = new MySqlConnection(Database.ConnectionString))
            {
                cn.Open();
                MySqlCommand cm = cn.CreateCommand();
                cm.CommandText = "INSERT INTO logactivity(ControllerName, ActionName, LogData, CreatedBy, CreatedDate) VALUES (@ControllerName, @ActionName, @LogData, @CreatedBy, @CreatedDate)";
                DoCommand(cm);
                cn.Close();
            }
        }

        protected override void DataPortal_Update()
        {
            using (MySqlConnection cn = new MySqlConnection(Database.ConnectionString))
            {
                cn.Open();
                MySqlCommand cm = cn.CreateCommand();
                cm.CommandText = "UPDATE logactivity SET ControllerName=@ControllerName, ActionName=@ActionName, LogData=@LogData, CreatedBy=@CreatedBy, CreatedDate=@CreatedDate WHERE LogId=@LogId";
                cm.Parameters.AddWithValue("@LogId", LogId);
                DoCommand(cm);
                cn.Close();
            }
        }

        protected override void DataPortal_DeleteSelf()
        {
            DataPortal_Delete(ReadProperty(LogIdProperty));
        }

        private void DataPortal_Delete(int id)
        {
            using (MySqlConnection cn = new MySqlConnection(Database.ConnectionString))
            {
                cn.Open();
                MySqlCommand cm = cn.CreateCommand();
                cm.CommandText = "DELETE FROM logactivity WHERE LogId=@LogId";
                cm.Parameters.AddWithValue("@LogId", LogId);
                cm.ExecuteNonQuery();
                cn.Close();
            }
        }

        private void DataPortal_Fetch(SafeDataReader dr)
        {

            SetProperty(ControllerNameProperty, dr.GetString(0));
            SetProperty(ActionNameProperty, dr.GetString(1));
            SetProperty(LogDataProperty, dr.GetString(2));
            SetProperty(CreatedByProperty, dr.GetString(3));
            SetProperty(CreatedDateProperty, dr.GetDateTime(4));

        }

        private int DoCommand(MySqlCommand cm)
        {
            int rowAffected = 0;

            cm.Parameters.AddWithValue("@ControllerName", ControllerName);
            cm.Parameters.AddWithValue("@ActionName", ActionName);
            cm.Parameters.AddWithValue("@LogData", LogData);
            cm.Parameters.AddWithValue("@CreatedBy", CreatedBy);
            cm.Parameters.AddWithValue("@CreatedDate", CreatedDate);

            rowAffected = cm.ExecuteNonQuery();
            return rowAffected;
        }

        #endregion

    }
}
