using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyKho.Models.Entities;

namespace QuanLyKho.Models.EntityConfigurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(cate => cate.Name)
                    .UseCollation("Latin1_General_CI_AI")
                    .IsRequired();

            builder.Property(cate => cate.Status).IsRequired();

            builder.HasOne(p => p.Category)

                    .WithMany(cate=>cate.Products)
                    .HasForeignKey(p=>p.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.CategoryBrand)
                    .WithMany(cate => cate.Products)
                    .HasForeignKey(p => p.CategoryBrandId)
                    .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
