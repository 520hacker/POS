using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using POS.Internals;
using POS.Models;
using POS.Properties;
using Pos.Internals.Extensions;
using Telerik.WinControls;

namespace POS
{
    public partial class SplashScreen : Form
    {
        public SplashScreen()
        {
            InitializeComponent();
        }

        //Use timer class
        private Timer tmr;

        private void SplashScreen_Shown(object sender, EventArgs e)
        {
            tmr = new Timer();
            //set time interval 3 sec
            tmr.Interval = 1000;
            //starts the timer
            tmr.Start();
            tmr.Tick += tmr_Tick;
        }

        private void tmr_Tick(object sender, EventArgs e)
        {
            tmr.Stop();

            var p = Application.StartupPath;

            if (!Directory.Exists(p + "\\data"))
                Directory.CreateDirectory(p + "\\data");
            if (!Directory.Exists(p + "\\data\\invoices"))
                Directory.CreateDirectory(p + "\\data\\invoices");
            if (!Directory.Exists(p + "\\themes"))
                Directory.CreateDirectory(p + "\\themes");

            foreach (var f in Directory.GetFiles(p + "\\themes", "*.tssp", SearchOption.AllDirectories))
            {
                ThemeResolutionService.LoadPackageFile(f);

                ThemeResolutionService.ApplicationThemeName = "Office2013Light";
            }

            ServiceLocator.ProductCategories = DataStorage.ReadProductCategories();
            ServiceLocator.Products = DataStorage.ReadProducts();
            ServiceLocator.Invoices = DataStorage.ReadInvoices();

            var t = new List<Product>(ServiceLocator.Products);
            t.Add(new Product { Category = 0, ID = "Rose", Price = 0.81, Tax = 0.19, Image = Resources.box.ToBytes(ImageFormat.Png) });

            ServiceLocator.Products = t.ToArray();

            SqlHelper.select_db(ServiceLocator.DataPath + "data");
            SqlHelper.connect("sa", "");

            var pr = DbContext.AddTable<Product>();
            
            SqlHelper.query(pr);
            SqlHelper.query("INSERT INTO Products VALUES (0, null, 1.23, null,null)");

            SqlHelper.commit();

            var r = DbContext.GetItems<Product>();

            SqlHelper.close();

            var frm = new MainForm();
            frm.Show();

            //hide this form
            this.Hide();
        }
    }
}