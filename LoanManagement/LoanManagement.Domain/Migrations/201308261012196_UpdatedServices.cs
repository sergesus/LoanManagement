namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedServices : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Services", "RestructureFee", c => c.Double(nullable: false));
            AddColumn("dbo.Services", "RestructureInterest", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Services", "RestructureInterest");
            DropColumn("dbo.Services", "RestructureFee");
        }
    }
}
