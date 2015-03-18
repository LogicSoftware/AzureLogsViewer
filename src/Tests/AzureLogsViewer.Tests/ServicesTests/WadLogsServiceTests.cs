using System;
using System.Collections.Generic;
using System.Linq;
using AzureLogsViewer.Model.Entities;
using AzureLogsViewer.Model.Services;
using AzureLogsViewer.Model.WadLogs;
using AzureLogsViewer.Tests.Helpers;
using NUnit.Framework;

namespace AzureLogsViewer.Tests.ServicesTests
{
    [TestFixture]
    public class WadLogsServiceTests : BaseIntegrationTest
    {
        private DateTime Now { get; set; }

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            Now = DateTime.UtcNow;
            WadLogsService.UtcNowTestsOverride = Now;
        }

        [Test]
        public void Dump_first_time_should_read_logs_for_dump_interval_starting_from_one_hour_ago()
        {
            //arrange

            //todo: set dump settings explicity

            var reader = WadLogsReaderStub.Create()
                                          .WithEntryOn(Now.AddHours(-1.4))
                                          .WithEntryOn(Now.AddHours(-0.7))
                                          .WithEntryOn(Now.AddHours(-0.2));

            //act
            RunDump(reader);

            //assert

            var actualEntries = DataContext.WadLogEntries.Select(x => x.EventDateTime).ToArray();
            var expectedEntries = new[] {Now.AddHours(-0.7)};

            Assert.That(expectedEntries, Is.EquivalentTo(actualEntries).WithinSeconds(), "should add only entries in range -1hour -> 0.5hour");
        }

        private void RunDump(WadLogsReaderStub reader)
        {
            var service = new WadLogsService
            {
                WadLogsReader = reader
            };

            service.Dump();
        }

        private class WadLogsReaderStub : IIWadLogsReader
        {
            private WadLogsReaderStub()
            {
                Entries = new List<WadLogEntry>();
            }

            public static WadLogsReaderStub Create()
            {
                return new WadLogsReaderStub();
            }

            public List<WadLogEntry> Entries { get; set; }

            public WadLogsReaderStub WithEntryOn(DateTime date)
            {
                Entries.Add(new WadLogEntry
                {
                    EventDateTime = date,
                    PartitionKey = "0" + date.Ticks,
                    RowKey = date.Ticks.ToString()
                });

                return this;
            }

            public List<WadLogEntry> Read(DateTime @from, DateTime to)
            {
                return Entries.Where(x => x.EventDateTime >= @from && x.EventDateTime <= to).ToList();
            }
        }
    }
}