using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace POS.Internals
{
    public class WindowManager
    {
        public static bool TwoMonitors
        {
            get
            {
                return Screen.AllScreens.Count() > 1;
            }
        }

        private static StatusWindow _sw;

        public static StatusWindow GetStatusWindow()
        {
            if (_sw != null)
                return _sw;

            if (!TwoMonitors)
                return null;

            _sw = new StatusWindow();
            Point p = new Point();

            p.X = Screen.AllScreens[1].WorkingArea.Left;
            p.Y = Screen.AllScreens[1].WorkingArea.Top;

            _sw.Location = p;

            return _sw;
        }
    }
}