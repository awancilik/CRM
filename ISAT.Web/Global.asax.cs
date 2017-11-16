using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ISAT.Web.Hubs;
using APILA.Model;
using APILA.CRM;
using System.Diagnostics;
using System.Web.Helpers;

namespace ISAT.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ISAT.Business.Database.SetConnectionString("Default");

            AntiForgeryConfig.SuppressIdentityHeuristicChecks = true;

        }
    }
}
