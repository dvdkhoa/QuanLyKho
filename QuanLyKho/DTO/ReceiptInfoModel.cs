using QuanLyKho.Models.Entities;

namespace QuanLyKho.DTO
{
    public class ReceiptInfoModel
    {
        public Receipt Receipt { get; set; }
        public List<ReceiptDetail> ReceiptDetails { get; set; } = new List<ReceiptDetail>(); 
    }
}
