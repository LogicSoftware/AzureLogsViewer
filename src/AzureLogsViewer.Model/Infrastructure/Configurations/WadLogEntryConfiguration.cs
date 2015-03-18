using System.Data.Entity.ModelConfiguration;
using AzureLogsViewer.Model.Entities;

namespace AzureLogsViewer.Model.Infrastructure.Configurations
{
    internal class WadLogEntryConfiguration : EntityTypeConfiguration<WadLogEntry>
    {
        public WadLogEntryConfiguration()
        {
            ToTable("WadLogEntries");
        }
    }
}