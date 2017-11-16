using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace APILA.CRM
{
    public class Notify<T> where T : new()
    {
        //assign connection string and sql command for listening 
        public Notify(string ConnectionString, string Command)
        {
            this.ConnectionString = ConnectionString;
            CollectionReturn = new List<T>();
            this.Command = Command;
            this.NotifyNewItem();
        }

        //event handler to notify the calling class
        public event EventHandler ItemReceived;
        private bool isFirst = true;
        public string ConnectionString { get; set; }
        public string Command { get; set; }
        //rows to return as a collection 
        public List<T> CollectionReturn { get; set; }
        //check if user has permission 
        private bool DoesUserHavePermission()
        {
            try
            {
                SqlClientPermission clientPermission =
                       new SqlClientPermission(PermissionState.Unrestricted);
                clientPermission.Demand();
                return true;
            }
            catch
            {
                return false;
            }
        }
        //initiate notification 
        private void NotifyNewItem()
        {
            if (DoesUserHavePermission())
            {
                if (isFirst)
                {
                    SqlDependency.Stop(ConnectionString);
                    SqlDependency.Start(ConnectionString);
                }
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        using (SqlCommand com = new SqlCommand(Command, conn))
                        {
                            com.Notification = null;
                            SqlDependency dep = new SqlDependency(com);
                            //subscribe to sql dependency event handler
                            dep.OnChange += new OnChangeEventHandler(dep_OnChange);
                            conn.Open();
                            using (var reader = com.ExecuteReader())
                            {
                                //convert reader to list<T> using reflection 
                                while (reader.Read())
                                {
                                    var obj = Activator.CreateInstance<T>();
                                    var properties = obj.GetType().GetProperties();
                                    foreach (var property in properties)
                                    {
                                        if (reader[property.Name] != DBNull.Value)
                                        {
                                            property.SetValue(obj, reader[property.Name], null);
                                        }
                                    }
                                    CollectionReturn.Add(obj);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    //Console.WriteLine(ex.Message);
                }
            }
        }
        //event handler
        private void dep_OnChange(object sender, SqlNotificationEventArgs e)
        {
            isFirst = false;
            var sometype = e.Info;
            //call notify item again 
            NotifyNewItem();
            //if it s an insert notify the calling class 
            if (sometype == SqlNotificationInfo.Insert)
                onItemReceived(e);
            SqlDependency dep = sender as SqlDependency;
            //unsubscribe 
            dep.OnChange -= new OnChangeEventHandler(dep_OnChange);
        }

        private void onItemReceived(SqlNotificationEventArgs eventArgs)
        {
            EventHandler handler = ItemReceived;
            if (handler != null)
                handler(this, eventArgs);
        }
    }
}
