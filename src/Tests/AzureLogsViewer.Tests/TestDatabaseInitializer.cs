using System.Data.Entity;
using System.Data.Entity.Migrations;
using AzureLogsViewer.Model.Entities;
using AzureLogsViewer.Model.Infrastructure;

namespace AzureLogsViewer.Tests
{
    internal class TestDatabaseInitializer : DropCreateDatabaseIfModelChanges<AlwDataContext>
    {
        protected override void Seed(AlwDataContext context)
        {
            base.Seed(context);

            context.WadLogsDumpSettings.AddOrUpdate(x => x.Id, new WadLogsDumpSettings { Id =  1});
        }
    }
}