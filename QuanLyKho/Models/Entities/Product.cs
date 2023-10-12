namespace QuanLyKho.Models.Entities
{
    public class Product
    {
        public String Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
        public double? PromotionPrice { get; set; }
        public string? Unit { get; set; }
        public string? Supplier { get; set; }
        public string? Image { get; set; }
        public string? Origin { get; set; }
        public string? Weight { get; set; }
        public DateTime? Expiry { get; set; }
        public DateTime? ManufactoringDate { get; set; }

        public int? CategoryId { get; set; }
        public Status Status { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime LastUpdated { get; set; }
        public Category? Category { get; set; }
        public List<ProductWareHouse>? ProductWareHouses { get; set; }
        public List<ReceiptDetail>? ReceiptDetails { get; set; }
        public List<CartDetail>? CartDetails { get; set; }
        public List<ProductClassification>? ProductClassifications { get; set; }
        public List<ProductImage>? ProductImages { get; set; }
        public List<ProductPromotion>? ProductPromotions { get; set; }
        public List<ProductDetailedConfig> ProductDetailedConfigs { get; set; }


        public Product()
        {
            this.Status = Status.Show;
        }


        // Khi tạo mới hoặc chỉnh sửa thì gọi
        public void UpdateTime()
        {
            this.CreatedTime = DateTime.Now;
            this.LastUpdated = DateTime.Now;
        }
    }
}
