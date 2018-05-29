using System.Collections.Generic;
using System.Linq;

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

        public int DumpOverlapInMinutes { get; set; }

        public int DumpSizeInMinutes { get; set; }

        public int DelayBetweenDumpsInMinutes { get; set; }

        public int LogsTTLInDays { get; set; }

        public virtual ICollection<WadLogsStorageSettings> Storages { get; set; }

        public IEnumerable<WadLogsStorageSettings> ConfiguredStorages() => Storages.Where(x => x.IsConfigured());

        public bool IsConfigured()
        {
            return ConfiguredStorages().Any();
        }
    }
}