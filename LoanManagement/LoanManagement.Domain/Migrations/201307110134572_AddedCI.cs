namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedCI : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Loans", "CI", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Loans", "CI");
        }
    }
}
