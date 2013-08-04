namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedDLoans : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DeclinedLoans",
                c => new
                    {
                        LoanID = c.Int(nullable: false),
                        Reason = c.String(),
                        DateDeclined = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.LoanID)
                .ForeignKey("dbo.Loans", t => t.LoanID)
                .Index(t => t.LoanID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.DeclinedLoans", new[] { "LoanID" });
            DropForeignKey("dbo.DeclinedLoans", "LoanID", "dbo.Loans");
            DropTable("dbo.DeclinedLoans");
        }
    }
}
