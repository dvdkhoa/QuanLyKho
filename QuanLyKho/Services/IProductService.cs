using QuanLyKho.DTO;
using QuanLyKho.Models.Entities;

namespace QuanLyKho.Services
{
    public interface IProductService
    {
        ProductInfoModel getProductInfo(string productId);
        List<Product> GetProductByWarehouseId(string warehouseId);
        bool CheckQuantityValid(string warehouseId, string productId, int quantity);
        int GetProductQuantity(string productId);
    }
}
