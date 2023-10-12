namespace QuanLyKho.Models.Entities
{
    public class Bill
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order? Order { get; set; }
        public double Price { get; set; }
        public double? MoneyReceived { get; set; }
        public double? MoneyRefund { get; set; }
        public DateTime DateTimePayment { get; set; }
        public int StatusPay { get; set; }
    }
}
