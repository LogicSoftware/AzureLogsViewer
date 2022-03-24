using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LogAnalyticsViewer.Model.Entities;
using LogAnalyticsViewer.Model.Services.Events;
using LogAnalyticsViewer.Worker;
using LogAnalyticsViewer.Worker.SlackIntegration;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.AutoMock;

namespace LogAnalytics.Tests;

[TestClass]
public class LogViewerWorkerTests : DbTestBase
{
    [TestMethod]
    public async Task DoWork_should_properly_handle_existing_events()
    {
        var timeGenerated = DateTime.Today;
        
        var dataContext = CreateContext(true);
        var query = new Query
        {
            DisplayName = "display name",
            QueryText = "query text",
            Channel = "channel",
            Events = new List<Event>
            {
                new()
                {
                    Message = "message to delete",
                    Source = "source",
                    TimeGenerated = timeGenerated
                },
                new()
                {
                    Message = "message to keep",
                    Source = "source",
                    TimeGenerated = timeGenerated
                }
            }
        };

        dataContext.Queries.Add(query);
        await dataContext.SaveChangesAsync();

        var mocker = new AutoMocker();
        mocker.Use(dataContext);

        var settings = new LogViewerSettings();
        mocker.Use(Mock.Of<IOptionsMonitor<LogViewerSettings>>(x => x.CurrentValue == settings));

        mocker.GetMock<IEventService>()
            .Setup(x => x.GetEventsForWorker(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(new List<Event>
            {
                new()
                {
                    Message = "message to keep",
                    Source = "source",
                    TimeGenerated = timeGenerated
                },
                new()
                {
                    Message = "new message",
                    Source = "source",
                    TimeGenerated = timeGenerated
                }
            });

        mocker.GetMock<ISlackIntegrationService>()
            .Setup(x => x.ProcessEvents(It.IsAny<List<Event>>(), It.IsAny<string>(), It.IsAny<int>()))
            .Returns(Task.CompletedTask);
        
        var worker = mocker.CreateInstance<LogViewerWorker>();

        await worker.DoWork();

        await using var newContext = CreateContext();
        newContext.Events.Where(x => x.Query.QueryId == query.QueryId)
            .Select(x => x.Message)
            .Should()
            .BeEquivalentTo(new[]
            {
                "message to keep",
                "new message",
            });
    }
}