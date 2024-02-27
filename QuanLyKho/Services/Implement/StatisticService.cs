using Microsoft.EntityFrameworkCore;
using QuanLyKho.DTO;
using QuanLyKho.Models.EF;
using QuanLyKho.Models.Entities;
using System.Net.WebSockets;

namespace QuanLyKho.Services.Implement
{
    public class StatisticService : IStatisticService
    {
        private readonly AppDbContext _context;
        private readonly IProductService _productService;

        public StatisticService(AppDbContext context, IProductService productService)
        {
            _context = context;
            _productService = productService;
        }

        public async Task<List<ProductInStock>> GetProductInStocks()
        {
            var productIds = await _context.Products.Select(p => p.Id).ToListAsync();

            var productInStocks = productIds.Select(id =>
            {
                var export = (from rd in _context.ReceiptDetails
                              join rc in _context.Receipts on rd.ReceiptId equals rc.Id
                              where rc.Type == ReceiptType.Export && rd.ProductId == id
                              select rd).Sum(rd => rd.Quantity);

                var import = (from rd in _context.ReceiptDetails
                              join rc in _context.Receipts on rd.ReceiptId equals rc.Id
                              where rc.Type == ReceiptType.Import && rd.ProductId == id
                              select rd).Sum(rd => rd.Quantity);

                var productInfo = _productService.getProductInfo(id);

                return new ProductInStock
                {
                    Id = productInfo.Id,
                    Name = productInfo.Name,
                    CategoryId = productInfo.CategoryId,
                    Description = productInfo.Description,
                    Price = productInfo.Price,
                    Status = productInfo.Status,
                    Supplier = productInfo.Supplier,
                    Unit = productInfo.Unit,
                    InventoryNumber = productInfo.Quantity,
                    Exported = export,
                    Imported = import,
                    InputMoney = productInfo.Price * import,
                    Revenue = productInfo.Price * export
                };
            }).ToList();
            return productInStocks;
        }
    }
}
