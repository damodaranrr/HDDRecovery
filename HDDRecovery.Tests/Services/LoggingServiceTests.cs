using FluentAssertions;
using HDDRecovery.Services;
using HDDRecovery.Tests.TestHelpers;
using Xunit;

namespace HDDRecovery.Tests.Services;

public class LoggingServiceTests : IDisposable
{
    private readonly string _testLogDirectory;
    
    public LoggingServiceTests()
    {
        _testLogDirectory = TestFileGenerator.CreateTestDirectory();
    }
    
    #region Constructor Tests
    
    [Fact]
    public void Constructor_CreatesLogDirectory()
    {
        // Arrange
        var logDir = Path.Combine(_testLogDirectory, "logs");
        
        // Act
        var service = new LoggingService(logDir);
        
        // Assert
        Directory.Exists(logDir).Should().BeTrue();
    }
    
    [Fact]
    public void Constructor_CreatesLogFile()
    {
        // Arrange
        var logDir = Path.Combine(_testLogDirectory, "logs");
        
        // Act
        var service = new LoggingService(logDir);
        var logFile = service.GetLogFilePath();
        
        // Assert
        File.Exists(logFile).Should().BeTrue();
    }
    
    #endregion
    
    #region LogInfo Tests
    
    [Fact]
    public void LogInfo_WritesInfoMessage_ToLogFile()
    {
        // Arrange
        var logDir = Path.Combine(_testLogDirectory, "logs");
        var service = new LoggingService(logDir);
        
        // Act
        service.LogInfo("Test info message");
        
        // Assert
        var logContent = File.ReadAllText(service.GetLogFilePath());
        logContent.Should().Contain("Test info message");
    }
    
    [Fact]
    public void LogInfo_IncludesTimestamp()
    {
        // Arrange
        var logDir = Path.Combine(_testLogDirectory, "logs");
        var service = new LoggingService(logDir);
        
        // Act
        service.LogInfo("Test message");
        
        // Assert
        var logContent = File.ReadAllText(service.GetLogFilePath());
        logContent.Should().MatchRegex(@"\[\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}\]");
    }
    
    [Fact]
    public void LogInfo_IncludesLogLevel()
    {
        // Arrange
        var logDir = Path.Combine(_testLogDirectory, "logs");
        var service = new LoggingService(logDir);
        
        // Act
        service.LogInfo("Test message");
        
        // Assert
        var logContent = File.ReadAllText(service.GetLogFilePath());
        logContent.Should().Contain("[INFO]");
    }
    
    #endregion
    
    #region LogWarning Tests
    
    [Fact]
    public void LogWarning_WritesWarningMessage_ToLogFile()
    {
        // Arrange
        var logDir = Path.Combine(_testLogDirectory, "logs");
        var service = new LoggingService(logDir);
        
        // Act
        service.LogWarning("Test warning message");
        
        // Assert
        var logContent = File.ReadAllText(service.GetLogFilePath());
        logContent.Should().Contain("Test warning message");
    }
    
    [Fact]
    public void LogWarning_IncludesCorrectLevel()
    {
        // Arrange
        var logDir = Path.Combine(_testLogDirectory, "logs");
        var service = new LoggingService(logDir);
        
        // Act
        service.LogWarning("Test warning");
        
        // Assert
        var logContent = File.ReadAllText(service.GetLogFilePath());
        logContent.Should().Contain("[WARNING]");
    }
    
    #endregion
    
    #region LogError Tests
    
    [Fact]
    public void LogError_WritesErrorMessage_ToLogFile()
    {
        // Arrange
        var logDir = Path.Combine(_testLogDirectory, "logs");
        var service = new LoggingService(logDir);
        
        // Act
        service.LogError("Test error message");
        
        // Assert
        var logContent = File.ReadAllText(service.GetLogFilePath());
        logContent.Should().Contain("Test error message");
        logContent.Should().Contain("[ERROR]");
    }
    
    [Fact]
    public void LogError_WithException_IncludesExceptionDetails()
    {
        // Arrange
        var logDir = Path.Combine(_testLogDirectory, "logs");
        var service = new LoggingService(logDir);
        var exception = new InvalidOperationException("Test exception");
        
        // Act
        service.LogError("Error occurred", exception);
        
        // Assert
        var logContent = File.ReadAllText(service.GetLogFilePath());
        logContent.Should().Contain("Error occurred");
        logContent.Should().Contain("Exception: Test exception");
    }
    
    [Fact]
    public void LogError_WithException_IncludesStackTrace()
    {
        // Arrange
        var logDir = Path.Combine(_testLogDirectory, "logs");
        var service = new LoggingService(logDir);
        var exception = new InvalidOperationException("Test exception");
        
        // Act
        service.LogError("Error occurred", exception);
        
        // Assert
        var logContent = File.ReadAllText(service.GetLogFilePath());
        logContent.Should().Contain("StackTrace:");
    }
    
    #endregion
    
    #region GetLogFilePath Tests
    
    [Fact]
    public void GetLogFilePath_ReturnsValidPath()
    {
        // Arrange
        var logDir = Path.Combine(_testLogDirectory, "logs");
        var service = new LoggingService(logDir);
        
        // Act
        var logPath = service.GetLogFilePath();
        
        // Assert
        logPath.Should().NotBeNullOrEmpty();
        logPath.Should().StartWith(logDir);
    }
    
    [Fact]
    public void GetLogFilePath_FileExists()
    {
        // Arrange
        var logDir = Path.Combine(_testLogDirectory, "logs");
        var service = new LoggingService(logDir);
        
        // Act
        var logPath = service.GetLogFilePath();
        
        // Assert
        File.Exists(logPath).Should().BeTrue();
    }
    
    #endregion
    
    #region Thread Safety Tests
    
    [Fact]
    public void LogInfo_ConcurrentCalls_AllMessagesWritten()
    {
        // Arrange
        var logDir = Path.Combine(_testLogDirectory, "logs");
        var service = new LoggingService(logDir);
        var messageCount = 100;
        var tasks = new List<Task>();
        
        // Act
        for (int i = 0; i < messageCount; i++)
        {
            var index = i;
            tasks.Add(Task.Run(() => service.LogInfo($"Message {index}")));
        }
        
        Task.WaitAll(tasks.ToArray());
        
        // Assert
        var logContent = File.ReadAllText(service.GetLogFilePath());
        for (int i = 0; i < messageCount; i++)
        {
            logContent.Should().Contain($"Message {i}");
        }
    }
    
    #endregion
    
    public void Dispose()
    {
        TestFileGenerator.Cleanup(_testLogDirectory);
    }
}
