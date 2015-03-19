using System;
using System.Collections.Generic;
using System.Linq;
using AzureLogsViewer.Model.Common;
using AzureLogsViewer.Model.Entities;
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

        //TODO : di
        public AlwDataContext DataContext { get; set; }

        public DateTime UtcNow { get { return UtcNowTestsOverride ?? DateTime.UtcNow; } }

        public void Dump()
        {
            var range = GetDumpRange();
            var entries = WadLogsReader.Read(range.From, range.To);
            var existingEntriesKeys = GetExistingEntriesKeys(range);

            var newEntries = entries.Where(x => !existingEntriesKeys.Contains(x.GetKey()))
                                    .ToList();

            GetDumpSettings().LatestDumpTime = DateTimeHelper.Min(range.To, UtcNow);

            DataContext.WadLogEntries.AddRange(newEntries);
            DataContext.SaveChanges();
        }

        private HashSet<WadLogEntryKey> GetExistingEntriesKeys(DateTimeRange range)
        {
            var entryKeys =
                DataContext.WadLogEntries.Where(x => x.EventDateTime >= range.From && x.EventDateTime <= range.To)
                           .Select(x => new WadLogEntryKey
                           {
                               PartitionKey = x.PartitionKey,
                               RowKey = x.RowKey
                           }).ToList();

            return new HashSet<WadLogEntryKey>(entryKeys);
        }

        private DateTimeRange GetDumpRange()
        {
            var settings = GetDumpSettings();
            var range = new DateTimeRange();

            range.From = settings.LatestDumpTime ?? UtcNow.AddHours(-1);
            range.To = range.From.AddMinutes(settings.DumpSizeInMinutes);

            range.From = range.From.AddMinutes(-settings.DumpOverlapInMinutes);

            return range;
        }

        private WadLogsDumpSettings GetDumpSettings()
        {
            return DataContext.WadLogsDumpSettings.First();
        }
    }
}