namespace QuanLyKho.Models.Entities
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string? Description { get; set; }
        public Status Status { get; set; }
        public List<Product>? Products { get; set; }

        public Category() 
        {
            this.Status = Status.Show;
        }
    }
}
