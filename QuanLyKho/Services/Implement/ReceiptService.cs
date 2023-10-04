using Microsoft.EntityFrameworkCore;
using QuanLyKho.DTO;
using QuanLyKho.Models;
using QuanLyKho.Models.EF;
using QuanLyKho.Models.Entities;

namespace QuanLyKho.Services.Implement
{
    public class ReceiptService : IReceiptService
    {
        private readonly AppDbContext _context;

        public ReceiptService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateImportReceipt(CreateReceiptModel createReceiptModel)
        {
            Receipt receipt = new Receipt { DateCreated = DateTime.Now, StaffId = createReceiptModel.StaffId, Type = ReceiptType.Import, WareHouseId = createReceiptModel.WarehouseId };

            await _context.Receipts.AddAsync(receipt);

            var details = createReceiptModel.Items.Select(item =>
            {
                return new ReceiptDetail { ProductId = item.ProductId, Quantity = item.Quantity, Status = Status.Show, ReceiptId = receipt.Id  };
            }).ToList();

            _context.ReceiptDetails.AddRange(details);

            return _context.SaveChanges() > 0;
        }

        public async Task<ReceiptInfoModel> GetFullInfoReceipt(int receiptId)
        {
            var receipt = await _context.Receipts
               .Include(r => r.Staff)
               .Include(r => r.WareHouse)
               .Include(r=>r.DestinationWarehouse)
               .FirstOrDefaultAsync(m => m.Id == receiptId);

            ReceiptInfoModel receiptInfo = new ReceiptInfoModel
            {
                Receipt = receipt,
                ReceiptDetails = _context.ReceiptDetails.Include(x=>x.Product).Where(x=>x.ReceiptId == receiptId).ToList(),
            };

            return receiptInfo;
        }
    }
}
