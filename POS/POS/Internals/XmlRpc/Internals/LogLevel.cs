namespace Rpc.Internals
{
    /// <summary>Define levels of logging.</summary><remarks> This duplicates
    /// similar enumerations in System.Diagnostics.EventLogEntryType. The 
    /// duplication was merited because .NET Compact Framework lacked the EventLogEntryType enum.</remarks>
    internal enum LogLevel
    {
        /// <summary>Information level, log entry for informational reasons only.</summary>
        Information,
        /// <summary>Warning level, indicates a possible problem.</summary>
        Warning,
        /// <summary>Error level, implies a significant problem.</summary>
        Error
    }
}