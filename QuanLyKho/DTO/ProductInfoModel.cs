using QuanLyKho.Models.Entities;

namespace QuanLyKho.DTO
{
    public class ProductInfoModel
    {
        public String Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public Status Status { get; set; }
        public double Price { get; set; }
        public string? Unit { get; set; }
        public string? Supplier { get; set; }
        public string? Origin { get; set; }
        public string? Weight { get; set; }
        public DateTime? Expiry { get; set; }
        public DateTime? ManufacturingDate { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? LastUpdated { get; set; }
        public double? PromotionPrice { get; set; }
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
        public int? CategoryBrandId { get; set; }
        public CategoryBrand? CategoryBrand { get; set; }
        public int Quantity { get;set; }
        public List<ProductWareHouse>? ProductWarehouses { get; set; }
        public List<ProductImage>? ProductImages { get; set; }
    }
}
