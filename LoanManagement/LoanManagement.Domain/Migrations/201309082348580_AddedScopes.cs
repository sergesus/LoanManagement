namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedScopes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Scopes",
                c => new
                    {
                        EmployeeID = c.Int(nullable: false),
                        ClientM = c.Boolean(nullable: false),
                        ServiceM = c.Boolean(nullable: false),
                        AgentM = c.Boolean(nullable: false),
                        BankM = c.Boolean(nullable: false),
                        EmployeeM = c.Boolean(nullable: false),
                        Application = c.Boolean(nullable: false),
                        Approval = c.Boolean(nullable: false),
                        Releasing = c.Boolean(nullable: false),
                        Payments = c.Boolean(nullable: false),
                        ManageCLosed = c.Boolean(nullable: false),
                        Resturcture = c.Boolean(nullable: false),
                        PaymentAdjustment = c.Boolean(nullable: false),
                        Archive = c.Boolean(nullable: false),
                        BackUp = c.Boolean(nullable: false),
                        UserAccounts = c.Boolean(nullable: false),
                        Reports = c.Boolean(nullable: false),
                        Statistics = c.Boolean(nullable: false),
                        Scopes = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.EmployeeID)
                .ForeignKey("dbo.Employees", t => t.EmployeeID)
                .Index(t => t.EmployeeID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Scopes", new[] { "EmployeeID" });
            DropForeignKey("dbo.Scopes", "EmployeeID", "dbo.Employees");
            DropTable("dbo.Scopes");
        }
    }
}
