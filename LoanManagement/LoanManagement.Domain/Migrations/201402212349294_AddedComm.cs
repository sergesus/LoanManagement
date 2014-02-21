namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedComm : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AgentCommissions",
                c => new
                    {
                        AgentCommissionID = c.Int(nullable: false, identity: true),
                        AgentName = c.String(),
                        NoOfAccounts = c.Int(nullable: false),
                        TotalRelease = c.Double(nullable: false),
                        TotalCommission = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.AgentCommissionID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AgentCommissions");
        }
    }
}
