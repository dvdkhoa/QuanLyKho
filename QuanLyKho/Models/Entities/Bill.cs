namespace QuanLyKho.Models.Entities
{
    public class Bill
    {
        /// <summary>
        /// Getter và Setter cho Id hóa đơn bán trực tiếp
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
        /// Getter và Setter cho Tiền thanh toán
        /// </summary>
        public double Price { get; set; }
        /// <summary>
        /// Getter và Setter cho Tiền nhận của khách
        /// </summary>
        public double? MoneyReceived { get; set; }
        /// <summary>
        /// Getter và Setter cho Tiền trả lại khách
        /// </summary>
        public double? MoneyRefund { get; set; }
        /// <summary>
        /// Getter và Setter cho Ngày giờ thành toán
        /// </summary>
        public DateTime DateTimePayment { get; set; }
        /// <summary>
        /// Getter và Setter cho Trạng thái thanh toán
        /// </summary>
        public int StatusPay { get; set; }
    }
}
