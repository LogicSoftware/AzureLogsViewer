﻿using System.Web;
using System.Web.Optimization;

namespace AzureLogsViewer.Web
{
    public class 
        BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery")
                        .Include("~/Scripts/jquery-{version}.js")
                        .Include("~/Scripts/jquery.cookie.js")
            );

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/moment.js",
                      "~/Scripts/bootstrap-datetimepicker.js"
                      
                      ));

            bundles.Add(new ScriptBundle("~/bundles/knockout").Include(
                      "~/Scripts/knockout-3.3.0.js"));

            bundles.Add(new ScriptBundle("~/bundles/underscore").Include(
                      "~/Scripts/underscore.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootsrap-datetimepicker.css",
                      "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/appjs").Include(
                      "~/Scripts/app/app.js",
                      "~/Scripts/app/utils/*.js",
                      "~/Scripts/app/ko/*.js").
                      IncludeDirectory("~/Scripts/app/views", "*.js", true)
            );


            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
#if !DEBUG
            BundleTable.EnableOptimizations = true;
#endif
        }
    }
}
