namespace QuanLyKho.Models.Entities
{
    public class ProductPromotion
    {
        /// <summary>
        /// Id CTKM
        /// </summary>
        public int PromotionId { get; set; }
        /// <summary>
        /// CTKM
        /// </summary>
        public Promotion Promotion { get; set; }
        /// <summary>
        /// Id sản phẩm
        /// </summary>
        public string ProductId { get; set; }
        /// <summary>
        /// Sản phẩm
        /// </summary>
        public Product Product { get; set; }
    }
}
