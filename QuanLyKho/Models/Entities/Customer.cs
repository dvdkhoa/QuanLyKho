namespace QuanLyKho.Models.Entities
{
    public class Customer
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public DateTime Dob { get; set; }
        public string Email { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime LastUpdated { get; set; }
        public Status Status { get; set; }
        public List<Order> Orders { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }  

    }
}
