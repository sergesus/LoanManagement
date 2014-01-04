namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedCollateral : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CollateralInformations",
                c => new
                    {
                        CollateralInformationID = c.Int(nullable: false, identity: true),
                        CollateralInformationNum = c.Int(nullable: false),
                        Field = c.String(),
                        ServiceID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CollateralInformationID)
                .ForeignKey("dbo.Services", t => t.ServiceID, cascadeDelete: true)
                .Index(t => t.ServiceID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.CollateralInformations", new[] { "ServiceID" });
            DropForeignKey("dbo.CollateralInformations", "ServiceID", "dbo.Services");
            DropTable("dbo.CollateralInformations");
        }
    }
}
