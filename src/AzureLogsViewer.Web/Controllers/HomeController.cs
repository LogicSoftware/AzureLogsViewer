using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AzureLogsViewer.Model.Services;
using Ninject;

namespace AzureLogsViewer.Web.Controllers
{
    public class HomeController : Controller
    {
        [Inject]
        public WadLogsService WadLogsService { get; set; }

        public ActionResult Index()
        {
            var model = WadLogsService.GetEntries();
            return View(model);
        }
    }
}