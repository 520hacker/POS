using System;

namespace POS.Models
{
    public class Product
    {
        public int Category { get; set; }
        public byte[] Image { get; set; }
        public string ID { get; set; }
        public double Price { get; set; }
        public double Tax { get; set; }
        public double TotalPrice { get { return Price + Tax; } }
    }
}