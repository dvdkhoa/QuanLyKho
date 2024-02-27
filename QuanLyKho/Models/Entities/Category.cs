namespace QuanLyKho.Models.Entities
{
    public class Category
    {
        /// <summary>
        /// Getter và Setter cho Id danh mục
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Getter và Setter cho Tên danh mục
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Getter và Setter cho Biểu tượng
        /// </summary>
        public string? Icon { get; set; }
        /// <summary>
        /// Getter và Setter cho Mô tả
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// Getter và Setter cho Thời gian tạo lần đầu
        /// </summary>
        public DateTime CreatedTime { get; set; }
        /// <summary>
        /// Getter và Setter cho Thời gian lần cuối cập nhật
        /// </summary>
        public DateTime LastUpdated { get; set; }
        /// <summary>
        /// Getter và Setter cho Trạng thái
        /// </summary>
        public Status Status { get; set; }
        /// <summary>
        /// Getter và Setter cho Danh sách cấu hình chi tiết của danh mục
        /// </summary>
        public List<CategoryDetailedConfig>? CategoryDetailedConfigs { get; set; }
        /// <summary>
        /// Getter và Setter cho Danh sách sản phẩm thuộc danh mục
        /// </summary>
        public List<Product>? Products { get; set; }

        /// <summary>
        /// Phương thức khởi tạo
        /// </summary>
        public Category()
        {
            this.Status = Status.Show;
        }



        // Khi tạo mới thì gọi
        /// <summary>
        /// Set thời gian tạo mới lần đầu
        /// </summary>
        public void SetCreatedTime()
        {
            this.CreatedTime = DateTime.Now;
            this.LastUpdated = DateTime.Now;
        }

        // Khi cập nhật thì gọi
        /// <summary>
        /// Set thời gian cập nhật sau mỗi lần thay đổi thông tin
        /// </summary>
        public void SetUpdatedTime()
        {
            this.LastUpdated = DateTime.Now;
        }
    }
}
