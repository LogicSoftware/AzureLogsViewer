using System;
using AzureLogsViewer.Model.Common;
using AzureLogsViewer.Model.WadLogs;

namespace AzureLogsViewer.Model.Entities
{
    public class WadLogEntry
    {
        public WadLogEntry()
        {}

        internal WadLogEntry(WadLogEntity entity)
        {
            PartitionKey = entity.PartitionKey;
            RowKey = entity.RowKey;
            Role = entity.Role;
            RoleInstance = entity.RoleInstance;
            Level = entity.Level;
            Message = entity.Message;
            Pid = entity.Pid;
            Tid = entity.Tid;
            EventId = entity.EventId;
            EventDateTime = entity.EventDateTime;
        }

        public long Id { get; set; }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string Role { get; set; }
        public string RoleInstance { get; set; }
        public int Level { get; set; }
        public string Message { get; set; }
        public int Pid { get; set; }
        public int Tid { get; set; }
        public int EventId { get; set; }
        public DateTime EventDateTime { get; set; }

        public WadLogEntryKey GetKey()
        {
            return new WadLogEntryKey(PartitionKey, RowKey);
        }
    }
}