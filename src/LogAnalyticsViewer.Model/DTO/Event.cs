using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace LogAnalyticsViewer.Model.DTO
{
    public class Event
    {
        public Event(IList<string> values)
        {
            this.TimeGenerated = DateTimeOffset.Parse(values[0], CultureInfo.InvariantCulture).UtcDateTime;
            this.Message = values[1];
            this.Source = values[2];
        }

        public DateTime TimeGenerated { get; set; }
        public string Message { get; set; }
        public string Source { get; set; }
    }
}
