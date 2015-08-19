using System.Text;
using System.Web.Mvc;
using AzureLogsViewer.Web.Code.Json;

namespace AzureLogsViewer.Web.Controllers
{
    public abstract class AlvBaseController : Controller
    {
        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonNetResult()
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior
            };
        }
    }
}