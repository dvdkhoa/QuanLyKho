namespace QuanLyKho.Models.Entities
{
    public class Bill
    {
        /// <summary>
        /// Id hóa đơn bán trực tiếp
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
        /// Tiền thanh toán
        /// </summary>
        public double Price { get; set; }
        /// <summary>
        /// Tiền nhận của khách
        /// </summary>
        public double? MoneyReceived { get; set; }
        /// <summary>
        /// Tiền trả lại khách
        /// </summary>
        public double? MoneyRefund { get; set; }
        /// <summary>
        /// Ngày giờ thành toán
        /// </summary>
        public DateTime DateTimePayment { get; set; }
        /// <summary>
        /// Trạng thái thanh toán
        /// </summary>
        public int StatusPay { get; set; }
    }
}
