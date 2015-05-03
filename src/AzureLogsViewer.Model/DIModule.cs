using AzureLogsViewer.Model.Services.SlackIntegration;
using AzureLogsViewer.Model.Services.WorkerTimers;
using Ninject.Modules;

namespace AzureLogsViewer.Model
{
    public class DIModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<TimerBase>().To<DumpWadLogsTimer>();
            Kernel.Bind<TimerBase>().To<CleanupStaleWadLogsTimer>();
            Kernel.Bind<TimerBase>().To<SlackIntegrationTimer>();

            Kernel.Bind<ISlackClient>().To<SlackClient>();
        }
    }
}