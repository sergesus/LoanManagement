namespace Sample_Maintenance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAddressEmployee : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "Address", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employees", "Address");
        }
    }
}
