namespace QuanLyKho.Models.Entities
{
    public class Staff
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public DateTime StartDay { get; set; }
        public string? WareHouseId { get; set; }
        public string? UserId { get; set; }
        public WareHouse? WareHouse { get; set; }
        public AppUser? User { get; set; }
    }
}
