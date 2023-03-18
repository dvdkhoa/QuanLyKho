﻿namespace QuanLyKho.Models.Entities
{
    public class WareHouse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public List<ProductWareHouse> ProductWareHouses { get; set; }
        public List<Staff> Staffs { get; set; }
        public List<Receipt> Receipts { get; set; }

    }
}