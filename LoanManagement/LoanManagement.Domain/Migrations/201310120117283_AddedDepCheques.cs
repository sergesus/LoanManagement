namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedDepCheques : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DepositedCheques", "FPaymentInfoID", c => c.Int(nullable: false));
            DropPrimaryKey("dbo.DepositedCheques", new[] { "DepositedChequeID" });
            AddPrimaryKey("dbo.DepositedCheques", "FPaymentInfoID");
            AddForeignKey("dbo.DepositedCheques", "FPaymentInfoID", "dbo.FPaymentInfoes", "FPaymentInfoID");
            CreateIndex("dbo.DepositedCheques", "FPaymentInfoID");
            DropColumn("dbo.DepositedCheques", "DepositedChequeID");
            DropColumn("dbo.DepositedCheques", "LoanID");
            DropColumn("dbo.DepositedCheques", "PaymentNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DepositedCheques", "PaymentNumber", c => c.Int(nullable: false));
            AddColumn("dbo.DepositedCheques", "LoanID", c => c.Int(nullable: false));
            AddColumn("dbo.DepositedCheques", "DepositedChequeID", c => c.Int(nullable: false, identity: true));
            DropIndex("dbo.DepositedCheques", new[] { "FPaymentInfoID" });
            DropForeignKey("dbo.DepositedCheques", "FPaymentInfoID", "dbo.FPaymentInfoes");
            DropPrimaryKey("dbo.DepositedCheques", new[] { "FPaymentInfoID" });
            AddPrimaryKey("dbo.DepositedCheques", "DepositedChequeID");
            DropColumn("dbo.DepositedCheques", "FPaymentInfoID");
        }
    }
}
