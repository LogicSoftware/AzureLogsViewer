using LogAnalyticsViewer.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogAnalyticsViewer.Model.Services.Events
{
    public class EventService
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public Task<IEnumerable<Event>> GetEvents(int queryId, DateTime from, DateTime to, params Filter[] filters)
        {
            var rng = new Random();
            return Task.FromResult(Enumerable.Range(1, 5).Select(index => new Event
            {
                TimeGenerated = DateTime.Now.AddDays(index),
                Message = Summaries[rng.Next(Summaries.Length)],
                QueryId = index,
            }));
        }
    }
}
