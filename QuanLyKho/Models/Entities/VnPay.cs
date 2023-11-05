namespace QuanLyKho.Models.Entities
{
    public class VnPay
    {
        /// <summary>
        /// Id hóa đơn thanh toán bằng Vnpay
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Id hóa đơn
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// Hóa đơn
        /// </summary>
        public Order Order { get; set; }
        /// <summary>
        /// Tên ngân hàng
        /// </summary>
        public string BankName { get; set; }
        /// <summary>
        /// Id giao dịch Vnpay
        /// </summary>
        public int TransactionId { get; set; }
        /// <summary>
        /// Trạng thái giao dịch
        /// </summary>
        public int TransactionStatus { get; set; }
        /// <summary>
        /// Tiền thanh toán
        /// </summary>
        public double Price { get; set; }
        /// <summary>
        /// Ngày giờ thanh toán
        /// </summary>
        public DateTime DateTimePayment { get; set; }
    }
}
