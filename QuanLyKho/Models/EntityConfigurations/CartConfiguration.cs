using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyKho.Models.Entities;

namespace QuanLyKho.Models.EntityConfigurations
{
    public class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.HasKey(t => t.Id);

            builder.HasOne(t => t.Customer)
                    .WithOne()
                    .HasForeignKey<Cart>(t=>t.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);
                   
        }
    }

    public class BillConfiguration : IEntityTypeConfiguration<Bill>
    {
        public void Configure(EntityTypeBuilder<Bill> builder)
        {
            builder.HasKey(b => b.Id);

            builder.HasOne(b => b.Order)
                    .WithMany(o=>o.Bills)
                    .HasForeignKey(b => b.OrderId)
                    .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
