using System.Web.Mvc;
using AzureLogsViewer.Model.DTO;
using AzureLogsViewer.Model.Services.SlackIntegration;
using Ninject;

namespace AzureLogsViewer.Web.Controllers
{
    public class SlackIntegrationController : AlvBaseController
    {
        [Inject]
        public SlackIntegrationService Service { get; set; }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetIntegrations()
        {
            var model = Service.GetIntegrationInfos();
            return Json(model);
        }

        [HttpPost]
        public ActionResult SaveIntegration(SlackIntegrationInfoModel model)
        {
            var result = Service.Save(model);
            return Json(result);
        }
    }
}