﻿namespace QuanLyKho.Models.Entities
{
    public class Product
    {
        public String Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public double Price { get; set; }
        public string Unit { get; set; }
        public string Supplier { get; set; }
        public string CategoryId { get; set; }
        public Category Category { get; set; }
        public List<ProductWareHouse> ProductWareHouses { get; set; }
        public List<ReceiptDetail> ReceiptDetails { get; set; }
    }
}