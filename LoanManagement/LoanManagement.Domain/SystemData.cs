using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace LoanManagement.Domain
{
    class SystemData
    {
        public class LoanContext : DbContext
        {
            public DbSet<User> Users { get; set; }
            public DbSet<Employee> Employees { get; set; }

            protected override void OnModelCreating(DbModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);
                modelBuilder.Entity<User>()
                    .HasKey(x => x.Username);
                modelBuilder.Entity<Employee>()
                    .HasKey(x => x.EmployeeID);
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
            public string FName { get; set; }
            public string MI { get; set; }
            public string LName { get; set; }
            public string Address { get; set; }
            public string Contact { get; set; }
            public string Email { get; set; }
            public byte[] Photo { get; set; }

            public virtual User User { get; set; }
        }

    }
}
