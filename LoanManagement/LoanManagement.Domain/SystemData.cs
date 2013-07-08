using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;


namespace LoanManagement.Domain
{

    public class SystemContext : DbContext
    {
        public DbSet<User> Users { get; set; }
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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
                .HasKey(x => x.Username)
                .HasRequired(x => x.Employee);
            modelBuilder.Entity<EmployeeAddress>()
                .HasKey(x => x.EmpAddID)
                .HasRequired(x => x.Employee);
            modelBuilder.Entity<EmployeeContact>()
                .HasKey(x => x.EmpContactID)
                .HasRequired(x => x.Employee);
            modelBuilder.Entity<Employee>()
                .HasKey(x => x.EmployeeID)
                .HasMany(x => x.User);
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

                
                
        }
    }

    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public int EmployeeID { get; set; }
        public virtual Employee Employee { get; set; }
    }

    public class Employee
    {
        public int EmployeeID { get; set; }
        public string FirstName { get; set; }
        public string MI { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }
        public string Position { get; set; }
        public string Department { get; set; }
        public string Email { get; set; }
        public byte[] Photo { get; set; }
        public bool Active { get; set; }

        public ICollection<User> User { get; set; }
        public ICollection<EmployeeAddress> EmployeeAddress { get; set; }
        public ICollection<EmployeeContact> EmployeeContact { get; set; }
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
        public double Penalty { get; set; }
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
        public string TypeOfLoan { get; set; }
        public string Type { get; set; }
        public string Mode { get; set; }
        public double Interest { get; set; }
        public double Deduction { get; set; }
        public double Penalty { get; set; }
        public double Commission { get; set; }
        public int Term { get; set; }
        public double Principal { get; set; }
        public int CoBorrower { get; set; }
        public string Status { get; set; }

        public int ClientID { get; set; }
        public int AgentID { get; set; }
        public virtual Client Client { get; set; }
        public virtual Agent Agent { get; set; }
        public ICollection<LoanApplication> LoanApplication { get; set; }
    }

    public class LoanApplication
    {
        public int LoanApplicationID { get; set; }
        public double AmmountApplied { get; set; }
        public DateTime DateApplied { get; set; }

        public int LoanID { get; set; }
        public virtual Loan Loan { get; set; }
    }

}
