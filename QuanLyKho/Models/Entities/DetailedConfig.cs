namespace QuanLyKho.Models.Entities
{
    public class DetailedConfig
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Status Status { get; set; }

        public List<CategoryDetailedConfig> CategoryDetailedConfigs { get; set; }
        public List<ProductDetailedConfig> ProductDetailedConfigs { get; set; }

    }
}
