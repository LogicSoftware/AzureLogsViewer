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
            SetDumpSettings(x =>
            {
                x.DumpSizeInMinutes = 30;
                x.LatestDumpTime = null;
            });
            
            var reader = WadLogsReaderStub.Create()
                                          .WithEntryOn(Now.AddHours(-1.4))
                                          .WithEntryOn(Now.AddHours(-0.7))
                                          .WithEntryOn(Now.AddHours(-0.2));
            //act
            RunDump(reader);

            //assert

            var actualEntries = DataContext.WadLogEntries.Select(x => x.EventDateTime).ToArray();
            var expectedEntries = new[] {Now.AddHours(-0.7)};

            Assert.That(actualEntries, Is.EquivalentTo(expectedEntries).WithinSeconds(), "should add only entries in range -1hour -> -0.5hour");
        }

        [Test]
        public void Dump_should_read_logs_in_dumpSize_interval_starting_from_latestdumptime()
        {
            //arrange
            SetDumpSettings(x =>
            {
                x.DumpSizeInMinutes = 30;
                x.DumpOverlapInMinutes = 1;
                x.LatestDumpTime = Now.AddHours(-0.5);
            });
            
            var reader = WadLogsReaderStub.Create()
                                          .WithEntryOn(Now.AddHours(-0.7))
                                          .WithEntryOn(Now.AddHours(-0.2))
                                          .WithEntryOn(Now.AddMinutes(-1))
                                          .WithEntryOn(Now.AddHours(1));
            //act
            RunDump(reader);

            //assert

            var actualEntries = DataContext.WadLogEntries.Select(x => x.EventDateTime).ToArray();
            var expectedEntries = new[] { Now.AddHours(-0.2), Now.AddMinutes(-1) };

            Assert.That(actualEntries, Is.EquivalentTo(expectedEntries).WithinSeconds(), "should add only entries in range -0.5h -> now");
        }

        [Test]
        public void Dump_should_read_part_of_previous_dump_to_save_logs_added_with_delay_in_azure_storage()
        {
            //arrange
            SetDumpSettings(x =>
            {
                x.DumpSizeInMinutes = 30;
                x.DumpOverlapInMinutes = 30;
                x.LatestDumpTime = Now.AddHours(-0.5);

            });
            
            var reader = WadLogsReaderStub.Create()
                                          .WithEntryOn(Now.AddHours(-1.5))
                                          .WithEntryOn(Now.AddHours(-0.7)) //should be read becuase lastdumpttime - overlapinminutes (-1h) < -0.7h
                                          .WithEntryOn(Now.AddHours(1));
            //act
            RunDump(reader);

            //assert

            var actualEntries = DataContext.WadLogEntries.Select(x => x.EventDateTime).ToArray();
            var expectedEntries = new[] { Now.AddHours(-0.7) };

            Assert.That(actualEntries, Is.EquivalentTo(expectedEntries).WithinSeconds(), "should add only entries in range -01h -> now");
        }

        [Test]
        public void Dump_should_not_add_entries_from_overlap_interval_twice()
        {
            //arrange
            SetDumpSettings(x =>
            {
                x.DumpSizeInMinutes = 30;
                x.DumpOverlapInMinutes = 30;
                x.LatestDumpTime = Now.AddHours(-0.5);

            });

            WadLogEntryBuilder.New().WithEventDate(Now.AddHours(-0.7))
                                    .Create();
            
            var reader = WadLogsReaderStub.Create()
                                          .WithEntryOn(Now.AddHours(-0.7)) //shouldn't add this entry because it was saved during previous dump
                                          .WithEntryOn(Now.AddHours(-0.75))
                                          .WithEntryOn(Now.AddHours(1));
            //act
            RunDump(reader);

            //assert

            var actualEntries = DataContext.WadLogEntries.Select(x => x.EventDateTime).ToArray();
            var expectedEntries = new[] { Now.AddHours(-0.7), Now.AddHours(-0.75) };

            Assert.That(actualEntries, Is.EquivalentTo(expectedEntries).WithinSeconds(), "should add only entry on -0.75h");
        }

        [Test]
        public void Dump_should_update_latest_dump_time()
        {
            //arrange
            SetDumpSettings(x =>
            {
                x.DumpSizeInMinutes = 30;
                x.DumpOverlapInMinutes = 30;
                x.LatestDumpTime = Now.AddHours(-1);
            });

            //act
            RunDump(WadLogsReaderStub.Create());

            //assert
            ResetDataContext();
            var dumpSettings = DataContext.WadLogsDumpSettings.First();

            Assert.That(dumpSettings.LatestDumpTime, Is.EqualTo(Now.AddMinutes(-30)).Within(1).Seconds);
        }

        [Test]
        public void Dump_should_not_set_latest_dump_time_in_future()
        {
            //arrange
            SetDumpSettings(x =>
            {
                x.DumpSizeInMinutes = 90;
                x.DumpOverlapInMinutes = 30;
                x.LatestDumpTime = Now.AddHours(-1);
            });

            //act
            RunDump(WadLogsReaderStub.Create());

            //assert
            ResetDataContext();
            var dumpSettings = DataContext.WadLogsDumpSettings.First();

            Assert.That(dumpSettings.LatestDumpTime, Is.EqualTo(Now).Within(1).Seconds);

        }

        [Test]
        public void CleanupStaleLogs_should_delete_logs_with_date_less_than_TTL()
        {
            //arrange
            SetDumpSettings(x => x.LogsTTLInDays = 2);

            var entry1 = WadLogEntryBuilder.New().WithEventDate(DateTime.UtcNow).Create();
            var entry2 = WadLogEntryBuilder.New().WithEventDate(DateTime.UtcNow.AddDays(-1)).Create();
            var entry3 = WadLogEntryBuilder.New().WithEventDate(DateTime.UtcNow.AddDays(-3)).Create(); // should be deleted
            var entry4 = WadLogEntryBuilder.New().WithEventDate(DateTime.UtcNow.AddDays(-4)).Create(); // should be deleted

            //act
            var service = new WadLogsService();
            service.CleanupStaleLogs();
            
            //assert
            ResetDataContext();
            var actualEntries = DataContext.WadLogEntries.Select(x => x.Id).ToArray();
            var expectedEntries = new[] { entry1.Id, entry2.Id };

            Assert.That(actualEntries, Is.EquivalentTo(expectedEntries), "should delete entry3 & entry4");
        }

        private void SetDumpSettings(Action<WadLogsDumpSettings> setters)
        {
            var dumpSettings = DataContext.WadLogsDumpSettings.First();
            setters(dumpSettings);
            DataContext.SaveChanges();
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