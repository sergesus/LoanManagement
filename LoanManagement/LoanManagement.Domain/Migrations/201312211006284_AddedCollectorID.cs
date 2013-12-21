namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedCollectorID : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Loans", "CollectortID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Loans", "CollectortID");
        }
    }
}
