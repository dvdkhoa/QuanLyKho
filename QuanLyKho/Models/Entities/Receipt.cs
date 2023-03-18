namespace QuanLyKho.Models.Entities
{
    public class Receipt
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public ReceiptType Type { get; set; }
        public string WareHouseId { get; set; }
        public WareHouse WareHouse { get; set; }
        public string StaffId { get; set; }
        public Staff Staff { get; set; }
        public List<ReceiptDetail> ReceiptDetails { get; set; }
    }
}
