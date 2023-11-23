using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyKho.Models.Entities;

namespace QuanLyKho.Models.EntityConfigurations
{
    public class StaffConfiguration : IEntityTypeConfiguration<Staff>
    {
        public void Configure(EntityTypeBuilder<Staff> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Name).IsRequired();

            builder.Property(s=>s.Email).IsRequired();

            builder.Property(s => s.DateOfBirth).IsRequired();

            builder.Property(s => s.Gender).IsRequired();

            builder.Property(s => s.Image).IsRequired(false);


            builder.HasOne(s=>s.WareHouse)
                    .WithMany(wh=>wh.Staffs)
                    .HasForeignKey(s=>s.WareHouseId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.User)
                    .WithOne()
                    .HasForeignKey<Staff>(s => s.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
