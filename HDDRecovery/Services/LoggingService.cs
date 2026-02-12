using System;
using System.IO;
using System.Text;

namespace HDDRecovery.Services
{
    /// <summary>
    /// Provides logging functionality for the recovery operations.
    /// </summary>
    public class LoggingService
    {
        private readonly string _logFilePath;
        private readonly object _lockObject = new object();

        /// <summary>
        /// Initializes a new instance of the LoggingService class.
        /// </summary>
        public LoggingService()
        {
            string logDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "HDDRecovery", "Logs");
            Directory.CreateDirectory(logDirectory);
            _logFilePath = Path.Combine(logDirectory, $"Recovery_{DateTime.Now:yyyyMMdd_HHmmss}.log");
        }

        /// <summary>
        /// Writes an informational message to the log.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void LogInfo(string message)
        {
            WriteLog("INFO", message);
        }

        /// <summary>
        /// Writes a warning message to the log.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void LogWarning(string message)
        {
            WriteLog("WARNING", message);
        }

        /// <summary>
        /// Writes an error message to the log.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void LogError(string message)
        {
            WriteLog("ERROR", message);
        }

        /// <summary>
        /// Writes an error message with exception details to the log.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="ex">The exception to log.</param>
        public void LogError(string message, Exception ex)
        {
            WriteLog("ERROR", $"{message} - Exception: {ex.Message}\nStack Trace: {ex.StackTrace}");
        }

        /// <summary>
        /// Gets the path to the current log file.
        /// </summary>
        /// <returns>The full path to the log file.</returns>
        public string GetLogFilePath()
        {
            return _logFilePath;
        }

        /// <summary>
        /// Writes a log entry with the specified level and message.
        /// </summary>
        /// <param name="level">The log level (INFO, WARNING, ERROR).</param>
        /// <param name="message">The message to log.</param>
        private void WriteLog(string level, string message)
        {
            lock (_lockObject)
            {
                try
                {
                    string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}";
                    File.AppendAllText(_logFilePath, logEntry + Environment.NewLine, Encoding.UTF8);
                }
                catch (Exception ex)
                {
                    // If we can't write to the log file, write to debug output
                    System.Diagnostics.Debug.WriteLine($"Failed to write to log: {ex.Message}");
                }
            }
        }
    }
}
