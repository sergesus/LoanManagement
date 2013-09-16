namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedPaidLoan : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PaidLoans",
                c => new
                    {
                        LoanID = c.Int(nullable: false),
                        DateFinished = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.LoanID)
                .ForeignKey("dbo.Loans", t => t.LoanID)
                .Index(t => t.LoanID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.PaidLoans", new[] { "LoanID" });
            DropForeignKey("dbo.PaidLoans", "LoanID", "dbo.Loans");
            DropTable("dbo.PaidLoans");
        }
    }
}
