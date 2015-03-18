using System;
using System.Linq;
using AzureLogsViewer.Model.Common;
using AzureLogsViewer.Model.Infrastructure;
using AzureLogsViewer.Model.WadLogs;

namespace AzureLogsViewer.Model.Services
{
    public class WadLogsService
    {
        internal static DateTime? UtcNowTestsOverride { get; set; }

        public WadLogsService()
        {
            DataContext = new AlwDataContext();
        }

        private IIWadLogsReader _wadLogsReader;

        public IIWadLogsReader WadLogsReader
        {
            get { return _wadLogsReader ?? (_wadLogsReader = new WadLogsReader()); }
            set { _wadLogsReader = value; }
        }

        public AlwDataContext DataContext { get; set; }

        public DateTime UtcNow { get { return UtcNowTestsOverride ?? DateTime.UtcNow; } }

        public void Dump()
        {
            var range = GetDumpRange();
            var entries = WadLogsReader.Read(range.From, range.To);

            DataContext.WadLogEntries.AddRange(entries);
            DataContext.SaveChanges();
        }

        private TimeRange GetDumpRange()
        {
            var settings = DataContext.WadLogsDumpSettings.First();
            var range = new TimeRange();
            
            if (settings.LatestDumpTime == null)
            {
                range.From = UtcNow.AddHours(-1);
            }

            range.To = range.From.AddMinutes(settings.DumpSizeInMinutes);

            return range;
        }
    }
}