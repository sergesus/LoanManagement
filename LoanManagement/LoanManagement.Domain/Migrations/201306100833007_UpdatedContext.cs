namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedContext : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Services",
                c => new
                    {
                        ServiceID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Department = c.String(),
                        Type = c.String(),
                        MinTerm = c.Int(nullable: false),
                        MaxTerm = c.Int(nullable: false),
                        MinValue = c.Double(nullable: false),
                        MaxValue = c.Double(nullable: false),
                        Interest = c.Double(nullable: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ServiceID);
            
            CreateTable(
                "dbo.Requirements",
                c => new
                    {
                        RequirementId = c.Int(nullable: false, identity: true),
                        RequirementNum = c.Int(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        ServiceID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RequirementId)
                .ForeignKey("dbo.Services", t => t.ServiceID, cascadeDelete: true)
                .Index(t => t.ServiceID);
            
            CreateTable(
                "dbo.Deductions",
                c => new
                    {
                        DeductionId = c.Int(nullable: false, identity: true),
                        DeductionNum = c.Int(nullable: false),
                        Name = c.String(),
                        Percentage = c.Double(nullable: false),
                        ServiceID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DeductionId)
                .ForeignKey("dbo.Services", t => t.ServiceID, cascadeDelete: true)
                .Index(t => t.ServiceID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Deductions", new[] { "ServiceID" });
            DropIndex("dbo.Requirements", new[] { "ServiceID" });
            DropForeignKey("dbo.Deductions", "ServiceID", "dbo.Services");
            DropForeignKey("dbo.Requirements", "ServiceID", "dbo.Services");
            DropTable("dbo.Deductions");
            DropTable("dbo.Requirements");
            DropTable("dbo.Services");
        }
    }
}
