namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedTempClearing : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.TempClearings", "ChequeInfo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TempClearings", "ChequeInfo", c => c.String());
        }
    }
}
