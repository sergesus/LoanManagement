namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedGenSOA : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GenSOAs",
                c => new
                    {
                        GenSOAID = c.Int(nullable: false, identity: true),
                        PaymentNumber = c.Int(nullable: false),
                        Amount = c.String(),
                        PaymentDate = c.DateTime(nullable: false),
                        RemainingBalance = c.String(),
                    })
                .PrimaryKey(t => t.GenSOAID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.GenSOAs");
        }
    }
}
