namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedRemaining : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.FPaymentInfoes", "RemainingBalance", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.FPaymentInfoes", "RemainingBalance", c => c.String());
        }
    }
}
