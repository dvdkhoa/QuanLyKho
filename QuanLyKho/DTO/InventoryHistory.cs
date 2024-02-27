using QuanLyKho.Models.Entities;

namespace QuanLyKho.DTO
{
    public class InventoryHistory
    {
        public Product Product { get; set; }
        public WareHouse WareHouse { get; set; }
        public ReceiptDetail ReceiptDetail { get; set; } 
    }
}
