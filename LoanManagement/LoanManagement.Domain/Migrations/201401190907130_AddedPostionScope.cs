namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedPostionScope : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PositionScopes",
                c => new
                    {
                        PositionID = c.Int(nullable: false),
                        MClient = c.Boolean(nullable: false),
                        MService = c.Boolean(nullable: false),
                        MAgent = c.Boolean(nullable: false),
                        MBank = c.Boolean(nullable: false),
                        MEmployee = c.Boolean(nullable: false),
                        MPosition = c.Boolean(nullable: false),
                        MHoliday = c.Boolean(nullable: false),
                        MRegistration = c.Boolean(nullable: false),
                        TApplication = c.Boolean(nullable: false),
                        TApproval = c.Boolean(nullable: false),
                        TReleasing = c.Boolean(nullable: false),
                        TPayments = c.Boolean(nullable: false),
                        TManageClosed = c.Boolean(nullable: false),
                        TResturcture = c.Boolean(nullable: false),
                        TPaymentAdjustment = c.Boolean(nullable: false),
                        TOnlineConfirmation = c.Boolean(nullable: false),
                        TCollection = c.Boolean(nullable: false),
                        UArchive = c.Boolean(nullable: false),
                        UBackUp = c.Boolean(nullable: false),
                        UUserAccounts = c.Boolean(nullable: false),
                        UReports = c.Boolean(nullable: false),
                        UStatistics = c.Boolean(nullable: false),
                        UScopes = c.Boolean(nullable: false),
                        UOnlineSettings = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PositionID)
                .ForeignKey("dbo.Positions", t => t.PositionID)
                .Index(t => t.PositionID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.PositionScopes", new[] { "PositionID" });
            DropForeignKey("dbo.PositionScopes", "PositionID", "dbo.Positions");
            DropTable("dbo.PositionScopes");
        }
    }
}
