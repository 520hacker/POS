using System;
using System.IO;
using POS.Internals.Database;
using POS.Models;

namespace POS
{
    public class DataStorage
    {
        const string ProductsPath = "\\data\\products.bin";
        const string CouponsPath = "\\data\\coupons.bin";

        public static Product[] ReadProducts()
        {
            if (File.Exists(ProductsPath))
                return DatabaseReader.Read<Product[]>(ProductsPath);
            return null;
        }
        public static void WriteProducts(Product[] prodcts)
        {
            DatabaseWriter.Write(ProductsPath, prodcts);
        }

        public static Coupon[] ReadCoupons()
        {
            if (File.Exists(ProductsPath))
                return DatabaseReader.Read<Coupon[]>(CouponsPath);
            return null;
        }
        public static void WriteCoupons(Coupon[] prodcts)
        {
            DatabaseWriter.Write(CouponsPath, prodcts);
        }
    }
}