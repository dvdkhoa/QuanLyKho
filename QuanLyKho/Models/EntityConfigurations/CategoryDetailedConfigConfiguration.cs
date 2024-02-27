using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyKho.Models.Entities;

namespace QuanLyKho.Models.EntityConfigurations
{
    public class CategoryDetailedConfigConfiguration : IEntityTypeConfiguration<CategoryDetailedConfig>
    {
        public void Configure(EntityTypeBuilder<CategoryDetailedConfig> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Status).IsRequired();

            builder.HasOne(t => t.Category)
                    .WithMany(cdc=>cdc.CategoryDetailedConfigs)
                    .HasForeignKey(t => t.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.DetailedConfig)
                    .WithMany(cdc => cdc.CategoryDetailedConfigs)
                    .HasForeignKey(t => t.ConfigId)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

