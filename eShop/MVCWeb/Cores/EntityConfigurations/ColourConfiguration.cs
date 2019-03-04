using System.Data.Entity.ModelConfiguration;
using MVCWeb.Cores.Entities;

namespace MVCWeb.Cores.EntityConfigurations
{
    public class ColourConfiguration : EntityTypeConfiguration<Colour>
    {
        public ColourConfiguration()
        {
            ToTable("Colour");
            HasKey(o => o.Id);
        }
    }
}