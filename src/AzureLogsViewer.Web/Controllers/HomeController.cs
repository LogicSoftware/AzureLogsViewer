using System;
using System.Web.Mvc;
using AzureLogsViewer.Model.DTO;
using AzureLogsViewer.Model.Services;
using AzureLogsViewer.Web.Models;
using Ninject;

namespace AzureLogsViewer.Web.Controllers
{
    public class HomeController : AlvBaseController
    {
        [Inject]
        public WadLogsService WadLogsService { get; set; }

        public ActionResult Index(int? id)
        {
            var model = new LogsPageModel
            {
                Id = id
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult GetLogs(WadLogsFilter filter)
        {
            var model = WadLogsService.GetEntries(filter);
            return Json(model);
        }
    }
}