namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ClearedCheques", "DateCleared", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ClearedCheques", "DateCleared", c => c.DateTime(nullable: false));
        }
    }
}
