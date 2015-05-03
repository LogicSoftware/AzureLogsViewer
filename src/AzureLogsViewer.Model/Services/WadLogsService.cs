using System;
using System.Collections.Generic;
using System.Linq;
using AzureLogsViewer.Model.DTO;
using AzureLogsViewer.Model.Entities;
using AzureLogsViewer.Model.Infrastructure;
using Ninject;

namespace AzureLogsViewer.Model.Services
{
    public class WadLogsService
    {
        [Inject]
        public AlvDataContext DataContext { get; set; }

        public IEnumerable<WadLogEntry> GetEntries(WadLogsFilter filter)
        {
            IQueryable<WadLogEntry> query = DataContext.WadLogEntries;
            
            if (filter.From.HasValue)
                query = query.Where(x => x.EventDateTime > filter.From);

            if (filter.To.HasValue)
                query = query.Where(x => x.EventDateTime < filter.To);

            if (filter.Level.HasValue)
                query = query.Where(x => x.Level == filter.Level);

            if (!string.IsNullOrWhiteSpace(filter.Role))
                query = query.Where(x => x.Role == filter.Role);

            foreach (var messageFilter in filter.MessageFilters.Where(x => !string.IsNullOrWhiteSpace(x.Text)))
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

            return query.OrderByDescending(x => x.EventDateTime)
                              .Take(1000)
                              .ToList();
        }
    }
}