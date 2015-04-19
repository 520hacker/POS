using System;
using POS.Internals;

namespace POS.Models
{
    public class Product : IDBObject<Product>
    {
        public int Category { get; set; }

        public byte[] Image { get; set; }

        public string ID { get; set; }

        public double Price { get; set; }

        public double Tax { get; set; }

        public Product From(dynamic d)
        {
            var p = new Product();

            p.Category = d.Category;
            p.Price = d.Price;
            p.Tax = d.Tax;

            return p;
        }

        public double TotalPrice
        {
            get
            {
                return this.Price + this.Tax;
            }
        }
    }
}