using jsonNewton = Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace QuanLyKho.Models.Entities
{
    public class OrderDetail
    {
        /// <summary>
        /// Getter và Setter cho Id chi tiết hóa đơn
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Getter và Setter cho Id hóa đơn
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// Getter và Setter cho Hóa đơn
        /// </summary>
        [jsonNewton.JsonIgnore]
        [JsonIgnore]
        public Order Order { get; set; }
        /// <summary>
        /// Getter và Setter cho Id sản phẩm
        /// </summary>
        public string ProductId { get; set; }
        /// <summary>
        /// Getter và Setter cho Sản phẩm
        /// </summary>
        public Product Product { get; set; }
        /// <summary>
        /// Getter và Setter cho Hình ảnh sản phẩm
        /// </summary>
        public string Image { get; set; }
        /// <summary>
        /// Getter và Setter cho Tên sản phẩm
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// Getter và Setter cho Số lượng
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// Getter và Setter cho Đơn giá
        /// </summary>
        public double Price { get; set; }
        /// <summary>
        /// Getter và Setter cho Id chi tiết sản phẩm kho
        /// </summary>
        public int? ProductWarehouseId { get; set; }
        /// <summary>
        /// Getter và Setter cho Chi tiết sản phẩm kho
        /// </summary>
        [jsonNewton.JsonIgnore]
        [JsonIgnore]
        public ProductWareHouse? ProductWareHouse { get; set; }
        /// <summary>
        /// Getter và Setter cho Thời gian tạo lần đầu
        /// </summary>
        public DateTime CreatedTime { get; set; }
        /// <summary>
        /// Getter và Setter cho Thời gian cập nhật lần cuối
        /// </summary>
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// Phương thức tính thành tiền
        /// </summary>
        public double getAmount()
        {
            return this.Price * this.Quantity;
        }
    }
}
