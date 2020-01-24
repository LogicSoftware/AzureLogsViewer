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
                    WorkspaceId = table.Column<string>(nullable: true),
                    ClientId = table.Column<string>(nullable: true),
                    ClientSecret = table.Column<string>(nullable: true),
                    Domain = table.Column<string>(nullable: true)
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
