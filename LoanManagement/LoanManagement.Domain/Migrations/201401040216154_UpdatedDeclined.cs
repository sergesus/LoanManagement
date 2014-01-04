namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedDeclined : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.DeclinedLoans", "DateDeclined", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DeclinedLoans", "DateDeclined", c => c.DateTime(nullable: false));
        }
    }
}
