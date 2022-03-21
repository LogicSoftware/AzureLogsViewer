using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogAnalyticsViewer.Model;
using LogAnalyticsViewer.Model.DTO;
using LogAnalyticsViewer.Model.Services.Events;
using LogAnalyticsViewer.Worker.SlackIntegration;
using Microsoft.Extensions.Options;

namespace LogAnalyticsViewer.Worker;

class LogViewerWorker
{
    private readonly LogViewerSettings _settings;

    private readonly LAVDataContext _dbContext;
    private readonly EventService _eventService;
    private readonly SlackIntegrationService _slackService;

    private readonly Dictionary</*QueryId*/int, List<Event>> _events = new();

    public LogViewerWorker( LAVDataContext dbContext, EventService eventService, SlackIntegrationService slackService, IOptionsMonitor<LogViewerSettings> settings)
    {
        _dbContext = dbContext;
        _eventService = eventService;
        _slackService = slackService;
        _settings = settings.CurrentValue;

    }
    public async Task DoWork()
    {
        var queries = _dbContext.Queries.Where(q => q.Enabled).ToList();

        foreach (var query in queries)
        {
            _events.TryGetValue(query.QueryId, out var prevBatch);

            var newBatch = await _eventService.GetEventsForWorker(query.QueryText, _settings.DumpSizeInMinutes);
            var newEvents = newBatch.Except(prevBatch ?? new List<Event>()).ToList();

            await _slackService.ProcessEvents(newEvents, query.Channel, query.QueryId);

            _events[query.QueryId] = newBatch;
        }
    }
}
