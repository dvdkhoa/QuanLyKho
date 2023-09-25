using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyKho.Models.Entities;

namespace QuanLyKho.Models.EntityConfigurations
{
    public class ProductDetailedConfigConfiguration : IEntityTypeConfiguration<ProductDetailedConfig>
    {
        public void Configure(EntityTypeBuilder<ProductDetailedConfig> builder)
        {
            builder.HasKey(t => t.Id);

            builder.HasOne(p => p.Product)
                    .WithMany(cate => cate.ProductDetailedConfigs)
                    .HasForeignKey(p => p.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.Config)
                    .WithMany(cate => cate.ProductDetailedConfigs)
                    .HasForeignKey(p => p.ConfigId)
                    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
