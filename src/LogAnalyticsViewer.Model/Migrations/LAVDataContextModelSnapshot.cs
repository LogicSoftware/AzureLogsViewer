﻿// <auto-generated />
using System;
using LogAnalyticsViewer.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LogAnalyticsViewer.Model.Migrations
{
    [DbContext(typeof(LAVDataContext))]
    partial class LAVDataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("LogAnalyticsViewer.Model.Entities.Event", b =>
                {
                    b.Property<int>("EventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EventId"), 1L, 1);

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("QueryId")
                        .HasColumnType("int");

                    b.Property<string>("Source")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("TimeGenerated")
                        .HasColumnType("datetime2");

                    b.HasKey("EventId");

                    b.HasIndex("QueryId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("LogAnalyticsViewer.Model.Entities.Query", b =>
                {
                    b.Property<int>("QueryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("QueryId"), 1L, 1);

                    b.Property<string>("Channel")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<bool>("Enabled")
                        .HasColumnType("bit");

                    b.Property<string>("QueryText")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("QueryId");

                    b.ToTable("Queries");

                    b.HasData(
                        new
                        {
                            QueryId = 1,
                            Channel = "#site-errors",
                            DisplayName = "epcore",
                            Enabled = true,
                            QueryText = "Event {TimeFilter}\r\n| where Source == \"Easy Projects\" \r\n| where EventLevel == 2 \r\n| project TimeGenerated, Message = RenderedDescription, Source = \"epcore\""
                        },
                        new
                        {
                            QueryId = 2,
                            Channel = "#site-errors",
                            DisplayName = "microservices",
                            Enabled = true,
                            QueryText = "production_services_CL {TimeFilter}\r\n| where LogLevel_s == \"Error\" \r\n| project TimeGenerated, Message = strcat(LogMessage_s, LogException_s), Source = LogProperties_Application_s"
                        });
                });

            modelBuilder.Entity("LogAnalyticsViewer.Model.Entities.Event", b =>
                {
                    b.HasOne("LogAnalyticsViewer.Model.Entities.Query", "Query")
                        .WithMany("Events")
                        .HasForeignKey("QueryId");

                    b.Navigation("Query");
                });

            modelBuilder.Entity("LogAnalyticsViewer.Model.Entities.Query", b =>
                {
                    b.Navigation("Events");
                });
#pragma warning restore 612, 618
        }
    }
}
