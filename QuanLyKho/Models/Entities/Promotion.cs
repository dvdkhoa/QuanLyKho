namespace QuanLyKho.Models.Entities
{
    public class Promotion
    {
        /// <summary>
        /// Getter và Setter cho Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Getter và Setter cho Tên CTKM
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Getter và Setter cho Loại khuyến mãi
        /// </summary>
        public PromotionType PromotionType { get; set; }
        /// <summary>
        /// Getter và Setter cho Phần trăm khuyến mãi
        /// </summary>
        public int? Percent { get; set; }
        /// <summary>
        /// Getter và Setter cho Mô tả
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// Getter và Setter cho Ngày bắt đầu
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// Getter và Setter cho Ngày kết thúc
        /// </summary>
        public DateTime EndDate { get; set; }
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
        /// Getter và Setter cho Danh sách chi tiết sản phẩm thuộc CTKM
        /// </summary>
        public List<ProductPromotion>? ProductPromotions { get; set; }


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

    public enum PromotionType
    {
        /// <summary>
        /// Khuyến mãi giảm giá
        /// </summary>
        Discount = 0,
        /// <summary>
        /// Khuyến mãi tặng quà
        /// </summary>
        Gift = 1,
    }
}
