using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using AzureLogsViewer.Model.DTO;
using AzureLogsViewer.Model.Entities;
using AzureLogsViewer.Model.Infrastructure;
using Ninject;

namespace AzureLogsViewer.Model.Services.SlackIntegration
{
    public class SlackIntegrationService
    {
        private static string _entryUrlFormat;

        [Inject]
        public AlvDataContext DataContext { get; set; }

        [Inject]
        public ISlackClient Client { get; set; }

        private static string EntryUrlFormat
        {
            get { return _entryUrlFormat ?? ( _entryUrlFormat = (ConfigurationManager.AppSettings["SiteUrl"] + "Home/Index/{0}")); }
        }

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
            var integrationInfos = DataContext.SlackIntegrationInfos.Where(x => x.Enabled);
            var messages = new List<SlackMessage>();
            foreach (var integrationInfo in integrationInfos)
            {
                var filter = integrationInfo.GetFilter();
                var matchItems = filter.Apply(entries.AsQueryable());

                messages.AddRange(matchItems.Select(x => CreateMessage(x, integrationInfo)));    
            }

            if (messages.Any())
            {
                DataContext.SlackMessages.AddRange(messages);
                DataContext.SaveChanges();
            }
        }

        private SlackMessage CreateMessage(WadLogEntry wadLogEntry, SlackIntegrationInfo integrationInfo)
        {
            var text = integrationInfo.MessagePattern;
            text = text.Replace("\\n", Environment.NewLine);
            text = text.Replace("{Date}", wadLogEntry.EventDateTime.ToString("G"));
            text = text.Replace("{Role}", wadLogEntry.Role);
            text = text.Replace("{Level}", wadLogEntry.Level.ToString());
            text = text.Replace("{Link}", string.Format(EntryUrlFormat, wadLogEntry.Id));

            var messagePattern = new Regex(@"\{Message:?(?<length>\d+)?\}", RegexOptions.Compiled);
            text = messagePattern.Replace(text, (m) =>
            {
                if (string.IsNullOrEmpty(m.Groups["length"].Value))
                    return wadLogEntry.Message;

                var length = int.Parse(m.Groups["length"].Value);
                return wadLogEntry.Message.Length > length ? wadLogEntry.Message.Substring(0, length) + "..." : wadLogEntry.Message;
            });

            return new SlackMessage
            {
                Chanel = integrationInfo.Chanel,
                WebHookUrl = integrationInfo.WebHookUrl,
                Text = text
            };
        }


        public IEnumerable<SlackIntegrationInfoModel> GetIntegrationInfos()
        {
            return DataContext.SlackIntegrationInfos.ToList()
                              .Select(x => new SlackIntegrationInfoModel(x))
                              .ToList();
        }

        public SlackIntegrationInfoModel Save(SlackIntegrationInfoModel model)
        {
            var enity = model.IsPersisted() ? DataContext.SlackIntegrationInfos.Find(model.Id) : new SlackIntegrationInfo();
            model.ApplyChanges(enity);
            if (!model.IsPersisted())
            {
                DataContext.SlackIntegrationInfos.Add(enity);
            }

            DataContext.SaveChanges();
            return new SlackIntegrationInfoModel(enity);
        }
    }
}