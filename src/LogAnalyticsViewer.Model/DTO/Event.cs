using System;
using System.Collections.Generic;
using System.Text;

namespace LogAnalyticsViewer.Model.DTO
{
    public class Event
    {
        public DateTime TimeGenerated { get; set; }
        public string Message { get; set; }
        public int QueryId { get; set; }
    }
}
