using System;
using LiteDB;

namespace POS.Models
{
    public class ProductCategory
    {
        [BsonId]
        public int id { get; set; }

        public string Name { get; set; }
    }
}