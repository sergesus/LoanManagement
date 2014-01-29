namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedCancelledLoan2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CancelledLoans",
                c => new
                    {
                        LoanID = c.Int(nullable: false),
                        DateClosed = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.LoanID)
                .ForeignKey("dbo.Loans", t => t.LoanID)
                .Index(t => t.LoanID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.CancelledLoans", new[] { "LoanID" });
            DropForeignKey("dbo.CancelledLoans", "LoanID", "dbo.Loans");
            DropTable("dbo.CancelledLoans");
        }
    }
}
