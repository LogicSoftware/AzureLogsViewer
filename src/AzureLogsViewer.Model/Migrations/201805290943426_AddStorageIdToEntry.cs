namespace AzureLogsViewer.Model.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddStorageIdToEntry : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WadLogEntries", "StorageId", c => c.Int(nullable: false, defaultValue: 1));
        }
        
        public override void Down()
        {
            DropColumn("dbo.WadLogEntries", "StorageId");
        }
    }
}
