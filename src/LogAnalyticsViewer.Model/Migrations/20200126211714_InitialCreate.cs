using Microsoft.EntityFrameworkCore.Migrations;

namespace LogAnalyticsViewer.Model.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DumpSettings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DumpOverlapInMinutes = table.Column<int>(nullable: false),
                    DumpSizeInMinutes = table.Column<int>(nullable: false),
                    DelayBetweenDumpsInMinutes = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DumpSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LogAnalyticsSettings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkspaceId = table.Column<string>(type: "varchar(50)", nullable: false),
                    ClientId = table.Column<string>(type: "varchar(50)", nullable: false),
                    ClientSecret = table.Column<string>(type: "varchar(100)", nullable: false),
                    Domain = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogAnalyticsSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Queries",
                columns: table => new
                {
                    QueryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DisplayName = table.Column<string>(type: "varchar(50)", nullable: false),
                    QueryText = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Queries", x => x.QueryId);
                });

            migrationBuilder.InsertData(
                table: "DumpSettings",
                columns: new[] { "Id", "DelayBetweenDumpsInMinutes", "DumpOverlapInMinutes", "DumpSizeInMinutes" },
                values: new object[] { 1, 15, 5, 30 });

            migrationBuilder.InsertData(
                table: "Queries",
                columns: new[] { "QueryId", "DisplayName", "QueryText" },
                values: new object[] { 1, "epcore", @"Event {0}
| where Source == ""Easy Projects"" 
| where EventLevel == 2 
| project TimeGenerated, Message = RenderedDescription, Source = ""epcore""" });

            migrationBuilder.InsertData(
                table: "Queries",
                columns: new[] { "QueryId", "DisplayName", "QueryText" },
                values: new object[] { 2, "microservices", @"production_services_CL {0} 
| where LogLevel_s == ""Error"" 
| project TimeGenerated, Message = strcat(LogMessage_s, LogException_s), Source = LogProperties_Application_s" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DumpSettings");

            migrationBuilder.DropTable(
                name: "LogAnalyticsSettings");

            migrationBuilder.DropTable(
                name: "Queries");
        }
    }
}
