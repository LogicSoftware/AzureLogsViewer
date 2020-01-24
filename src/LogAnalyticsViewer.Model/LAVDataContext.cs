using LogAnalyticsViewer.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace LogAnalyticsViewer.Model
{
    public class LAVDataContext: DbContext
    {
        public LAVDataContext(DbContextOptions options): base(options)
        {

        }

        public DbSet<Query> Queries { get; set; }
        public DbSet<DumpSettings> DumpSettings { get; set; }
        public DbSet<LogAnalyticsSettings> LogAnalyticsSettings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Query>(entity => entity.Property(e => e.QueryText).IsRequired());

            modelBuilder.Entity<DumpSettings>().HasData(new DumpSettings { Id = 1 });
        }
    }
}
