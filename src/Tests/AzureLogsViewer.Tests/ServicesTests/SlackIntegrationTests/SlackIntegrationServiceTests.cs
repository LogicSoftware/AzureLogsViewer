using System;
using System.Collections.Generic;
using System.Linq;
using AzureLogsViewer.Model.DTO;
using AzureLogsViewer.Model.Entities;
using AzureLogsViewer.Model.Services.SlackIntegration;
using Moq;
using NUnit.Framework;

namespace AzureLogsViewer.Tests.ServicesTests.SlackIntegrationTests
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
        public void ProcessLogEntries_should_replace_link_with_url_to_this_item()
        {
            var date = DateTime.Today.AddDays(-5);
            CreateStubIntegration("Test {Message}, {Link}");

            var entries = new List<WadLogEntry>
            {
                new WadLogEntry { Id = 25, Message = "First", Level = 1, EventDateTime = date, Role = "Web"}
            };

            GetService<SlackIntegrationService>().ProcessLogEntries(entries);

            var slackMessages = DataContext.SlackMessages.Select(x => x.Text).ToArray();
            var expectedMessages = new[] { "Test First, http://mydomain.com/Home/Index/25"};

            CollectionAssert.AreEquivalent(expectedMessages, slackMessages);
        }  
        
        [Test]
        public void ProcessLogEntries_should_truncate_message_if_specified_lenght_less_than_message_lenght()
        {
            var date = DateTime.Today.AddDays(-5);
            CreateStubIntegration("Test {Level}-{Message:5}");

            var entries = new List<WadLogEntry>
            {
                new WadLogEntry {Message = "Abcdef", Level = 1, EventDateTime = date, Role = "Web"}
            };

            GetService<SlackIntegrationService>().ProcessLogEntries(entries);

            var slackMessages = DataContext.SlackMessages.Select(x => x.Text).ToArray();
            var expectedMessages = new[] { "Test 1-Abcde..." };

            CollectionAssert.AreEquivalent(expectedMessages, slackMessages);
        }  
        
        [Test]
        public void ProcessLogEntries_should_not_truncate_message_if_specified_lenght_greater_than_message_lenght()
        {
            var date = DateTime.Today.AddDays(-5);
            CreateStubIntegration("Test {Level}-{Message:10}");

            var entries = new List<WadLogEntry>
            {
                new WadLogEntry {Message = "Abcdef", Level = 1, EventDateTime = date, Role = "Web"}
            };

            GetService<SlackIntegrationService>().ProcessLogEntries(entries);

            var slackMessages = DataContext.SlackMessages.Select(x => x.Text).ToArray();
            var expectedMessages = new[] { "Test 1-Abcdef" };

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
        public void ProcessLogEntries_should_create_message_should_work_with_multiple_integrations()
        {
            CreateStubIntegration("{Message}", x => x.Level = 1, chanel: "#chanel1");
            CreateStubIntegration("{Message}", x => x.Level = 2, chanel: "#chanel2");

            var entries = new List<WadLogEntry>
            {
                new WadLogEntry {Message = "First", Level = 1},
                new WadLogEntry {Message = "Second", Level = 2},
            };

            GetService<SlackIntegrationService>().ProcessLogEntries(entries);

            var slackMessages = DataContext.SlackMessages.Select(x => new { x.Text, x.Chanel }).ToArray();
            var expectedMessages = new[]
            {
                new { Text = "First", Chanel = "#chanel1"},
                new { Text = "Second", Chanel = "#chanel2"}
            };

            CollectionAssert.AreEquivalent(expectedMessages, slackMessages);
        }  
        
        [Test]
        public void ProcessLogEntries_should_not_create_messages_for_disabled_integrations()
        {
            CreateStubIntegration("{Message}", enabled: false);

            var entries = new List<WadLogEntry>
            {
                new WadLogEntry {Message = "First", Level = 1}
            };

            GetService<SlackIntegrationService>().ProcessLogEntries(entries);

            var slackMessagesCount = DataContext.SlackMessages.Count();
            
            Assert.That(slackMessagesCount.Equals(0), "service shouldn't generate any message because integration is disabled");
        }  

        private void CreateStubIntegration(string message, Action<WadLogsFilter> filterSetup = null, string chanel = null, bool enabled = true)
        {
            var integrationInfo = new SlackIntegrationInfo
            {
                Enabled = enabled,
                MessagePattern = message,
                Chanel = chanel ?? _chanel,
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