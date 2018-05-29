using System;

namespace AzureLogsViewer.Model.Entities
{
    public class WadLogsStorageSettings
    {
        public int Id { get; set; }

        public DateTime? LatestDumpTime { get; set; }

        public string StorageConnectionString { get; set; }

        public virtual WadLogsDumpSettings WadLogsDumpSetting { get; set; }

        public int WadLogsDumpSettingId { get; set; }

        public bool IsConfigured() => !string.IsNullOrWhiteSpace(StorageConnectionString);
    }
}