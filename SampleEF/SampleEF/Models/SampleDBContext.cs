using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using SampleEF.Models.Mapping;

namespace SampleEF.Models
{
    public partial class SampleDBContext : DbContext
    {
        static SampleDBContext()
        {
            Database.SetInitializer<SampleDBContext>(null);
        }

        public SampleDBContext()
            : base("Name=SampleDBContext")
        {
        }

        public DbSet<State_Tbl> State_Tbl { get; set; }
        public DbSet<User_Tbl> User_Tbl { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new State_TblMap());
            modelBuilder.Configurations.Add(new User_TblMap());
        }
    }
}
