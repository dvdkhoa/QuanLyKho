using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text.Json.Serialization;

namespace QuanLyKho.Models.Entities
{
    public class Receipt
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public ReceiptType Type { get; set; }
        public Status Status { get; set; }
        public string WareHouseId { get; set; }

        [JsonIgnore]
        [InverseProperty("Receipts")]
        public WareHouse? WareHouse { get; set; }

        public string? DestinationWarehouseId { get; set; }

        [ForeignKey("DestinationWarehouseId")]
        public WareHouse? DestinationWarehouse { get; set; }
        public string StaffId { get; set; }

        [JsonIgnore]
        public Staff? Staff { get; set; }
        public List<ReceiptDetail>? ReceiptDetails { get; set; }

        public Receipt()
        {
            this.Status = Status.Show;
        }

        public string getTotalPrice()
        {
            var total = this.ReceiptDetails?.Sum(x => x.getAmount()).ToString();

            if (string.IsNullOrEmpty(total))
                return null;

            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");   // try with "en-US"
            string totalString = double.Parse(total).ToString("#,###", cul.NumberFormat);

            return totalString;
        }
    }
}
