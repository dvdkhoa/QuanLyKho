namespace QuanLyKho.Models.Entities
{
    public class Cart
    {
        /// <summary>
        /// Getter và Setter cho Id giỏ hàng
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Getter và Setter cho Id khách hàng
        /// </summary>
        public string CustomerId { get; set; }
        /// <summary>
        /// Getter và Setter cho Khách hàng
        /// </summary>
        public Customer Customer { get; set; }

        /// <summary>
        /// Getter và Setter cho Danh sách chi tiết giỏ hàng
        /// </summary>
        public List<CartDetail> CartDetails { get; set; }
    }
}
