using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuanLyKho.Models.Entities;
using QuanLyKho.Models.EntityConfigurations;
using System.Reflection.Emit;

namespace QuanLyKho.Models.EF
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.EnableSensitiveDataLogging();
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.ApplyConfiguration(new CategoryConfiguration());
            builder.ApplyConfiguration(new ProductConfiguration());
            builder.ApplyConfiguration(new ProductWareHouseConfiguration());
            builder.ApplyConfiguration(new ReceiptConfiguration());
            builder.ApplyConfiguration(new ReceiptDetailConfiguration());
            builder.ApplyConfiguration(new StaffConfiguration());
            builder.ApplyConfiguration(new WareHouseConfiguration());
            //builder.ApplyConfiguration(new ClassificationConfiguration());
            //builder.ApplyConfiguration(new ProductClassificationConfiguration());
            builder.ApplyConfiguration(new ProductImageConfiguration());
            builder.ApplyConfiguration(new OrderConfiguration());
            builder.ApplyConfiguration(new OrderDetailConfiguration());
            builder.ApplyConfiguration(new CustomerConfiguration());
            builder.ApplyConfiguration(new CartConfiguration());
            builder.ApplyConfiguration(new CartDetailConfiguration());
            builder.ApplyConfiguration(new NewConfiguration());
            builder.ApplyConfiguration(new PromotionConfiguration());
            builder.ApplyConfiguration(new ProductPromotionConfiguration());
            builder.ApplyConfiguration(new CategoryDetailedConfigConfiguration());
            builder.ApplyConfiguration(new DetailedConfigConfiguration());
            builder.ApplyConfiguration(new ProductDetailedConfigConfiguration());
            builder.ApplyConfiguration(new BillConfiguration());
            builder.ApplyConfiguration(new VnPayConfiguration());
            builder.ApplyConfiguration(new BrandConfiguration());
            builder.ApplyConfiguration(new CategoryBrandConfiguration());
            builder.ApplyConfiguration(new BannerConfiguration());




            // Cấu hình các bảng Identity
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName.StartsWith("AspNet"))
                {
                    var newName = tableName.Substring(6);

                    entityType.SetTableName(newName);
                }
            }

        }


        // Tạo các DbSet
        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<CategoryBrand> CategoryBrands { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<WareHouse> WareHouses { get; set; }
        public DbSet<ProductWareHouse> ProductWareHouses { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<ReceiptDetail> ReceiptDetails { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        //public DbSet<ProductClassification> ProductClassifications { get; set; }
        //public DbSet<Classification> Classifications { get; set; }
        public DbSet<DetailedConfig> DetailedConfigs { get; set; }
        public DbSet<CategoryDetailedConfig> CategoryDetailedConfigs { get; set; }
        public DbSet<ProductDetailedConfig> ProductDetailedConfigs { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<ProductPromotion> ProductPromotions { get; set; }
        public DbSet<New> News { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartDetail> CartDetails { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<VnPay> VnPays { get; set; }
        public DbSet<Banner> Banners { get; set; }
    }
}
