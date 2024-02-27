namespace QuanLyKho.Models.Entities
{
    public class ProductWareHouse
    {
        /// <summary>
        /// Getter và Setter cho Id
        /// </summary>
        public int Id { get; set; }
        public string WareHouseId { get; set; }
        /// <summary>
        /// Getter và Setter cho Id sản phẩm
        /// </summary>
        public string ProductId { get; set; }
        /// <summary>
        /// Getter và Setter cho Số lượng
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// Getter và Setter cho Sản phẩm
        /// </summary>
        public Product Product { get; set; }
        public WareHouse WareHouse { get; set; }
        /// <summary>
        /// Getter và Setter cho Trạng thái
        /// </summary>
        public Status Status { get; set; }
        /// <summary>
        /// Getter và Setter cho Thời gian tạo lần đầu
        /// </summary>
        public DateTime CreatedTime { get; set; }
        /// <summary>
        /// Getter và Setter cho Thời gian cập nhật lần cuối
        /// </summary>
        public DateTime LastUpdated { get; set; }
        /// <summary>
        /// Getter và Setter cho Danh sách chi tiết hóa đơn
        /// </summary>
        public List<OrderDetail> OrderDetails { get; set; }

        /// <summary>
        /// Phương thức khởi tạo
        /// </summary>
        public ProductWareHouse()
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
