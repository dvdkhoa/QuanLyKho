using Newtonsoft.Json;
using System.Globalization;

namespace QuanLyKho.Models.Entities
{
    public class ReceiptDetail
    {
        public int Id { get; set; }
        public int ReceiptId { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public Status Status { get; set; }

        [JsonIgnore]
        public Receipt Receipt { get; set; }

        [JsonIgnore]
        public Product Product { get; set; }

        public ReceiptDetail()
        {
            this.Status = Status.Show;
        }

        public double getAmount()
        {
            return this.Quantity * this.Product.Price;
        }

        public string getAmountToVND()
        {
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");   // try with "en-US"
            string amountStr = double.Parse(this.getAmount().ToString()).ToString("#,###", cul.NumberFormat);

            return amountStr;
        }
    }
}
