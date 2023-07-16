namespace QuanLyKho.Models.Entities
{
    public class Cart
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public List<CartDetail> CartDetails { get; set; }
    }
}
