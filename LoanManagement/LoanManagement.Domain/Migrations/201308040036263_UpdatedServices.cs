namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedServices : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Services", "Holding", c => c.Double(nullable: false));
            AddColumn("dbo.Services", "DaifPenalty", c => c.Double(nullable: false));
            AddColumn("dbo.Services", "ClosedAccountPenalty", c => c.Double(nullable: false));
            DropColumn("dbo.Services", "Penalty");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Services", "Penalty", c => c.Double(nullable: false));
            DropColumn("dbo.Services", "ClosedAccountPenalty");
            DropColumn("dbo.Services", "DaifPenalty");
            DropColumn("dbo.Services", "Holding");
        }
    }
}
