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

            builder.Property(cate => cate.Name).IsRequired();

            builder.Property(cate => cate.Status).IsRequired();

            builder.HasOne(t=>t.User).WithOne()
                    .HasForeignKey<Customer>(t=>t.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            }
    }
}
