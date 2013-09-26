namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedAuditTrail : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AuditTrails",
                c => new
                    {
                        AuditTrailID = c.Int(nullable: false, identity: true),
                        EmployeeID = c.Int(nullable: false),
                        Action = c.String(),
                        DateAndTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.AuditTrailID)
                .ForeignKey("dbo.Employees", t => t.EmployeeID, cascadeDelete: true)
                .Index(t => t.EmployeeID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.AuditTrails", new[] { "EmployeeID" });
            DropForeignKey("dbo.AuditTrails", "EmployeeID", "dbo.Employees");
            DropTable("dbo.AuditTrails");
        }
    }
}
