using QuanLyKho.Models.Entities;

namespace QuanLyKho.DTO
{
    public class EditProductModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
        public double? PromotionPrice { get; set; }
        public string? Unit { get; set; }
        public string? Supplier { get; set; }
        public string? Origin { get; set; }
        public string? Weight { get; set; }
        public DateTime? Expiry { get; set; }
        public DateTime? ManufactoringDate { get; set; }

        public int? CategoryId { get; set; }
        public int? CategoryBrandId { get; set; }
        public Status Status { get; set; }
        public Category? Category { get; set; }
        public CategoryBrand? CategoryBrand { get; set; }

    }
}
