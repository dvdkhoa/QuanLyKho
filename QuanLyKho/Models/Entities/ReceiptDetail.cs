using Newtonsoft.Json;
using System.Globalization;

namespace QuanLyKho.Models.Entities
{
    public class ReceiptDetail
    {
        /// <summary>
        /// Getter và Setter cho Id chi tiết phiếu nhập/xuất/chuyển kho
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Getter và Setter cho Id phiếu nhập/xuất/chuyển kho
        /// </summary>
        public int ReceiptId { get; set; }
        /// <summary>
        /// Getter và Setter cho Id sản phẩm
        /// </summary>
        public string ProductId { get; set; }
        /// <summary>
        /// Getter và Setter cho Số lượng
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// Getter và Setter cho Trạng thái
        /// </summary>
        public Status Status { get; set; }

        /// <summary>
        /// Getter và Setter cho Phiếu nhập/xuất/chuyển kho
        /// </summary>
        [JsonIgnore]
        public Receipt Receipt { get; set; }

        /// <summary>
        /// Getter và Setter cho Sản phẩm
        /// </summary>
        [JsonIgnore]
        public Product Product { get; set; }

        /// <summary>
        /// Phương thức khởi tạo
        /// </summary>
        public ReceiptDetail()
        {
            this.Status = Status.Show;
        }

        /// <summary>
        /// Phương thức tính thành tiền
        /// </summary>
        public double getAmount()
        {
            return this.Quantity * this.Product.Price;
        }

        /// <summary>
        /// Phương thức tính thành tiền trả về chuỗi format tiền Việt Nam
        /// </summary>
        public string getAmountToVND()
        {
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");   // try with "en-US"
            string amountStr = double.Parse(this.getAmount().ToString()).ToString("#,###", cul.NumberFormat);

            return amountStr;
        }
    }
}
