namespace QuanLyKho.Models.Entities
{
    public class Promotion
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Percent { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime LastUpdated { get; set; }
        public Status Status { get; set; }

        public List<ProductPromotion> ProductPromotions { get; set; }
    }
}
