using LogAnalyticsViewer.Model.DTO;
using Microsoft.Azure.OperationalInsights;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Rest.Azure.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogAnalyticsViewer.Model.Services.Events
{
    public class EventService
    {
        private readonly ILogger<EventService> _logger;
        private readonly LogAnalyticsSettings _laSettings;

        public EventService(ILogger<EventService> logger, IOptionsMonitor<LogAnalyticsSettings> laSettings)
        {
            _logger = logger;
            _laSettings = laSettings.CurrentValue;
        }

        public async Task<List<Event>> GetEventsForWorker(string query, int timeInMinutes)
        {
            try
            {
                var queryStr = new QueryBuilder()
                .AddQuery(query)
                .AddTimeFilter(timeInMinutes)
                .Create();
            
                return await GetEvents(queryStr);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fail get logs for query {query}", ex);
                return new List<Event>();
            }
        }

        public Task<List<Event>> GetEventsForClient(List<string> queries, DateTime? from, DateTime? to, List<MessageFilter> filters = null)
        {
            var queryStr = new QueryBuilder()
                .AddQueries(queries)
                .AddDateFilter(from, to)
                .AddMessageFilter(filters)
                .AddTopFilter(100)
                .Create();

            return GetEvents(queryStr);
        }

        private async Task<List<Event>> GetEvents(string query)
        {
            var credentials = await ApplicationTokenProvider.LoginSilentAsync(
                _laSettings.Domain,
                _laSettings.ClientId,
                _laSettings.ClientSecret,
                _laSettings.AdSettings
            );

            using var client = new OperationalInsightsDataClient(credentials);
            client.WorkspaceId = _laSettings.WorkspaceId;

            var rows = (await client.QueryAsync(query)).Tables.First().Rows;

            return rows.Select(r => new Event(r)).ToList();
        }
    }
}
