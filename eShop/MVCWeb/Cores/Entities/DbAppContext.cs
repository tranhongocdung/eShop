using System;
using System.Data.Entity;
using MVCWeb.Cores.EntityConfigurations;

namespace MVCWeb.Cores.Entities
{
    public interface IDbAppContext : IDisposable
    {

    }
    public class DbAppContext : DbContext, IDbAppContext
    {
        public DbAppContext()
            : base("name=eShopDBConnectionString")
        {
            //Database.SetInitializer(new CreateDatabaseIfNotExists<DbAppContext>());
            Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Config> Configs { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Colour> Colours { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new CategoryConfiguration());
            modelBuilder.Configurations.Add(new ConfigConfiguration());
            modelBuilder.Configurations.Add(new CustomerConfiguration());
            modelBuilder.Configurations.Add(new OrderConfiguration());
            modelBuilder.Configurations.Add(new OrderDetailConfiguration());
            modelBuilder.Configurations.Add(new OrderStatusConfiguration());
            modelBuilder.Configurations.Add(new ProductConfiguration());
            modelBuilder.Configurations.Add(new SizeConfiguration());
            modelBuilder.Configurations.Add(new SupplierConfiguration());
            modelBuilder.Configurations.Add(new ColourConfiguration());
            modelBuilder.Configurations.Add(new ProductVariantConfiguration());
            modelBuilder.Configurations.Add(new UserConfiguration());
        }
    }
}