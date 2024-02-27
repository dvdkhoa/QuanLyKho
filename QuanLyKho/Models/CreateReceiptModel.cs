
namespace QuanLyKho.Models
{
    public class CreateReceiptModel
    {
        public string StaffId { get; set; }
        public string WarehouseId { get; set; }
        public List<CreateReceiptDetailModel> Items { get; set; }
    }

    public class CreateReceiptDetailModel
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
