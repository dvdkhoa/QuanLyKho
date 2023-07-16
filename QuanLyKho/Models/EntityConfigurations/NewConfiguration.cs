using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyKho.Models.Entities;

namespace QuanLyKho.Models.EntityConfigurations
{
    public class NewConfiguration : IEntityTypeConfiguration<New>
    {
        public void Configure(EntityTypeBuilder<New> builder)
        {
            builder.HasKey(cate => cate.Id);

            builder.Property(cate => cate.Title).IsRequired();

            builder.Property(cate => cate.Status).IsRequired();
        }
    }
}
