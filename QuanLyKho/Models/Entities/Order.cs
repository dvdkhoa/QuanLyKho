namespace QuanLyKho.Models.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string? CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Note { get; set; }
        public string? PaymentMethod { get; set; }
        public double TmpTotal { get; set; }
        public double Total { get; set; }
        public string StaffId { get; set; }
        public string? StoreId { get; set; }
        public ShipStatus ShipStatus { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime LastUpdated { get; set; }

        public Staff? Staff { get; set; }
        public Customer? Customer { get; set; }
        public WareHouse? Store { get; set; }

        public List<OrderDetail>? OrderDetails { get; set; }


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

    public enum PaymentStatus
    {
        Unpaid,
        Paid
    }


    public enum ShipStatus
    {
        NotApproved = 0,
        BeingShipped = 1,
        Success = 2,
        Canceled = -1
    }


    public class PaymentMethod
    {
        public const string COD = "COD";
        public const string VnPay = "VNPAY";
        public const string Direct = "Direct";
    }
}
