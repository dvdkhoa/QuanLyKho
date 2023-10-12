namespace QuanLyKho.Models.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string? CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Note { get; set; }
        public string? PaymentMethod { get; set; }
        public double TmpTotal { get; set; }
        public double Total { get; set; }
        public string StaffId { get; set; }
        public string? StoreId { get; set; }
        public int ShipStatus { get; set; }
        public int PaymentStatus { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime LastUpdated { get; set; }

        public Staff Staff { get; set; }
        public Customer Customer { get; set; }
        public WareHouse Store { get; set; }

        public List<Bill>? Bills { get; set; }
        public List<VnPay>? VnPays { get; set; }


    }
}
