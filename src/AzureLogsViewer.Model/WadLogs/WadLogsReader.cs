using System;
using System.Collections.Generic;
using System.Linq;
using AzureLogsViewer.Model.Entities;
using Microsoft.WindowsAzure.Storage;

namespace AzureLogsViewer.Model.WadLogs
{
    public class WadLogsReader : IIWadLogsReader
    {
        public WadLogsReader(string cloudStorageConnectionString)
        {
            CloudStorageConnectionString = cloudStorageConnectionString;
        }

        private string CloudStorageConnectionString { get; set; }

        public List<WadLogEntry> Read(DateTime from, DateTime to)
        {
            var storageAccount = CloudStorageAccount.Parse(CloudStorageConnectionString);
            var tableClient = storageAccount.CreateCloudTableClient();
            var wadLogs = tableClient.GetTableReference("WADLogsTable");

            return wadLogs.CreateQuery<WadLogEntity>()
                .Where(
                wl => 
                        String.Compare(wl.PartitionKey,"0" + from.Ticks.ToString()) >=0 && 
                        String.Compare(wl.PartitionKey, "0" + to.Ticks.ToString()) < 0
                )
                .ToList()
                .Select(x => new WadLogEntry(x))
                .ToList();
        }
    }
}