using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyKho.Models.Entities;

namespace QuanLyKho.Models.EntityConfigurations
{
    public class ReceiptDetailConfiguration : IEntityTypeConfiguration<ReceiptDetail>
    {
        public void Configure(EntityTypeBuilder<ReceiptDetail> builder)
        {
            builder.HasKey(rd => rd.Id);

            builder.Property(rd => rd.Quantity).IsRequired();

            builder.HasOne(rd => rd.Receipt)
                    .WithMany(w => w.ReceiptDetails)
                    .HasForeignKey(p => p.ReceiptId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(rd => rd.Product)
                    .WithMany(w => w.ReceiptDetails)
                    .HasForeignKey(p => p.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
