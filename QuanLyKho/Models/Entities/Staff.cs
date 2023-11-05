using System.Text.Json.Serialization;

namespace QuanLyKho.Models.Entities
{
    public class Staff
    {
        /// <summary>
        /// Id nhân viên
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Tên nhân viên
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Ngày sinh
        /// </summary>
        public DateTime DateOfBirth { get; set; }
        /// <summary>
        /// Giới tính
        /// </summary>
        public Gender Gender { get; set; }
        /// <summary>
        /// Điện thoại
        /// </summary>
        public string? PhoneNumber { get; set; }
        /// <summary>
        /// Địa chỉ email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Địa chỉ
        /// </summary>
        public string? Address { get; set; }
        /// <summary>
        /// Ngày vào làm
        /// </summary>
        public DateTime StartDay { get; set; }
        /// <summary>
        /// Id kho/cửa hàng
        /// </summary>
        public string? WareHouseId { get; set; }
        /// <summary>
        /// Id tài khoản
        /// </summary>
        public string? UserId { get; set; }
        /// <summary>
        /// Trạng thái
        /// </summary>
        public Status Status { get; set; }
        /// <summary>
        /// Thời gian tạo lần đầu
        /// </summary>
        public DateTime CreatedTime { get; set; }
        /// <summary>
        /// Thời gian cập nhật lần cuối
        /// </summary>
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// Danh sách hóa đơn đã tạo/duyệt
        /// </summary>
        public List<Order>? Orders { get; set; }


        /// <summary>
        /// Kho/Cửa hàng
        /// </summary>
        [JsonIgnore]
        public WareHouse? WareHouse { get; set; }
        /// <summary>
        /// Tài khoản
        /// </summary>
        [JsonIgnore]
        public AppUser? User { get; set; }

        /// <summary>
        /// Phương thức khởi tạo
        /// </summary>
        public Staff()
        {
            this.Status = Status.Show;
        }


        // Khi tạo mới thì gọi
        /// <summary>
        /// Set thời gian tạo mới lần đầu
        /// </summary>
        public void SetCreatedTime()
        {
            this.CreatedTime = DateTime.Now;
            this.LastUpdated = DateTime.Now;
        }

        // Khi cập nhật thì gọi
        /// <summary>
        /// Set thời gian cập nhật sau mỗi lần thay đổi thông tin
        /// </summary>
        public void SetUpdatedTime()
        {
            this.LastUpdated = DateTime.Now;
        }
    }
}
