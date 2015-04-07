using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;

namespace AzureLogsViewer.Web.Code.Json
{
    public sealed class JsonNetValueProviderFactory : ValueProviderFactory
    {
        /// <summary>
        /// Returns a value-provider object for the specified controller context.
        /// </summary>
        /// <param name="controllerContext">An object that encapsulates information about the current HTTP request.</param>
        /// <returns>A value-provider object.</returns>
        public override IValueProvider GetValueProvider(ControllerContext controllerContext)
        {
            if (!controllerContext.HttpContext.Request.ContentType.StartsWith("application/json", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            var jsonContent = new StreamReader(controllerContext.HttpContext.Request.InputStream).ReadToEnd();
            if (String.IsNullOrWhiteSpace(jsonContent) || !controllerContext.HttpContext.Response.IsClientConnected)
            {
                return null;
            }

            var jsonRequestParams = JsonConversion.Deserialize<Dictionary<string, JToken>>(jsonContent);

            return new DictionaryValueProvider<JToken>(jsonRequestParams, CultureInfo.CurrentCulture);
        }
    }
}