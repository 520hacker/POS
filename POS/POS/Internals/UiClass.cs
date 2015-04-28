using System;
using System.Linq;
using Telerik.WinControls.UI;

namespace POS.Internals
{
    public class UiClass
    {
        private readonly MainForm mfrm;

        public UiClass(MainForm mfrm)
        {
            this.mfrm = mfrm;
        }

        public void Info(string title, string content)
        {
            mfrm.InfoWindow(title, content);
        }

        public void AddPayButton(string title, dynamic callback)
        {
            var btn = new RadButton();
            btn.Text = title;
            btn.Click += (s, e) => callback(s);

            mfrm.AddPayButton(btn);
        }
    }
}