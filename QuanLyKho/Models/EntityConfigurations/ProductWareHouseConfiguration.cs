using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyKho.Models.Entities;

namespace QuanLyKho.Models.EntityConfigurations
{
    public class ProductWareHouseConfiguration : IEntityTypeConfiguration<ProductWareHouse>
    {
        public void Configure(EntityTypeBuilder<ProductWareHouse> builder)
        {
            builder.HasKey(pw => pw.Id);

            builder.Property(pw => pw.Quantity).IsRequired();

            builder.HasOne(pw => pw.Product)
                    .WithMany(p=>p.ProductWareHouses)
                    .HasForeignKey(p => p.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(pw=>pw.WareHouse)
                    .WithMany(w=>w.ProductWareHouses)
                    .HasForeignKey(w => w.WareHouseId)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
