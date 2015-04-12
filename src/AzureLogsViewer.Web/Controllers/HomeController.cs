using System;
using System.Text;
using System.Web.Mvc;
using AzureLogsViewer.Model.DTO;
using AzureLogsViewer.Model.Services;
using AzureLogsViewer.Web.Code.Json;
using Ninject;

namespace AzureLogsViewer.Web.Controllers
{
    public class HomeController : Controller
    {
        [Inject]
        public WadLogsService WadLogsService { get; set; }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetLogs(WadLogsFilter filter)
        {
            var model = WadLogsService.GetEntries(filter);
            return Json(model);
        }

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