using AzureLogsViewer.Model.Infrastructure;
using Ninject.Modules;
using Ninject.Web.Common;

namespace AzureLogsViewer.Web.Code
{
    public class WebDIModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<AlwDataContext>().To<AlwDataContext>().InRequestScope();
        }
    }
}