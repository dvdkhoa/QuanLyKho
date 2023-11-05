using jsonNewton = Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace QuanLyKho.Models.Entities
{
    public class OrderDetail
    {
        /// <summary>
        /// Id chi tiết hóa đơn
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Id hóa đơn
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// Hóa đơn
        /// </summary>
        [jsonNewton.JsonIgnore]
        [JsonIgnore]
        public Order Order { get; set; }
        /// <summary>
        /// Id sản phẩm
        /// </summary>
        public string ProductId { get; set; }
        /// <summary>
        /// Sản phẩm
        /// </summary>
        public Product Product { get; set; }
        /// <summary>
        /// Hình ảnh sản phẩm
        /// </summary>
        public string Image { get; set; }
        /// <summary>
        /// Tên sản phẩm
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// Số lượng
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// Đơn giá
        /// </summary>
        public double Price { get; set; }
        /// <summary>
        /// Id chi tiết sản phẩm kho
        /// </summary>
        public int? ProductWarehouseId { get; set; }
        /// <summary>
        /// Chi tiết sản phẩm kho
        /// </summary>
        [jsonNewton.JsonIgnore]
        [JsonIgnore]
        public ProductWareHouse? ProductWareHouse { get; set; }
        /// <summary>
        /// Thời gian tạo lần đầu
        /// </summary>
        public DateTime CreatedTime { get; set; }
        /// <summary>
        /// Thời gian cập nhật lần cuối
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
