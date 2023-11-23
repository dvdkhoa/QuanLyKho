using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text.Json.Serialization;

namespace QuanLyKho.Models.Entities
{
    public class Receipt
    {
        /// <summary>
        /// Getter và Setter cho Id phiếu
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Getter và Setter cho Thời gian tạo lần đầu
        /// </summary>
        public DateTime DateCreated { get; set; }
        /// <summary>
        /// Getter và Setter cho Loại phiếu
        /// </summary>
        public ReceiptType Type { get; set; }
        /// <summary>
        /// Getter và Setter cho Trạng thái
        /// </summary>
        public Status Status { get; set; }
        /// <summary>
        /// Getter và Setter cho Id Kho/Cửa hàng
        /// </summary>
        public string WareHouseId { get; set; }

        /// <summary>
        /// Getter và Setter cho Kho/Cửa hàng
        /// </summary>
        [JsonIgnore]
        [InverseProperty("Receipts")]
        public WareHouse? WareHouse { get; set; }

        /// <summary>
        /// Getter và Setter cho Id Kho/Cửa hàng đích đến
        /// </summary>
        public string? DestinationWarehouseId { get; set; }

        /// <summary>
        /// Getter và Setter cho Kho/Cửa hàng đích đến
        /// </summary>
        [ForeignKey("DestinationWarehouseId")]
        public WareHouse? DestinationWarehouse { get; set; }
        /// <summary>
        /// Getter và Setter cho Id nhân viên
        /// </summary>
        public string StaffId { get; set; }

        /// <summary>
        /// Getter và Setter cho Nhân viên
        /// </summary>
        [JsonIgnore]
        public Staff? Staff { get; set; }
        /// <summary>
        /// Getter và Setter cho Danh sách chi tiết phiếu
        /// </summary>
        public List<ReceiptDetail>? ReceiptDetails { get; set; }

        /// <summary>
        /// Phương thức khởi tạo
        /// </summary>
        public Receipt()
        {
            this.Status = Status.Show;
        }

        /// <summary>
        /// Tính tổng tiền
        /// </summary>
        public string getTotalPrice()
        {
            var total = this.ReceiptDetails?.Sum(x => x.getAmount()).ToString();

            if (string.IsNullOrEmpty(total))
                return null;

            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");   // try with "en-US"
            string totalString = double.Parse(total).ToString("#,###", cul.NumberFormat);

            return totalString;
        }

        // Khi tạo mới thì gọi
        /// <summary>
        /// Set thời gian tạo mới lần đầu
        /// </summary>
        public void SetCreatedTime()
        {
            this.DateCreated = DateTime.Now;
        }

        
    }
}
