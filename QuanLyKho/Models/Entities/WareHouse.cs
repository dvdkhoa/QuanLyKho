
using System.Text.Json.Serialization;

namespace QuanLyKho.Models.Entities
{
    public class WareHouse
    {
        /// <summary>
        /// Id kho/cửa hàng
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Tên kho
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Số điện thoại
        /// </summary>
        public string? PhoneNumber { get; set; }
        /// <summary>
        /// Địa chỉ
        /// </summary>
        public string? Address { get; set; }
        /// <summary>
        /// Danh sách sản phẩm kho
        /// </summary>
        public List<ProductWareHouse>? ProductWareHouses { get; set; }

        /// <summary>
        /// Danh sách nhân viên
        /// </summary>
        [JsonIgnore]
        public List<Staff>? Staffs { get; set; }
        /// <summary>
        /// Danh sách phiếu nhập/xuất/chuyển kho
        /// </summary>
        public List<Receipt>? Receipts { get; set; }
        /// <summary>
        /// Trạng thái
        /// </summary>
        public Status Status { get; set; }
        /// <summary>
        /// Thời gian tạo lần đầu
        /// </summary>
        public DateTime? CreatedTime { get; set; }
        /// <summary>
        /// Thời gian lần cuối cập nhật
        /// </summary>
        public DateTime? LastUpdated { get; set; }
        /// <summary>
        /// Danh sách đơn hàng
        /// </summary>
        public List<Order>? Orders { get; set; }


        /// <summary>
        /// Phương thức khởi tạo
        /// </summary>
        public WareHouse()
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

    public enum WarehouseType
    {
        Warehouse,
        Store
    }
}
