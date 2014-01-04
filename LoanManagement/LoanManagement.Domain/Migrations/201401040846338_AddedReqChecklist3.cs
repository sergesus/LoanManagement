namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedReqChecklist3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RequirementChecklists", "DateConfirmed", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RequirementChecklists", "DateConfirmed");
        }
    }
}
