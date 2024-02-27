namespace QuanLyKho.Models.Entities
{
    public class Classification
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime LastUpdated { get; set; }
        public Status Status { get; set; }

        public List<ProductClassification>? ProductClassifications { get; set; }
    }
}
