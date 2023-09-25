using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyKho.Models.Entities;

namespace QuanLyKho.Models.EntityConfigurations
{
    public class DetailedConfigConfiguration : IEntityTypeConfiguration<DetailedConfig>
    {
        public void Configure(EntityTypeBuilder<DetailedConfig> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Name).IsRequired();

            builder.Property(t => t.Status).IsRequired();

        }
    }
}

