namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CollateralLoanInfo2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CollateralLoanInfoes", "LoanID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CollateralLoanInfoes", "LoanID");
        }
    }
}
