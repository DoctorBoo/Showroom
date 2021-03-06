﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using yFabric.DataContexts;

namespace yFabric
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static long TicketNr { get; private set; }
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

			StaticCache.LoadStaticCache();
        }

        void Session_Start(object sender, EventArgs e)
        {
            TicketNr++;
        }
        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            
        }
        protected void Application_EndRequest(Object sender, EventArgs e)
        {
            var path = Request.Path;
            if (!HttpContext.Current.Request.IsAuthenticated && !String.IsNullOrWhiteSpace(path) && path.ToLower().EndsWith(".html"))
            {
                HttpContext.Current.Response.ClearContent();
                Response.RedirectToRoutePermanent("Default");
            }
        }
    }
}