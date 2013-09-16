namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedViewLoans : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ViewLoans",
                c => new
                    {
                        ViewLoanID = c.Int(nullable: false, identity: true),
                        PaymentNumber = c.Int(nullable: false),
                        PaymentInfo = c.String(),
                        TotalPayment = c.String(),
                        DueDate = c.String(),
                        PaymentDate = c.String(),
                        Status = c.String(),
                        DateCleared = c.String(),
                    })
                .PrimaryKey(t => t.ViewLoanID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ViewLoans");
        }
    }
}
