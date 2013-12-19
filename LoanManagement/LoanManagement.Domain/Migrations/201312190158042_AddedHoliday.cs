namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedHoliday : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TemporaryLoanApplications",
                c => new
                    {
                        TemporaryLoanApplicationID = c.Int(nullable: false, identity: true),
                        AmountApplied = c.Double(nullable: false),
                        Term = c.Int(nullable: false),
                        Mode = c.String(),
                        DateApplied = c.DateTime(nullable: false),
                        ExpirationDate = c.DateTime(nullable: false),
                        ServiceID = c.Int(nullable: false),
                        ClientID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TemporaryLoanApplicationID);
            
            CreateTable(
                "dbo.Holidays",
                c => new
                    {
                        HolidayID = c.Int(nullable: false, identity: true),
                        HolidayName = c.String(),
                        Description = c.String(),
                        isYearly = c.Boolean(nullable: false),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.HolidayID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Holidays");
            DropTable("dbo.TemporaryLoanApplications");
        }
    }
}
