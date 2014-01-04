namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedReqChecklist2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RequirementChecklists", "EmployeeID", c => c.Int(nullable: false));
            AddForeignKey("dbo.RequirementChecklists", "EmployeeID", "dbo.Employees", "EmployeeID", cascadeDelete: true);
            CreateIndex("dbo.RequirementChecklists", "EmployeeID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.RequirementChecklists", new[] { "EmployeeID" });
            DropForeignKey("dbo.RequirementChecklists", "EmployeeID", "dbo.Employees");
            DropColumn("dbo.RequirementChecklists", "EmployeeID");
        }
    }
}
