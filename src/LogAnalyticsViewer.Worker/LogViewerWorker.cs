using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogAnalyticsViewer.Model;
using LogAnalyticsViewer.Model.Entities;
using LogAnalyticsViewer.Model.Services;
using LogAnalyticsViewer.Model.Services.Events;
using LogAnalyticsViewer.Worker.SlackIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MoreLinq.Extensions;

namespace LogAnalyticsViewer.Worker;

public class LogViewerWorker
{
    private readonly LogViewerSettings _settings;

    private readonly LAVDataContext _dbContext;
    private readonly IEventService _eventService;
    private readonly ISlackIntegrationService _slackService;
    private readonly ILogger<LogViewerWorker> _logger;

    public LogViewerWorker(LAVDataContext dbContext, IEventService eventService, ISlackIntegrationService slackService, IOptionsMonitor<LogViewerSettings> settings, ILogger<LogViewerWorker> logger)
    {
        _dbContext = dbContext;
        _eventService = eventService;
        _slackService = slackService;
        _logger = logger;
        _settings = settings.CurrentValue;
    }
    
    public async Task DoWork()
    {
        var queries = _dbContext.Queries
            .Include(x => x.Events)
            .Where(q => q.Enabled)
            .AsAsyncEnumerable();

        await foreach (var query in queries)
        {
            var newBatch = await _eventService.GetEventsForWorker(query.QueryText, _settings.DumpSizeInMinutes);

            var newEvents = new List<Event>();

            var oldBatch = query.Events.ToList();
            
            newBatch
                .FullJoin(oldBatch,
                    x => x,
                    newEvent =>
                    {
                        newEvent.Query = query;
                        _dbContext.Events.Add(newEvent);
                        newEvents.Add(newEvent);
                        return newEvent;
                    },
                    oldEvent =>
                    {
                        _dbContext.Events.Remove(oldEvent);
                        return oldEvent;
                    },
                    (_, oldEvent) => oldEvent,
                    new EventContentComparer())
                .Consume();

            _logger.LogInformation("Processing {Query}: Got {NewCount} events, have {OldCount} events, report {ReportCount} events", query.DisplayName, newBatch.Count, oldBatch.Count, newEvents.Count);
            
            if (newEvents.Any())
            {
                await _slackService.ProcessEvents(newEvents, query.Channel, query.QueryId);
            }
        }
        await _dbContext.SaveChangesAsync();
    }
}
