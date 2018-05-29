using System;
using AzureLogsViewer.Model.Entities;
using AzureLogsViewer.Model.Infrastructure;

namespace AzureLogsViewer.Tests.Helpers
{
    class WadLogEntryBuilder
    {
        private WadLogEntry _wadLogEntry;

        private WadLogEntryBuilder()
        {
            _wadLogEntry = new WadLogEntry()
            {
                StorageId = 1
            };
        }

        public WadLogEntryBuilder WithEventDate(DateTime date)
        {
            _wadLogEntry.EventDateTime = date;
            _wadLogEntry.PartitionKey = "0" + date.Ticks;
            _wadLogEntry.RowKey = date.Ticks.ToString();
            return this;
        }

        public WadLogEntry Create()
        {
            using (var context = new AlvDataContext())
            {
                context.WadLogEntries.Add(_wadLogEntry);
                context.SaveChanges();
            }

            return _wadLogEntry;
        }

        public static WadLogEntryBuilder New()
        {
            return new WadLogEntryBuilder();
        }
    }
}