using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyKho.Models.Entities;

namespace QuanLyKho.Models.EntityConfigurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(cate => cate.Id);

            builder.Property(cate=>cate.Name).IsRequired();

            builder.Property(cate=>cate.Status).IsRequired();
        }
    }

    public class BrandConfiguration : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder.HasKey(br => br.Id);

            builder.Property(br => br.Name).IsRequired();
        }
    }
    public class CategoryBrandConfiguration : IEntityTypeConfiguration<CategoryBrand>
    {
        public void Configure(EntityTypeBuilder<CategoryBrand> builder)
        {
            builder.HasKey(cb => cb.Id);

            builder.HasOne(cb => cb.Brand)
                    .WithMany(t => t.CategoryBrands)
                    .HasForeignKey(t => t.BrandId)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
