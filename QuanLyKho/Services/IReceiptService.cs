using QuanLyKho.DTO;
using QuanLyKho.Models;

namespace QuanLyKho.Services
{
    public interface IReceiptService
    {
        Task<bool> CreateImportReceipt(CreateReceiptModel createReceiptModel);
        Task<ReceiptInfoModel> GetFullInfoReceipt(int receiptId);
    }
}
