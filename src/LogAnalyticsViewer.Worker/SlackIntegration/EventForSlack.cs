using LogAnalyticsViewer.Model.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using LogAnalyticsViewer.Model.Entities;

namespace LogAnalyticsViewer.Worker.SlackIntegration
{
    public class EventForSlack : Event
    {
        public EventForSlack(Event e) =>
            (TimeGenerated, Message, Source) =
            (e.TimeGenerated, e.Message, e.Source);

        public IDictionary<string, int> Profile { get; set; }
        public int Total { get; set; } = 1;
    }
}
