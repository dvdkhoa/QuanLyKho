namespace QuanLyKho.Models.Entities
{
    public class ProductPromotion
    {
        public int PromotionId { get; set; }
        public Promotion Promotion { get; set; }
        public string ProductId { get; set; }
        public Product Product { get; set; }
    }
}
