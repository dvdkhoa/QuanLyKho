namespace QuanLyKho.Models.Entities
{
    public class ProductPromotion
    {
        /// <summary>
        /// Getter và Setter cho Id CTKM
        /// </summary>
        public int PromotionId { get; set; }
        /// <summary>
        /// Getter và Setter cho CTKM
        /// </summary>
        public Promotion Promotion { get; set; }
        /// <summary>
        /// Getter và Setter cho Id sản phẩm
        /// </summary>
        public string ProductId { get; set; }
        /// <summary>
        /// Getter và Setter cho Sản phẩm
        /// </summary>
        public Product Product { get; set; }
    }
}
