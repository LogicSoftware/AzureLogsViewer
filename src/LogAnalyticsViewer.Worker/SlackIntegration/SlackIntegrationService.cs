using LogAnalyticsViewer.Model.DTO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace LogAnalyticsViewer.Worker.SlackIntegration
{
    public class SlackIntegrationService
    {
        private readonly SlackClient _client;
        private readonly ILogger<SlackIntegrationService> _logger;
        private readonly SlackIntegrationSettings _settings;

        public SlackIntegrationService(
            ILogger<SlackIntegrationService> logger,
            IOptionsMonitor<SlackIntegrationSettings> settings,
            SlackClient client) =>
            (_logger, _settings, _client) =
            (logger, settings.CurrentValue, client);

        public void ProcessEvents(List<Event> newEvents, string forChannel)
        {
            var processor = new SimilarityProcessor();
            var unique = processor.GetUniqueWithTotal(newEvents);
                
            var result = unique
                .Take(_settings.RatePerQuery)
                .Select(e => CreateMessage(e))
                .ToList();

            result.ForEach(message => PostMessage(message, forChannel));
        }       

        private string CreateMessage(EventForSlack e)
        {
            var link = _settings.EventUrlFormat
                .Replace("{TimeGenerated}", e.TimeGenerated.ToString("s", CultureInfo.InvariantCulture))
                .Replace("{Source}", e.Source);
                
            var text = _settings.MessagePattern
                .Replace("{Date}", e.TimeGenerated.ToString("G"))
                .Replace("{Link}", link)
                .Replace("{Source}", e.Source)
                .Replace("{Message}", Substring(e.Message))
                .Replace("{Total}", e.Total.ToString());

            return text;
        }

        private void PostMessage(string message, string channel)
        {
            try
            {
                _client.PostMessage(_settings.WebHookUrl, message, channel);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Fail post slack message for channel {0}", channel), ex);
            }
        }

        private string Substring(string message) => message.Length <= _settings.MessageLength
            ? message
            : $"{message.Substring(0, _settings.MessageLength)}...";
    }
}

