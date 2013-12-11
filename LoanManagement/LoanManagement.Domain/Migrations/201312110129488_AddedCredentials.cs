namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedCredentials : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clients", "Username", c => c.String());
            AddColumn("dbo.Clients", "Password", c => c.String());
            AddColumn("dbo.Clients", "isConfirmed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Clients", "isConfirmed");
            DropColumn("dbo.Clients", "Password");
            DropColumn("dbo.Clients", "Username");
        }
    }
}
