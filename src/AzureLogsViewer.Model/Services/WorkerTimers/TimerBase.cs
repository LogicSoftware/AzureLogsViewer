using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AzureLogsViewer.Model.Infrastructure;
using Ninject;

namespace AzureLogsViewer.Model.Services.WorkerTimers
{
    public abstract class TimerBase : IDisposable
    {
        private bool _isDisposed = false;
        private Task _infiniteTask;
        private CancellationTokenSource _cancellationTokenSource;

        private bool IsWorking
        {
            get { return _infiniteTask != null; }
        }

        [Inject]
        public IKernel Kernel { get; set; }

        private TimeSpan GetNextDelay()
        {
            try
            {
                using (var childKernel = WorkerKernelBuilder.Create(Kernel))
                {
                    return GetNextDelay(childKernel);
                }
            }
            catch (Exception ex)
            {
                //todo: log ex
                return TimeSpan.FromMinutes(1); //run every minute if something going wrong..
            }
        }

        protected abstract TimeSpan GetNextDelay(IKernel kernel);

        protected abstract void Action(IKernel kernel, CancellationToken token);

        protected abstract void HandleError(Exception exception);

        private void DoWork(CancellationToken token)
        {
            try
            {
                using (var childKernel = WorkerKernelBuilder.Create(Kernel))
                {
                    Action(childKernel, token);
                }
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private async Task CreateLoopTask(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                DoWork(token);
                await Task.Delay(GetNextDelay(), token);
            }
        }

        public void Start()
        {
            if (IsWorking || _isDisposed)
                return;

            _cancellationTokenSource = new CancellationTokenSource();
            var token = _cancellationTokenSource.Token;

            //http://blogs.msdn.com/b/pfxteam/archive/2011/10/24/10229468.aspx
            // differents between Task.Factory.StartNew & Task.Run
            // it's very important to use here Task.Run (because this method automatically wrap <Task<Task<T>> into promise task)
            //
            // Task.Wait(_infiniteTask) won't work correctly(_infiniteTasks status will be RanToCompletion always) 
            //                                          if we use Task.Factory.StartNew 
            _infiniteTask = Task.Run(async () =>
            {
                try
                {
                    await CreateLoopTask(token);
                }
                catch (OperationCanceledException)
                {
                    //do nothing because otherwise we should wrap all Task.Wait with try catch AggregatedException
                }
            });
        }

        public void Stop()
        {
            StopInternal(TimeSpan.Zero);
        }

        public void Stop(TimeSpan timeout)
        {
            StopInternal(timeout);
        }

        public static int StopAll(IEnumerable<TimerBase> timers, TimeSpan timeout)
        {
            var workingTimers = timers.Where(x => x != null && x.IsWorking).ToList();

            foreach (var timerBase in workingTimers)
                timerBase.Cancel();

            Task.WaitAll(workingTimers.Select(x => x._infiniteTask).ToArray(), timeout);
            var stoppedCount = workingTimers.Count(x => x._infiniteTask.Status == TaskStatus.RanToCompletion);

            foreach (var timerBase in workingTimers)
                timerBase.Reset();

            return stoppedCount;
        }

        public static CancellationTokenSource CreateLinkedCancellationTokenSource(params TimerBase[] timers)
        {
            var workingTimers = timers.Where(x => x.IsWorking);
            return CancellationTokenSource.CreateLinkedTokenSource(workingTimers.Select(x => x._cancellationTokenSource.Token).ToArray());
        }

        private void StopInternal(TimeSpan timeout)
        {
            if (!IsWorking)
                return;

            Cancel();

            if (timeout != TimeSpan.Zero)
                _infiniteTask.Wait(timeout);

            Reset();
        }

        private void Cancel()
        {
            if (!_cancellationTokenSource.Token.IsCancellationRequested)
                _cancellationTokenSource.Cancel();
        }

        private void Reset()
        {
            // we expect here only RanToCompletion or WaitingForActivation (initial state of promise task which is created by Task.Run )
            // but add here all allowed statuses for task disposing
            if (_infiniteTask.Status == TaskStatus.Canceled ||
                _infiniteTask.Status == TaskStatus.RanToCompletion ||
                _infiniteTask.Status == TaskStatus.Faulted)
            {
                _cancellationTokenSource.Dispose();
                _infiniteTask.Dispose();
            }

            _cancellationTokenSource = null;
            _infiniteTask = null;
        }

        public void Dispose()
        {
            if (_isDisposed)
                return;

            Dispose(true);
            _isDisposed = true;
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                if (IsWorking)
                    Stop();
            }
        }
    }
}