namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedPaymentInfo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FPaymentInfoes", "PaymentStatus", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FPaymentInfoes", "PaymentStatus");
        }
    }
}
