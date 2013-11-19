namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedNullableForDates : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Clients", "Birthday", c => c.DateTime());
            AlterColumn("dbo.Dependents", "Birthday", c => c.DateTime());
            AlterColumn("dbo.Spouses", "Birthday", c => c.DateTime());
            AlterColumn("dbo.TempDependents", "Birthday", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TempDependents", "Birthday", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Spouses", "Birthday", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Dependents", "Birthday", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Clients", "Birthday", c => c.DateTime(nullable: false));
        }
    }
}
