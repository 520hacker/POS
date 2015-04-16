using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.ClearScript;
using Microsoft.ClearScript.Windows;
using POS.Internals;
using POS.Internals.ScriptEngine;
using POS.Internals.ScriptEngine.ModuleSystem;
using POS.Internals.ScriptEngine.ModuleSystem;
using POS.Models;
using POS.Properties;
using Pos.Internals.Extensions;
using Telerik.WinControls;

namespace POS
{
    public partial class SplashScreen : Form
    {
        //Use timer class
        private Timer tmr;

        public SplashScreen()
        {
            this.InitializeComponent();
        }

        private void SplashScreen_Shown(object sender, EventArgs e)
        {
            this.tmr = new Timer();
            //set time interval 3 sec
            this.tmr.Interval = 1000;
            //starts the timer
            this.tmr.Start();
            this.tmr.Tick += this.tmr_Tick;
        }

        private void tmr_Tick(object sender, EventArgs e)
        {
            this.tmr.Stop();

            var p = Application.StartupPath;

            var ps = PluginLoader.Load(Application.StartupPath + "\\Plugins");
            var fs = PluginLoader.Call("init");
            
            if (!Directory.Exists(string.Format("{0}\\data", p)))
            {
                Directory.CreateDirectory(string.Format("{0}\\data", p));
            }
            if (!Directory.Exists(string.Format("{0}\\data\\invoices", p)))
            {
                Directory.CreateDirectory(string.Format("{0}\\data\\invoices", p));
            }
            if (!Directory.Exists(string.Format("{0}\\themes", p)))
            {
                Directory.CreateDirectory(string.Format("{0}\\themes", p));
            }

            foreach (var f in Directory.GetFiles(string.Format("{0}\\themes", p), "*.tssp", SearchOption.AllDirectories))
            {
                ThemeResolutionService.LoadPackageFile(f);
            }

            ServiceLocator.ProductCategories = DataStorage.ReadProductCategories();
            ServiceLocator.Products = DataStorage.ReadProducts();
            ServiceLocator.Invoices = DataStorage.ReadInvoices();

            var t = new List<Product>(ServiceLocator.Products);
            t.Add(new Product { Category = 0, ID = "Rose", Price = 0.81, Tax = 0.19, Image = Resources.box.ToBytes(ImageFormat.Png) });

            ServiceLocator.Products = t.ToArray();

            SqlHelper.select_db(string.Format("{0}data", ServiceLocator.DataPath));
            SqlHelper.connect("sa", "");

            var pr = DbContext.AddTable<Product>();
            
            SqlHelper.query(pr);

            string base64 = "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";
            SqlHelper.query(string.Format("INSERT INTO Products VALUES (0, null, {0}, 1.23, null)", base64));

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