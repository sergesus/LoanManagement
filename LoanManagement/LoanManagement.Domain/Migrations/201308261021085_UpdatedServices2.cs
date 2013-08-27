namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedServices2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Services", "AdjustmentFee", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Services", "AdjustmentFee");
        }
    }
}
