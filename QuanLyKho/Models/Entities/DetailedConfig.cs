namespace QuanLyKho.Models.Entities
{
    public class DetailedConfig
    {
        /// <summary>
        /// Id cấu hình
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Tên cấu hình
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Trạng thái
        /// </summary>
        public Status Status { get; set; }

        /// <summary>
        /// Danh sách chi tiết cấu hình danh mục
        /// </summary>
        public List<CategoryDetailedConfig> CategoryDetailedConfigs { get; set; }
        /// <summary>
        /// Danh sách chi tiết cấu hình sản phẩm
        /// </summary>
        public List<ProductDetailedConfig> ProductDetailedConfigs { get; set; }

    }
}
