using System;
using System.Linq;
using LiteDB;
using POS.Models;

namespace POS
{
    public class DbContext
    {
        public static LiteCollection<Coupon> CouponCollection
        {
            get
            {
                if (IsOpened)
                    return GetCollection<Coupon>("Coupons");
                return null;
            }
        }

        public static LiteCollection<Invoice> InvoiceCollection
        {
            get
            {
                if (IsOpened)
                    return GetCollection<Invoice>("Invoices");
                return null;
            }
        }

        public static LiteCollection<Product> ProductCollection
        {
            get
            {
                if (IsOpened)
                    return GetCollection<Product>("Products");
                return null;
            }
        }

        public static LiteCollection<ProductCategory> ProductCategoryCollection
        {
            get
            {
                if (IsOpened)
                    return GetCollection<ProductCategory>("ProductCategories");
                return null;
            }
        }

        private static LiteDatabase db;

        public static bool IsOpened { get; set; }

        public static void Open(string filename)
        {
            db = new LiteDatabase("Filename=" + filename);
            IsOpened = true;
            db.BeginTrans();
        }

        public static LiteCollection<T> GetCollection<T>(string name)
            where T : new()
        {
            return db.GetCollection<T>(name);
        }

        public static void Close()
        {
            db.Commit();

            db.Dispose();
            IsOpened = false;
        }
    }
}