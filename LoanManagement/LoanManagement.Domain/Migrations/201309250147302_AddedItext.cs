namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedItext : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.iTexts",
                c => new
                    {
                        FPaymentInfoID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FPaymentInfoID)
                .ForeignKey("dbo.FPaymentInfoes", t => t.FPaymentInfoID)
                .Index(t => t.FPaymentInfoID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.iTexts", new[] { "FPaymentInfoID" });
            DropForeignKey("dbo.iTexts", "FPaymentInfoID", "dbo.FPaymentInfoes");
            DropTable("dbo.iTexts");
        }
    }
}
