using System;
using System.Collections.Generic;
using System.Text;

namespace LogAnalyticsViewer.Model.DTO
{
    public class EventRequest
    {
        public int? QueryId { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public List<MessageFilter> MessageFilters { get; set; }
    }
}
