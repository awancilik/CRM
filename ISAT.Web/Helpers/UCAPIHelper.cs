using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Mvc;
using ISAT.Web.Hubs;

namespace ISAT.Web.Helpers
{
    public class UCAPIHelper : Controller
    {
        public void callHubNotif(string number, string _event)
        {
            var objHub = new NotificationHub();
            //var user = HttpContext.User.Identity.Name;
            objHub.SendNotif("PTAplikasiLintasarta_2010", number, _event);
        }
    }
}