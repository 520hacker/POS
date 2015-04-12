using System;
using System.IO;
using POS.Internals.Database;
using POS.Models;

namespace POS
{
    public class DataStorage
    {
        public static string ProductsPath = ServiceLocator.DataPath + "\\products.bin";
        public static string ProductCategoriesPath = ServiceLocator.DataPath + "\\productCategories.bin";
        public static string CouponsPath = ServiceLocator.DataPath + "\\coupons.bin";

        public static Product[] ReadProducts()
        {
            if (File.Exists(ProductsPath))
                return DatabaseReader.Read<Product[]>(ProductsPath);
            return new Product[0];
        }

        public static void WriteProducts(Product[] prodcts)
        {
            DatabaseWriter.Write(ProductsPath, prodcts);
        }

        public static Coupon[] ReadCoupons()
        {
            if (File.Exists(ProductsPath))
                return DatabaseReader.Read<Coupon[]>(CouponsPath);
            return new Coupon[0];
        }

        public static void WriteCoupons(Coupon[] prodcts)
        {
            DatabaseWriter.Write(CouponsPath, prodcts);
        }

        public static ProductCategory[] ReadProductCategories()
        {
            if (File.Exists(ProductCategoriesPath))
                return DatabaseReader.Read<ProductCategory[]>(ProductCategoriesPath);
            return new ProductCategory[0];
        }

        public static void WriteProductCategories(ProductCategory[] prodcts)
        {
            DatabaseWriter.Write(ProductCategoriesPath, prodcts);
        }
    }
}