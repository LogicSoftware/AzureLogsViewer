using System;
using System.Collections.Generic;

namespace AzureLogsViewer.Model.DTO
{
    public class WadLogsFilter
    {
        public WadLogsFilter()
        {
            MessageFilters = new List<MessageFilter>();
        }

        public DateTime? From { get; set; }

        public DateTime? To { get; set; }

        public int? Level { get; set; }

        public string Role { get; set; }

        public IEnumerable<MessageFilter> MessageFilters { get; set; }
    }
}