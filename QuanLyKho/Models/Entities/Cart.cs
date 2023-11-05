namespace QuanLyKho.Models.Entities
{
    public class Cart
    {
        /// <summary>
        /// Id giỏ hàng
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Id khách hàng
        /// </summary>
        public string CustomerId { get; set; }
        /// <summary>
        /// Khách hàng
        /// </summary>
        public Customer Customer { get; set; }

        /// <summary>
        /// Danh sách chi tiết giỏ hàng
        /// </summary>
        public List<CartDetail> CartDetails { get; set; }
    }
}
