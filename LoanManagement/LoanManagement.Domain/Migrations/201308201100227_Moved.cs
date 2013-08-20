namespace LoanManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Moved : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Users", "Employee_EmployeeID", "dbo.Employees");
            DropIndex("dbo.Users", new[] { "Employee_EmployeeID" });
            RenameColumn(table: "dbo.Users", name: "Employee_EmployeeID", newName: "EmployeeID");
            CreateTable(
                "dbo.EmployeeAddresses",
                c => new
                    {
                        EmpAddID = c.Int(nullable: false, identity: true),
                        Street = c.String(),
                        Province = c.String(),
                        City = c.String(),
                        EmployeeID = c.Int(nullable: false),
                        Agent_AgentID = c.Int(),
                    })
                .PrimaryKey(t => t.EmpAddID)
                .ForeignKey("dbo.Employees", t => t.EmployeeID, cascadeDelete: true)
                .ForeignKey("dbo.Agents", t => t.Agent_AgentID)
                .Index(t => t.EmployeeID)
                .Index(t => t.Agent_AgentID);
            
            CreateTable(
                "dbo.EmployeeContacts",
                c => new
                    {
                        EmpContactID = c.Int(nullable: false, identity: true),
                        Contact = c.String(),
                        EmployeeID = c.Int(nullable: false),
                        Agent_AgentID = c.Int(),
                    })
                .PrimaryKey(t => t.EmpContactID)
                .ForeignKey("dbo.Employees", t => t.EmployeeID, cascadeDelete: true)
                .ForeignKey("dbo.Agents", t => t.Agent_AgentID)
                .Index(t => t.EmployeeID)
                .Index(t => t.Agent_AgentID);
            
            CreateTable(
                "dbo.TempAddresses",
                c => new
                    {
                        TempAddressID = c.Int(nullable: false, identity: true),
                        Street = c.String(),
                        Province = c.String(),
                        City = c.String(),
                    })
                .PrimaryKey(t => t.TempAddressID);
            
            CreateTable(
                "dbo.TempContacts",
                c => new
                    {
                        TempContactID = c.Int(nullable: false, identity: true),
                        Contact = c.String(),
                    })
                .PrimaryKey(t => t.TempContactID);
            
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
                        BankNum = c.Int(nullable: false),
                        Street = c.String(),
                        Province = c.String(),
                        City = c.String(),
                        BankID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BankAddID)
                .ForeignKey("dbo.Banks", t => t.BankID, cascadeDelete: true)
                .Index(t => t.BankID);
            
            CreateTable(
                "dbo.TempoDeductions",
                c => new
                    {
                        DeductionId = c.Int(nullable: false, identity: true),
                        DeductionNum = c.Int(nullable: false),
                        Name = c.String(),
                        Percentage = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.DeductionId);
            
            CreateTable(
                "dbo.TempoRequirements",
                c => new
                    {
                        RequirementId = c.Int(nullable: false, identity: true),
                        RequirementNum = c.Int(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.RequirementId);
            
            CreateTable(
                "dbo.Services",
                c => new
                    {
                        ServiceID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Department = c.String(),
                        Type = c.String(),
                        MinTerm = c.Int(nullable: false),
                        MaxTerm = c.Int(nullable: false),
                        MinValue = c.Double(nullable: false),
                        MaxValue = c.Double(nullable: false),
                        Interest = c.Double(nullable: false),
                        AgentCommission = c.Double(nullable: false),
                        Holding = c.Double(nullable: false),
                        DaifPenalty = c.Double(nullable: false),
                        ClosedAccountPenalty = c.Double(nullable: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ServiceID);
            
            CreateTable(
                "dbo.Requirements",
                c => new
                    {
                        RequirementId = c.Int(nullable: false, identity: true),
                        RequirementNum = c.Int(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        ServiceID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RequirementId)
                .ForeignKey("dbo.Services", t => t.ServiceID, cascadeDelete: true)
                .Index(t => t.ServiceID);
            
            CreateTable(
                "dbo.Deductions",
                c => new
                    {
                        DeductionId = c.Int(nullable: false, identity: true),
                        DeductionNum = c.Int(nullable: false),
                        Name = c.String(),
                        Percentage = c.Double(nullable: false),
                        ServiceID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DeductionId)
                .ForeignKey("dbo.Services", t => t.ServiceID, cascadeDelete: true)
                .Index(t => t.ServiceID);
            
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
                        Photo = c.Binary(),
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
            
            CreateTable(
                "dbo.Agents",
                c => new
                    {
                        AgentID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        MI = c.String(),
                        LastName = c.String(),
                        Suffix = c.String(),
                        Email = c.String(),
                        Photo = c.Binary(),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.AgentID);
            
            CreateTable(
                "dbo.AgentContacts",
                c => new
                    {
                        AgentContactID = c.Int(nullable: false, identity: true),
                        CNumber = c.Int(nullable: false),
                        Contact = c.String(),
                        AgentID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AgentContactID)
                .ForeignKey("dbo.Agents", t => t.AgentID, cascadeDelete: true)
                .Index(t => t.AgentID);
            
            CreateTable(
                "dbo.AgentAddresses",
                c => new
                    {
                        AgentAddressID = c.Int(nullable: false, identity: true),
                        AddressNumber = c.Int(nullable: false),
                        Street = c.String(),
                        Province = c.String(),
                        City = c.String(),
                        AgentID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AgentAddressID)
                .ForeignKey("dbo.Agents", t => t.AgentID, cascadeDelete: true)
                .Index(t => t.AgentID);
            
            CreateTable(
                "dbo.TempAgentAddresses",
                c => new
                    {
                        TempAgentAddressID = c.Int(nullable: false, identity: true),
                        AddressNumber = c.Int(nullable: false),
                        Street = c.String(),
                        Province = c.String(),
                        City = c.String(),
                    })
                .PrimaryKey(t => t.TempAgentAddressID);
            
            CreateTable(
                "dbo.TempAgentContacts",
                c => new
                    {
                        TempAgentContactID = c.Int(nullable: false, identity: true),
                        CNumber = c.Int(nullable: false),
                        Contact = c.String(),
                    })
                .PrimaryKey(t => t.TempAgentContactID);
            
            CreateTable(
                "dbo.Loans",
                c => new
                    {
                        LoanID = c.Int(nullable: false, identity: true),
                        Term = c.Int(nullable: false),
                        CoBorrower = c.Int(nullable: false),
                        Mode = c.String(),
                        Status = c.String(),
                        CI = c.Int(nullable: false),
                        ServiceID = c.Int(nullable: false),
                        ClientID = c.Int(nullable: false),
                        AgentID = c.Int(nullable: false),
                        BankID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.LoanID)
                .ForeignKey("dbo.Clients", t => t.ClientID, cascadeDelete: true)
                .ForeignKey("dbo.Services", t => t.ServiceID, cascadeDelete: true)
                .Index(t => t.ClientID)
                .Index(t => t.ServiceID);
            
            CreateTable(
                "dbo.LoanApplications",
                c => new
                    {
                        LoanID = c.Int(nullable: false),
                        AmountApplied = c.Double(nullable: false),
                        DateApplied = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.LoanID)
                .ForeignKey("dbo.Loans", t => t.LoanID)
                .Index(t => t.LoanID);
            
            CreateTable(
                "dbo.DeclinedLoans",
                c => new
                    {
                        LoanID = c.Int(nullable: false),
                        Reason = c.String(),
                        DateDeclined = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.LoanID)
                .ForeignKey("dbo.Loans", t => t.LoanID)
                .Index(t => t.LoanID);
            
            CreateTable(
                "dbo.ApprovedLoans",
                c => new
                    {
                        LoanID = c.Int(nullable: false),
                        AmountApproved = c.Double(nullable: false),
                        DateApproved = c.DateTime(nullable: false),
                        ReleaseDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.LoanID)
                .ForeignKey("dbo.Loans", t => t.LoanID)
                .Index(t => t.LoanID);
            
            CreateTable(
                "dbo.ReleasedLoans",
                c => new
                    {
                        LoanID = c.Int(nullable: false),
                        DateReleased = c.DateTime(nullable: false),
                        Principal = c.Double(nullable: false),
                        NetProceed = c.Double(nullable: false),
                        TotalLoan = c.Double(nullable: false),
                        MonthlyPayment = c.Double(nullable: false),
                        AgentsCommission = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.LoanID)
                .ForeignKey("dbo.Loans", t => t.LoanID)
                .Index(t => t.LoanID);
            
            CreateTable(
                "dbo.FPaymentInfoes",
                c => new
                    {
                        FPaymentInfoID = c.Int(nullable: false, identity: true),
                        PaymentNumber = c.Int(nullable: false),
                        ChequeInfo = c.String(),
                        Amount = c.Double(nullable: false),
                        ChequeDueDate = c.DateTime(nullable: false),
                        PaymentDate = c.DateTime(nullable: false),
                        RemainingBalance = c.Double(nullable: false),
                        PaymentStatus = c.String(),
                        LoanID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FPaymentInfoID)
                .ForeignKey("dbo.Loans", t => t.LoanID, cascadeDelete: true)
                .Index(t => t.LoanID);
            
            CreateTable(
                "dbo.DepositedCheques",
                c => new
                    {
                        FPaymentInfoID = c.Int(nullable: false),
                        DepositDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.FPaymentInfoID)
                .ForeignKey("dbo.FPaymentInfoes", t => t.FPaymentInfoID)
                .Index(t => t.FPaymentInfoID);
            
            CreateTable(
                "dbo.ClearedCheques",
                c => new
                    {
                        FPaymentInfoID = c.Int(nullable: false),
                        DateCleared = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.FPaymentInfoID)
                .ForeignKey("dbo.FPaymentInfoes", t => t.FPaymentInfoID)
                .Index(t => t.FPaymentInfoID);
            
            CreateTable(
                "dbo.ReturnedCheques",
                c => new
                    {
                        FPaymentInfoID = c.Int(nullable: false),
                        DateReturned = c.DateTime(nullable: false),
                        Remarks = c.String(),
                        Fee = c.Double(nullable: false),
                        isPaid = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.FPaymentInfoID)
                .ForeignKey("dbo.FPaymentInfoes", t => t.FPaymentInfoID)
                .Index(t => t.FPaymentInfoID);
            
            CreateTable(
                "dbo.ClosedAccounts",
                c => new
                    {
                        ClosedAccountID = c.Int(nullable: false, identity: true),
                        LoanID = c.Int(nullable: false),
                        DateClosed = c.DateTime(nullable: false),
                        Fee = c.Double(nullable: false),
                        isPaid = c.Boolean(nullable: false),
                        Remarks = c.String(),
                    })
                .PrimaryKey(t => t.ClosedAccountID)
                .ForeignKey("dbo.Loans", t => t.LoanID, cascadeDelete: true)
                .Index(t => t.LoanID);
            
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
            
            CreateTable(
                "dbo.HeldCheques",
                c => new
                    {
                        HeldChequeID = c.Int(nullable: false, identity: true),
                        LoanID = c.Int(nullable: false),
                        PaymentNumber = c.Int(nullable: false),
                        OriginalPaymentDate = c.DateTime(nullable: false),
                        NewPaymentDate = c.DateTime(nullable: false),
                        DateHeld = c.DateTime(nullable: false),
                        HoldingFee = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.HeldChequeID);
            
            CreateTable(
                "dbo.TempClearings",
                c => new
                    {
                        TempClearingID = c.Int(nullable: false, identity: true),
                        FPaymentInfoID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TempClearingID)
                .ForeignKey("dbo.FPaymentInfoes", t => t.FPaymentInfoID, cascadeDelete: true)
                .Index(t => t.FPaymentInfoID);
            
            AddColumn("dbo.Employees", "FirstName", c => c.String());
            AddColumn("dbo.Employees", "LastName", c => c.String());
            AddColumn("dbo.Employees", "Suffix", c => c.String());
            AddColumn("dbo.Employees", "Position", c => c.String());
            AddColumn("dbo.Employees", "Department", c => c.String());
            AddColumn("dbo.Employees", "Active", c => c.Boolean(nullable: false));
            AddForeignKey("dbo.Users", "EmployeeID", "dbo.Employees", "EmployeeID", cascadeDelete: true);
            CreateIndex("dbo.Users", "EmployeeID");
            DropColumn("dbo.Employees", "FName");
            DropColumn("dbo.Employees", "LName");
            DropColumn("dbo.Employees", "Address");
            DropColumn("dbo.Employees", "Contact");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Employees", "Contact", c => c.String());
            AddColumn("dbo.Employees", "Address", c => c.String());
            AddColumn("dbo.Employees", "LName", c => c.String());
            AddColumn("dbo.Employees", "FName", c => c.String());
            DropIndex("dbo.TempClearings", new[] { "FPaymentInfoID" });
            DropIndex("dbo.ClosedAccounts", new[] { "LoanID" });
            DropIndex("dbo.ReturnedCheques", new[] { "FPaymentInfoID" });
            DropIndex("dbo.ClearedCheques", new[] { "FPaymentInfoID" });
            DropIndex("dbo.DepositedCheques", new[] { "FPaymentInfoID" });
            DropIndex("dbo.FPaymentInfoes", new[] { "LoanID" });
            DropIndex("dbo.ReleasedLoans", new[] { "LoanID" });
            DropIndex("dbo.ApprovedLoans", new[] { "LoanID" });
            DropIndex("dbo.DeclinedLoans", new[] { "LoanID" });
            DropIndex("dbo.LoanApplications", new[] { "LoanID" });
            DropIndex("dbo.Loans", new[] { "ServiceID" });
            DropIndex("dbo.Loans", new[] { "ClientID" });
            DropIndex("dbo.AgentAddresses", new[] { "AgentID" });
            DropIndex("dbo.AgentContacts", new[] { "AgentID" });
            DropIndex("dbo.References", new[] { "ClientID" });
            DropIndex("dbo.Spouses", new[] { "ClientID" });
            DropIndex("dbo.Works", new[] { "ClientID" });
            DropIndex("dbo.Dependents", new[] { "ClientID" });
            DropIndex("dbo.ClientContacts", new[] { "ClientID" });
            DropIndex("dbo.HomeAddresses", new[] { "ClientID" });
            DropIndex("dbo.Deductions", new[] { "ServiceID" });
            DropIndex("dbo.Requirements", new[] { "ServiceID" });
            DropIndex("dbo.BankAddresses", new[] { "BankID" });
            DropIndex("dbo.EmployeeContacts", new[] { "Agent_AgentID" });
            DropIndex("dbo.EmployeeContacts", new[] { "EmployeeID" });
            DropIndex("dbo.EmployeeAddresses", new[] { "Agent_AgentID" });
            DropIndex("dbo.EmployeeAddresses", new[] { "EmployeeID" });
            DropIndex("dbo.Users", new[] { "EmployeeID" });
            DropForeignKey("dbo.TempClearings", "FPaymentInfoID", "dbo.FPaymentInfoes");
            DropForeignKey("dbo.ClosedAccounts", "LoanID", "dbo.Loans");
            DropForeignKey("dbo.ReturnedCheques", "FPaymentInfoID", "dbo.FPaymentInfoes");
            DropForeignKey("dbo.ClearedCheques", "FPaymentInfoID", "dbo.FPaymentInfoes");
            DropForeignKey("dbo.DepositedCheques", "FPaymentInfoID", "dbo.FPaymentInfoes");
            DropForeignKey("dbo.FPaymentInfoes", "LoanID", "dbo.Loans");
            DropForeignKey("dbo.ReleasedLoans", "LoanID", "dbo.Loans");
            DropForeignKey("dbo.ApprovedLoans", "LoanID", "dbo.Loans");
            DropForeignKey("dbo.DeclinedLoans", "LoanID", "dbo.Loans");
            DropForeignKey("dbo.LoanApplications", "LoanID", "dbo.Loans");
            DropForeignKey("dbo.Loans", "ServiceID", "dbo.Services");
            DropForeignKey("dbo.Loans", "ClientID", "dbo.Clients");
            DropForeignKey("dbo.AgentAddresses", "AgentID", "dbo.Agents");
            DropForeignKey("dbo.AgentContacts", "AgentID", "dbo.Agents");
            DropForeignKey("dbo.References", "ClientID", "dbo.Clients");
            DropForeignKey("dbo.Spouses", "ClientID", "dbo.Clients");
            DropForeignKey("dbo.Works", "ClientID", "dbo.Clients");
            DropForeignKey("dbo.Dependents", "ClientID", "dbo.Clients");
            DropForeignKey("dbo.ClientContacts", "ClientID", "dbo.Clients");
            DropForeignKey("dbo.HomeAddresses", "ClientID", "dbo.Clients");
            DropForeignKey("dbo.Deductions", "ServiceID", "dbo.Services");
            DropForeignKey("dbo.Requirements", "ServiceID", "dbo.Services");
            DropForeignKey("dbo.BankAddresses", "BankID", "dbo.Banks");
            DropForeignKey("dbo.EmployeeContacts", "Agent_AgentID", "dbo.Agents");
            DropForeignKey("dbo.EmployeeContacts", "EmployeeID", "dbo.Employees");
            DropForeignKey("dbo.EmployeeAddresses", "Agent_AgentID", "dbo.Agents");
            DropForeignKey("dbo.EmployeeAddresses", "EmployeeID", "dbo.Employees");
            DropForeignKey("dbo.Users", "EmployeeID", "dbo.Employees");
            DropColumn("dbo.Employees", "Active");
            DropColumn("dbo.Employees", "Department");
            DropColumn("dbo.Employees", "Position");
            DropColumn("dbo.Employees", "Suffix");
            DropColumn("dbo.Employees", "LastName");
            DropColumn("dbo.Employees", "FirstName");
            DropTable("dbo.TempClearings");
            DropTable("dbo.HeldCheques");
            DropTable("dbo.GenSOAs");
            DropTable("dbo.ClosedAccounts");
            DropTable("dbo.ReturnedCheques");
            DropTable("dbo.ClearedCheques");
            DropTable("dbo.DepositedCheques");
            DropTable("dbo.FPaymentInfoes");
            DropTable("dbo.ReleasedLoans");
            DropTable("dbo.ApprovedLoans");
            DropTable("dbo.DeclinedLoans");
            DropTable("dbo.LoanApplications");
            DropTable("dbo.Loans");
            DropTable("dbo.TempAgentContacts");
            DropTable("dbo.TempAgentAddresses");
            DropTable("dbo.AgentAddresses");
            DropTable("dbo.AgentContacts");
            DropTable("dbo.Agents");
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
            DropTable("dbo.Deductions");
            DropTable("dbo.Requirements");
            DropTable("dbo.Services");
            DropTable("dbo.TempoRequirements");
            DropTable("dbo.TempoDeductions");
            DropTable("dbo.BankAddresses");
            DropTable("dbo.Banks");
            DropTable("dbo.TempContacts");
            DropTable("dbo.TempAddresses");
            DropTable("dbo.EmployeeContacts");
            DropTable("dbo.EmployeeAddresses");
            RenameColumn(table: "dbo.Users", name: "EmployeeID", newName: "Employee_EmployeeID");
            CreateIndex("dbo.Users", "Employee_EmployeeID");
            AddForeignKey("dbo.Users", "Employee_EmployeeID", "dbo.Employees", "EmployeeID");
        }
    }
}
