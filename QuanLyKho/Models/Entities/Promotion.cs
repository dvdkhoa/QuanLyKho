namespace QuanLyKho.Models.Entities
{
    public class Promotion
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Tên CTKM
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Loại khuyến mãi
        /// </summary>
        public PromotionType PromotionType { get; set; }
        /// <summary>
        /// Phần trăm khuyến mãi
        /// </summary>
        public int? Percent { get; set; }
        /// <summary>
        /// Mô tả
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// Ngày bắt đầu
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// Ngày kết thúc
        /// </summary>
        public DateTime EndDate { get; set; }
        /// <summary>
        /// Thời gian tạo lần đầu
        /// </summary>
        public DateTime CreatedTime { get; set; }
        /// <summary>
        /// Thời gian lần cuối cập nhật
        /// </summary>
        public DateTime LastUpdated { get; set; }
        /// <summary>
        /// Trạng thái
        /// </summary>
        public Status Status { get; set; }

        /// <summary>
        /// Danh sách chi tiết sản phẩm thuộc CTKM
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
        Discount,
        /// <summary>
        /// Khuyến mãi tặng quà
        /// </summary>
        Gift,
    }
}
