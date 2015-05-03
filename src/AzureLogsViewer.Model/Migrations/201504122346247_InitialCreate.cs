namespace AzureLogsViewer.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WadLogEntries",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        PartitionKey = c.String(),
                        RowKey = c.String(),
                        Role = c.String(),
                        RoleInstance = c.String(),
                        Level = c.Int(nullable: false),
                        Message = c.String(),
                        Pid = c.Int(nullable: false),
                        Tid = c.Int(nullable: false),
                        EventId = c.Int(nullable: false),
                        EventDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WadLogsDumpSettings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LatestDumpTime = c.DateTime(),
                        DumpOverlapInMinutes = c.Int(nullable: false),
                        DumpSizeInMinutes = c.Int(nullable: false),
                        DelayBetweenDumpsInMinutes = c.Int(nullable: false),
                        LogsTTLInDays = c.Int(nullable: false),
                        StorageConnectionString = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.WadLogsDumpSettings");
            DropTable("dbo.WadLogEntries");
        }
    }
}
