using System;

namespace POS.Models
{
    public class Product
    {
        public string ID { get; set; }
        public decimal Price { get; set; }
        public decimal Tax { get; set; }
        public decimal TotalPrice { get { return Price + Tax; } }
    }
}