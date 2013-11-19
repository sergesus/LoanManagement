namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedPosScope : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Scopes", "PositionM", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Scopes", "PositionM");
        }
    }
}
