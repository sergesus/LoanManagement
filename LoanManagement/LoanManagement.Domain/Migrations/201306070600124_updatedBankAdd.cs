namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedBankAdd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BankAddresses", "BankNum", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BankAddresses", "BankNum");
        }
    }
}
