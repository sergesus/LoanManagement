namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedMPaymentInfo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MPaymentInfoes",
                c => new
                    {
                        MPaymentInfoID = c.Int(nullable: false, identity: true),
                        PaymentNumber = c.Int(nullable: false),
                        Amount = c.Double(nullable: false),
                        PreviousBalance = c.Double(nullable: false),
                        BalanceInterest = c.Double(nullable: false),
                        TotalBalance = c.Double(nullable: false),
                        TotalAmount = c.Double(nullable: false),
                        DueDate = c.DateTime(nullable: false),
                        PaymentDate = c.DateTime(),
                        RemainingLoanBalance = c.Double(nullable: false),
                        PaymentStatus = c.String(),
                        LoanID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MPaymentInfoID)
                .ForeignKey("dbo.Loans", t => t.LoanID, cascadeDelete: true)
                .Index(t => t.LoanID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.MPaymentInfoes", new[] { "LoanID" });
            DropForeignKey("dbo.MPaymentInfoes", "LoanID", "dbo.Loans");
            DropTable("dbo.MPaymentInfoes");
        }
    }
}
