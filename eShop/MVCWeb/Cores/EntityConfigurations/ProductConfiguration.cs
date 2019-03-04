using System.Data.Entity.ModelConfiguration;
using MVCWeb.Cores.Entities;

namespace MVCWeb.Cores.EntityConfigurations
{
    public class ProductConfiguration : EntityTypeConfiguration<Product>
    {
        public ProductConfiguration()
        {
            ToTable("Product");
            HasKey(o => o.Id);
            Property(o => o.ProductName).IsRequired();
            HasMany(o => o.Categories).WithMany(o => o.Products).Map(o =>
            {
                o.MapLeftKey("ProductId");
                o.MapRightKey("CategoryId");
                o.ToTable("Product_Category");
            });
        }
    }
}