using AutoMapper;
using QuanLyKho.Areas.Admin.Pages.Role;
using QuanLyKho.DTO;
using QuanLyKho.Models.Entities;

namespace QuanLyKho.Mappers
{
    public class ProductMapper : Profile
    {
        public ProductMapper()
        {
            CreateMap<EditProductModel, Product>();
            CreateMap<Product, EditProductModel > ();
        }
    }
}
