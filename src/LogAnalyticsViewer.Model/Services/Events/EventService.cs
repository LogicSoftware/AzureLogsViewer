using LogAnalyticsViewer.Model.DTO;
using LogAnalyticsViewer.Model.Entities;
using Microsoft.Azure.OperationalInsights;
using Microsoft.Rest.Azure.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogAnalyticsViewer.Model.Services.Events
{
    public class EventService
    {
        private LAVDataContext _dbContext { get; set; }

        public EventService(LAVDataContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public Task<IEnumerable<Event>> GetEvents(DateTime from, DateTime to, int? queryId = null, List<MessageFilter> filters = null)
        {
            var select = _dbContext.Queries.AsQueryable();
            
            if (queryId != null)
            {
                select = select.Where(q => q.QueryId == queryId);
            }

            var queries = select
                .Select(x => x.QueryText)
                .ToList();

            var queryStr = new QueryBuilder()
                .AddQueries(queries)
                .AddDateFilter(from, to)
                .AddMessageFilter(filters)
                .Create();

            return GetEvents(queryStr);
        }

        private async Task<IEnumerable<Event>> GetEvents(string query)
        {
            var settings = _dbContext.LogAnalyticsSettings.First();

            var credentials = await ApplicationTokenProvider.LoginSilentAsync(
                settings.Domain,
                settings.ClientId,
                settings.ClientSecret,
                Consts.AdSettings
            );

            using var client = new OperationalInsightsDataClient(credentials);
            client.WorkspaceId = settings.WorkspaceId;

            var rows = (await client.QueryAsync(query)).Tables.First().Rows;

            return rows.Select(r => new Event(r));
        }
    }
}
