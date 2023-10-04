namespace QuanLyKho.Models.Entities
{
    public class New
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Image { get; set; }
        public string Content { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime LastUpdated { get; set; }
        public Status Status { get; set; }


        // Khi tạo mới hoặc chỉnh sửa thì gọi
        public void UpdateTime()
        {
            this.CreatedTime = DateTime.Now;
            this.LastUpdated = DateTime.Now;
        }
    }
}
