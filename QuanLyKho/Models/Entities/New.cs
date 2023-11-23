namespace QuanLyKho.Models.Entities
{
    public class New
    {
        /// <summary>
        /// Getter và Setter cho Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Getter và Setter cho Tiêu đề
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Getter và Setter cho Mô tả
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Getter và Setter cho Hình ảnh
        /// </summary>
        public string? Image { get; set; }
        /// <summary>
        /// Getter và Setter cho nội dung
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// Getter và Setter cho Thời gian tạo
        /// </summary>
        public DateTime CreatedTime { get; set; }
        /// <summary>
        /// Getter và Setter cho Thời gian cập nhật lần cuối
        /// </summary>
        public DateTime LastUpdated { get; set; }
        /// <summary>
        /// Getter và Setter cho Trạng thái
        /// </summary>
        public Status Status { get; set; }

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
