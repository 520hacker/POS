using System;
using System.Windows.Forms;
using POS.Internals;
using POS.Models;

namespace POS
{
    public static class ServiceLocator
    {
        public static History<Product> ProductHistory = new History<Product>();

        public static ProductCategory[] ProductCategories { get; set; }

        public static Product[] Products { get; set; }

        public static Invoice[] Invoices { get; set; }

        public static string DataPath
        {
            get
            {
                return string.Format("{0}\\data\\", Application.StartupPath);
            }
        }
    }
}