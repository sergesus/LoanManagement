namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Checker2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ApprovedLoans", "AmountApproved", c => c.Double());
            AlterColumn("dbo.ReleasedLoans", "Principal", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ReleasedLoans", "Principal", c => c.Double(nullable: false));
            AlterColumn("dbo.ApprovedLoans", "AmountApproved", c => c.Double(nullable: false));
        }
    }
}
