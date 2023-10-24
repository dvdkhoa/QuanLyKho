using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyKho.Models.Entities;

namespace QuanLyKho.Models.EntityConfigurations
{
    public class BillConfiguration : IEntityTypeConfiguration<Bill>
    {
        public void Configure(EntityTypeBuilder<Bill> builder)
        {
            builder.HasKey(b => b.Id);

            builder.HasOne(b => b.Order)
                   .WithOne()
                   .HasForeignKey<Bill>(t => t.OrderId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
