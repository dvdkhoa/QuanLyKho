using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyKho.Models.Entities;

namespace QuanLyKho.Models.EntityConfigurations
{
    public class ProductPromotionConfiguration : IEntityTypeConfiguration<ProductPromotion>
    {
        public void Configure(EntityTypeBuilder<ProductPromotion> builder)
        {
            builder.HasKey(t => new {t.ProductId, t.PromotionId});
            
            builder.HasOne(t=>t.Promotion)
                    .WithMany(pm=>pm.ProductPromotions)
                    .HasForeignKey(pm => pm.PromotionId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.Product)
                    .WithMany(pm => pm.ProductPromotions)
                    .HasForeignKey(pm => pm.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
