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
            modelBuilder.Entity<DumpSettings>().HasData(new DumpSettings { Id = 1 });

            modelBuilder.Entity<Query>().HasData(
                new Query { QueryId = 1, DisplayName = "epcore", Channel = "#site-errors", QueryText = @"Event {0}
| where Source == ""Easy Projects"" 
| where EventLevel == 2 
| project TimeGenerated, Message = RenderedDescription, Source = ""epcore""", },
                new Query { QueryId = 2, DisplayName = "microservices", Channel = "#site-errors", QueryText = @"production_services_CL {0} 
| where LogLevel_s == ""Error"" 
| project TimeGenerated, Message = strcat(LogMessage_s, LogException_s), Source = LogProperties_Application_s" });
        }
    }
}
