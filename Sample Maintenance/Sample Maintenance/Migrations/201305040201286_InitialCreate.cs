namespace Sample_Maintenance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        empId = c.String(nullable: false, maxLength: 128),
                        FName = c.String(),
                        MI = c.String(),
                        LName = c.String(),
                        Photo = c.Binary(),
                    })
                .PrimaryKey(t => t.empId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Employees");
        }
    }
}
