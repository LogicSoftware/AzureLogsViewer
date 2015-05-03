namespace AzureLogsViewer.Model.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<AzureLogsViewer.Model.Infrastructure.AlvDataContext>
    {
        public Configuration()
        {
#if DEBUG
            AutomaticMigrationsEnabled = true;
#else
            AutomaticMigrationsEnabled = false;
#endif

            ContextKey = "AzureLogsViewer.Model.Infrastructure.AlvDataContext";
        }

        protected override void Seed(AzureLogsViewer.Model.Infrastructure.AlvDataContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
