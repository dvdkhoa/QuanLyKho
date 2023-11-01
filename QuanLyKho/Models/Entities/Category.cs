namespace QuanLyKho.Models.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Icon { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime LastUpdated { get; set; }
        public Status Status { get; set; }
        public List<CategoryDetailedConfig>? CategoryDetailedConfigs { get; set; }
        public List<Product>? Products { get; set; }

        public Category() 
        {
            this.Status = Status.Show;
        }



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
}
