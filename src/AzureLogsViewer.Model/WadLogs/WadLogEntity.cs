using System;

namespace AzureLogsViewer.Model.WadLogs
{
    internal class WadLogEntity
       : Microsoft.WindowsAzure.Storage.Table.TableEntity
    {
        public WadLogEntity()
        {
        }

        public string Role { get; set; }
        public string RoleInstance { get; set; }
        public int Level { get; set; }
        public string Message { get; set; }
        public int Pid { get; set; }
        public int Tid { get; set; }
        public int EventId { get; set; }
        public DateTime EventDateTime
        {
            get
            {
                return new DateTime(long.Parse(this.PartitionKey.Substring(1)));
            }
        }
    }
}