namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedTempClearing : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TempClearings",
                c => new
                    {
                        TempClearingID = c.Int(nullable: false, identity: true),
                        FPaymentInfoID = c.Int(nullable: false),
                        ChequeInfo = c.String(),
                    })
                .PrimaryKey(t => t.TempClearingID)
                .ForeignKey("dbo.FPaymentInfoes", t => t.FPaymentInfoID, cascadeDelete: true)
                .Index(t => t.FPaymentInfoID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.TempClearings", new[] { "FPaymentInfoID" });
            DropForeignKey("dbo.TempClearings", "FPaymentInfoID", "dbo.FPaymentInfoes");
            DropTable("dbo.TempClearings");
        }
    }
}
