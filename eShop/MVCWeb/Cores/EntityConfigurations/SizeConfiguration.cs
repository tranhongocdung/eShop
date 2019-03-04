using System.Data.Entity.ModelConfiguration;
using MVCWeb.Cores.Entities;

namespace MVCWeb.Cores.EntityConfigurations
{
    public class SizeConfiguration : EntityTypeConfiguration<Size>
    {
        public SizeConfiguration()
        {
            ToTable("Size");
            HasKey(o => o.Id);
        }
    }
}