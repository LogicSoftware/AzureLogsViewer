using System;
using System.Threading;
using Ninject;

namespace AzureLogsViewer.Model.Services.WorkerTimers
{
    public class CleanupStaleWadLogsTimer : TimerBase
    {
        protected override TimeSpan GetNextDelay(IKernel kernel)
        {
            //TODO: add own setting
            return kernel.Get<WadLogsDumpService>().GetDelayBetweenDumps().Add(TimeSpan.FromMinutes(3));
        }

        protected override void Action(IKernel kernel, CancellationToken token)
        {
            kernel.Get<WadLogsDumpService>().CleanupStaleLogs();
        }

        protected override void HandleError(Exception exception)
        {
            //TODO : log error
        }
    }
}