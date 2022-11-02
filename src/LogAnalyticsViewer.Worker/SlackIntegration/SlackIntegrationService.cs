using LogAnalyticsViewer.Model.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogAnalyticsViewer.Model.Entities;

namespace LogAnalyticsViewer.Worker.SlackIntegration
{
    public interface ISlackIntegrationService
    {
        Task ProcessEvents(List<Event> newEvents, string forChannel, int queryId);
    }

    public class SlackIntegrationService : ISlackIntegrationService
    {
        private readonly ISlackClient _client;
        private readonly ILogger<SlackIntegrationService> _logger;
        private readonly SlackIntegrationSettings _settings;

        public SlackIntegrationService(
            ILogger<SlackIntegrationService> logger,
            IOptionsMonitor<SlackIntegrationSettings> settings,
            ISlackClient client) =>
            (_logger, _settings, _client) =
            (logger, settings.CurrentValue, client);

        public async Task ProcessEvents(List<Event> newEvents, string forChannel, int queryId)
        {
            var processor = new SimilarityProcessor();
            var unique = processor.GetUniqueWithTotal(newEvents);

            var result = unique
                .Take(_settings.RatePerQuery)
                .Select(e => CreateMessage(e, queryId));

            foreach (var message in result)
            {
                await PostMessage(message, forChannel);
            }
        }

        private string CreateMessage(EventForSlack e, int queryId)
        {
            var link = _settings.EventUrlFormat
                .Replace("{From}", e.TimeGenerated.ToCommonFormat())
                .Replace("{To}", e.TimeGenerated.AddMilliseconds(1).ToCommonFormat())
                .Replace("{QueryId}", queryId.ToString());
                
            var text = _settings.MessagePattern
                .Replace("{Date}", e.TimeGenerated.ToString("G"))
                .Replace("{Link}", link)
                .Replace("{Source}", e.Source)
                .Replace("{Message}", Substring(e.Message))
                .Replace("{Total}", e.Total.ToString());

            return text;
        }

        private async Task PostMessage(string message, string channel)
        {
            try
            {
                await _client.PostMessage(_settings.ApiToken, message, channel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fail post slack message for channel {Channel}", channel);
            }
        }

        private string Substring(string message) => message.Length <= _settings.MessageLength
            ? message
            : $"{message[.._settings.MessageLength]}...";
    }
}

