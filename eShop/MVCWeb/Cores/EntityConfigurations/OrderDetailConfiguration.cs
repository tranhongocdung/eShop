using System.Data.Entity.ModelConfiguration;
using MVCWeb.Cores.Entities;

namespace MVCWeb.Cores.EntityConfigurations
{
    public class OrderDetailConfiguration : EntityTypeConfiguration<OrderDetail>
    {
        public OrderDetailConfiguration()
        {
            ToTable("OrderDetail");
            HasKey(o => o.Id);
            HasRequired(o => o.Order).WithMany(o => o.OrderDetails).HasForeignKey(o => o.OrderId);
            HasRequired(o => o.ProductVariant).WithMany(o => o.OrderDetails).HasForeignKey(o => o.ProductVariantId);
        }
    }
}