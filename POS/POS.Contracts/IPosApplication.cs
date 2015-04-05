using POS.Contracts.Architecture;

namespace POS.Contracts
{
    /// <summary>
    /// Defines the application interface that can be used by plug-ins.
    /// </summary>
    public interface IPosApplication : IPlugInBasedApplication
    {
        /// <summary>
        /// This method can be called by plugins to show a message to the user if needed.
        /// </summary>
        /// <param name="message">The message to be shown</param>
        void ShowMessage(string message);
    }
}
