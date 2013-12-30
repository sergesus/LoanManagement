namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedMPayment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MPaymentInfoes", "TotalPayment", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MPaymentInfoes", "TotalPayment");
        }
    }
}
