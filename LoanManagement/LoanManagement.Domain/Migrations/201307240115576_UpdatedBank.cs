namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedBank : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Loans", "BankID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Loans", "BankID");
        }
    }
}
