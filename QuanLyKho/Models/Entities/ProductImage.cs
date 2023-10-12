namespace QuanLyKho.Models.Entities
{
    public class ProductImage
    {
        public int Id { get; set; }
        public string ProductId { get; set; }
        public string Path { get; set; }
        public string? Description { get; set; }
        public int? Order { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime LastUpdated { get; set; }
        public Status Status { get; set; }
        public Product Product { get; set; }


        // Khi tạo mới hoặc chỉnh sửa thì gọi
        public void UpdateTime()
        {
            this.CreatedTime = DateTime.Now;
            this.LastUpdated = DateTime.Now;
        }
    }
}
