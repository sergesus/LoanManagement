namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Username = c.String(nullable: false, maxLength: 128),
                        Password = c.String(),
                        EmployeeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Username)
                .ForeignKey("dbo.Employees", t => t.EmployeeID, cascadeDelete: true)
                .Index(t => t.EmployeeID);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        EmployeeID = c.Int(nullable: false, identity: true),
                        FName = c.String(),
                        MI = c.String(),
                        LName = c.String(),
                        Address = c.String(),
                        Contact = c.String(),
                        Email = c.String(),
                        Photo = c.Binary(),
                    })
                .PrimaryKey(t => t.EmployeeID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Users", new[] { "EmployeeID" });
            DropForeignKey("dbo.Users", "EmployeeID", "dbo.Employees");
            DropTable("dbo.Employees");
            DropTable("dbo.Users");
        }
    }
}
