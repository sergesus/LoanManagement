namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedPhotoOnClient : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clients", "Photo", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Clients", "Photo");
        }
    }
}
