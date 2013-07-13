namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedOneToOne : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.LoanApplications", "LoanID", "dbo.Loans");
            DropIndex("dbo.LoanApplications", new[] { "LoanID" });
            DropPrimaryKey("dbo.LoanApplications", new[] { "LoanApplicationID" });
            AddPrimaryKey("dbo.LoanApplications", "LoanID");
            AddForeignKey("dbo.LoanApplications", "LoanID", "dbo.Loans", "LoanID");
            CreateIndex("dbo.LoanApplications", "LoanID");
            DropColumn("dbo.LoanApplications", "LoanApplicationID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.LoanApplications", "LoanApplicationID", c => c.Int(nullable: false, identity: true));
            DropIndex("dbo.LoanApplications", new[] { "LoanID" });
            DropForeignKey("dbo.LoanApplications", "LoanID", "dbo.Loans");
            DropPrimaryKey("dbo.LoanApplications", new[] { "LoanID" });
            AddPrimaryKey("dbo.LoanApplications", "LoanApplicationID");
            CreateIndex("dbo.LoanApplications", "LoanID");
            AddForeignKey("dbo.LoanApplications", "LoanID", "dbo.Loans", "LoanID", cascadeDelete: true);
        }
    }
}
