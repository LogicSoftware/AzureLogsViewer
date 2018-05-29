using System.Data.Entity.ModelConfiguration;
using AzureLogsViewer.Model.Entities;

namespace AzureLogsViewer.Model.Infrastructure.Configurations
{
    internal class WadLogsStorageSettingsConfiguration : EntityTypeConfiguration<WadLogsStorageSettings>
    {
        public WadLogsStorageSettingsConfiguration()
        {
            ToTable("WadLogsStorageSettings");

            HasRequired(x => x.WadLogsDumpSetting)
                .WithMany(x => x.Storages)
                .HasForeignKey(x => x.WadLogsDumpSettingId).WillCascadeOnDelete();
        }
    }
}