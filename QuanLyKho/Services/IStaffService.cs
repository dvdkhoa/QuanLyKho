using QuanLyKho.Models.Entities;

namespace QuanLyKho.Services
{
    public interface IStaffService
    {
        bool CheckStaffExistsById(string id);
        string GetStaffIdByName(Staff staff);

        Task<Staff> CreateStaff(Staff staff);
    }
}

