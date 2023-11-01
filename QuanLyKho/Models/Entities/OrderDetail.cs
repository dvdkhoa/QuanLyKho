﻿using jsonNewton = Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace QuanLyKho.Models.Entities
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        [jsonNewton.JsonIgnore]
        [JsonIgnore]
        public Order Order { get; set; }
        public string ProductId { get; set; }
        public Product Product { get; set; }
        public string Image { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public int? ProductWarehouseId { get; set; }
        [jsonNewton.JsonIgnore]
        [JsonIgnore]
        public ProductWareHouse? ProductWareHouse { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime LastUpdated { get; set; }

        public double getAmount()
        {
            return this.Price * this.Quantity;
        }
    }
}
