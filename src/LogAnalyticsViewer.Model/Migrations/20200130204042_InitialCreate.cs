using Microsoft.EntityFrameworkCore.Migrations;

namespace LogAnalyticsViewer.Model.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Queries",
                columns: table => new
                {
                    QueryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DisplayName = table.Column<string>(type: "varchar(50)", nullable: false),
                    QueryText = table.Column<string>(nullable: false),
                    Enabled = table.Column<bool>(nullable: false),
                    Channel = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Queries", x => x.QueryId);
                });

            migrationBuilder.InsertData(
                table: "Queries",
                columns: new[] { "QueryId", "Channel", "DisplayName", "Enabled", "QueryText" },
                values: new object[] { 1, "#site-errors", "epcore", true, @"Event {0}
| where Source == ""Easy Projects"" 
| where EventLevel == 2 
| project TimeGenerated, Message = RenderedDescription, Source = ""epcore""" });

            migrationBuilder.InsertData(
                table: "Queries",
                columns: new[] { "QueryId", "Channel", "DisplayName", "Enabled", "QueryText" },
                values: new object[] { 2, "#site-errors", "microservices", true, @"production_services_CL {0} 
| where LogLevel_s == ""Error"" 
| project TimeGenerated, Message = strcat(LogMessage_s, LogException_s), Source = LogProperties_Application_s" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Queries");
        }
    }
}
