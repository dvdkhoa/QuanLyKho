namespace QuanLyKho.Models.Entities
{
    public class VnPay
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public string BankName { get; set; }
        public int TransactionId { get; set; }
        public int TransactionStatus { get; set; }
        public double Price { get; set; }
        public DateTime DateTimePayment { get; set; }
    }
}
