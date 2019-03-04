using System.Data.Entity.ModelConfiguration;
using MVCWeb.Cores.Entities;

namespace MVCWeb.Cores.EntityConfigurations
{
    public class OrderStatusConfiguration : EntityTypeConfiguration<OrderStatus>
    {
        public OrderStatusConfiguration()
        {
            ToTable("OrderStatus");
            HasKey(o => o.Id);
        }
    }
}