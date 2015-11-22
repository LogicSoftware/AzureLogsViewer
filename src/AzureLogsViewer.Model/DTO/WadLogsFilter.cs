using System;
using System.Collections.Generic;
using System.Linq;
using AzureLogsViewer.Model.Entities;

namespace AzureLogsViewer.Model.DTO
{
    public class WadLogsFilter
    {
        public WadLogsFilter()
        {
            MessageFilters = new List<MessageFilter>();
        }

        public int? Id { get; set; }

        public DateTime? From { get; set; }

        public DateTime? To { get; set; }

        public int? Level { get; set; }

        public string Role { get; set; }

        public IEnumerable<MessageFilter> MessageFilters { get; set; }

        public IQueryable<WadLogEntry> Apply(IQueryable<WadLogEntry> query)
        {
            //ignone all other filters if id was specified
            if (Id.HasValue)
            {
                return query.Where(x => x.Id == Id.Value);
            }

            if (From.HasValue)
                query = query.Where(x => x.EventDateTime > From);

            if (To.HasValue)
                query = query.Where(x => x.EventDateTime < To);

            if (Level.HasValue)
                query = query.Where(x => x.Level == Level);

            if (!string.IsNullOrWhiteSpace(Role))
                query = query.Where(x => x.Role == Role);

            foreach (var messageFilter in MessageFilters.Where(x => !string.IsNullOrWhiteSpace(x.Text)))
            {
                if (messageFilter.Type == TextFilterType.Like)
                {
                    query = query.Where(x => x.Message.Contains(messageFilter.Text));
                }
                else
                {
                    query = query.Where(x => !x.Message.Contains(messageFilter.Text));
                }
            }

            return query;
        }
    }
}