namespace QuanLyKho.Models.Entities
{
    public class DetailedConfig
    {
        /// <summary>
        /// Getter và Setter cho Id cấu hình
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Getter và Setter cho Tên cấu hình
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Getter và Setter cho Trạng thái
        /// </summary>
        public Status Status { get; set; }

        /// <summary>
        /// Getter và Setter cho Danh sách chi tiết cấu hình danh mục
        /// </summary>
        public List<CategoryDetailedConfig> CategoryDetailedConfigs { get; set; }
        /// <summary>
        /// Getter và Setter cho Danh sách chi tiết cấu hình sản phẩm
        /// </summary>
        public List<ProductDetailedConfig> ProductDetailedConfigs { get; set; }

    }
}
