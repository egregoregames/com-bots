namespace ComBots.Logs
{
    /// <summary>
    /// Defines the logging verbosity levels.
    /// </summary>
    public enum LogLevel
    {
        /// <summary>Logs all messages (Log, LogWarning, LogError).</summary>
        INFO_ERRORS_WARNINGS,
        /// <summary>Logs only warnings and errors (LogWarning, LogError).</summary>
        WARNINGS_ERRORS,
        /// <summary>Logs only errors (LogError).</summary>
        ERRORS,
        /// <summary>Logs nothing.</summary>
        NONE
    }
}
