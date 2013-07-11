namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedAgent : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Loans", "AgentID", "dbo.Agents");
            DropIndex("dbo.Loans", new[] { "AgentID" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.Loans", "AgentID");
            AddForeignKey("dbo.Loans", "AgentID", "dbo.Agents", "AgentID", cascadeDelete: true);
        }
    }
}
