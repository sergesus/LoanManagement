using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SampleEF.Models.Mapping
{
    public class User_TblMap : EntityTypeConfiguration<User_Tbl>
    {
        public User_TblMap()
        {
            // Primary Key
            this.HasKey(t => new { t.User_Id, t.User_Name, t.User_Pass });

            // Properties
            this.Property(t => t.User_Id)
                .IsRequired();

            this.Property(t => t.User_Name)
                .IsRequired();

            this.Property(t => t.User_Pass)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("User_Tbl");
            this.Property(t => t.User_Id).HasColumnName("User_Id");
            this.Property(t => t.User_Name).HasColumnName("User_Name");
            this.Property(t => t.User_Pass).HasColumnName("User_Pass");
        }
    }
}
