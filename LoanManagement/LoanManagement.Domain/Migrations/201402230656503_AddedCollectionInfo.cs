namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedCollectionInfo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CollectionInfoes",
                c => new
                    {
                        CollectionInfoID = c.Int(nullable: false, identity: true),
                        TotalCollection = c.Double(nullable: false),
                        DateCollected = c.DateTime(nullable: false),
                        LoanID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CollectionInfoID)
                .ForeignKey("dbo.Loans", t => t.LoanID, cascadeDelete: true)
                .Index(t => t.LoanID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.CollectionInfoes", new[] { "LoanID" });
            DropForeignKey("dbo.CollectionInfoes", "LoanID", "dbo.Loans");
            DropTable("dbo.CollectionInfoes");
        }
    }
}
