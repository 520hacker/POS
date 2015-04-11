using System;
using System.IO;
using System.Resources;
using System.Windows.Forms;
using System.Reflection;
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
            if (!Directory.Exists(p + "\\themes"))
                Directory.CreateDirectory(p + "\\themes");

            foreach (var f in Directory.GetFiles(p + "\\themes", "*.tssp", SearchOption.AllDirectories))
            {
                ThemeResolutionService.LoadPackageFile(f);

                ThemeResolutionService.ApplicationThemeName = "Office2013Light";
            }

            var frm = new MainForm();
            frm.Show();

            //hide this form
            this.Hide();
        }
    }
}