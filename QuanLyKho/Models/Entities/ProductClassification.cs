namespace QuanLyKho.Models.Entities
{
    public class ProductClassification
    {
        public int Id { get; set; }
        public int ClassificationId { get; set; }
        public Classification Classification { get; set; }
        public string ProductId { get; set; }
        public Product Product { get; set; }
    }
}
