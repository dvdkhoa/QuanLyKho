namespace QuanLyKho.Models.Entities
{
    public class Order
    {
        /// <summary>
        /// Id hóa đơn
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Id khách hàng
        /// </summary>
        public string? CustomerId { get; set; }
        /// <summary>
        /// Tên khách hàng
        /// </summary>
        public string? CustomerName { get; set; }
        /// <summary>
        /// Số điện thoại
        /// </summary>
        public string? PhoneNumber { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public string? Email { get; set; }
        /// <summary>
        /// Địa chỉ
        /// </summary>
        public string? Address { get; set; }
        /// <summary>
        /// Ghi chú
        /// </summary>
        public string? Note { get; set; }
        /// <summary>
        /// Phương thức thanh toán
        /// </summary>
        public string? PaymentMethod { get; set; }
        /// <summary>
        /// Tạm tính
        /// </summary>
        public double TmpTotal { get; set; }
        /// <summary>
        /// Tổng cộng
        /// </summary>
        public double Total { get; set; }
        /// <summary>
        /// Id nhân viên
        /// </summary>
        public string StaffId { get; set; }
        /// <summary>
        /// Id cửa hàng
        /// </summary>
        public string? StoreId { get; set; }
        /// <summary>
        /// Trạng thái giao hàng
        /// </summary>
        public ShipStatus ShipStatus { get; set; }
        /// <summary>
        /// Trạng thái thanh toán
        /// </summary>
        public PaymentStatus PaymentStatus { get; set; }
        /// <summary>
        /// Thời gian tạo lần đầu
        /// </summary>
        public DateTime CreatedTime { get; set; }
        /// <summary>
        /// Thời gian cập nhật lần cuối
        /// </summary>
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// Nhân viên
        /// </summary>
        public Staff? Staff { get; set; }
        /// <summary>
        /// Khách hàng
        /// </summary>
        public Customer? Customer { get; set; }
        /// <summary>
        /// Cửa hàng
        /// </summary>
        public WareHouse? Store { get; set; }

        /// <summary>
        /// Danh sách chi tiết hóa đơn
        /// </summary>
        public List<OrderDetail>? OrderDetails { get; set; }


        // Khi tạo mới thì gọi
        public void SetCreatedTime()
        {
            this.CreatedTime = DateTime.Now;
            this.LastUpdated = DateTime.Now;
        }

        // Khi cập nhật thì gọi
        public void SetUpdatedTime()
        {
            this.LastUpdated = DateTime.Now;
        }
    }

    public enum PaymentStatus
    {
        /// <summary>
        /// Chưa thanh toán
        /// </summary>
        Unpaid,
        /// <summary>
        /// Đã thanh toán
        /// </summary>
        Paid
    }


    public enum ShipStatus
    {
        /// <summary>
        /// Chưa được duyệt
        /// </summary>
        NotApproved = 0,
        /// <summary>
        /// Đang vận chuyển
        /// </summary>
        BeingShipped = 1,
        /// <summary>
        /// Thành công
        /// </summary>
        Success = 2,
        /// <summary>
        /// Bị hủy
        /// </summary>
        Canceled = -1
    }


    public class PaymentMethod
    {
        /// <summary>
        /// Thanh toán khi nhận hàng
        /// </summary>
        public const string COD = "COD";
        /// <summary>
        /// Thanh toán Vnpay
        /// </summary>
        public const string VnPay = "VNPAY";
        /// <summary>
        /// Thanh toán trực tiếp tại cửa hàng
        /// </summary>
        public const string Direct = "DIRECT";
    }
}
