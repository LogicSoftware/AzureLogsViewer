using System;

namespace AzureLogsViewer.Model.Entities
{
    public class WadLogsDumpSettings
    {
        public WadLogsDumpSettings()
        {
            DumpOverlapInMinutes = 5;
            DumpSizeInMinutes = 30;
        }

        public int Id { get; set; }

        public DateTime? LatestDumpTime { get; set; }

        public int DumpOverlapInMinutes { get; set; }

        public int DumpSizeInMinutes { get; set; }
    }
}