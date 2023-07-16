using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyKho.Models.Entities;

namespace QuanLyKho.Models.EntityConfigurations
{
    public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.HasKey(t => new { t.OrderId, t.ProductId });

            builder.Property(t => t.ProductName).IsRequired();

            builder.Property(t => t.Quantity).IsRequired();

            builder.Property(t => t.Price).IsRequired();

            builder.HasKey(t => new { t.OrderId, t.ProductId });

            builder.HasOne(t => t.Product)
                    .WithOne()
                    .HasForeignKey<OrderDetail>(t => t.ProductId);

            builder.HasOne(t => t.Order)
                    .WithOne()
                    .HasForeignKey<OrderDetail>(t => t.OrderId);
        }
    }
}
