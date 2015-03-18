using System.Data.Entity.ModelConfiguration;
using AzureLogsViewer.Model.Entities;

namespace AzureLogsViewer.Model.Infrastructure.Configurations
{
    internal class WadLogsDumpSettingsConfiguration : EntityTypeConfiguration<WadLogsDumpSettings>
    {
        public WadLogsDumpSettingsConfiguration()
        {
            ToTable("WadLogsDumpSettings");
        }
    }
}