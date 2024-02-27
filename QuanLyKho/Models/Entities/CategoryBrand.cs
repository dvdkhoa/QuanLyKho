namespace QuanLyKho.Models.Entities
{
    public class CategoryBrand
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public int BrandId { get; set; }
        public Brand? Brand { get; set; }
        public List<Product>? Products { get; set; }

    }
}
