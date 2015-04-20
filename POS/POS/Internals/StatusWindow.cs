using System;
using System.Windows.Forms;

namespace POS.Internals
{
    public partial class StatusWindow : Form
    {
        public StatusWindow()
        {
            InitializeComponent();
        }

        public void SetDisplay(Control c)
        {
            Controls.Clear();

            c.Dock = DockStyle.Fill;

            Controls.Add(c);
        }
    }
}