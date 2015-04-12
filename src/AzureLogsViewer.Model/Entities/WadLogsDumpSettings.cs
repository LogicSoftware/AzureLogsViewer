using System;

namespace AzureLogsViewer.Model.Entities
{
    public class WadLogsDumpSettings
    {
        public WadLogsDumpSettings()
        {
            DelayBetweenDumpsInMinutes = 15;
            DumpOverlapInMinutes = 5;
            DumpSizeInMinutes = 30;
            LogsTTLInDays = 14;
        }

        public int Id { get; set; }

        public DateTime? LatestDumpTime { get; set; }

        public int DumpOverlapInMinutes { get; set; }

        public int DumpSizeInMinutes { get; set; }

        public int DelayBetweenDumpsInMinutes { get; set; }

        public int LogsTTLInDays { get; set; }

        public string StorageConnectionString { get; set; }

        public bool IsConfigured()
        {
            return !string.IsNullOrWhiteSpace(StorageConnectionString);
        }
    }
}