using System;
using System.Windows.Forms;
using POS.Properties;

namespace POS.Internals.Designer.Internal
{
    public class SmartTagButton : PictureBox
    {
        public SmartTagButton()
        {
            Image = Resources.smarttag;
            SizeMode = PictureBoxSizeMode.AutoSize;

            MouseLeave += (sender, e) => { Image = Resources.smarttag; };
            MouseHover += (sender, e) => { Image = Resources.smarttag_hover; };
        }
    }
}
