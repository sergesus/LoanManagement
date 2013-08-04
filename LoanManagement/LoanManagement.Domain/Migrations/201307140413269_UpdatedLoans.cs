namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedLoans : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Loans", "ServiceID", c => c.Int(nullable: false));
            AddForeignKey("dbo.Loans", "ServiceID", "dbo.Services", "ServiceID", cascadeDelete: true);
            CreateIndex("dbo.Loans", "ServiceID");
            DropColumn("dbo.Loans", "TypeOfLoan");
            DropColumn("dbo.Loans", "Type");
            DropColumn("dbo.Loans", "Interest");
            DropColumn("dbo.Loans", "Deduction");
            DropColumn("dbo.Loans", "Penalty");
            DropColumn("dbo.Loans", "Commission");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Loans", "Commission", c => c.Double(nullable: false));
            AddColumn("dbo.Loans", "Penalty", c => c.Double(nullable: false));
            AddColumn("dbo.Loans", "Deduction", c => c.Double(nullable: false));
            AddColumn("dbo.Loans", "Interest", c => c.Double(nullable: false));
            AddColumn("dbo.Loans", "Type", c => c.String());
            AddColumn("dbo.Loans", "TypeOfLoan", c => c.String());
            DropIndex("dbo.Loans", new[] { "ServiceID" });
            DropForeignKey("dbo.Loans", "ServiceID", "dbo.Services");
            DropColumn("dbo.Loans", "ServiceID");
        }
    }
}
