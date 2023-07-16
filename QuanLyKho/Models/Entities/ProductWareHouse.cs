namespace QuanLyKho.Models.Entities
{
    public class ProductWareHouse
    {
        public int Id { get; set; }
        public string WareHouseId { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public Product Product { get; set; }
        public WareHouse WareHouse { get; set; }
        public Status Status { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime LastUpdated { get; set; }

        public ProductWareHouse()
        {
            this.Status = Status.Show;
        }
    }
}
