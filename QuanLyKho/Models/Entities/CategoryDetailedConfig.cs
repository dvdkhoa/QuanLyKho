namespace QuanLyKho.Models.Entities
{
    
    public class CategoryDetailedConfig
    {
        /// <summary>
        /// Id chi tiết cấu hình danh mục
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Id danh mục
        /// </summary>
        public int CategoryId { get; set; }
        /// <summary>
        /// Danh mục
        /// </summary>
        public Category Category { get; set; }
        /// <summary>
        /// Id cấu hình chi tiết
        /// </summary>
        public int ConfigId { get; set; }
        /// <summary>
        /// Cấu hình chi tiết
        /// </summary>
        public DetailedConfig DetailedConfig { get; set; }
        /// <summary>
        /// Trạng thái
        /// </summary>
        public Status Status { get; set; }
        /// <summary>
        /// Thời gian tạo lần đầu
        /// </summary>
        public DateTime CreatedTime { get; set; }
        /// <summary>
        /// Thời gian cập nhật lần cuối
        /// </summary>
        public DateTime LastUpdated { get; set; }


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
