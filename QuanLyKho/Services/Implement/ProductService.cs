﻿using Microsoft.EntityFrameworkCore;
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
            var product = _context.Products.Include(i => i.Category).FirstOrDefault(p => p.Id == productId);

            if (product is null)
                return null;

            int quantity = _context.ProductWareHouses.Where(pw => pw.ProductId == productId).Sum(pw => pw.Quantity);
            List<ProductWareHouse> productWareHouses = _context.ProductWareHouses.Where(pw => pw.ProductId == productId).ToList();

            var productInfo = new ProductInfoModel
            {
                Id = product.Id,
                Name = product.Name,
                Category = product.Category,
                CategoryId = product.CategoryId,
                Description = product.Description,
                Price = product.Price,
                Status = product.Status,
                Supplier = product.Supplier,
                Unit = product.Unit,
                Quantity = quantity,
                ProductWarehouses = productWareHouses
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
    }
}