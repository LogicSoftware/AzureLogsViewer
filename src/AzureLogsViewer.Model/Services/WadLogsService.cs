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

        public IEnumerable<WadLogEntry> GetEntries()
        {
            return DataContext.WadLogEntries.OrderByDescending(x => x.EventDateTime)
                              .Take(100)
                              .ToList();
        }
    }
}