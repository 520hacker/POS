using System.Windows.Forms;
using POS.Contracts.Architecture;

namespace POS.Contracts
{
    /// <summary>
    /// Defines the application interface that can be used by plug-ins.
    /// </summary>
    public interface IPosApplication : IPlugInBasedApplication
    {
        void AddPayButton();
        void ShowAlert(string content, ToolTipIcon icon);
    }
}