namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedFPaymentInfo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ReleasedLoans",
                c => new
                    {
                        LoanID = c.Int(nullable: false),
                        AmountReleased = c.Double(nullable: false),
                        DateReleased = c.DateTime(nullable: false),
                        Principal = c.Double(nullable: false),
                        NetProceed = c.Double(nullable: false),
                        TotalLoan = c.Double(nullable: false),
                        MonthlyPayment = c.Double(nullable: false),
                        AgentsCommission = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.LoanID)
                .ForeignKey("dbo.Loans", t => t.LoanID)
                .Index(t => t.LoanID);
            
            CreateTable(
                "dbo.FPaymentInfoes",
                c => new
                    {
                        FPaymentInfoID = c.Int(nullable: false, identity: true),
                        PaymentNumber = c.Int(nullable: false),
                        ChequeInfo = c.String(),
                        Amount = c.Double(nullable: false),
                        PaymentDate = c.DateTime(nullable: false),
                        RemainingBalance = c.String(),
                        LoanID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FPaymentInfoID)
                .ForeignKey("dbo.Loans", t => t.LoanID, cascadeDelete: true)
                .Index(t => t.LoanID);
            
            DropColumn("dbo.Loans", "Principal");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Loans", "Principal", c => c.Double(nullable: false));
            DropIndex("dbo.FPaymentInfoes", new[] { "LoanID" });
            DropIndex("dbo.ReleasedLoans", new[] { "LoanID" });
            DropForeignKey("dbo.FPaymentInfoes", "LoanID", "dbo.Loans");
            DropForeignKey("dbo.ReleasedLoans", "LoanID", "dbo.Loans");
            DropTable("dbo.FPaymentInfoes");
            DropTable("dbo.ReleasedLoans");
        }
    }
}
