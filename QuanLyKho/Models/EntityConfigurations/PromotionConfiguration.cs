using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyKho.Models.Entities;

namespace QuanLyKho.Models.EntityConfigurations
{
    public class PromotionConfiguration : IEntityTypeConfiguration<Promotion>
    {
        public void Configure(EntityTypeBuilder<Promotion> builder)
        {
            builder.HasKey(cate => cate.Id);

            builder.Property(cate => cate.Name).IsRequired();

            builder.Property(cate => cate.StartDate).IsRequired();

            builder.Property(cate => cate.EndDate).IsRequired();

            builder.Property(cate => cate.Status).IsRequired();
        }
    }
}
