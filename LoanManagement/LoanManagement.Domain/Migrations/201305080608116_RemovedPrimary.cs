namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedPrimary : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.EmployeeAddresses", "Primary");
            DropColumn("dbo.EmployeeContacts", "Primary");
        }
        
        public override void Down()
        {
            AddColumn("dbo.EmployeeContacts", "Primary", c => c.Boolean(nullable: false));
            AddColumn("dbo.EmployeeAddresses", "Primary", c => c.Boolean(nullable: false));
        }
    }
}
