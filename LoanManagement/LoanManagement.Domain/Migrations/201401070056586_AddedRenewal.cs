namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedRenewal : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LoanRenewals",
                c => new
                    {
                        LoanRenewalID = c.Int(nullable: false, identity: true),
                        LoanID = c.Int(nullable: false),
                        newLoanID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.LoanRenewalID)
                .ForeignKey("dbo.Loans", t => t.LoanID, cascadeDelete: true)
                .Index(t => t.LoanID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.LoanRenewals", new[] { "LoanID" });
            DropForeignKey("dbo.LoanRenewals", "LoanID", "dbo.Loans");
            DropTable("dbo.LoanRenewals");
        }
    }
}
