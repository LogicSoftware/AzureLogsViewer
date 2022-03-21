using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LogAnalyticsViewer.Worker
{
    public class LogViewer : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly int _delayInMilliseconds;
        private readonly ILogger<LogViewer> _logger;

        public LogViewer(
            IOptionsMonitor<LogViewerSettings> settings,
            IServiceScopeFactory scopeFactory,
            ILogger<LogViewer> logger
        ) => 
            (_scopeFactory, _logger, _delayInMilliseconds) =
            (scopeFactory, logger, settings.CurrentValue.DelayBetweenDumpsInMinutes * 60 * 1000);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await DoWork();
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, "Errors notification fails");
                }

                await Task.Delay(_delayInMilliseconds, stoppingToken);
            }
        }

        private async Task DoWork()
        {
            using var scope = _scopeFactory.CreateScope();
            var worker = scope.ServiceProvider.GetRequiredService<LogViewerWorker>();
            await worker.DoWork();
        }
    }

}
