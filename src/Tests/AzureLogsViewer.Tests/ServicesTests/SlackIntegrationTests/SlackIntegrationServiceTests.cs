using System;
using System.Collections.Generic;
using System.Linq;
using AzureLogsViewer.Model.DTO;
using AzureLogsViewer.Model.Entities;
using AzureLogsViewer.Model.Services;
using AzureLogsViewer.Model.Services.SlackIntegration;
using AzureLogsViewer.Model.WadLogs;
using AzureLogsViewer.Tests.Helpers;
using Moq;
using NUnit.Framework;

namespace AzureLogsViewer.Tests.ServicesTests
{
    [TestFixture]
    public class SlackIntegrationServiceTests : BaseIntegrationTest
    {
        private string _webHookUrl = "http://slack.com/fdafafd";
        private string _chanel = "#dev";

        [Test]
        public void ProcessLogEntries_should_create_message_for_logentries_satisfied_by_filter()
        {
            CreateStubIntegration("{Message}", x => x.Level = 2);

            var entries = new List<WadLogEntry>
            {
                new WadLogEntry {Message = "First", Level = 1},
                new WadLogEntry {Message = "Second", Level = 2},
            };

            GetService<SlackIntegrationService>().ProcessLogEntries(entries);

            var slackMessages = DataContext.SlackMessages.Select(x => x.Text).ToArray();
            var expectedMessages = new[] {"Second"};

            CollectionAssert.AreEquivalent(expectedMessages, slackMessages);
        }  
        
        [Test]
        public void ProcessLogEntries_should_format_slack_message_correctly()
        {
            var date = DateTime.Today.AddDays(-5);
            CreateStubIntegration("Test {Date}-{Level}-{Role}-{Message}");

            var entries = new List<WadLogEntry>
            {
                new WadLogEntry {Message = "First", Level = 1, EventDateTime = date, Role = "Web"}
            };

            GetService<SlackIntegrationService>().ProcessLogEntries(entries);

            var slackMessages = DataContext.SlackMessages.Select(x => x.Text).ToArray();
            var expectedMessages = new[] {string.Format("Test {0}-1-Web-First", date.ToString("G"))};

            CollectionAssert.AreEquivalent(expectedMessages, slackMessages);
        }  
        
        [Test]
        public void ProcessLogEntries_should_set_chanel_and_webhookurl_for_message()
        {
            CreateStubIntegration("{Message}");

            var entries = new List<WadLogEntry>
            {
                new WadLogEntry {Message = "First"}
            };

            GetService<SlackIntegrationService>().ProcessLogEntries(entries);

            var slackMessage = DataContext.SlackMessages.Single();

            Assert.That(slackMessage.WebHookUrl, Is.EqualTo(_webHookUrl), "webhookurl should be set");
            Assert.That(slackMessage.Chanel, Is.EqualTo(_chanel), "chanel should be set");
        }

        [Test]
        public void SendMessages_should_send_existing_message()
        {
            DataContext.SlackMessages.Add(new SlackMessage
            {
                Text = "test",
                Chanel = "#dev",
                WebHookUrl = "http://slack/12",
            });

            DataContext.SaveChanges();

            var service = GetService<SlackIntegrationService>();
            service.Client = Mock.Of<ISlackClient>();

            service.SendMessages();

            Mock.Get(service.Client).Verify(x => x.PostMessage("http://slack/12", "test", "#dev"), Times.Once, "should send existing message");
        }

        [Test]
        public void SendMessages_should_delete_sent_messages()
        {
            DataContext.SlackMessages.Add(new SlackMessage
            {
                Text = "test",
                Chanel = "#dev",
                WebHookUrl = "http://slack/12",
            });

            DataContext.SaveChanges();

            var service = GetService<SlackIntegrationService>();
            service.Client = Mock.Of<ISlackClient>();

            service.SendMessages();

            ResetDataContext();

            var messagesCount = DataContext.SlackMessages.Count();
            Assert.That(messagesCount, Is.EqualTo(0), "should delete sent messages");
        }

        [Test]
        public void test()
        {
            var info = new SlackIntegrationInfo();
            info.SetFilter(new WadLogsFilter
            {
                Level = 2,
                Role = "LogicSoftware.EasyProjects.SendNotifications"
            });

            Console.WriteLine(info.SerializedFilter);
        }

        private void CreateStubIntegration(string message, Action<WadLogsFilter> filterSetup = null)
        {
            var integrationInfo = new SlackIntegrationInfo
            {
                Enabled = true,
                MessagePattern = message,
                Chanel = _chanel,
                WebHookUrl = _webHookUrl
            };

            var wadLogsFilter = new WadLogsFilter();
            if(filterSetup != null)
                filterSetup(wadLogsFilter);

            integrationInfo.SetFilter(wadLogsFilter);

            DataContext.SlackIntegrationInfos.Add(integrationInfo);
            DataContext.SaveChanges();
        }
    }
}