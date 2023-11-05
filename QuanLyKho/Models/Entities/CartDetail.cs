namespace QuanLyKho.Models.Entities
{
    public class CartDetail
    {
        /// <summary>
        /// Id chi tiết giỏ hàng
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Id giỏ hàng
        /// </summary>
        public int CartId { get; set; }
        /// <summary>
        /// Giỏ hàng
        /// </summary>
        public Cart Cart { get; set; }
        /// <summary>
        /// Id sản phẩm
        /// </summary>
        public string ProductId { get; set; }
        /// <summary>
        /// Sản phẩm
        /// </summary>
        public Product Product { get; set; }
        /// <summary>
        /// Số lượng
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// Tên sản phẩm
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// Giá sản phẩm
        /// </summary>
        public double ProducPrice { get; set; }
        /// <summary>
        /// Giá khuyến mãi
        /// </summary>
        public double PromotionPrice { get; set; }
        /// <summary>
        /// Hình ảnh sản phẩm
        /// </summary>
        public string ProductImage { get; set; }
        /// <summary>
        /// Thời gian tạo lần đầu
        /// </summary>
        public DateTime CreatedTime { get; set; }
        /// <summary>
        /// Thời gian cập nhật lần cuối
        /// </summary>
        public DateTime LastUpdated { get; set; }
        /// <summary>
        /// Trạng thái
        /// </summary>
        public Status Status { get; set; }
    }
}
