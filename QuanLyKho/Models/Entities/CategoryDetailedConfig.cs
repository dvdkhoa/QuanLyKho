namespace QuanLyKho.Models.Entities
{
    
    public class CategoryDetailedConfig
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int ConfigId { get; set; }
        public DetailedConfig DetailedConfig { get; set; }
        public Status Status { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime LastUpdated { get; set; }


        // Khi tạo mới hoặc chỉnh sửa thì gọi
        public void UpdateTime()
        {
            this.CreatedTime = DateTime.Now;
            this.LastUpdated = DateTime.Now;
        }
    }
}
