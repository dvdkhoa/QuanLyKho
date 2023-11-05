namespace QuanLyKho.Models.Entities
{
    public class Product
    {
        /// <summary>
        /// Id
        /// </summary>
        public String Id { get; set; }
        /// <summary>
        /// Tên sản phẩm
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Mô tả
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// Đơn giá
        /// </summary>
        public double Price { get; set; }
        /// <summary>
        /// Giá khuyến mãi
        /// </summary>
        public double? PromotionPrice { get; set; }
        /// <summary>
        /// Đơn vị tính
        /// </summary>
        public string? Unit { get; set; }
        /// <summary>
        /// Nhà cung cấp
        /// </summary>
        public string? Supplier { get; set; }
        /// <summary>
        /// Hình ảnh
        /// </summary>
        public string? Image { get; set; }
        /// <summary>
        /// Xuất xứ
        /// </summary>
        public string? Origin { get; set; }
        /// <summary>
        /// Cân nặng
        /// </summary>
        public string? Weight { get; set; }
        /// <summary>
        /// Thời gian hết hạn
        /// </summary>
        public DateTime? Expiry { get; set; }
        /// <summary>
        /// Ngày sản xuất
        /// </summary>
        public DateTime? ManufactoringDate { get; set; }

        /// <summary>
        /// Id danh mục
        /// </summary>
        public int? CategoryId { get; set; }
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
        /// <summary>
        /// Danh mục
        /// </summary>
        public Category? Category { get; set; }
        /// <summary>
        /// Danh sách chi tiết sản phẩm kho
        /// </summary>
        public List<ProductWareHouse>? ProductWareHouses { get; set; }
        /// <summary>
        /// Danh sách chi tiết phiếu xuất/nhập/chuyển kho
        /// </summary>
        public List<ReceiptDetail>? ReceiptDetails { get; set; }
        /// <summary>
        /// Danh sách đơn hàng
        /// </summary>
        public List<CartDetail>? CartDetails { get; set; }
        /// <summary>
        /// Danh sách chi tiết đơn hàng
        /// </summary>
        public List<OrderDetail>? OrderDetails { get; set; }
        /// <summary>
        /// Danh sách hình ảnh
        /// </summary>
        public List<ProductImage>? ProductImages { get; set; }
        /// <summary>
        /// Danh sách chi tiết sản phẩm khuyến mãi
        /// </summary>
        public List<ProductPromotion>? ProductPromotions { get; set; }
        /// <summary>
        /// Danh sách chi tiết cấu hình chi tiết của sản phẩm
        /// </summary>
        public List<ProductDetailedConfig>? ProductDetailedConfigs { get; set; }


        /// <summary>
        /// Phương thức khởi tạo
        /// </summary>
        public Product()
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
