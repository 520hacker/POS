using System;
using System.Collections.Generic;
using System.Linq;
using POS.Internals;

namespace POS.Models
{
    public class Invoice
    {
        public enum InvoiceCurrency
        {
            BTC,
            EUR
        };

        public string ID { get; set; }

        public DateTime Date { get; set; }

        public List<Product> Products { get; set; }

        public double TotalPrice { get; set; }

        public InvoiceCurrency Currency { get; set; }
        
        public static Invoice New()
        {
            var ret = new Invoice();

            ret.Date = DateTime.Now;
            ret.Products = new List<Product>();
            ret.ID = VoucherID.NewID().ToString();

            return ret;
        }
    }
}