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
        /// <summary>
        /// This method can be called by plugins to show a message to the user if needed.
        /// </summary>
        /// <param name="message">The message to be shown</param>
        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }
    }
}
