namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedRestructred : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RestructuredLoans",
                c => new
                    {
                        LoanID = c.Int(nullable: false),
                        NewLoanID = c.Int(nullable: false),
                        Fee = c.Double(nullable: false),
                        DateRestructured = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.LoanID)
                .ForeignKey("dbo.Loans", t => t.LoanID)
                .Index(t => t.LoanID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.RestructuredLoans", new[] { "LoanID" });
            DropForeignKey("dbo.RestructuredLoans", "LoanID", "dbo.Loans");
            DropTable("dbo.RestructuredLoans");
        }
    }
}
