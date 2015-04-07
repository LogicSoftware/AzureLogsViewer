using System;
using System.Collections.Generic;
using System.Linq;
using AzureLogsViewer.Model.Entities;
using AzureLogsViewer.Model.Infrastructure;
using Ninject;

namespace AzureLogsViewer.Model.Services
{
    public class WadLogsService
    {
        [Inject]
        public AlwDataContext DataContext { get; set; }

        public IEnumerable<WadLogEntry> GetEntries(DateTime? from, DateTime? to)
        {
            IQueryable<WadLogEntry> query = DataContext.WadLogEntries;
            
            if (from.HasValue)
                query = query.Where(x => x.EventDateTime > from);

            if (to.HasValue)
                query = query.Where(x => x.EventDateTime < to);

            return query.OrderByDescending(x => x.EventDateTime)
                              .Take(100)
                              .ToList();
        }
    }
}