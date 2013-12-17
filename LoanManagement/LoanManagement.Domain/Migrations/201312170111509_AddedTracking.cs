namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedTracking : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clients", "TrackingNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Clients", "TrackingNumber");
        }
    }
}
