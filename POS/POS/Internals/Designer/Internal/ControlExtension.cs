using System.Drawing;
using System.Windows.Forms;

namespace POS.Internals.Designer.Internal
{
    internal static class ControlExtension
    {
        public static int GetClientWidth(this Control theControl)
        {
            return theControl.Width - Resizer.Decoration;
        }

        public static int GetClientHeight(this Control theControl)
        {
            return theControl.Height - Resizer.Decoration;
        }

        public static Size GetClientSize(this Control theControl)
        {
            return new Size(theControl.GetClientWidth(), theControl.GetClientHeight());
        }

        public static void SetClientWidth(this Control theControl, int width)
        {
            theControl.Width = width + Resizer.Decoration;
        }

        public static void SetClientHeight(this Control theControl, int height)
        {
            theControl.Height = height + Resizer.Decoration;
        }

        public static void SetClientSize(this Control theControl, Size size)
        {
            theControl.SetClientWidth(size.Width);
            theControl.SetClientHeight(size.Height);
        }
    }
}