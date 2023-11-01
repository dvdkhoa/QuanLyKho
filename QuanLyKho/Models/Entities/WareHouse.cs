
using System.Text.Json.Serialization;

namespace QuanLyKho.Models.Entities
{
    public class WareHouse
    {
        public string Id { get; set; }

        public string Name { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public List<ProductWareHouse>? ProductWareHouses { get; set; }

        [JsonIgnore]
        public List<Staff>? Staffs { get; set; }
        public List<Receipt>? Receipts { get; set; }
        public Status Status { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? LastUpdated { get; set; }
        public List<Order>? Orders { get; set; }


        public WareHouse()
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

    public enum WarehouseType
    {
        Warehouse,
        Store
    }
}
