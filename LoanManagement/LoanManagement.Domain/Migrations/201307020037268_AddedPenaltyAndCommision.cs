namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedPenaltyAndCommision : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Services", "AgentCommission", c => c.Double(nullable: false));
            AddColumn("dbo.Services", "Penalty", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Services", "Penalty");
            DropColumn("dbo.Services", "AgentCommission");
        }
    }
}
