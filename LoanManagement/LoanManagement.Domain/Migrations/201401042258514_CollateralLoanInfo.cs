namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CollateralLoanInfo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CollateralLoanInfoes",
                c => new
                    {
                        CollateralLoanInfoID = c.Int(nullable: false, identity: true),
                        CollateralInformationID = c.Int(nullable: false),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.CollateralLoanInfoID)
                .ForeignKey("dbo.CollateralInformations", t => t.CollateralInformationID, cascadeDelete: true)
                .Index(t => t.CollateralInformationID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.CollateralLoanInfoes", new[] { "CollateralInformationID" });
            DropForeignKey("dbo.CollateralLoanInfoes", "CollateralInformationID", "dbo.CollateralInformations");
            DropTable("dbo.CollateralLoanInfoes");
        }
    }
}
