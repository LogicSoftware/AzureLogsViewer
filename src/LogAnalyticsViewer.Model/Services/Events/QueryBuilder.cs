using LogAnalyticsViewer.Model.DTO;
using LogAnalyticsViewer.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LogAnalyticsViewer.Model.Services.Events
{
    internal class QueryBuilder
    {
        private List<string> _queries = new List<string>();    
        private List<TimeFilter> _timeFilters = new List<TimeFilter>();
        private List<MessageFilter> _messageFilters = new List<MessageFilter>();
        private string _topFilter = string.Empty;

        public QueryBuilder AddQuery(string query)
        {
            _queries.Add(query);
            return this;
        }

        public QueryBuilder AddQueries(List<string> queries)
        {
            _queries.AddRange(queries);
            return this;
        }

        public QueryBuilder AddDateFilter(DateTime? from, DateTime? to)
        {
            _timeFilters.Add(new TimeFilter(from, TimeFilterType.GreaterOrEqual));
            _timeFilters.Add(new TimeFilter(to, TimeFilterType.LessOrEqual));
          
            return this;
        }

        public QueryBuilder AddTimeFilter(int timeInMinutes)
        {
            _timeFilters.Add(new TimeFilter(timeInMinutes));
            return this;
        }

        public QueryBuilder AddMessageFilter(List<MessageFilter> filters)
        {
            _messageFilters.AddRange(filters);
            return this;
        }

        public QueryBuilder AddTopFilter(int top)
        {
            _topFilter = $@"
| take {top}";
            return this;
        }

        public string Create()
        {          
            var timeFilter = JoinFilters(_timeFilters.Select(f => f?.FilterStr));

            var query = string.Join(@"
| union ", _queries.Select(q => $"({q.Replace("{TimeFilter}", timeFilter)}{_topFilter})"));

            var messagesFilter = JoinFilters(_messageFilters.Select(f => f?.FilterStr));
            var queryWithMessageFilters = $"{query}{messagesFilter}";

            return queryWithMessageFilters;
        }

        private string JoinFilters(IEnumerable<string> filters)
        {
            var nonEmptyFilters = filters.Where(v => !string.IsNullOrEmpty(v));
            var result = string.Join(" and ", nonEmptyFilters);

            return string.IsNullOrEmpty(result) ? string.Empty : @$"
| where {result}";
        }
    }
}
