using System;
using System.Linq;
using System.Windows.Forms;

namespace POS.Internals
{
    public class UiClass
    {
        private readonly MainForm mfrm;

        public UiClass(MainForm mfrm)
        {
            this.mfrm = mfrm;
        }

        public void AddPayButton(Button btn)
        {
            mfrm.AddPayButton(btn);
        }
    }
}