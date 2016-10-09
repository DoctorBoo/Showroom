using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Optimization;

namespace yFabric
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
			bundles.Add(new ScriptBundle("~/bundles/jqueryold").Include(
					"~/Scripts/jquery.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js",
				 //"~/Scripts/jquery.min.js",
                "~/Scripts/jquery.signalR-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/signalr").Include(
                "~/Scripts/jquery.signalR-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.unobtrusive*",
                "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/knockout").Include(
                "~/Scripts/knockout-{version}.js",
                "~/Scripts/knockout.validation.js"));

            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                "~/Scripts/sammy-{version}.js",
                "~/Scripts/app/common.js",
                "~/Scripts/app/app.datamodel.js",
                "~/Scripts/app/app.viewmodel.js",
                "~/Scripts/app/home.viewmodel.js",
                "~/Scripts/app/_run.js"));

            bundles.Add(new ScriptBundle("~/bundles/chat").Include("~/Scripts/_communicator.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js"));
			
			//Telerik
			bundles.Add(new ScriptBundle("~/bundles/kendo").Include(
						"~/Scripts/kendo/2014.3.1411/kendo.all.min.js",
						"~/Scripts/kendo/2014.3.1411/kendo.aspnetmvc.min.js",
						"~/Scripts/kendo/2014.3.1411/kendo.timezones.min.js"));

			bundles.Add(new ScriptBundle("~/bundles/demo").Include(
						"~/Scripts/console.js",
						"~/Scripts/prettify.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                 "~/Content/bootstrap.css",
				 //"~/Content/web/kendo.bootstrap.min.css",
                 "~/Content/Site.css"));
			
			//Telerik
			bundles.Add(new StyleBundle("~/Content/kendo/css").Include(
						"~/Content/web/kendo.common.min.css",
						"~/Content/web/kendo.rtl.min.css",
						"~/Content/web/kendo.default.min.css",
						"~/Content/web/kendo.default.mobile.min.css",
						"~/Content/dataviz/kendo.dataviz.min.css",
						"~/Content/dataviz/kendo.dataviz.default.min.css",
						"~/Content/mobile/kendo.mobile.all.min.css"));
			bundles.Add(new StyleBundle("~/Content/shared/css").Include(
						"~/Content/shared/examples-offline.css"));

			bundles.IgnoreList.Clear();
        }
    }
}
