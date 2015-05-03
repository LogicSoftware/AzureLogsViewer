using System;
using System.Collections.Generic;
using System.Linq;
using AzureLogsViewer.Model.Entities;
using AzureLogsViewer.Model.Infrastructure;
using Ninject;

namespace AzureLogsViewer.Model.Services.SlackIntegration
{
    public class SlackIntegrationService
    {
        [Inject]
        public AlvDataContext DataContext { get; set; }

        [Inject]
        public ISlackClient Client { get; set; }

        public void ProcessLogEntries(List<WadLogEntry> entries)
        {
            try
            {
                ProcessLogEntriesImpl(entries);
            }
            catch (Exception ex)
            {
                AlvLogger.LogError("Error during process log entries by slack integration", ex);
            }
        }

        public void SendMessages()
        {
            var messages = DataContext.SlackMessages.Take(100).ToList();
            foreach (var slackMessage in messages)
            {
                PostMessage(slackMessage);
            }
        }

        private void PostMessage(SlackMessage slackMessage)
        {
            try
            {
                Client.PostMessage(slackMessage.WebHookUrl, slackMessage.Text, slackMessage.Chanel);
                DataContext.SlackMessages.Remove(slackMessage);
                DataContext.SaveChanges();
            }
            catch (Exception ex)
            {
                AlvLogger.LogError(string.Format("Fail post slack message with id {0}", slackMessage.Id), ex);
            }
        }

        private void ProcessLogEntriesImpl(List<WadLogEntry> entries)
        {
            var integrationInfo = DataContext.SlackIntegrationInfos.FirstOrDefault();
            if (integrationInfo == null || !integrationInfo.Enabled)
            {
                return;
            }

            var filter = integrationInfo.GetFilter();
            var matchItems = filter.Apply(entries.AsQueryable());

            var messages = matchItems.Select(x => CreateMessage(x, integrationInfo));

            DataContext.SlackMessages.AddRange(messages);
            DataContext.SaveChanges();
        }

        private SlackMessage CreateMessage(WadLogEntry wadLogEntry, SlackIntegrationInfo integrationInfo)
        {
            var text = integrationInfo.MessagePattern;
            text = text.Replace("{Date}", wadLogEntry.EventDateTime.ToString("G"));
            text = text.Replace("{Role}", wadLogEntry.Role);
            text = text.Replace("{Level}", wadLogEntry.Level.ToString());
            text = text.Replace("{Message}", wadLogEntry.Message);
            return new SlackMessage
            {
                Chanel = integrationInfo.Chanel,
                WebHookUrl = integrationInfo.WebHookUrl,
                Text = text
            };
        }

        
    }
}