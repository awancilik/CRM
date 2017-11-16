using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using APILA.CRM;
using APILA.Model;
using Microsoft.AspNet.SignalR.Hubs;
using ISAT.Web.Models;

namespace ISAT.Web.Hubs
{
    public class NotificationHub : Hub
    {
        public void SendNotif(string userId, string message, string _event)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            if (userId != null)
            {
                if (_event == "callRinging")
                {

                    context.Clients.User(userId).sendNotif(message);
                }
                //InsertCall.Delete();
            }
        }
    }
}