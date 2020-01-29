﻿// <auto-generated />
using LogAnalyticsViewer.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LogAnalyticsViewer.Model.Migrations
{
    [DbContext(typeof(LAVDataContext))]
    partial class LAVDataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("LogAnalyticsViewer.Model.Entities.DumpSettings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DelayBetweenDumpsInMinutes")
                        .HasColumnType("int");

                    b.Property<int>("DumpOverlapInMinutes")
                        .HasColumnType("int");

                    b.Property<int>("DumpSizeInMinutes")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("DumpSettings");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            DelayBetweenDumpsInMinutes = 15,
                            DumpOverlapInMinutes = 5,
                            DumpSizeInMinutes = 30
                        });
                });

            modelBuilder.Entity("LogAnalyticsViewer.Model.Entities.LogAnalyticsSettings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClientId")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("ClientSecret")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Domain")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("WorkspaceId")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.ToTable("LogAnalyticsSettings");
                });

            modelBuilder.Entity("LogAnalyticsViewer.Model.Entities.Query", b =>
                {
                    b.Property<int>("QueryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

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
                            QueryText = @"Event {0}
| where Source == ""Easy Projects"" 
| where EventLevel == 2 
| project TimeGenerated, Message = RenderedDescription, Source = ""epcore"""
                        },
                        new
                        {
                            QueryId = 2,
                            Channel = "#site-errors",
                            DisplayName = "microservices",
                            Enabled = true,
                            QueryText = @"production_services_CL {0} 
| where LogLevel_s == ""Error"" 
| project TimeGenerated, Message = strcat(LogMessage_s, LogException_s), Source = LogProperties_Application_s"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
