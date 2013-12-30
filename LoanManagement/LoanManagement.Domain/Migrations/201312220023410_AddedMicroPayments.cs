namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedMicroPayments : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MicroPayments",
                c => new
                    {
                        MPaymentInfoID = c.Int(nullable: false),
                        Amount = c.Double(nullable: false),
                        DatePaid = c.DateTime(),
                    })
                .PrimaryKey(t => t.MPaymentInfoID)
                .ForeignKey("dbo.MPaymentInfoes", t => t.MPaymentInfoID)
                .Index(t => t.MPaymentInfoID);
            
            AddColumn("dbo.MPaymentInfoes", "ExcessBalance", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropIndex("dbo.MicroPayments", new[] { "MPaymentInfoID" });
            DropForeignKey("dbo.MicroPayments", "MPaymentInfoID", "dbo.MPaymentInfoes");
            DropColumn("dbo.MPaymentInfoes", "ExcessBalance");
            DropTable("dbo.MicroPayments");
        }
    }
}
