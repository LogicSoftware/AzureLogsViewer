using System;
using System.Threading;
using AzureLogsViewer.Model.Services.SlackIntegration;
using Ninject;

namespace AzureLogsViewer.Model.Services.WorkerTimers
{
    public class SlackIntegrationTimer : TimerBase
    {
        protected override TimeSpan GetNextDelay(IKernel kernel)
        {
            return TimeSpan.FromMinutes(1);
        }

        protected override void Action(IKernel kernel, CancellationToken token)
        {
            kernel.Get<SlackIntegrationService>().SendMessages();
        }
    }
}