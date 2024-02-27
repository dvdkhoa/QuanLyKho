using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyKho.Models.Entities;

namespace QuanLyKho.Models.EntityConfigurations
{
    public class VnPayConfiguration : IEntityTypeConfiguration<VnPay>
    {
        public void Configure(EntityTypeBuilder<VnPay> builder)
        {
            builder.HasKey(b => b.Id);

            builder.HasOne(b => b.Order)
                    .WithOne()
                    .HasForeignKey<VnPay>(t => t.OrderId)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
