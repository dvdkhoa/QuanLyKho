namespace QuanLyKho.Models.Entities
{
    public class ProductDetailedConfig
    {
        public int Id { get; set; }
        public string ProductId { get; set; }
        public int ConfigId { get; set; }
        public string? Value { get; set; }

        public Product? Product { get; set; }
        public DetailedConfig? Config { get; set; }
    }
}
