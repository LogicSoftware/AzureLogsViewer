using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace LogAnalyticsViewer.Model.Entities;

public class Event
{
    public Event() { }

    public Event(IList<string> values)
    {
        TimeGenerated = DateTime.Parse(values[0], CultureInfo.InvariantCulture);
        Message = values[1];
        Source = values[2];
    }

    [Key]  
    public int EventId { get; set; }

    public Query Query { get; set; }
        
    public DateTime TimeGenerated { get; init; }
    public string Message { get; set; }
    public string Source { get; set; }
}
