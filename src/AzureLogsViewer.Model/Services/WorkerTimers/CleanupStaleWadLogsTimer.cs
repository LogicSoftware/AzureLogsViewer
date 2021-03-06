using System;
using System.Threading;
using Ninject;

namespace AzureLogsViewer.Model.Services.WorkerTimers
{
    public class CleanupStaleWadLogsTimer : TimerBase
    {
        protected override TimeSpan GetNextDelay(IKernel kernel)
        {
            return kernel.Get<WadLogsDumpService>().GetDelayForNextDump().Add(TimeSpan.FromMinutes(3));
        }

        protected override void Action(IKernel kernel, CancellationToken token)
        {
            kernel.Get<WadLogsDumpService>().CleanupStaleLogs();
        }
    }
}