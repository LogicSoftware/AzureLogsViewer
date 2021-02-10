using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;

namespace LogAnalyticsViewer.Model.DTO
{
    public class Event
    {
        public Event() { }

        public Event(IList<string> values)
        {
            TimeGenerated = DateTime.Parse(values[0], CultureInfo.InvariantCulture);
            Message = values[1];
            Source = values[2];
        }

        public DateTime TimeGenerated { get; set; }
        public string Message { get; set; }
        public string Source { get; set; }

        public override int GetHashCode() => TimeGenerated.GetHashCode();

        public override bool Equals(object obj) =>
            obj switch
            {
                Event b => b.TimeGenerated == TimeGenerated && b.Message == Message && b.Source == Source,
                _ => false
            };
    }
}
