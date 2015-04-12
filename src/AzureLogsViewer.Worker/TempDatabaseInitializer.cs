using System.Data.Entity;
using System.Data.Entity.Migrations;
using AzureLogsViewer.Model.Entities;
using AzureLogsViewer.Model.Infrastructure;

namespace AzureLogsViewer.WorkerConsole
{
    //TODO: remove it..
    internal class TempDatabaseInitializer : DropCreateDatabaseIfModelChanges<AlwDataContext>
    {
        protected override void Seed(AlwDataContext context)
        {
            base.Seed(context);

            context.WadLogsDumpSettings.AddOrUpdate(x => x.Id, new WadLogsDumpSettings { Id = 1 });
        }
    }
}