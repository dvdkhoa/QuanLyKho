

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace QuanLyKho.Models.Entities
{
    public class Order
    {
        /// <summary>
        /// Getter và Setter cho Id hóa đơn
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Getter và Setter cho Id khách hàng
        /// </summary>
        public string? CustomerId { get; set; }
        /// <summary>
        /// Getter và Setter cho Tên khách hàng
        /// </summary>
        public string? CustomerName { get; set; }
        /// <summary>
        /// Getter và Setter cho Số điện thoại
        /// </summary>
        public string? PhoneNumber { get; set; }
        /// <summary>
        /// Getter và Setter cho Email
        /// </summary>
        public string? Email { get; set; }
        /// <summary>
        /// Getter và Setter cho Địa chỉ
        /// </summary>
        public string? Address { get; set; }
        /// <summary>
        /// Getter và Setter cho Ghi chú
        /// </summary>
        public string? Note { get; set; }
        /// <summary>
        /// Getter và Setter cho Phương thức thanh toán
        /// </summary>
        public string? PaymentMethod { get; set; }
        /// <summary>
        /// Getter và Setter cho Tạm tính
        /// </summary>
        public double TmpTotal { get; set; }
        /// <summary>
        /// Getter và Setter cho Tổng cộng
        /// </summary>
        public double Total { get; set; }
        /// <summary>
        /// Getter và Setter cho Id nhân viên
        /// </summary>
        public string StaffId { get; set; }
        /// <summary>
        /// Getter và Setter cho Id cửa hàng
        /// </summary>
        public string? StoreId { get; set; }
        /// <summary>
        /// Getter và Setter cho Trạng thái giao hàng
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public ShipStatus ShipStatus { get; set; }
        /// <summary>
        /// Getter và Setter cho Trạng thái thanh toán
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public PaymentStatus PaymentStatus { get; set; }
        /// <summary>
        /// Getter và Setter cho Thời gian tạo lần đầu
        /// </summary>
        public DateTime CreatedTime { get; set; }
        /// <summary>
        /// Getter và Setter cho Thời gian cập nhật lần cuối
        /// </summary>
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// Getter và Setter cho Nhân viên
        /// </summary>
        public Staff? Staff { get; set; }
        /// <summary>
        /// Getter và Setter cho Khách hàng
        /// </summary>
        public Customer? Customer { get; set; }
        /// <summary>
        /// Getter và Setter cho Cửa hàng
        /// </summary>
        public WareHouse? Store { get; set; }

        /// <summary>
        /// Getter và Setter cho Danh sách chi tiết hóa đơn
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
        Unpaid = 0,
        /// <summary>
        /// Đã thanh toán
        /// </summary>
        Paid = 1
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
