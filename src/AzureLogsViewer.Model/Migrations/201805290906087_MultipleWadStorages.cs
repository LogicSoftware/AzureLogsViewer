namespace AzureLogsViewer.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MultipleWadStorages : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WadLogsStorageSettings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LatestDumpTime = c.DateTime(),
                        StorageConnectionString = c.String(),
                        WadLogsDumpSettingId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.WadLogsDumpSettings", t => t.WadLogsDumpSettingId, cascadeDelete: true)
                .Index(t => t.WadLogsDumpSettingId);

            Sql(@"
                INSERT INTO WadLogsStorageSettings(LatestDumpTime, StorageConnectionString, WadLogsDumpSettingId)
                SELECT s.LatestDumpTime, s.StorageConnectionString, s.Id FROM WadLogsDumpSettings as s
            ");
            
            DropColumn("dbo.WadLogsDumpSettings", "LatestDumpTime");
            DropColumn("dbo.WadLogsDumpSettings", "StorageConnectionString");
        }
        
        public override void Down()
        {
            AddColumn("dbo.WadLogsDumpSettings", "StorageConnectionString", c => c.String());
            AddColumn("dbo.WadLogsDumpSettings", "LatestDumpTime", c => c.DateTime());
            DropForeignKey("dbo.WadLogsStorageSettings", "WadLogsDumpSettingId", "dbo.WadLogsDumpSettings");
            DropIndex("dbo.WadLogsStorageSettings", new[] { "WadLogsDumpSettingId" });
            DropTable("dbo.WadLogsStorageSettings");
        }
    }
}
