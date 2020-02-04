using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LogAnalyticsViewer.Model;
using LogAnalyticsViewer.Model.DTO;
using LogAnalyticsViewer.Model.Services.Events;
using LogAnalyticsViewer.Worker.SlackIntegration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LogAnalyticsViewer.Worker
{
    public class LogViewer : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly LogViewerSettings _settings;
        private readonly ILogger<LogViewer> _logger;
                
        private Dictionary</*QueryId*/int, List<Event>> _events = new Dictionary<int, List<Event>>();

        public LogViewer(
            IOptionsMonitor<LogViewerSettings> settings,
            IServiceScopeFactory scopeFactory,
            ILogger<LogViewer> logger
        ) => 
            (_scopeFactory, _settings, _logger) =
            (scopeFactory, settings.CurrentValue, logger);

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

                await Task.Delay(_settings.DelayBetweenDumpsInMinutes, stoppingToken);
            }
        }

        private async Task DoWork()
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<LAVDataContext>();
            var eventService = scope.ServiceProvider.GetRequiredService<EventService>();
            var slackService = scope.ServiceProvider.GetRequiredService<SlackIntegrationService>();

            var queries = dbContext.Queries.Where(q => q.Enabled).ToList();

            foreach (var query in queries)
            {
                _events.TryGetValue(query.QueryId, out var prevBatch);

                var newBatch = await eventService.GetEventsForWorker(query.QueryText, _settings.DumpSizeInMinutes);
                var newEvents = newBatch.Except(prevBatch ?? new List<Event>()).ToList();

                slackService.ProcessEvents(newEvents, query.Channel, query.QueryId);

                _events[query.QueryId] = newBatch;
            }
        }
    }
}
