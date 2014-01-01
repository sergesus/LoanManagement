namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedTempLoans : DbMigration
    {
        public override void Up()
        {
            AddForeignKey("dbo.TemporaryLoanApplications", "ClientID", "dbo.Clients", "ClientID", cascadeDelete: true);
            AddForeignKey("dbo.TemporaryLoanApplications", "ServiceID", "dbo.Services", "ServiceID", cascadeDelete: true);
            CreateIndex("dbo.TemporaryLoanApplications", "ClientID");
            CreateIndex("dbo.TemporaryLoanApplications", "ServiceID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.TemporaryLoanApplications", new[] { "ServiceID" });
            DropIndex("dbo.TemporaryLoanApplications", new[] { "ClientID" });
            DropForeignKey("dbo.TemporaryLoanApplications", "ServiceID", "dbo.Services");
            DropForeignKey("dbo.TemporaryLoanApplications", "ClientID", "dbo.Clients");
        }
    }
}
