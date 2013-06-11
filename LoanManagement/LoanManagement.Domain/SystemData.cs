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
}
