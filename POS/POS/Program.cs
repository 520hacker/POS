using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls;

namespace POS
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ThemeResolutionService.ApplicationThemeName = new Telerik.WinControls.Themes.TelerikMetroTouchTheme().ThemeName;

            Application.Run(new MainForm());
        }
    }
}