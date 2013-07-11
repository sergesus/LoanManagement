namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedLoanAndLoanApplication : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Loans",
                c => new
                    {
                        LoanID = c.Int(nullable: false, identity: true),
                        TypeOfLoan = c.String(),
                        Type = c.String(),
                        Mode = c.String(),
                        Interest = c.Double(nullable: false),
                        Deduction = c.Double(nullable: false),
                        Penalty = c.Double(nullable: false),
                        Commission = c.Double(nullable: false),
                        Term = c.Int(nullable: false),
                        Principal = c.Double(nullable: false),
                        CoBorrower = c.Int(nullable: false),
                        Status = c.String(),
                        ClientID = c.Int(nullable: false),
                        AgentID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.LoanID)
                .ForeignKey("dbo.Clients", t => t.ClientID, cascadeDelete: true)
                .ForeignKey("dbo.Agents", t => t.AgentID, cascadeDelete: true)
                .Index(t => t.ClientID)
                .Index(t => t.AgentID);
            
            CreateTable(
                "dbo.LoanApplications",
                c => new
                    {
                        LoanApplicationID = c.Int(nullable: false, identity: true),
                        AmmountApplied = c.Double(nullable: false),
                        DateApplied = c.DateTime(nullable: false),
                        LoanID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.LoanApplicationID)
                .ForeignKey("dbo.Loans", t => t.LoanID, cascadeDelete: true)
                .Index(t => t.LoanID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.LoanApplications", new[] { "LoanID" });
            DropIndex("dbo.Loans", new[] { "AgentID" });
            DropIndex("dbo.Loans", new[] { "ClientID" });
            DropForeignKey("dbo.LoanApplications", "LoanID", "dbo.Loans");
            DropForeignKey("dbo.Loans", "AgentID", "dbo.Agents");
            DropForeignKey("dbo.Loans", "ClientID", "dbo.Clients");
            DropTable("dbo.LoanApplications");
            DropTable("dbo.Loans");
        }
    }
}
