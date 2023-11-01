namespace QuanLyKho.Models.Entities
{
    public class Promotion
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public PromotionType PromotionType { get; set; }
        public int? Percent { get; set; }
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime LastUpdated { get; set; }
        public Status Status { get; set; }

        public List<ProductPromotion>? ProductPromotions { get; set; }


        // Khi tạo mới thì gọi
        public void SetCreatedTime()
        {
            this.CreatedTime = DateTime.Now;
            this.LastUpdated = DateTime.Now;
        }

        // Khi cập nhật thì gọi
        public void SetUpdatedTime()
        {
            this.LastUpdated = DateTime.Now;
        }
    }

    public enum PromotionType
    {
        Discount,
        Gift,
    }
}
