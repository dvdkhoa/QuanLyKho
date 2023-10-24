using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyKho.Models.Entities;

namespace QuanLyKho.Models.EntityConfigurations
{
    public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.ProductName).IsRequired();

            builder.Property(t => t.Quantity).IsRequired();

            builder.Property(t => t.Price).IsRequired();

            builder.HasOne(t => t.Product)
                    .WithMany(t => t.OrderDetails)
                    .HasForeignKey(t => t.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.Order)
                    .WithMany(t => t.OrderDetails)
                    .HasForeignKey(t => t.OrderId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.ProductWareHouse)
                    .WithMany(t => t.OrderDetails)
                    .HasForeignKey(t => t.ProductWarehouseId)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
