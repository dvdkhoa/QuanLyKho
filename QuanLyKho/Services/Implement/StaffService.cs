using QuanLyKho.Extensions;
using QuanLyKho.Models.EF;
using QuanLyKho.Models.Entities;

namespace QuanLyKho.Services.Implement
{
    public class StaffService : IStaffService
    {
        private readonly AppDbContext _context;

        public StaffService(AppDbContext context)
        {
            _context = context;
        }

        public bool CheckStaffExistsById(string id)
        {
            return _context.Staffs.Any(s => s.Id == id);
        }

        public async Task<Staff> CreateStaff(Staff staff)
        {
            var id = this.GetStaffIdByName(staff); // sinh mã id dựa vào chữ cái đầu của tên

            staff.Id = id;

            staff.SetCreatedTime();

            _context.Add(staff);

            var kq = await _context.SaveChangesAsync() > 0;

            if (kq)
                return staff;
            return null;
        }


        public string GetStaffIdByName(Staff staff)
        {
            var id = Helpers.GetFirstLetterInString(staff.Name);

            var exists = this.CheckStaffExistsById(id);
            if (exists)
            {
                string year = staff.DateOfBirth.Year.ToString();

                int id_tail = Helpers.GetLastTwoDigitsOfYear(year);

                var id_temp = id + id_tail.ToString();

                var exists2 = this.CheckStaffExistsById(id_temp);
                if (exists2)
                    return id + Helpers.GetRandomTwoDigitNumberMinusGivenNumber(id_tail).ToString();
                return id_temp;
            }
            return id;
        }




    }
}
