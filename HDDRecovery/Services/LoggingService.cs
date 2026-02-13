namespace HDDRecovery.Services;

/// <summary>
/// Service for logging operations
/// </summary>
public class LoggingService
{
    private readonly string _logFilePath;
    private readonly object _lockObject = new object();
    
    public LoggingService(string logDirectory = "Logs")
    {
        if (!Directory.Exists(logDirectory))
        {
            Directory.CreateDirectory(logDirectory);
        }
        
        _logFilePath = Path.Combine(logDirectory, $"recovery_{DateTime.Now:yyyyMMdd_HHmmss}.log");
        
        if (!File.Exists(_logFilePath))
        {
            File.WriteAllText(_logFilePath, $"Log started at {DateTime.Now}\n");
        }
    }
    
    public void LogInfo(string message)
    {
        WriteLog("INFO", message);
    }
    
    public void LogWarning(string message)
    {
        WriteLog("WARNING", message);
    }
    
    public void LogError(string message, Exception? exception = null)
    {
        var errorMessage = message;
        if (exception != null)
        {
            errorMessage += $"\nException: {exception.Message}\nStackTrace: {exception.StackTrace}";
        }
        WriteLog("ERROR", errorMessage);
    }
    
    public string GetLogFilePath()
    {
        return _logFilePath;
    }
    
    private void WriteLog(string level, string message)
    {
        lock (_lockObject)
        {
            var logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}\n";
            File.AppendAllText(_logFilePath, logEntry);
        }
    }
}
