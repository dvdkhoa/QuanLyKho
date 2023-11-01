using System.Text.Json.Serialization;

namespace QuanLyKho.Models.Entities
{
    public class Staff
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string? PhoneNumber { get; set; }
        public string Email { get; set; }
        public string? Address { get; set; }
        public DateTime StartDay { get; set; }
        public string? WareHouseId { get; set; }
        public string? UserId { get; set; }
        public Status Status { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime LastUpdated { get; set; }

        public List<Order>? Orders { get; set; }


        [JsonIgnore]
        public WareHouse? WareHouse { get; set; }
        [JsonIgnore]
        public AppUser? User { get; set; }

        public Staff()
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
