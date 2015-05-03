namespace AzureLogsViewer.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SlackIntegration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SlackIntegrationInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Enabled = c.Boolean(nullable: false),
                        WebHookUrl = c.String(),
                        Chanel = c.String(),
                        SerializedFilter = c.String(),
                        MessagePattern = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SlackMessages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        Chanel = c.String(),
                        WebHookUrl = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SlackMessages");
            DropTable("dbo.SlackIntegrationInfoes");
        }
    }
}
