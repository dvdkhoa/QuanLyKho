namespace QuanLyKho.Models.Entities
{
    public class ProductDetailedConfig
    {
        /// <summary>
        /// Getter và Setter cho Id chi tiết cấu hình sản phẩm
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Getter và Setter cho Id sản phẩm
        /// </summary>
        public string ProductId { get; set; }
        /// <summary>
        /// Getter và Setter cho Id cấu hình chi tiết
        /// </summary>
        public int ConfigId { get; set; }
        /// <summary>
        /// Getter và Setter cho Nội dung
        /// </summary>
        public string? Value { get; set; }

        /// <summary>
        /// Getter và Setter cho Sản phẩm
        /// </summary>
        public Product? Product { get; set; }
        /// <summary>
        /// Getter và Setter cho Cấu hình chi tiết
        /// </summary>
        public DetailedConfig? Config { get; set; }
    }
}
