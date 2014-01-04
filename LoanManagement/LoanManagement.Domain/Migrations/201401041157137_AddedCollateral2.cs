namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedCollateral2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TempCollateralInformations",
                c => new
                    {
                        TempCollateralInformationID = c.Int(nullable: false, identity: true),
                        TempCollateralInformationNum = c.Int(nullable: false),
                        Field = c.String(),
                    })
                .PrimaryKey(t => t.TempCollateralInformationID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TempCollateralInformations");
        }
    }
}
