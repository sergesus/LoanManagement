using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SampleEF.Models.Mapping
{
    public class State_TblMap : EntityTypeConfiguration<State_Tbl>
    {
        public State_TblMap()
        {
            // Primary Key
            this.HasKey(t => t.state);

            // Properties
            this.Property(t => t.state)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("State_Tbl");
            this.Property(t => t.state).HasColumnName("state");
        }
    }
}
