using jsonNewton = Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace QuanLyKho.Models.Entities
{
    public class Staff
    {
        /// <summary>
        /// Getter và Setter cho Id nhân viên
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Getter và Setter cho Tên nhân viên
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Getter và Setter cho Ngày sinh
        /// </summary>
        public DateTime DateOfBirth { get; set; }
        /// <summary>
        /// Getter và Setter cho Giới tính
        /// </summary>
        public Gender Gender { get; set; }
        /// <summary>
        /// Getter và Setter cho Điện thoại
        /// </summary>
        public string? PhoneNumber { get; set; }
        /// <summary>
        /// Getter và Setter cho Địa chỉ email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Getter và Setter cho Địa chỉ
        /// </summary>
        public string? Address { get; set; }
        /// <summary>
        /// Getter và Setter cho Ngày vào làm
        /// </summary>
        public DateTime StartDay { get; set; }
        /// <summary>
        /// Getter và Setter cho Id kho/cửa hàng
        /// </summary>
        public string? WareHouseId { get; set; }
        /// <summary>
        /// Getter và Setter cho Id tài khoản
        /// </summary>
        public string? UserId { get; set; }
        /// <summary>
        /// Getter và Setter cho Trạng thái
        /// </summary>
        public Status Status { get; set; }
        /// <summary>
        /// Getter và Setter cho Thời gian tạo lần đầu
        /// </summary>
        public DateTime CreatedTime { get; set; }
        /// <summary>
        /// Getter và Setter cho Thời gian cập nhật lần cuối
        /// </summary>
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// Getter và Setter cho Danh sách hóa đơn đã lập/duyệt
        /// </summary>
        [jsonNewton.JsonIgnore]
        [JsonIgnore]
        /// <summary>
        /// Danh sách hóa đơn đã tạo/duyệt
        /// </summary>
        public List<Order>? Orders { get; set; }


        /// <summary>
        /// Getter và Setter cho Kho/Cửa hàng
        /// </summary>
        [jsonNewton.JsonIgnore]
        [JsonIgnore]
        public WareHouse? WareHouse { get; set; }
        /// <summary>
        /// Getter và Setter cho Tài khoản
        /// </summary>
        [jsonNewton.JsonIgnore]
        [JsonIgnore]
        public AppUser? User { get; set; }

        /// <summary>
        /// Getter và Setter cho Ảnh nhân viên
        /// </summary>
        public string? Image { get; set; }

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
