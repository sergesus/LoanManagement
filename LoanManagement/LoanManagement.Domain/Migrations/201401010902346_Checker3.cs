namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Checker3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ApprovedLoans", "DateApproved", c => c.DateTime());
            AlterColumn("dbo.ApprovedLoans", "ReleaseDate", c => c.DateTime());
            AlterColumn("dbo.ReleasedLoans", "NetProceed", c => c.Double());
            AlterColumn("dbo.ReleasedLoans", "TotalLoan", c => c.Double());
            AlterColumn("dbo.ReleasedLoans", "MonthlyPayment", c => c.Double());
            AlterColumn("dbo.ReleasedLoans", "AgentsCommission", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ReleasedLoans", "AgentsCommission", c => c.Double(nullable: false));
            AlterColumn("dbo.ReleasedLoans", "MonthlyPayment", c => c.Double(nullable: false));
            AlterColumn("dbo.ReleasedLoans", "TotalLoan", c => c.Double(nullable: false));
            AlterColumn("dbo.ReleasedLoans", "NetProceed", c => c.Double(nullable: false));
            AlterColumn("dbo.ApprovedLoans", "ReleaseDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ApprovedLoans", "DateApproved", c => c.DateTime(nullable: false));
        }
    }
}
