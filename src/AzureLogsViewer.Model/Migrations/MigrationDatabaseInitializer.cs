using System.Data.Entity;
using System.Data.Entity.Migrations;
using AzureLogsViewer.Model.Entities;
using AzureLogsViewer.Model.Infrastructure;

namespace AzureLogsViewer.Model.Migrations
{
    public class MigrationDatabaseInitializer : MigrateDatabaseToLatestVersion<AlvDataContext, Configuration>, IDatabaseInitializer<AlvDataContext>
    {
        public override void InitializeDatabase(AlvDataContext context)
        {
            base.InitializeDatabase(context);

            context.WadLogsDumpSettings.AddOrUpdate(x => x.Id, new WadLogsDumpSettings { Id = 1 });
        }
    }
}