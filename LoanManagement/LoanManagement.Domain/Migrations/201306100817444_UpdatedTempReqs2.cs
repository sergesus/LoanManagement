namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedTempReqs2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TempoDeductions", "Percentage", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TempoDeductions", "Percentage", c => c.Int(nullable: false));
        }
    }
}
