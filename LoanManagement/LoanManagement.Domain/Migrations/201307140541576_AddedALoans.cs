namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedALoans : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApprovedLoans",
                c => new
                    {
                        LoanID = c.Int(nullable: false),
                        AmountApproved = c.Double(nullable: false),
                        DateApproved = c.DateTime(nullable: false),
                        ReleaseDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.LoanID)
                .ForeignKey("dbo.Loans", t => t.LoanID)
                .Index(t => t.LoanID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.ApprovedLoans", new[] { "LoanID" });
            DropForeignKey("dbo.ApprovedLoans", "LoanID", "dbo.Loans");
            DropTable("dbo.ApprovedLoans");
        }
    }
}
