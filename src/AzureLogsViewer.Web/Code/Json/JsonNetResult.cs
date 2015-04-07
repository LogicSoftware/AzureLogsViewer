using System;
using System.Web;
using System.Web.Mvc;

namespace AzureLogsViewer.Web.Code.Json
{
    /// <summary>
    /// Json.Net result
    /// </summary>
    public class JsonNetResult : JsonResult
    {
        public bool EncodeAsHtml { get; set; }

        public bool UseCamelCaseJsonConvention { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            // todo: add JsonRequestBehavior support, as for now it completely ignores AllowGet/DenyGet logic

            var response = context.HttpContext.Response;

            response.ContentType = !String.IsNullOrEmpty(this.ContentType) ? this.ContentType : "application/json";

            if (this.ContentEncoding != null)
            {
                response.ContentEncoding = this.ContentEncoding;
            }

            if (this.Data != null)
            {
                var serializedJson = JsonConversion.Serialize(this.Data);

                if (this.EncodeAsHtml)
                {
                    serializedJson = HttpUtility.HtmlEncode(serializedJson);
                }

                response.Write(serializedJson);
            }
        }
    }
}