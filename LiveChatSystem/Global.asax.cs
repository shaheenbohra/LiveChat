using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace LiveChatSystem
{
    public class MvcApplication : System.Web.HttpApplication
    {
        String strConnString = ConfigurationManager.ConnectionStrings["DbLiveChat"].ConnectionString;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //Start SqlDependency with application initialization

            SqlDependency.Start(strConnString);

        }
        protected void Application_End()
        {
            //Stop SQL dependency
            SqlDependency.Stop(strConnString);
        }
    }
}
