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

        public static LiteDatabase DB;

        public static bool IsOpened { get; set; }

        public static void Open(string filename)
        {
            DB = new LiteDatabase("Filename=" + filename);
            IsOpened = true;
            DB.BeginTrans();
        }

        public static LiteCollection<T> GetCollection<T>(string name)
            where T : new()
        {
            return DB.GetCollection<T>(name);
        }

        public static void Close()
        {
            DB.Commit();

            DB.Dispose();
            IsOpened = false;
        }

        public static void Commit()
        {
            DB.Commit();
        }
    }
}