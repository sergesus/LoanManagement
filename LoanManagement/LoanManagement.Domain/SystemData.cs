using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;


namespace LoanManagement.Domain
{

    public class iContext : DbContext
    {
        public DbSet<State> State { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Scope> Scopes { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeAddress> EmployeeAddresses { get; set; }
        public DbSet<EmployeeContact> EmployeeContacts { get; set; }
        public DbSet<TempAddress> TempAdresses { get; set; }
        public DbSet<TempContact> TempContacts { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<BankAddress> BankAdresses { get; set; }
        public DbSet<TempoDeduction> TempoDeductions { get; set; }
        public DbSet<TempoRequirement> TempoRequirements { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Deduction> Deductions { get; set; }
        public DbSet<Requirement> Requirements { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<HomeAddress> HomeAddresses { get; set; }
        public DbSet<ClientContact> ClientContacts { get; set; }
        public DbSet<Dependent> Dependents { get; set; }
        public DbSet<Work> Works { get; set; }
        public DbSet<Spouse> Spouses { get; set; }
        public DbSet<Reference> References { get; set; }
        public DbSet<TempHomeAddress> TempHomeAddresses { get; set; }
        public DbSet<TempClientContact> TempClientContacts { get; set; }
        public DbSet<TempDependent> TempDependents { get; set; }
        public DbSet<TempWork> TempWorks { get; set; }
        public DbSet<TempReference> TempReferences { get; set; }
        public DbSet<Agent> Agents { get; set; }
        public DbSet<AgentContact> AgentContacts { get; set; }
        public DbSet<AgentAddress> AgentAddresses { get; set; }
        public DbSet<TempAgentAddress> TempAgentAddresses { get; set; }
        public DbSet<TempAgentContact> TempAgentContact { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<LoanApplication> LoanApplications { get; set; }
        public DbSet<GenSOA> GenSOA { get; set; }
        public DbSet<DeclinedLoan> DeclinedLoans { get; set; }
        public DbSet<ApprovedLoan> ApprovedLoans { get; set; }
        public DbSet<ReleasedLoan> ReleasedLoans { get; set; }
        public DbSet<FPaymentInfo> FPaymentInfo { get; set; }
        public DbSet<iText> iTexts { get; set; }
        public DbSet<HeldCheque> HeldCheques { get; set; }
        public DbSet<DepositedCheque> DepositedCheques { get; set; }
        public DbSet<TempClearing> TempClearings { get; set; }
        public DbSet<ClearedCheque> ClearedCheques { get; set; }
        public DbSet<ReturnedCheque> ReturnedCheques { get; set; }
        public DbSet<ClosedAccount> ClosedAccounts { get; set; }
        public DbSet<AdjustedLoan> AdjustedLoans { get; set; }
        public DbSet<RestructuredLoan> RestructuredLoans { get; set; }
        public DbSet<PaidLoan> PaidLoans { get; set; }
        public DbSet<ViewLoan> ViewLoans { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
                .HasKey(x => x.EmployeeID)
                .HasRequired(x => x.Employee);
            modelBuilder.Entity<Scope>()
                .HasKey(x => x.EmployeeID)
                .HasRequired(x => x.Employee);
            modelBuilder.Entity<EmployeeAddress>()
                .HasKey(x => x.EmpAddID)
                .HasRequired(x => x.Employee);
            modelBuilder.Entity<EmployeeContact>()
                .HasKey(x => x.EmpContactID)
                .HasRequired(x => x.Employee);
            modelBuilder.Entity<Employee>()
                .HasKey(x => x.EmployeeID);
            modelBuilder.Entity<TempContact>()
                .HasKey(x => x.TempContactID);
            modelBuilder.Entity<Bank>()
                .HasKey(x => x.BankID)
                .HasMany(x => x.BankAddress);
            modelBuilder.Entity<BankAddress>()
                .HasKey(x=> x.BankAddID);
            modelBuilder.Entity<TempoDeduction>()
                .HasKey(x => x.DeductionId);
            modelBuilder.Entity<TempoRequirement>()
                .HasKey(x => x.RequirementId);
            modelBuilder.Entity<Service>()
                .HasKey(x => x.ServiceID);
            modelBuilder.Entity<Requirement>()
                .HasKey(x => x.RequirementId);
            modelBuilder.Entity<Deduction>()
                .HasKey(x => x.DeductionId);
            modelBuilder.Entity<Client>()
                .HasKey(x => x.ClientID);
            modelBuilder.Entity<HomeAddress>()
                .HasKey(x => x.AddressID);
            modelBuilder.Entity<ClientContact>()
                .HasKey(x => x.ContactID);
            modelBuilder.Entity<Dependent>()
                .HasKey(x => x.DependentID);
            modelBuilder.Entity<Work>()
                .HasKey(x => x.WorkID);
            modelBuilder.Entity<Spouse>()
                .HasKey(x => x.SpouseID);
            modelBuilder.Entity<Reference>()
                .HasKey(x => x.ReferenceID);
            modelBuilder.Entity<TempHomeAddress>()
                .HasKey(x => x.AddressID);
            modelBuilder.Entity<TempClientContact>()
                .HasKey(x => x.ContactID);
            modelBuilder.Entity<TempDependent>()
                .HasKey(x => x.DependentID);
            modelBuilder.Entity<TempWork>()
                .HasKey(x => x.WorkID);
            modelBuilder.Entity<TempReference>()
                .HasKey(x => x.ReferenceID);
            modelBuilder.Entity<LoanApplication>()
                .HasKey(x=> x.LoanID)
                .HasRequired(x => x.Loan);
            modelBuilder.Entity<DeclinedLoan>()
                .HasKey(x => x.LoanID)
                .HasRequired(x => x.Loan);
            modelBuilder.Entity<ApprovedLoan>()
                .HasKey(x => x.LoanID)
                .HasRequired(x => x.Loan);
            modelBuilder.Entity<ReleasedLoan>()
                .HasKey(x => x.LoanID)
                .HasRequired(x => x.Loan);
            modelBuilder.Entity<DepositedCheque>()
                .HasKey(x => x.FPaymentInfoID)
                .HasRequired(x => x.FPaymentInfo);
            modelBuilder.Entity<ClearedCheque>()
                .HasKey(x => x.FPaymentInfoID)
                .HasRequired(x => x.FPaymentInfo);
            modelBuilder.Entity<ReturnedCheque>()
                .HasKey(x => x.FPaymentInfoID)
                .HasRequired(x => x.FPaymentInfo);

            modelBuilder.Entity<AdjustedLoan>()
                .HasKey(x => x.LoanID)
                .HasRequired(x => x.Loan);
            modelBuilder.Entity<RestructuredLoan>()
                .HasKey(x => x.LoanID)
                .HasRequired(x => x.Loan);
            modelBuilder.Entity<PaidLoan>()
                .HasKey(x => x.LoanID)
                .HasRequired(x => x.Loan);
            modelBuilder.Entity<iText>()
                .HasKey(x => x.FPaymentInfoID)
                .HasRequired(x => x.FPaymentInfo);
                
        }
    }

    public class State
    {
        public int StateID { get; set; }
        public int iState { get; set; }
    }

    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public int EmployeeID { get; set; }
        public virtual Employee Employee { get; set; }
    }

    public class Scope
    {
        public int EmployeeID { get; set; }
        public bool ClientM { get; set; }
        public bool ServiceM { get; set; }
        public bool AgentM { get; set; }
        public bool BankM { get; set; }
        public bool EmployeeM { get; set; }

        public bool Application { get; set; }
        public bool Approval { get; set; }
        public bool Releasing { get; set; }
        public bool Payments { get; set; }
        public bool ManageCLosed { get; set; }
        public bool Resturcture { get; set; }
        public bool PaymentAdjustment { get; set; }

        public bool Archive { get; set; }
        public bool BackUp { get; set; }
        public bool UserAccounts { get; set; }
        public bool Reports { get; set; }
        public bool Statistics { get; set; }
        public bool Scopes { get; set; }

        public virtual Employee Employee { get; set; }
    }

    public class Employee
    {
        public int EmployeeID { get; set; }
        public string FirstName { get; set; }
        public string MI { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }
        public int PositionID { get; set; }
        public string Department { get; set; }
        public string Email { get; set; }
        public byte[] Photo { get; set; }
        public bool Active { get; set; }

        public virtual Position Position { get; set; }
        public virtual User User { get; set; }
        public virtual Scope Scope { get; set; }
        public ICollection<EmployeeAddress> EmployeeAddress { get; set; }
        public ICollection<EmployeeContact> EmployeeContact { get; set; }
    }

    public class Position
    {
        public int PositionID { get; set; }
        public string PositionName { get; set; }
        public string Description { get; set; }
    }

    public class EmployeeAddress
    {
        public int EmpAddID { get; set; }
        public string Street { get; set; }
        public string Province { get; set; }
        public string City { get; set; }

        public int EmployeeID { get; set; }
        public virtual Employee Employee { get; set; }
    }

    public class EmployeeContact
    {
        public int EmpContactID { get; set; }
        public string Contact { get; set; }

        public int EmployeeID { get; set; }
        public virtual Employee Employee { get; set; }
    }

    public class Bank
    {
        public int BankID { get; set; }
        public string BankName { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }

        public ICollection<BankAddress> BankAddress { get; set; }
    }

    public class BankAddress
    {
        public int BankAddID { get; set; }
        public int BankNum { get; set; }
        public string Street { get; set; }
        public string Province { get; set; }
        public string City { get; set; }

        public int BankID { get; set; }
        public virtual Bank Bank { get; set; }
    }

    public class Service
    {
        public int ServiceID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Department { get; set; }
        public string Type { get; set; }
        public int MinTerm{ get; set;}
        public int MaxTerm{ get; set;}
        public double MinValue { get; set; }
        public double MaxValue { get; set; }
        public double Interest {get;set;}
        public double AgentCommission { get; set; }
        public double Holding { get; set; }
        public double DaifPenalty { get; set; }
        public double ClosedAccountPenalty { get; set; }
        public double RestructureFee { get; set; }
        public double RestructureInterest { get; set; }
        public double AdjustmentFee { get; set; }
        public double LatePaymentPenalty { get; set; }
        public bool Active { get; set; }

        public ICollection<Requirement> Requirement { get; set; }
        public ICollection<Deduction> Deduction { get; set; }
    }


    public class Requirement
    {
        public int RequirementId { get; set; }
        public int RequirementNum { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int ServiceID{ get; set; }
        public virtual Service Service { get; set; }
    }

    public class Deduction
    {
        public int DeductionId { get; set; }
        public int DeductionNum { get; set; }
        public string Name { get; set; }
        public double Percentage { get; set; }

        public int ServiceID { get; set; }
        public virtual Service Service { get; set; }
    }

    public class TempoRequirement
    {
        
        public int RequirementId { get; set; }
        public int RequirementNum { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class TempoDeduction
    {
        public int DeductionId { get; set; }
        public int DeductionNum { get; set; }
        public string Name { get; set; }
        public double Percentage { get; set; }
    }




    public class TempAddress
    {
        public int TempAddressID { get; set; }
        public string Street { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
    }

    public class TempContact
    {
        public int TempContactID { get; set; }
        public string Contact { get; set; }
    }

    public class Client
    {
        public int ClientID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Suffix { get; set; }
        public DateTime Birthday { get; set; }
        public string Sex { get; set; }
        public string Status { get; set; }
        public string SSS { get; set; }
        public string TIN { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }
        public byte[] Photo { get; set; }

        public ICollection<HomeAddress> HomeAddress { get; set; }
        public ICollection<ClientContact> ClientContact { get; set; }
        public ICollection<Dependent> Dependent { get; set; }
        public ICollection<Work> Work { get; set; }
        public ICollection<Spouse> Spouse { get; set; }
        public ICollection<Reference> Reference { get; set; }
    }

    public class HomeAddress
    {
        public int AddressID { get; set; }
        public int AddressNumber { get; set; }
        public string Street { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string OwnershipType { get; set; }
        public double MonthlyFee { get; set; }
        public string LengthOfStay { get; set; }

        public int ClientID { get;set; }
        public virtual Client Client{ get; set; }
    }

    public class ClientContact
    {
        public int ContactID { get; set; }
        public int ContactNumber { get; set; }
        public string Contact { get; set; }
        public bool Primary { get; set; }

        public int ClientID { get; set; }
        public virtual Client Client { get; set; }
    }

    public class Dependent
    {
        public int DependentID { get; set; }
        public int DependentNumber { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Suffix { get; set; }
        public DateTime Birthday { get; set; }
        public string School { get; set; }

        public int ClientID { get; set; }
        public virtual Client Client { get; set; }
    }

    public class Work
    {
        public int WorkID { get; set; }
        public int WorkNumber { get; set; }
        public string BusinessName { get; set; }
        public string DTI { get; set; }
        public string Street { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Employment { get; set; }
        public string LengthOfStay { get; set; }
        public string BusinessNumber { get; set; }
        public string Position { get; set; }
        public double MonthlyIncome { get; set; }
        public string PLNumber { get; set; }
        public string status { get; set; }

        public int ClientID { get; set; }
        public virtual Client Client { get; set; }
    }

    public class Spouse
    {
        public int SpouseID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Suffix { get; set; }
        public DateTime Birthday { get; set; }
        public string BusinessName { get; set; }
        public string DTI { get; set; }
        public string Street { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Employment { get; set; }
        public string LengthOfStay { get; set; }
        public string BusinessNumber { get; set; }
        public string Position { get; set; }
        public double MonthlyIncome { get; set; }
        public string PLNumber { get; set; }
        public string status { get; set; }

        public int ClientID { get; set; }
        public virtual Client Client { get; set; }
    }

    public class Reference
    {
        public int ReferenceID { get; set; }
        public int ReferenceNumber { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Suffix { get; set; }
        public string Street { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Contact { get; set; }

        public int ClientID { get; set; }
        public virtual Client Client { get; set; }
    }

    //asd

    public class TempHomeAddress
    {
        public int AddressID { get; set; }
        public int AddressNumber { get; set; }
        public string Street { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string OwnershipType { get; set; }
        public double MonthlyFee { get; set; }
        public string LengthOfStay { get; set; }
    }

    public class TempClientContact
    {
        public int ContactID { get; set; }
        public int ContactNumber { get; set; }
        public string Contact { get; set; }
        public bool Primary { get; set; }
    }

    public class TempDependent
    {
        public int DependentID { get; set; }
        public int DependentNumber { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Suffix { get; set; }
        public DateTime Birthday { get; set; }
        public string School { get; set; }
    }

    public class TempWork
    {
        public int WorkID { get; set; }
        public int WorkNumber { get; set; }
        public string BusinessName { get; set; }
        public string DTI { get; set; }
        public string Street { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Employment { get; set; }
        public string LengthOfStay { get; set; }
        public string BusinessNumber { get; set; }
        public string Position { get; set; }
        public double MonthlyIncome { get; set; }
        public string PLNumber { get; set; }
        public string status { get; set; }
    }

    public class TempReference
    {
        public int ReferenceID { get; set; }
        public int ReferenceNumber { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Suffix { get; set; }
        public string Street { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Contact { get; set; }
    }

    public class Agent
    {
        public int AgentID { get; set; }
        public string FirstName { get; set; }
        public string MI { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }
        public string Email { get; set; }
        public byte[] Photo { get; set; }
        public bool Active { get; set; }

        public ICollection<EmployeeAddress> AgentAddress { get; set; }
        public ICollection<EmployeeContact> AgentContact { get; set; }
    }

    public class AgentAddress
    {
        public int AgentAddressID { get; set; }
        public int AddressNumber { get; set; }
        public string Street { get; set; }
        public string Province { get; set; }
        public string City { get; set; }

        public int AgentID { get; set; }
        public virtual Agent Agent { get; set; }
    }

    public class AgentContact
    {
        public int AgentContactID { get; set; }
        public int CNumber { get; set; }
        public string Contact { get; set; }

        public int AgentID { get; set; }
        public virtual Agent Agent { get; set; }
    }

    public class TempAgentAddress
    {
        public int TempAgentAddressID { get; set; }
        public int AddressNumber { get; set; }
        public string Street { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
    }

    public class TempAgentContact
    {
        public int TempAgentContactID { get; set; }
        public int CNumber { get; set; }
        public string Contact { get; set; }
    }

    public class Loan
    {
        public int LoanID { get; set; }
        public int Term { get; set; }
        public int CoBorrower { get; set; }
        public string Mode { get; set; }
        public string Status { get; set; }
        public int CI { get; set; }

        public int ServiceID { get; set; }
        public int ClientID { get; set; }
        public int AgentID { get; set; }
        public int BankID { get; set; }
        public virtual Client Client { get; set; }
        public virtual Service Service { get; set; }
        //public virtual Bank Bank { get; set; }
        public virtual LoanApplication LoanApplication { get; set; }
        public virtual DeclinedLoan DeclinedLoan { get; set; }
        public virtual ApprovedLoan ApprovedLoan { get; set; }
        public virtual ReleasedLoan ReleasedLoan { get; set; }
        public virtual AdjustedLoan AdjustedLoan { get; set; }
        public virtual PaidLoan PaidLoan { get; set; }
        public virtual RestructuredLoan RestructuredLoan { get; set; }
        public ICollection<FPaymentInfo> FPaymentInfo { get; set; }
        public ICollection<ClosedAccount> ClosedAccount { get; set; }
    }

    public class LoanApplication
    {
        public int LoanID { get; set; }
        public double AmountApplied { get; set; }
        public DateTime DateApplied { get; set; }
        
        public virtual Loan Loan { get; set; }
    }

    public class DeclinedLoan
    {
        public int LoanID { get; set; }
        public string Reason { get; set; }
        public DateTime DateDeclined { get; set; }

        public virtual Loan Loan { get; set; }
    }

    public class ApprovedLoan
    {
        public int LoanID { get; set; }
        public double AmountApproved { get; set; }
        public DateTime DateApproved { get; set; }
        public DateTime ReleaseDate { get; set; }

        public virtual Loan Loan { get; set; }
    }

    public class ReleasedLoan
    {
        public int LoanID { get; set; }
        public DateTime DateReleased { get; set; }
        public double Principal { get; set; }
        public double NetProceed { get; set; }
        public double TotalLoan { get; set; }
        public double MonthlyPayment { get; set; }
        public double AgentsCommission { get; set; }
        
        public virtual Loan Loan { get; set; }
    }

    public class iText
    {
        public int FPaymentInfoID { get; set; }
        public virtual FPaymentInfo FPaymentInfo { get; set; }
    }


    public class FPaymentInfo
    {
        public int FPaymentInfoID { get; set; }
        public int PaymentNumber { get; set; }
        public string ChequeInfo { get; set; }
        public double Amount { get; set; }
        public DateTime ChequeDueDate { get; set; }
        public DateTime PaymentDate { get; set; }
        public double RemainingBalance { get; set; }
        public string PaymentStatus { get; set; }

        public int LoanID { get; set; }
        public virtual Loan Loan { get; set; }
        public virtual DepositedCheque DepositedCheque { get; set; }
        public virtual ClearedCheque ClearCheque { get; set; }
        public virtual ReturnedCheque ReturnedCheque { get; set; }
        public virtual iText iText { get; set; }
    }

    public class HeldCheque
    {
        public int HeldChequeID { get; set; }
        public int LoanID { get; set; }
        public int PaymentNumber { get; set; }
        public DateTime OriginalPaymentDate { get; set; }
        public DateTime NewPaymentDate { get; set; }
        public DateTime DateHeld { get; set; }
        public double HoldingFee { get; set; }
    }

    public class DepositedCheque
    {
        public int FPaymentInfoID { get; set; }
        public DateTime DepositDate { get; set; }

        public virtual FPaymentInfo FPaymentInfo { get; set; }
    }

    public class ClearedCheque
    {
        public int FPaymentInfoID { get; set; }
        public DateTime? DateCleared { get; set; }

        public virtual FPaymentInfo FPaymentInfo { get; set; }
    }

    public class ClosedAccount
    {
        public int ClosedAccountID { get; set; }
        public int LoanID { get; set; }
        public DateTime DateClosed { get; set; }
        public double Fee { get; set; }
        public bool isPaid { get; set; }
        public string Remarks { get; set; }

        public virtual Loan Loan { get; set; }
    }

    public class ReturnedCheque
    {
        public int FPaymentInfoID { get; set; }
        public DateTime DateReturned { get; set; }
        public string Remarks { get; set; }
        public double Fee { get; set; }
        public bool isPaid { get; set; }

        public virtual FPaymentInfo FPaymentInfo { get; set; }
    }

    public class TempClearing
    {
        public int TempClearingID { get; set; }
        public int FPaymentInfoID { get; set; }

        public virtual FPaymentInfo FPaymentInfo { get; set; }
    }

    public class AdjustedLoan
    {
        public int LoanID { get; set; }
        public int Days { get; set; }
        public double Fee { get; set; }
        public DateTime DateAdjusted { get; set; }

        public virtual Loan Loan { get; set; }
    }

    public class RestructuredLoan
    {
        public int LoanID { get; set; }
        public int NewLoanID { get; set; }
        public double Fee { get; set; }
        public DateTime DateRestructured { get; set; }

        public virtual Loan Loan { get; set; }
    }

    public class PaidLoan
    {
        public int LoanID { get; set; }
        public DateTime DateFinished { get; set; }

        public virtual Loan Loan { get; set; }
    }

    public class ViewLoan
    {
        public int ViewLoanID { get; set; }
        public int PaymentNumber { get; set; }
        public string PaymentInfo { get; set; }
        public string TotalPayment { get; set; }
        public string DueDate { get; set; }
        public string PaymentDate { get; set; }
        public string Status { get; set; }
        public string DateCleared { get; set; }
    }

    public class GenSOA
    {
        public int GenSOAID { get; set; }
        public int PaymentNumber { get; set; }
        public string Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string RemainingBalance { get; set; }
    }

}
