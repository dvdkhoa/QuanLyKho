using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyKho.Models.Entities;

namespace QuanLyKho.Models.EntityConfigurations
{
    public class CartDetailConfiguration : IEntityTypeConfiguration<CartDetail>
    {
        public void Configure(EntityTypeBuilder<CartDetail> builder)
        {
            builder.HasKey(t => new { t.CartId, t.ProductId });

            builder.Property(t => t.Quantity).IsRequired();

            builder.Property(t => t.ProductName).IsRequired();

            builder.Property(t => t.ProducPrice).IsRequired();

            builder.Property(t => t.Status).IsRequired();

            builder.HasOne(t => t.Cart)
                    .WithMany(c => c.CartDetails)
                    .HasForeignKey(t => t.CartId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.Product)
                    .WithOne()
                    .HasForeignKey<CartDetail>(t => t.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
