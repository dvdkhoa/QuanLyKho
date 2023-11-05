namespace QuanLyKho.Models.Entities
{
    public class ProductDetailedConfig
    {
        /// <summary>
        /// Id chi tiết cấu hình sản phẩm
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Id sản phẩm
        /// </summary>
        public string ProductId { get; set; }
        /// <summary>
        /// Id cấu hình chi tiết
        /// </summary>
        public int ConfigId { get; set; }
        /// <summary>
        /// Nội dung
        /// </summary>
        public string? Value { get; set; }

        /// <summary>
        /// Sản phẩm
        /// </summary>
        public Product? Product { get; set; }
        /// <summary>
        /// Cấu hình chi tiết
        /// </summary>
        public DetailedConfig? Config { get; set; }
    }
}
