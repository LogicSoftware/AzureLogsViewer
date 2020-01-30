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
using Microsoft.Extensions.Options;

namespace LogAnalyticsViewer.Worker
{
    public class LogViewer : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly LogViewerSettings _settings;
                
        private Dictionary</*QueryId*/int, List<Event>> _events = new Dictionary<int, List<Event>>();

        public LogViewer(
            IOptionsMonitor<LogViewerSettings> settings,
            IServiceScopeFactory scopeFactory) => 
            (_scopeFactory, _settings) =
            (scopeFactory, settings.CurrentValue);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
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

                    slackService.ProcessEvents(newEvents, query.Channel);

                    _events[query.QueryId] = newBatch;
                }

                await Task.Delay(_settings.DelayBetweenDumpsInMinutes, stoppingToken);
            }
        }
    }
}
