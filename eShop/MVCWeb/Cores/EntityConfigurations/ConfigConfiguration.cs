using System.Data.Entity.ModelConfiguration;
using MVCWeb.Cores.Entities;

namespace MVCWeb.Cores.EntityConfigurations
{
    public class ConfigConfiguration : EntityTypeConfiguration<Config>
    {
        public ConfigConfiguration()
        {
            ToTable("Config");
            HasKey(o => o.Id);
        }
    }
}