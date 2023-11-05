namespace QuanLyKho.Models.Entities
{
    public class Customer
    {
        /// <summary>
        /// Id khách hàng
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Tên khách hàng
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Địa chỉ
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// Số điện thoại
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// Ngày sinh
        /// </summary>
        public DateTime Dob { get; set; }
        /// <summary>
        /// Địa chỉ email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Thời gian tạo lần đầu
        /// </summary>
        public DateTime CreatedTime { get; set; }
        /// <summary>
        /// Thời gian cập nhật lần cuối
        /// </summary>
        public DateTime LastUpdated { get; set; }
        /// <summary>
        /// Trạng thái
        /// </summary>
        public Status Status { get; set; }
        /// <summary>
        /// Danh sách các hóa đơn đã mua
        /// </summary>
        public List<Order>? Orders { get; set; }
        /// <summary>
        /// Id tài khoản
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// Tài khoản
        /// </summary>
        public AppUser? User { get; set; }


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
