namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedCollector : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PassedToCollectors", "TotalPaidBeforePassing", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PassedToCollectors", "TotalPaidBeforePassing");
        }
    }
}
