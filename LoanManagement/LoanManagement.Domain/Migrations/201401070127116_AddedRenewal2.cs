namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedRenewal2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LoanRenewals", "Status", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.LoanRenewals", "Status");
        }
    }
}
