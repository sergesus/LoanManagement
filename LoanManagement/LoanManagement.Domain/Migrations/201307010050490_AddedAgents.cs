namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedAgents : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Agents",
                c => new
                    {
                        AgentID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        MI = c.String(),
                        LastName = c.String(),
                        Suffix = c.String(),
                        Email = c.String(),
                        Photo = c.Binary(),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.AgentID);
            
            CreateTable(
                "dbo.AgentContacts",
                c => new
                    {
                        AgentContactID = c.Int(nullable: false, identity: true),
                        CNumber = c.Int(nullable: false),
                        Contact = c.String(),
                        AgentID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AgentContactID)
                .ForeignKey("dbo.Agents", t => t.AgentID, cascadeDelete: true)
                .Index(t => t.AgentID);
            
            CreateTable(
                "dbo.AgentAddresses",
                c => new
                    {
                        AgentAddressID = c.Int(nullable: false, identity: true),
                        AddressNumber = c.Int(nullable: false),
                        Street = c.String(),
                        Province = c.String(),
                        City = c.String(),
                        AgentID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AgentAddressID)
                .ForeignKey("dbo.Agents", t => t.AgentID, cascadeDelete: true)
                .Index(t => t.AgentID);
            
            CreateTable(
                "dbo.TempAgentAddresses",
                c => new
                    {
                        TempAgentAddressID = c.Int(nullable: false, identity: true),
                        AddressNumber = c.Int(nullable: false),
                        Street = c.String(),
                        Province = c.String(),
                        City = c.String(),
                    })
                .PrimaryKey(t => t.TempAgentAddressID);
            
            CreateTable(
                "dbo.TempAgentContacts",
                c => new
                    {
                        TempAgentContactID = c.Int(nullable: false, identity: true),
                        CNumber = c.Int(nullable: false),
                        Contact = c.String(),
                    })
                .PrimaryKey(t => t.TempAgentContactID);
            
            AddColumn("dbo.EmployeeAddresses", "Agent_AgentID", c => c.Int());
            AddColumn("dbo.EmployeeContacts", "Agent_AgentID", c => c.Int());
            AddForeignKey("dbo.EmployeeAddresses", "Agent_AgentID", "dbo.Agents", "AgentID");
            AddForeignKey("dbo.EmployeeContacts", "Agent_AgentID", "dbo.Agents", "AgentID");
            CreateIndex("dbo.EmployeeAddresses", "Agent_AgentID");
            CreateIndex("dbo.EmployeeContacts", "Agent_AgentID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.AgentAddresses", new[] { "AgentID" });
            DropIndex("dbo.AgentContacts", new[] { "AgentID" });
            DropIndex("dbo.EmployeeContacts", new[] { "Agent_AgentID" });
            DropIndex("dbo.EmployeeAddresses", new[] { "Agent_AgentID" });
            DropForeignKey("dbo.AgentAddresses", "AgentID", "dbo.Agents");
            DropForeignKey("dbo.AgentContacts", "AgentID", "dbo.Agents");
            DropForeignKey("dbo.EmployeeContacts", "Agent_AgentID", "dbo.Agents");
            DropForeignKey("dbo.EmployeeAddresses", "Agent_AgentID", "dbo.Agents");
            DropColumn("dbo.EmployeeContacts", "Agent_AgentID");
            DropColumn("dbo.EmployeeAddresses", "Agent_AgentID");
            DropTable("dbo.TempAgentContacts");
            DropTable("dbo.TempAgentAddresses");
            DropTable("dbo.AgentAddresses");
            DropTable("dbo.AgentContacts");
            DropTable("dbo.Agents");
        }
    }
}
