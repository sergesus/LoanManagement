namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Checker21 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PositionScopes", "TRenewal", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PositionScopes", "TRenewal");
        }
    }
}
