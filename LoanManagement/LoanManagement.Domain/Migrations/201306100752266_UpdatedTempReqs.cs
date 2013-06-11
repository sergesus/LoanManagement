namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedTempReqs : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TempoDeductions",
                c => new
                    {
                        DeductionId = c.Int(nullable: false, identity: true),
                        DeductionNum = c.Int(nullable: false),
                        Name = c.String(),
                        Percentage = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DeductionId);
            
            CreateTable(
                "dbo.TempoRequirements",
                c => new
                    {
                        RequirementId = c.Int(nullable: false, identity: true),
                        RequirementNum = c.Int(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.RequirementId);
            
            DropTable("dbo.TempDeductions");
            DropTable("dbo.TempRequirements");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TempRequirements",
                c => new
                    {
                        RequirementNum = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.RequirementNum);
            
            CreateTable(
                "dbo.TempDeductions",
                c => new
                    {
                        DeductionNum = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Percentage = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DeductionNum);
            
            DropTable("dbo.TempoRequirements");
            DropTable("dbo.TempoDeductions");
        }
    }
}
