using System;
using System.Collections.Generic;
using AzureLogsViewer.Model.Common;
using AzureLogsViewer.Model.Services.WorkerTimers;
using Ninject;

namespace AzureLogsViewer.Model.Services
{
    public class Worker
    {
        [Inject]
        public IEnumerable<TimerBase> Timers { get; set; }

        public void Start()
        {
            Timers.ForEach(x => x.Start());
        }

        public void Stop()
        {
            TimerBase.StopAll(Timers, TimeSpan.FromSeconds(15));
        }
    }
}