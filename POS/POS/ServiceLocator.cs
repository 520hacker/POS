using System;
using System.Collections.Generic;
using System.Windows.Forms;
using POS.Internals;
using POS.Internals.I18N;
using POS.Models;

namespace POS
{
    public static class ServiceLocator
    {
        public static History<Product> ProductHistory = new History<Product>();

        public static List<ProductCategory> ProductCategories { get; set; }

        public static List<Product> Products { get; set; }

        public static List<Invoice> Invoices { get; set; }

        public static string DataPath
        {
            get
            {
                return string.Format("{0}\\data\\", Application.StartupPath);
            }
        }

        public static Catalog LanguageCatalog = new Catalog();
    }
}