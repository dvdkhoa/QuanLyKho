namespace QuanLyKho.Models.Entities
{
    public class ReceiptDetail
    {
        public int Id { get; set; }
        public int ReceiptId { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public Receipt Receipt { get; set; }
        public Product Product { get; set; }
    }
}
