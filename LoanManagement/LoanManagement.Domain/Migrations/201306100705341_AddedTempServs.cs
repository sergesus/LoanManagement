namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedTempServs : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TempDeductions",
                c => new
                    {
                        DeductionNum = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Percentage = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DeductionNum);
            
            CreateTable(
                "dbo.TempRequirements",
                c => new
                    {
                        RequirementNum = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.RequirementNum);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TempRequirements");
            DropTable("dbo.TempDeductions");
        }
    }
}
