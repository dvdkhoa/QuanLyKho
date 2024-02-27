using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyKho.Models.Entities;

namespace QuanLyKho.Models.EntityConfigurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(o => o.CustomerId).IsRequired(false);
            builder.Property(o => o.StaffId).IsRequired(false);
            builder.Property(o => o.StoreId).IsRequired(false);
            //builder.Property(o => o.ShipStatus).IsRequired(false);
            //builder.Property(o => o.PaymentStatus).IsRequired(false);




            builder.HasOne(t => t.Staff)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(t => t.StaffId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.Store)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(t => t.StoreId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(t => t.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
