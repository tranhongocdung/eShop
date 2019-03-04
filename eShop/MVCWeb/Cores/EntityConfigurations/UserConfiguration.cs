using System.Data.Entity.ModelConfiguration;
using MVCWeb.Cores.Entities;

namespace MVCWeb.Cores.EntityConfigurations
{
    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            ToTable("User");
            HasKey(o => o.Id);
        }
    }
}