using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AzureLogsViewer.Web.Code;
using AzureLogsViewer.Web.Code.Json;

namespace AzureLogsViewer.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // json handling
            ValueProviderFactories.Factories.Remove(ValueProviderFactories.Factories.OfType<JsonValueProviderFactory>().Single());
            ValueProviderFactories.Factories.Add(new JsonNetValueProviderFactory());

            ModelBinders.Binders.DefaultBinder = new AlvDefaultModelBinder();
        }
    }
}
