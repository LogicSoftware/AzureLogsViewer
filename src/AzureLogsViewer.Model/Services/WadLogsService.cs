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

            query = filter.Apply(query);
            
            return query.OrderByDescending(x => x.EventDateTime)
                              .Take(1000)
                              .ToList();
        }
    }
}