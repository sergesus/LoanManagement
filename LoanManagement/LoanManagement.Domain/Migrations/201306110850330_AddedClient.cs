namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedClient : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        ClientID = c.Int(nullable: false, identity: true),
                        LastName = c.String(),
                        FirstName = c.String(),
                        MiddleName = c.String(),
                        Suffix = c.String(),
                        Birthday = c.DateTime(nullable: false),
                        Sex = c.String(),
                        Status = c.String(),
                        SSS = c.String(),
                        TIN = c.String(),
                        Email = c.String(),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ClientID);
            
            CreateTable(
                "dbo.HomeAddresses",
                c => new
                    {
                        AddressID = c.Int(nullable: false, identity: true),
                        AddressNumber = c.Int(nullable: false),
                        Street = c.String(),
                        Province = c.String(),
                        City = c.String(),
                        OwnershipType = c.String(),
                        MonthlyFee = c.Double(nullable: false),
                        LengthOfStay = c.String(),
                        ClientID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AddressID)
                .ForeignKey("dbo.Clients", t => t.ClientID, cascadeDelete: true)
                .Index(t => t.ClientID);
            
            CreateTable(
                "dbo.ClientContacts",
                c => new
                    {
                        ContactID = c.Int(nullable: false, identity: true),
                        ContactNumber = c.Int(nullable: false),
                        Contact = c.String(),
                        Primary = c.Boolean(nullable: false),
                        ClientID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ContactID)
                .ForeignKey("dbo.Clients", t => t.ClientID, cascadeDelete: true)
                .Index(t => t.ClientID);
            
            CreateTable(
                "dbo.Dependents",
                c => new
                    {
                        DependentID = c.Int(nullable: false, identity: true),
                        DependentNumber = c.Int(nullable: false),
                        LastName = c.String(),
                        FirstName = c.String(),
                        MiddleName = c.String(),
                        Suffix = c.String(),
                        Birthday = c.DateTime(nullable: false),
                        School = c.String(),
                        ClientID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DependentID)
                .ForeignKey("dbo.Clients", t => t.ClientID, cascadeDelete: true)
                .Index(t => t.ClientID);
            
            CreateTable(
                "dbo.Works",
                c => new
                    {
                        WorkID = c.Int(nullable: false, identity: true),
                        WorkNumber = c.Int(nullable: false),
                        BusinessName = c.String(),
                        DTI = c.String(),
                        Street = c.String(),
                        Province = c.String(),
                        City = c.String(),
                        Employment = c.String(),
                        LengthOfStay = c.String(),
                        BusinessNumber = c.String(),
                        Position = c.String(),
                        MonthlyIncome = c.Double(nullable: false),
                        PLNumber = c.String(),
                        status = c.String(),
                        ClientID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.WorkID)
                .ForeignKey("dbo.Clients", t => t.ClientID, cascadeDelete: true)
                .Index(t => t.ClientID);
            
            CreateTable(
                "dbo.Spouses",
                c => new
                    {
                        SpouseID = c.Int(nullable: false, identity: true),
                        LastName = c.String(),
                        FirstName = c.String(),
                        MiddleName = c.String(),
                        Suffix = c.String(),
                        Birthday = c.DateTime(nullable: false),
                        BusinessName = c.String(),
                        DTI = c.String(),
                        Street = c.String(),
                        Province = c.String(),
                        City = c.String(),
                        Employment = c.String(),
                        LengthOfStay = c.String(),
                        BusinessNumber = c.String(),
                        Position = c.String(),
                        MonthlyIncome = c.Double(nullable: false),
                        PLNumber = c.String(),
                        status = c.String(),
                        ClientID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SpouseID)
                .ForeignKey("dbo.Clients", t => t.ClientID, cascadeDelete: true)
                .Index(t => t.ClientID);
            
            CreateTable(
                "dbo.References",
                c => new
                    {
                        ReferenceID = c.Int(nullable: false, identity: true),
                        ReferenceNumber = c.Int(nullable: false),
                        LastName = c.String(),
                        FirstName = c.String(),
                        MiddleName = c.String(),
                        Suffix = c.String(),
                        Street = c.String(),
                        Province = c.String(),
                        City = c.String(),
                        Contact = c.String(),
                        ClientID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ReferenceID)
                .ForeignKey("dbo.Clients", t => t.ClientID, cascadeDelete: true)
                .Index(t => t.ClientID);
            
            CreateTable(
                "dbo.TempHomeAddresses",
                c => new
                    {
                        AddressID = c.Int(nullable: false, identity: true),
                        AddressNumber = c.Int(nullable: false),
                        Street = c.String(),
                        Province = c.String(),
                        City = c.String(),
                        OwnershipType = c.String(),
                        MonthlyFee = c.Double(nullable: false),
                        LengthOfStay = c.String(),
                    })
                .PrimaryKey(t => t.AddressID);
            
            CreateTable(
                "dbo.TempClientContacts",
                c => new
                    {
                        ContactID = c.Int(nullable: false, identity: true),
                        ContactNumber = c.Int(nullable: false),
                        Contact = c.String(),
                        Primary = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ContactID);
            
            CreateTable(
                "dbo.TempDependents",
                c => new
                    {
                        DependentID = c.Int(nullable: false, identity: true),
                        DependentNumber = c.Int(nullable: false),
                        LastName = c.String(),
                        FirstName = c.String(),
                        MiddleName = c.String(),
                        Suffix = c.String(),
                        Birthday = c.DateTime(nullable: false),
                        School = c.String(),
                    })
                .PrimaryKey(t => t.DependentID);
            
            CreateTable(
                "dbo.TempWorks",
                c => new
                    {
                        WorkID = c.Int(nullable: false, identity: true),
                        WorkNumber = c.Int(nullable: false),
                        BusinessName = c.String(),
                        DTI = c.String(),
                        Street = c.String(),
                        Province = c.String(),
                        City = c.String(),
                        Employment = c.String(),
                        LengthOfStay = c.String(),
                        BusinessNumber = c.String(),
                        Position = c.String(),
                        MonthlyIncome = c.Double(nullable: false),
                        PLNumber = c.String(),
                        status = c.String(),
                    })
                .PrimaryKey(t => t.WorkID);
            
            CreateTable(
                "dbo.TempReferences",
                c => new
                    {
                        ReferenceID = c.Int(nullable: false, identity: true),
                        ReferenceNumber = c.Int(nullable: false),
                        LastName = c.String(),
                        FirstName = c.String(),
                        MiddleName = c.String(),
                        Suffix = c.String(),
                        Street = c.String(),
                        Province = c.String(),
                        City = c.String(),
                        Contact = c.String(),
                    })
                .PrimaryKey(t => t.ReferenceID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.References", new[] { "ClientID" });
            DropIndex("dbo.Spouses", new[] { "ClientID" });
            DropIndex("dbo.Works", new[] { "ClientID" });
            DropIndex("dbo.Dependents", new[] { "ClientID" });
            DropIndex("dbo.ClientContacts", new[] { "ClientID" });
            DropIndex("dbo.HomeAddresses", new[] { "ClientID" });
            DropForeignKey("dbo.References", "ClientID", "dbo.Clients");
            DropForeignKey("dbo.Spouses", "ClientID", "dbo.Clients");
            DropForeignKey("dbo.Works", "ClientID", "dbo.Clients");
            DropForeignKey("dbo.Dependents", "ClientID", "dbo.Clients");
            DropForeignKey("dbo.ClientContacts", "ClientID", "dbo.Clients");
            DropForeignKey("dbo.HomeAddresses", "ClientID", "dbo.Clients");
            DropTable("dbo.TempReferences");
            DropTable("dbo.TempWorks");
            DropTable("dbo.TempDependents");
            DropTable("dbo.TempClientContacts");
            DropTable("dbo.TempHomeAddresses");
            DropTable("dbo.References");
            DropTable("dbo.Spouses");
            DropTable("dbo.Works");
            DropTable("dbo.Dependents");
            DropTable("dbo.ClientContacts");
            DropTable("dbo.HomeAddresses");
            DropTable("dbo.Clients");
        }
    }
}
