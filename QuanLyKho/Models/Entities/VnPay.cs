namespace QuanLyKho.Models.Entities
{
    public class VnPay
    {
        /// <summary>
        /// Getter và Setter cho Id hóa đơn thanh toán bằng Vnpay
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Getter và Setter cho Id hóa đơn
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// Getter và Setter cho Hóa đơn
        /// </summary>
        public Order Order { get; set; }
        /// <summary>
        /// Getter và Setter cho Tên ngân hàng
        /// </summary>
        public string BankName { get; set; }
        /// <summary>
        /// Getter và Setter cho Id giao dịch Vnpay
        /// </summary>
        public int TransactionId { get; set; }
        /// <summary>
        /// Getter và Setter cho Trạng thái giao dịch
        /// </summary>
        public int TransactionStatus { get; set; }
        /// <summary>
        /// Getter và Setter cho Tiền thanh toán
        /// </summary>
        public double Price { get; set; }
        /// <summary>
        /// Getter và Setter cho Ngày giờ thanh toán
        /// </summary>
        public DateTime DateTimePayment { get; set; }
    }
}
