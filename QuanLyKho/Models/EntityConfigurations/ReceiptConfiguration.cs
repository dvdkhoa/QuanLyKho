using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyKho.Models.Entities;

namespace QuanLyKho.Models.EntityConfigurations
{
    public class ReceiptConfiguration : IEntityTypeConfiguration<Receipt>
    {
        public void Configure(EntityTypeBuilder<Receipt> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.DateCreated).IsRequired();

            builder.Property(r => r.Type).IsRequired();

            builder.HasOne(r => r.WareHouse)
                    .WithMany(w => w.Receipts)
                    .HasForeignKey(p => p.WareHouseId)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
