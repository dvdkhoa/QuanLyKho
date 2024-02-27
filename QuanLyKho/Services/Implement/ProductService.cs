using Microsoft.EntityFrameworkCore;
using QuanLyKho.DTO;
using QuanLyKho.Models.EF;
using QuanLyKho.Models.Entities;

namespace QuanLyKho.Services.Implement
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public List<Product> GetProductByWarehouseId(string warehouseId)
        {
            var productWarehouse = _context.ProductWareHouses.Where(pw => pw.WareHouseId == warehouseId).Select(pw => pw.ProductId).ToList();

            return _context.Products.Where(p => productWarehouse.Contains(p.Id)).ToList();
        }

        public ProductInfoModel? getProductInfo(string productId)
        {
            var product = _context.Products.Include(i => i.Category)
                                            .Include(i => i.CategoryBrand).ThenInclude(i => i.Brand)
                                            .Include(p => p.ProductImages)
                                            .FirstOrDefault(p => p.Id == productId);

            if (product is null)
                return null;

            int quantity = this.GetProductQuantity(productId);
            List<ProductWareHouse> productWareHouses = _context.ProductWareHouses
                                                                .Include(pw => pw.WareHouse)
                                                                .Where(pw => pw.ProductId == productId).ToList();

            var productInfo = new ProductInfoModel
            {
                Id = product.Id,
                Name = product.Name,
                Category = product.Category,
                CategoryId = product.CategoryId,
                CategoryBrandId = product.CategoryBrandId,
                CategoryBrand = product.CategoryBrand,
                Description = product.Description,
                Price = product.Price,
                Status = product.Status,
                Supplier = product.Supplier,
                Unit = product.Unit,
                Origin = product.Origin,
                Weight = product.Weight,
                ManufacturingDate = product.ManufactoringDate,
                CreatedTime = product.CreatedTime,
                Expiry = product.Expiry,
                PromotionPrice = product.PromotionPrice,
                Quantity = quantity,
                LastUpdated = product.LastUpdated,
                ProductWarehouses = productWareHouses,
                ProductImages = product.ProductImages
            };

            return productInfo;
        }

        public bool CheckQuantityValid(string warehouseId, string productId, int quantity)
        {
            var productWarehouse = _context.ProductWareHouses.Where(pw => pw.WareHouseId == warehouseId).FirstOrDefault(pw => pw.ProductId == productId);

            if (productWarehouse == null)
            {
                return false;
            }

            if (quantity > productWarehouse.Quantity)
                return false;

            return true;
        }

        public int GetProductQuantity(string productId)
        {
            return _context.ProductWareHouses.Where(pw => pw.ProductId == productId).Sum(pw => pw.Quantity);
        }
    }
}
