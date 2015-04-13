using System;
using System.Windows.Forms;
using POS.Contracts.Architecture;
using POS.Contracts;

namespace POS
{
    /// <summary>
    /// Implements ICalculatorApplication and provides a container/manager for plug-ins
    /// </summary>
    [PlugInApplication("POS")]
    internal class PlugInManager : PlugInBasedApplication<IPosPlugIn>, IPosApplication
    {
        private MainForm _mf;

        public PlugInManager(MainForm mf)
        {
            _mf = mf;
        }

        public void AddPayButton()
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        public void ShowAlert(string content, ToolTipIcon icon)
        {
            _mf.notifyIcon1.ShowBalloonTip(5000, "POS", content, icon);
        }
    }
}
