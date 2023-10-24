using Microsoft.AspNetCore.Http;
using QuanLyKho.Models.Entities;

namespace QuanLyKho.Models
{
    public class CreateProductModel
    {
        public Product? Product { get; set; }
        public Category? Category { get; set; }
        public List<IFormFile> Images {  get; set; }

    }
}
