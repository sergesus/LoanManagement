namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tester : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ApprovedLoans", "AmountApproved", c => c.Double(nullable: false));
            AlterColumn("dbo.ReleasedLoans", "Principal", c => c.Double(nullable: false));
            AlterColumn("dbo.ReleasedLoans", "NetProceed", c => c.Double(nullable: false));
            AlterColumn("dbo.ReleasedLoans", "TotalLoan", c => c.Double(nullable: false));
            AlterColumn("dbo.ReleasedLoans", "MonthlyPayment", c => c.Double(nullable: false));
            AlterColumn("dbo.ReleasedLoans", "AgentsCommission", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ReleasedLoans", "AgentsCommission", c => c.Double());
            AlterColumn("dbo.ReleasedLoans", "MonthlyPayment", c => c.Double());
            AlterColumn("dbo.ReleasedLoans", "TotalLoan", c => c.Double());
            AlterColumn("dbo.ReleasedLoans", "NetProceed", c => c.Double());
            AlterColumn("dbo.ReleasedLoans", "Principal", c => c.Double());
            AlterColumn("dbo.ApprovedLoans", "AmountApproved", c => c.Double());
        }
    }
}
