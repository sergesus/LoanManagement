namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedVisitorCOunter : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OnlineSettings", "Visitor", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OnlineSettings", "Visitor");
        }
    }
}
