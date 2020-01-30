using LogAnalyticsViewer.Model;
using LogAnalyticsViewer.Model.Services.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using LogAnalyticsViewer.Worker.SlackIntegration;

namespace LogAnalyticsViewer.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)            
                .UseWindowsService()              
                .ConfigureServices((hostContext, services) =>
                {
                    services.Configure<LogViewerSettings>(hostContext.Configuration.GetSection("LogViewer"));
                    services.Configure<SlackIntegrationSettings>(hostContext.Configuration.GetSection("SlackIntegration"));
                    services.Configure<LogAnalyticsSettings>(hostContext.Configuration.GetSection("LogViewer"));

                    services.AddDbContext<LAVDataContext>(options =>
                        options.UseSqlServer(hostContext.Configuration.GetConnectionString("ConnectionString")));

                    services.AddTransient(typeof(EventService));
                    services.AddTransient(typeof(SlackClient));
                    services.AddTransient(typeof(SlackIntegrationService));
                    
                    services.AddHostedService<LogViewer>();
                });
    }
}
