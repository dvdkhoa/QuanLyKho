namespace QuanLyKho.Models.Entities
{
    public class CartDetail
    {
        public int CartId { get; set; }
        public Cart Cart { get; set; }
        public string ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; }
        public double ProducPrice { get; set; }
        public string ProductImage { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime LastUpdated { get; set;}
        public Status Status { get; set; }
    }
}
