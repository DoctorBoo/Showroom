using Repository.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Etherean
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //get from config
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            log4net.Config.XmlConfigurator.Configure();
            Log<MvcApplication>.Write.Info("Entering application.");

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
