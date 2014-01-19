namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Checke3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OnlineSettings", "ContactInfo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.OnlineSettings", "ContactInfo");
        }
    }
}
