using System.Data.Entity.ModelConfiguration;
using MVCWeb.Cores.Entities;

namespace MVCWeb.Cores.EntityConfigurations
{
    public class ProductVariantConfiguration : EntityTypeConfiguration<ProductVariant>
    {
        public ProductVariantConfiguration()
        {
            ToTable("ProductVariant");
            HasKey(o => o.Id);
            HasRequired(o => o.Colour).WithMany(o => o.ProductVariants).HasForeignKey(o => o.ColourId);
            HasRequired(o => o.Size).WithMany(o => o.ProductVariants).HasForeignKey(o => o.SizeId);
            HasRequired(o => o.Product).WithMany(o => o.ProductVariants).HasForeignKey(o => o.ProductId);
        }
    }
}