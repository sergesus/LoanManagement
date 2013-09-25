namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedServices : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Services", "LatePaymentPenalty", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Services", "LatePaymentPenalty");
        }
    }
}
