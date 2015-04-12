using System;

namespace AzureLogsViewer.Model.DTO
{
    public class WadLogsFilter
    {
        public DateTime? From { get; set; }

        public DateTime? To { get; set; }

        public int? Level { get; set; }

        public string Role { get; set; }

        public string Message { get; set; }
    }
}