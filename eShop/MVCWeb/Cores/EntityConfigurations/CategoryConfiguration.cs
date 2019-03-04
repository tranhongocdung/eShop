using System.Data.Entity.ModelConfiguration;
using MVCWeb.Cores.Entities;

namespace MVCWeb.Cores.EntityConfigurations
{
    public class CategoryConfiguration : EntityTypeConfiguration<Category>
    {
        public CategoryConfiguration()
        {
            ToTable("Category");
            HasKey(o => o.Id);
            Property(x => x.CategoryName).IsRequired();
            HasOptional(o => o.ParentCategory).WithMany(o => o.ChildCategories).HasForeignKey(o => o.ParentId);
            HasMany(o => o.Products).WithMany(o => o.Categories).Map(o =>
            {
                o.MapLeftKey("CategoryId");
                o.MapRightKey("ProductId");
                o.ToTable("Product_Category");
            });
        }
    }
}