namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedToAmount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LoanApplications", "AmountApplied", c => c.Double(nullable: false));
            DropColumn("dbo.LoanApplications", "AmmountApplied");
        }
        
        public override void Down()
        {
            AddColumn("dbo.LoanApplications", "AmmountApplied", c => c.Double(nullable: false));
            DropColumn("dbo.LoanApplications", "AmountApplied");
        }
    }
}
