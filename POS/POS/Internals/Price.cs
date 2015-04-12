using System;
using System.Collections.Generic;
using System.Linq;
using POS.Models;

namespace POS.Internals
{
    public class Price
    {
        public static event EventHandler PriceChanged;

        private static List<Product> prices = new List<Product>();

        public static void Add(Product d)
        {
            prices.Add(d);

            if (PriceChanged != null)
            {
                PriceChanged(d, null);
            }
        }

        public static void Sub(Product d)
        {
            prices.Remove(d);

            if (PriceChanged != null)
            {
                PriceChanged(d, null);
            }
        }

        public static void RemoveLast()
        {
            if(prices.Count > 0)
                prices.RemoveAt(prices.Count -1);

            if (PriceChanged != null)
            {
                PriceChanged(null, null);
            }
        }

        public static string Value { get { return string.Format("{0:0.##}", (from p in prices select p.TotalPrice).Sum()).Replace(",", "."); } }
    }
}