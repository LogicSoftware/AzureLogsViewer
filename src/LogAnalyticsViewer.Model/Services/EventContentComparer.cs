using System;
using System.Collections.Generic;
using LogAnalyticsViewer.Model.Entities;

namespace LogAnalyticsViewer.Model.Services;

public class EventContentComparer : IEqualityComparer<Event>
{
    public bool Equals(Event x, Event y)
    {
        if (ReferenceEquals(x, y))
            return true;
        if (ReferenceEquals(x, null))
            return false;
        if (ReferenceEquals(y, null))
            return false;
        if (x.GetType() != y.GetType())
            return false;

        return x.TimeGenerated.Equals(y.TimeGenerated) &&
               x.Message == y.Message &&
               x.Source == y.Source;
    }
    public int GetHashCode(Event obj)
    {
        return HashCode.Combine(obj.TimeGenerated, obj.Message, obj.Source);
    }
}
