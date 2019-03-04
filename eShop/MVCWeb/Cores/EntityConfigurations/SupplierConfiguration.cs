using System.Data.Entity.ModelConfiguration;
using MVCWeb.Cores.Entities;

namespace MVCWeb.Cores.EntityConfigurations
{
    public class SupplierConfiguration : EntityTypeConfiguration<Supplier>
    {
        public SupplierConfiguration()
        {
            ToTable("Supplier");
            HasKey(o => o.Id);
        }
    }
}