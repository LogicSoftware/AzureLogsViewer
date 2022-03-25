using LogAnalyticsViewer.Model;
using LogAnalyticsViewer.Model.Services.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using LogAnalyticsViewer.Worker.SlackIntegration;
using Microsoft.Extensions.Logging;

namespace LogAnalyticsViewer.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            UpdateDatabase(host);
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)            
                .UseWindowsService()              
                .ConfigureServices((hostContext, services) =>
                {
                    services.Configure<LogViewerSettings>(hostContext.Configuration.GetSection("LogViewer"));
                    services.Configure<SlackIntegrationSettings>(hostContext.Configuration.GetSection("SlackIntegration"));
                    services.Configure<LogAnalyticsSettings>(hostContext.Configuration.GetSection("LogAnalytics"));

                    services.AddDbContext<LAVDataContext>(options =>
                        options.UseSqlServer(hostContext.Configuration.GetConnectionString("ConnectionString")));

                    services.AddTransient<IEventService, EventService>();
                    services.AddTransient<ISlackClient, SlackClient>();
                    services.AddTransient<ISlackIntegrationService, SlackIntegrationService>();
                    services.AddScoped<LogViewerWorker>();
                    services.AddHttpClient();
                    
                    services.AddHostedService<LogViewer>();
                    services.AddLogging(lb => lb.AddAzureWebAppDiagnostics());
                });



        private static void UpdateDatabase(IHost app)
        {
            using var serviceScope = app.Services
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();
            using var context = serviceScope.ServiceProvider.GetRequiredService<LAVDataContext>();
            
            context.Database.Migrate();
        }
    }
}
