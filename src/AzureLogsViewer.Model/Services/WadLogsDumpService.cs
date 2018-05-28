using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using AzureLogsViewer.Model.Common;
using AzureLogsViewer.Model.Entities;
using AzureLogsViewer.Model.Infrastructure;
using AzureLogsViewer.Model.Services.SlackIntegration;
using AzureLogsViewer.Model.WadLogs;
using Ninject;

namespace AzureLogsViewer.Model.Services
{
    public class WadLogsDumpService
    {
        internal static DateTime? UtcNowTestsOverride { get; set; }
        internal IIWadLogsReader LogsReaderOverride { get; set; }

        [Inject]
        public AlvDataContext DataContext { get; set; }

        [Inject]
        public SlackIntegrationService SlackIntegrationService { get; set; }

        public DateTime UtcNow { get { return UtcNowTestsOverride ?? DateTime.UtcNow; } }

        public void Dump()
        {
            var settings = GetDumpSettings();
            if(!settings.IsConfigured())
                return;

            var range = GetDumpRange(settings);

            var logsReader = CreateLogsReader(settings);
            var entries = logsReader.Read(range.From, range.To);

            var existingEntriesKeys = GetExistingEntriesKeys(range);

            var newEntries = entries.Where(x => !existingEntriesKeys.Contains(x.GetKey()))
                                    .ToList();

            GetDumpSettings().LatestDumpTime = DateTimeHelper.Min(range.To, UtcNow);

            BulkInsert(newEntries);

            DataContext.SaveChanges();

            ProcessByIntegrations(newEntries);
        }

        private void BulkInsert(List<WadLogEntry> newEntries)
        {
            SqlBulkCopy copy = new SqlBulkCopy(DataContext.Database.Connection.ConnectionString);
            copy.DestinationTableName = "WadLogEntries";

            var columns = typeof(WadLogEntry).GetProperties().Where(x => x.Name != nameof(WadLogEntry.Id)).ToList();
            var dataTable = new DataTable();

            foreach (var column in columns)
            {
                copy.ColumnMappings.Add(column.Name, column.Name);
                dataTable.Columns.Add(column.Name, column.PropertyType);
            }
            
            foreach (var entry in newEntries)
            {
                var row = dataTable.NewRow();
                row[nameof(WadLogEntry.EventDateTime)] = entry.EventDateTime;
                row[nameof(WadLogEntry.EventId)] = entry.EventId;
                row[nameof(WadLogEntry.Level)] = entry.Level;
                row[nameof(WadLogEntry.Message)] = entry.Message;
                row[nameof(WadLogEntry.PartitionKey)] = entry.PartitionKey;
                row[nameof(WadLogEntry.Pid)] = entry.Pid;
                row[nameof(WadLogEntry.Role)] = entry.Role;
                row[nameof(WadLogEntry.RoleInstance)] = entry.RoleInstance;
                row[nameof(WadLogEntry.RowKey)] = entry.RowKey;
                row[nameof(WadLogEntry.Tid)] = entry.Tid;

                dataTable.Rows.Add(row);
            }


            copy.WriteToServer(dataTable.Rows.Cast<DataRow>().ToArray());
        }

        private void ProcessByIntegrations(List<WadLogEntry> newEntries)
        {
            SlackIntegrationService.ProcessLogEntries(newEntries);
        }

        public void CleanupStaleLogs()
        {
            var settings = GetDumpSettings();
            var date = UtcNow.AddDays(-settings.LogsTTLInDays);

            DataContext.Database.ExecuteSqlCommand("DELETE TOP (10000) FROM WadLogEntries WHERE EventDateTime < @p0", date);
        }

        private HashSet<WadLogEntryKey> GetExistingEntriesKeys(DateTimeRange range)
        {
            var entryKeys =
                DataContext.WadLogEntries.AsNoTracking()
                           .Where(x => x.EventDateTime >= range.From && x.EventDateTime <= range.To)
                           .Select(x => new WadLogEntryKey
                           {
                               PartitionKey = x.PartitionKey,
                               RowKey = x.RowKey
                           }).ToList();

            return new HashSet<WadLogEntryKey>(entryKeys);
        }

        private DateTimeRange GetDumpRange(WadLogsDumpSettings settings)
        {
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

        public TimeSpan GetDelayForNextDump()
        {
            var settings = GetDumpSettings();
            // if settings is not configured yet we run next try in one minute
            // because we want start first dump as soon as possible after storage connection string is specified
            if (!settings.IsConfigured())
                return TimeSpan.FromMinutes(1);

            return TimeSpan.FromMinutes(settings.DelayBetweenDumpsInMinutes);
        }

        private IIWadLogsReader CreateLogsReader(WadLogsDumpSettings settings)
        {
            if(!settings.IsConfigured())
                throw new ArgumentException("settings should be configured (i.e. has storage connection string)");

            return LogsReaderOverride ?? new WadLogsReader(settings.StorageConnectionString);
        }
    }
}