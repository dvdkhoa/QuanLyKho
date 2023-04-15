using QuanLyKho.DTO;

namespace QuanLyKho.Services
{
    public interface IStatisticService 
    {
        Task<List<ProductInStock>> GetProductInStocks();
    }
}
