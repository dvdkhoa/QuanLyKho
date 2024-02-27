using QuanLyKho.Models.Entities;

namespace QuanLyKho.DTO
{
    public class ProductInStock
    {
        public String Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public Status Status { get; set; }
        public double Price { get; set; }
        public string? Unit { get; set; }
        public string? Supplier { get; set; }
        public int? CategoryId { get; set; }
        public int? Imported { get; set; }
        public int? Exported { get; set; }
        public double? InputMoney { get; set; }
        public double? Revenue { get; set; }
        public int? InventoryNumber { get; set; }

        
    }
}
