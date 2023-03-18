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

            builder.Property(cate => cate.Name).IsRequired();

            builder.Property(cate => cate.Status).IsRequired();

            builder.HasOne(p => p.Category)
                    .WithMany(cate=>cate.Products)
                    .HasForeignKey(p=>p.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
