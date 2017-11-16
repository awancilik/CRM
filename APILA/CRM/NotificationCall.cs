using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APILA.CRM
{
    public class NotificationCall : Notify<CRMCall>
    {
        public NotificationCall(string ConnectionString, string Command) : base(ConnectionString, Command)
        {
           
        }
    }
}
