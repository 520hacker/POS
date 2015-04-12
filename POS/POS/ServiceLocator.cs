using System;
using System.Windows.Forms;
using POS.Models;

namespace POS
{
    public static class ServiceLocator
    {
        public static ProductCategory[] ProductCategories { get; set; }
        public static Product[] Products { get; set; }

        public static string DataPath
        {
            get
            {
                return Application.StartupPath + "\\data\\";
            }
        }
    }
}