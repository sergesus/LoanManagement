namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedReqChecklist : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RequirementChecklists",
                c => new
                    {
                        RequirementChecklistID = c.Int(nullable: false, identity: true),
                        RequirementId = c.Int(nullable: false),
                        LoanID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RequirementChecklistID)
                .ForeignKey("dbo.Requirements", t => t.RequirementId, cascadeDelete: true)
                .Index(t => t.RequirementId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.RequirementChecklists", new[] { "RequirementId" });
            DropForeignKey("dbo.RequirementChecklists", "RequirementId", "dbo.Requirements");
            DropTable("dbo.RequirementChecklists");
        }
    }
}
