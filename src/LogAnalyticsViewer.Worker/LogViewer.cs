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
        private readonly TimeSpan _delay;
        private readonly ILogger<LogViewer> _logger;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly bool _singleDump;

        public LogViewer(
            IOptionsMonitor<LogViewerSettings> settings,
            IServiceScopeFactory scopeFactory,
            ILogger<LogViewer> logger,
            IHostApplicationLifetime hostApplicationLifetime
        )
        {
            _scopeFactory = scopeFactory;
            _delay = TimeSpan.FromMinutes(settings.CurrentValue.DumpSizeInMinutes);
            _logger = logger;
            _hostApplicationLifetime = hostApplicationLifetime;
            _singleDump = settings.CurrentValue.SingleDump;
            if (_singleDump)
            {
                _logger.LogInformation("Single dump mode enabled");
            }
        }

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

                if (_singleDump)
                {
                    break;
                }
                await Task.Delay(_delay, stoppingToken);
            }
            _hostApplicationLifetime.StopApplication();
        }

        private async Task DoWork()
        {
            using var scope = _scopeFactory.CreateScope();
            var worker = scope.ServiceProvider.GetRequiredService<LogViewerWorker>();
            await worker.DoWork();
        }
    }

}
