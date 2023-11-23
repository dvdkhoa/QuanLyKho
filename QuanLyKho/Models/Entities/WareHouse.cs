using jsonNewton = Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace QuanLyKho.Models.Entities
{
    public class WareHouse
    {
        /// <summary>
        /// Getter và Setter cho Id kho/cửa hàng
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Getter và Setter cho Tên kho
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Getter và Setter cho Số điện thoại
        /// </summary>
        public string? PhoneNumber { get; set; }
        /// <summary>
        /// Getter và Setter cho Địa chỉ
        /// </summary>
        public string? Address { get; set; }
        /// <summary>
        /// Getter và Setter cho Danh sách sản phẩm kho
        /// </summary>
        public List<ProductWareHouse>? ProductWareHouses { get; set; }

        /// <summary>
        /// Getter và Setter cho Danh sách nhân viên
        /// </summary>
        [jsonNewton.JsonIgnore]
        [JsonIgnore]
        public List<Staff>? Staffs { get; set; }
        /// <summary>
        /// Getter và Setter cho Danh sách phiếu nhập/xuất/chuyển kho
        /// </summary>
        [jsonNewton.JsonIgnore]
        [JsonIgnore]
        public List<Receipt>? Receipts { get; set; }
        /// <summary>
        /// Getter và Setter cho Trạng thái
        /// </summary>
        public Status Status { get; set; }
        /// <summary>
        /// Getter và Setter cho Thời gian tạo lần đầu
        /// </summary>
        public DateTime? CreatedTime { get; set; }
        /// <summary>
        /// Getter và Setter cho Thời gian lần cuối cập nhật
        /// </summary>
        public DateTime? LastUpdated { get; set; }
        /// <summary>
        /// Getter và Setter cho Danh sách đơn hàng
        /// </summary>
        [jsonNewton.JsonIgnore]
        [JsonIgnore]
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
