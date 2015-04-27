using System;
using LiteDB;

namespace POS.Models
{
    public class Product
    {
        public byte[] Image { get; set; }

        public DbRef<ProductCategory> Category { get; set; }

        [BsonId]
        public int _id { get; set; }

        public string ID { get; set; }

        public double Price { get; set; }

        public double Tax { get; set; }

        [BsonIgnore]
        public double TotalPrice
        {
            get
            {
                return this.Price + this.Tax;
            }
        }
    }
}