using LogAnalyticsViewer.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogAnalyticsViewer.Model.DTO
{
    public class Filter
    {
        public string Text { get; set; }
        public FilterType Type { get; set; } = FilterType.Like;
    }
}
