using System;
using System.Windows.Forms;

namespace POS
{
    public partial class MainForm : Telerik.WinControls.UI.RadForm
    {
        public MainForm()
        {
            InitializeComponent();

            Cursor.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            QrCodeDialog.Show("bitcoin:address?amount=100&label=this is a test", this);
        }
    }
}