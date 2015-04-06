using System.Data.Entity;
using AzureLogsViewer.Model.Entities;

namespace AzureLogsViewer.Model.Infrastructure
{
    public class AlwDataContext : DbContext
    {
        public AlwDataContext()
            : base("alw")
        {}

        public DbSet<WadLogEntry> WadLogEntries { get; set; }

        public DbSet<WadLogsDumpSettings> WadLogsDumpSettings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.AddFromAssembly(typeof (AlwDataContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}