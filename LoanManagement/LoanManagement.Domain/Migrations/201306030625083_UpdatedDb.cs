namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Banks",
                c => new
                    {
                        BankID = c.Int(nullable: false, identity: true),
                        BankName = c.String(),
                        Description = c.String(),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.BankID);
            
            CreateTable(
                "dbo.BankAddresses",
                c => new
                    {
                        BankAddID = c.Int(nullable: false, identity: true),
                        Street = c.String(),
                        Province = c.String(),
                        City = c.String(),
                        BankID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BankAddID)
                .ForeignKey("dbo.Banks", t => t.BankID, cascadeDelete: true)
                .Index(t => t.BankID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.BankAddresses", new[] { "BankID" });
            DropForeignKey("dbo.BankAddresses", "BankID", "dbo.Banks");
            DropTable("dbo.BankAddresses");
            DropTable("dbo.Banks");
        }
    }
}
