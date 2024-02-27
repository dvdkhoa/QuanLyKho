
using Newtonsoft.Json;
using QuanLyKho.Models.Entities;
using System.Text.Json.Serialization;

namespace QuanLyKho.DTO
{
    
    public class ProductStatisticInfoModel
    {
        public String Id { get; set; }
        public string Name { get; set; }
        public Status Status { get; set; }
        public double Price { get; set; }
        public string? Unit { get; set; }
        public int? Imported { get; set; }
        public string InputMoney { get; set; }
        public int? InventoryNumber { get; set; }

        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? DateImported { get; set; }
        public string WarehouseId { get; set; }

    }
}
