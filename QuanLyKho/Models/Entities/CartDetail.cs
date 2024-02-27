namespace QuanLyKho.Models.Entities
{
    public class CartDetail
    {
        /// <summary>
        /// Getter và Setter cho Id chi tiết giỏ hàng
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Getter và Setter cho Id giỏ hàng
        /// </summary>
        public int CartId { get; set; }
        /// <summary>
        /// Getter và Setter cho Giỏ hàng
        /// </summary>
        public Cart Cart { get; set; }
        /// <summary>
        /// Getter và Setter cho Id sản phẩm
        /// </summary>
        public string ProductId { get; set; }
        /// <summary>
        /// Getter và Setter cho Sản phẩm
        /// </summary>
        public Product Product { get; set; }
        /// <summary>
        /// Getter và Setter cho Số lượng
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// Getter và Setter cho Tên sản phẩm
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// Getter và Setter cho Giá sản phẩm
        /// </summary>
        public double ProducPrice { get; set; }
        /// <summary>
        /// Getter và Setter cho Giá khuyến mãi
        /// </summary>
        public double? PromotionPrice { get; set; }
        /// <summary>
        /// Getter và Setter cho Hình ảnh sản phẩm
        /// </summary>
        public string ProductImage { get; set; }
        /// <summary>
        /// Getter và Setter cho Thời gian tạo lần đầu
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
    }
}
