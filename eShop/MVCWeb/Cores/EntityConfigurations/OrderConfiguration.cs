using System.Data.Entity.ModelConfiguration;
using MVCWeb.Cores.Entities;

namespace MVCWeb.Cores.EntityConfigurations
{
    public class OrderConfiguration : EntityTypeConfiguration<Order>
    {
        public OrderConfiguration()
        {
            ToTable("Order");
            HasKey(o => o.Id);
            HasRequired(o => o.Customer).WithMany(o => o.Orders).HasForeignKey(o => o.CustomerId);
            HasRequired(o => o.OrderStatus).WithMany(o => o.Orders).HasForeignKey(o => o.OrderStatusId);
            HasOptional(o => o.CreatedBy).WithMany(o => o.Orders).HasForeignKey(o => o.CreatedById);
        }
    }
}