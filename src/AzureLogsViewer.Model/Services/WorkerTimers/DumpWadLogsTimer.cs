using System;
using System.Threading;
using Ninject;

namespace AzureLogsViewer.Model.Services.WorkerTimers
{
    public class DumpWadLogsTimer : TimerBase
    {
        protected override TimeSpan GetNextDelay(IKernel kernel)
        {
            return kernel.Get<WadLogsDumpService>().GetDelayBetweenDumps();
        }

        protected override void Action(IKernel kernel, CancellationToken token)
        {
            kernel.Get<WadLogsDumpService>().Dump();
        }

        protected override void HandleError(Exception exception)
        {
            //TODO : log error
        }
    }
}