using System;
using System.IO;
using System.Resources;
using System.Windows.Forms;
using System.Reflection;

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
            // Get a reference to the entry assembly (Startup.exe)
            Assembly exe = typeof(Program).Assembly;

            var frm = new MainForm();
            frm.Show();

            //hide this form
            this.Hide();
        }
    }
}