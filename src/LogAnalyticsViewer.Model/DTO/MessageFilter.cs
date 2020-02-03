using LogAnalyticsViewer.Model.Enums;
using System;

namespace LogAnalyticsViewer.Model.DTO
{
    public class MessageFilter
    {
        public string Text { get; set; }
        public MessageFilterType Type { get; set; } = MessageFilterType.Like;

        public string FilterStr => Type switch
        {
            MessageFilterType.Like => $"Message contains \"{Text}\"",
            MessageFilterType.NotLike => $"Message !contains \"{Text}\"",

            _ => throw new InvalidOperationException($"Invalid message filter type: {Type}")
        };
    }
}
