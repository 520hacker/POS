using System;
using System.IO;
using POS.Internals.Database;
using POS.Models;

namespace POS
{
    public class DataStorage
    {
        public static string ProductsPath = string.Format("{0}\\products.bin", ServiceLocator.DataPath);
        public static string ProductCategoriesPath = string.Format("{0}\\productCategories.bin", ServiceLocator.DataPath);
        public static string CouponsPath = string.Format("{0}\\coupons.bin", ServiceLocator.DataPath);
        public static string InvoicesPath = string.Format("{0}\\invoices.bin", ServiceLocator.DataPath);

        public static Product[] ReadProducts()
        {
            if (File.Exists(ProductsPath))
            {
                return DatabaseReader.Read<Product[]>(ProductsPath);
            }
            return new Product[0];
        }

        public static void WriteProducts(Product[] prodcts)
        {
            DatabaseWriter.Write(ProductsPath, prodcts);
        }

        public static Coupon[] ReadCoupons()
        {
            if (File.Exists(ProductsPath))
            {
                return DatabaseReader.Read<Coupon[]>(CouponsPath);
            }
            return new Coupon[0];
        }

        public static void WriteCoupons(Coupon[] prodcts)
        {
            DatabaseWriter.Write(CouponsPath, prodcts);
        }

        public static ProductCategory[] ReadProductCategories()
        {
            if (File.Exists(ProductCategoriesPath))
            {
                return DatabaseReader.Read<ProductCategory[]>(ProductCategoriesPath);
            }
            return new ProductCategory[0];
        }

        public static void WriteProductCategories(ProductCategory[] prodcts)
        {
            DatabaseWriter.Write(ProductCategoriesPath, prodcts);
        }

        public static Invoice[] ReadInvoices()
        {
            if (File.Exists(InvoicesPath))
            {
                return DatabaseReader.Read<Invoice[]>(InvoicesPath);
            }
            return new Invoice[0];
        }

        public static void WriteCoupons(Invoice[] invoices)
        {
            DatabaseWriter.Write(InvoicesPath, invoices);
        }
    }
}