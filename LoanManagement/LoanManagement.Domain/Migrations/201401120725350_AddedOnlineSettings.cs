namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedOnlineSettings : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OnlineSettings",
                c => new
                    {
                        OnlineSettingID = c.Int(nullable: false, identity: true),
                        HomeDescription = c.String(),
                        AboutDescription = c.String(),
                        MissionVision = c.String(),
                    })
                .PrimaryKey(t => t.OnlineSettingID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.OnlineSettings");
        }
    }
}
