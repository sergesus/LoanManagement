namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedPaymentAdjustment : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AdjustedLoans",
                c => new
                    {
                        LoanID = c.Int(nullable: false),
                        Days = c.Int(nullable: false),
                        Fee = c.Double(nullable: false),
                        DateAdjusted = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.LoanID)
                .ForeignKey("dbo.Loans", t => t.LoanID)
                .Index(t => t.LoanID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.AdjustedLoans", new[] { "LoanID" });
            DropForeignKey("dbo.AdjustedLoans", "LoanID", "dbo.Loans");
            DropTable("dbo.AdjustedLoans");
        }
    }
}
