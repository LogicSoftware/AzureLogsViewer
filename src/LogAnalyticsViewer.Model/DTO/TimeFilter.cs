using LogAnalyticsViewer.Model.Enums;
using LogAnalyticsViewer.Model.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogAnalyticsViewer.Model.DTO
{
    public class TimeFilter
    {
        public DateTime? Date { get; set; }
        public int? TimeInMinutes { get; set; }
        public TimeFilterType Type { get; set; }

        public TimeFilter(DateTime? date, TimeFilterType type) => (Date, Type) = (date, type);
        public TimeFilter(int timeInMinutes) => (TimeInMinutes, Type) = (timeInMinutes, TimeFilterType.Ago);

        public string FilterStr => Type switch
        {            
            TimeFilterType.LessOrEqual => Date != null ? $"TimeGenerated <= {Date.Value.ToAzureDate()}" : string.Empty,
            TimeFilterType.GreaterOrEqual => Date != null ? $"TimeGenerated >= {Date.Value.ToAzureDate()}" : string.Empty,
            TimeFilterType.Ago => TimeInMinutes != null ? $"TimeGenerated <= ago({TimeInMinutes}m)" : string.Empty,

            _ => throw new InvalidOperationException($"Invalid time filter: {this}")
        };

        public override string ToString()
        {
            return $"Type: {Type}, Date: {Date}, TimeInMinutes: {TimeInMinutes}";
        }
    }
}
