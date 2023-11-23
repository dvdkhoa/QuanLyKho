using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyKho.Models.Entities;

namespace QuanLyKho.Models.EntityConfigurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(cus => cus.Name).IsRequired();

            builder.Property(cus => cus.Image).IsRequired(false);

            builder.Property(cus => cus.Status).IsRequired();

            builder.HasOne(t => t.User).WithOne()
                    .HasForeignKey<Customer>(t => t.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
