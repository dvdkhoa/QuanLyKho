using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyKho.Models.Entities;

namespace QuanLyKho.Models.EntityConfigurations
{
    //public class ProductClassificationConfiguration : IEntityTypeConfiguration<ProductClassification>
    //{
    //    public void Configure(EntityTypeBuilder<ProductClassification> builder)
    //    {
    //        builder.HasKey(t => t.Id);

    //        builder.HasOne(t => t.Classification)
    //                .WithMany(c => c.ProductClassifications)
    //                .HasForeignKey(t => t.ClassificationId)
    //                .OnDelete(DeleteBehavior.Restrict);

    //        builder.HasOne(t => t.Product)
    //                .WithMany(p => p.ProductClassifications)
    //                .HasForeignKey(t => t.ProductId)
    //                .OnDelete(DeleteBehavior.Restrict);
    //    }
    //}
}
