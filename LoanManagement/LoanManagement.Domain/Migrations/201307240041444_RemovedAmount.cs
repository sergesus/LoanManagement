namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedAmount : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ReleasedLoans", "AmountReleased");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ReleasedLoans", "AmountReleased", c => c.Double(nullable: false));
        }
    }
}
