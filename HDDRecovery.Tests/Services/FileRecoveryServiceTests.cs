using FluentAssertions;
using HDDRecovery.Models;
using HDDRecovery.Services;
using HDDRecovery.Tests.TestHelpers;
using Moq;
using Xunit;

namespace HDDRecovery.Tests.Services;

public class FileRecoveryServiceTests : IDisposable
{
    private readonly string _testDirectory;
    private readonly LoggingService _logger;
    
    public FileRecoveryServiceTests()
    {
        _testDirectory = TestFileGenerator.CreateTestDirectory();
        var logDir = Path.Combine(_testDirectory, "logs");
        _logger = new LoggingService(logDir);
    }
    
    #region Constructor Tests
    
    [Fact]
    public void Constructor_NullLogger_ThrowsArgumentNullException()
    {
        // Act & Assert
        var act = () => new FileRecoveryService(null!);
        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("logger");
    }
    
    #endregion
    
    #region RecoverFilesAsync Tests
    
    [Fact]
    public async Task RecoverFilesAsync_ValidOptions_ReturnsSuccessResult()
    {
        // Arrange
        var service = new FileRecoveryService(_logger);
        var sourceDir = Path.Combine(_testDirectory, "source");
        var destDir = Path.Combine(_testDirectory, "dest");
        Directory.CreateDirectory(sourceDir);
        Directory.CreateDirectory(destDir);
        TestFileGenerator.CreateTestFiles(sourceDir, 3);
        
        var options = new RecoveryOptions
        {
            SourcePath = sourceDir,
            DestinationPath = destDir
        };
        
        // Act
        var result = await service.RecoverFilesAsync(options);
        
        // Assert
        result.Should().NotBeNull();
        result.SuccessfulFiles.Should().Be(3);
        result.FailedFiles.Should().Be(0);
    }
    
    [Fact]
    public async Task RecoverFilesAsync_NullOptions_ThrowsArgumentNullException()
    {
        // Arrange
        var service = new FileRecoveryService(_logger);
        
        // Act & Assert
        var act = async () => await service.RecoverFilesAsync(null!);
        await act.Should().ThrowAsync<ArgumentNullException>();
    }
    
    [Fact]
    public async Task RecoverFilesAsync_EmptySourcePath_ThrowsArgumentException()
    {
        // Arrange
        var service = new FileRecoveryService(_logger);
        var options = new RecoveryOptions
        {
            SourcePath = string.Empty,
            DestinationPath = "dest"
        };
        
        // Act & Assert
        var act = async () => await service.RecoverFilesAsync(options);
        await act.Should().ThrowAsync<ArgumentException>();
    }
    
    [Fact]
    public async Task RecoverFilesAsync_EmptyDestinationPath_ThrowsArgumentException()
    {
        // Arrange
        var service = new FileRecoveryService(_logger);
        var options = new RecoveryOptions
        {
            SourcePath = "source",
            DestinationPath = string.Empty
        };
        
        // Act & Assert
        var act = async () => await service.RecoverFilesAsync(options);
        await act.Should().ThrowAsync<ArgumentException>();
    }
    
    [Fact]
    public async Task RecoverFilesAsync_SourceNotFound_ThrowsDirectoryNotFoundException()
    {
        // Arrange
        var service = new FileRecoveryService(_logger);
        var options = new RecoveryOptions
        {
            SourcePath = Path.Combine(_testDirectory, "nonexistent"),
            DestinationPath = Path.Combine(_testDirectory, "dest")
        };
        
        // Act & Assert
        var act = async () => await service.RecoverFilesAsync(options);
        await act.Should().ThrowAsync<DirectoryNotFoundException>();
    }
    
    [Fact]
    public async Task RecoverFilesAsync_CopiesAllFiles_Successfully()
    {
        // Arrange
        var service = new FileRecoveryService(_logger);
        var sourceDir = Path.Combine(_testDirectory, "source");
        var destDir = Path.Combine(_testDirectory, "dest");
        Directory.CreateDirectory(sourceDir);
        Directory.CreateDirectory(destDir);
        TestFileGenerator.CreateTestFiles(sourceDir, 5);
        
        var options = new RecoveryOptions
        {
            SourcePath = sourceDir,
            DestinationPath = destDir
        };
        
        // Act
        var result = await service.RecoverFilesAsync(options);
        
        // Assert
        result.SuccessfulFiles.Should().Be(5);
        Directory.GetFiles(destDir).Length.Should().Be(5);
    }
    
    [Fact]
    public async Task RecoverFilesAsync_PreservesFolderStructure_WhenEnabled()
    {
        // Arrange
        var service = new FileRecoveryService(_logger);
        var sourceDir = Path.Combine(_testDirectory, "source");
        var destDir = Path.Combine(_testDirectory, "dest");
        Directory.CreateDirectory(sourceDir);
        Directory.CreateDirectory(destDir);
        
        var subDir = Path.Combine(sourceDir, "subfolder");
        Directory.CreateDirectory(subDir);
        File.WriteAllText(Path.Combine(subDir, "file.txt"), "content");
        
        var options = new RecoveryOptions
        {
            SourcePath = sourceDir,
            DestinationPath = destDir,
            PreserveFolderStructure = true
        };
        
        // Act
        await service.RecoverFilesAsync(options);
        
        // Assert
        var expectedFile = Path.Combine(destDir, "subfolder", "file.txt");
        File.Exists(expectedFile).Should().BeTrue();
    }
    
    [Fact]
    public async Task RecoverFilesAsync_FlattensStructure_WhenDisabled()
    {
        // Arrange
        var service = new FileRecoveryService(_logger);
        var sourceDir = Path.Combine(_testDirectory, "source");
        var destDir = Path.Combine(_testDirectory, "dest");
        Directory.CreateDirectory(sourceDir);
        Directory.CreateDirectory(destDir);
        
        var subDir = Path.Combine(sourceDir, "subfolder");
        Directory.CreateDirectory(subDir);
        File.WriteAllText(Path.Combine(subDir, "file.txt"), "content");
        
        var options = new RecoveryOptions
        {
            SourcePath = sourceDir,
            DestinationPath = destDir,
            PreserveFolderStructure = false
        };
        
        // Act
        await service.RecoverFilesAsync(options);
        
        // Assert
        var expectedFile = Path.Combine(destDir, "file.txt");
        File.Exists(expectedFile).Should().BeTrue();
        Directory.Exists(Path.Combine(destDir, "subfolder")).Should().BeFalse();
    }
    
    [Fact]
    public async Task RecoverFilesAsync_OverwritesFiles_WhenEnabled()
    {
        // Arrange
        var service = new FileRecoveryService(_logger);
        var sourceDir = Path.Combine(_testDirectory, "source");
        var destDir = Path.Combine(_testDirectory, "dest");
        Directory.CreateDirectory(sourceDir);
        Directory.CreateDirectory(destDir);
        
        var sourceFile = Path.Combine(sourceDir, "file.txt");
        var destFile = Path.Combine(destDir, "file.txt");
        File.WriteAllText(sourceFile, "new content");
        File.WriteAllText(destFile, "old content");
        
        var options = new RecoveryOptions
        {
            SourcePath = sourceDir,
            DestinationPath = destDir,
            OverwriteExisting = true
        };
        
        // Act
        await service.RecoverFilesAsync(options);
        
        // Assert
        File.ReadAllText(destFile).Should().Be("new content");
    }
    
    [Fact]
    public async Task RecoverFilesAsync_SkipsExistingFiles_WhenOverwriteDisabled()
    {
        // Arrange
        var service = new FileRecoveryService(_logger);
        var sourceDir = Path.Combine(_testDirectory, "source");
        var destDir = Path.Combine(_testDirectory, "dest");
        Directory.CreateDirectory(sourceDir);
        Directory.CreateDirectory(destDir);
        
        var sourceFile = Path.Combine(sourceDir, "file.txt");
        var destFile = Path.Combine(destDir, "file.txt");
        File.WriteAllText(sourceFile, "new content");
        File.WriteAllText(destFile, "old content");
        
        var options = new RecoveryOptions
        {
            SourcePath = sourceDir,
            DestinationPath = destDir,
            OverwriteExisting = false
        };
        
        // Act
        var result = await service.RecoverFilesAsync(options);
        
        // Assert
        File.ReadAllText(destFile).Should().Be("old content");
        result.SkippedFiles.Should().Be(1);
    }
    
    [Fact]
    public async Task RecoverFilesAsync_AppliesFileFilter_Correctly()
    {
        // Arrange
        var service = new FileRecoveryService(_logger);
        var sourceDir = Path.Combine(_testDirectory, "source");
        var destDir = Path.Combine(_testDirectory, "dest");
        Directory.CreateDirectory(sourceDir);
        Directory.CreateDirectory(destDir);
        
        File.WriteAllText(Path.Combine(sourceDir, "file1.txt"), "content");
        File.WriteAllText(Path.Combine(sourceDir, "file2.doc"), "content");
        File.WriteAllText(Path.Combine(sourceDir, "file3.txt"), "content");
        
        var options = new RecoveryOptions
        {
            SourcePath = sourceDir,
            DestinationPath = destDir,
            FileFilter = "*.txt"
        };
        
        // Act
        var result = await service.RecoverFilesAsync(options);
        
        // Assert
        result.SuccessfulFiles.Should().Be(2);
        Directory.GetFiles(destDir, "*.txt").Length.Should().Be(2);
    }
    
    [Fact]
    public async Task RecoverFilesAsync_CancellationRequested_StopsRecovery()
    {
        // Arrange
        var service = new FileRecoveryService(_logger);
        var sourceDir = Path.Combine(_testDirectory, "source");
        var destDir = Path.Combine(_testDirectory, "dest");
        Directory.CreateDirectory(sourceDir);
        Directory.CreateDirectory(destDir);
        TestFileGenerator.CreateTestFiles(sourceDir, 10);
        
        var options = new RecoveryOptions
        {
            SourcePath = sourceDir,
            DestinationPath = destDir
        };
        
        var cts = new CancellationTokenSource();
        cts.Cancel();
        
        // Act
        var result = await service.RecoverFilesAsync(options, cts.Token);
        
        // Assert
        result.Cancelled.Should().BeTrue();
        result.SuccessfulFiles.Should().BeLessThan(10);
    }
    
    [Fact]
    public async Task RecoverFilesAsync_RaisesProgressChangedEvent()
    {
        // Arrange
        var service = new FileRecoveryService(_logger);
        var sourceDir = Path.Combine(_testDirectory, "source");
        var destDir = Path.Combine(_testDirectory, "dest");
        Directory.CreateDirectory(sourceDir);
        Directory.CreateDirectory(destDir);
        TestFileGenerator.CreateTestFiles(sourceDir, 3);
        
        var options = new RecoveryOptions
        {
            SourcePath = sourceDir,
            DestinationPath = destDir
        };
        
        var progressValues = new List<int>();
        service.ProgressChanged += (sender, progress) => progressValues.Add(progress);
        
        // Act
        await service.RecoverFilesAsync(options);
        
        // Assert
        progressValues.Should().NotBeEmpty();
        progressValues.Should().Contain(100);
    }
    
    [Fact]
    public async Task RecoverFilesAsync_RaisesStatusChangedEvent()
    {
        // Arrange
        var service = new FileRecoveryService(_logger);
        var sourceDir = Path.Combine(_testDirectory, "source");
        var destDir = Path.Combine(_testDirectory, "dest");
        Directory.CreateDirectory(sourceDir);
        Directory.CreateDirectory(destDir);
        TestFileGenerator.CreateTestFiles(sourceDir, 2);
        
        var options = new RecoveryOptions
        {
            SourcePath = sourceDir,
            DestinationPath = destDir
        };
        
        var statusMessages = new List<string>();
        service.StatusChanged += (sender, status) => statusMessages.Add(status);
        
        // Act
        await service.RecoverFilesAsync(options);
        
        // Assert
        statusMessages.Should().NotBeEmpty();
        statusMessages.Should().Contain(s => s.Contains("Scanning files"));
    }
    
    [Fact]
    public async Task RecoverFilesAsync_HandlesFileErrors_Gracefully()
    {
        // Arrange
        var service = new FileRecoveryService(_logger);
        var sourceDir = Path.Combine(_testDirectory, "source");
        var destDir = Path.Combine(_testDirectory, "dest");
        Directory.CreateDirectory(sourceDir);
        Directory.CreateDirectory(destDir);
        
        // Create a file and then lock it
        var lockedFile = Path.Combine(sourceDir, "locked.txt");
        using var stream = TestFileGenerator.CreateLockedFile(lockedFile);
        
        // Create a normal file too
        File.WriteAllText(Path.Combine(sourceDir, "normal.txt"), "content");
        
        var options = new RecoveryOptions
        {
            SourcePath = sourceDir,
            DestinationPath = destDir
        };
        
        // Act
        var result = await service.RecoverFilesAsync(options);
        
        // Assert
        result.FailedFiles.Should().BeGreaterThan(0);
    }
    
    [Fact]
    public async Task RecoverFilesAsync_CountsSuccessfulFiles_Correctly()
    {
        // Arrange
        var service = new FileRecoveryService(_logger);
        var sourceDir = Path.Combine(_testDirectory, "source");
        var destDir = Path.Combine(_testDirectory, "dest");
        Directory.CreateDirectory(sourceDir);
        Directory.CreateDirectory(destDir);
        TestFileGenerator.CreateTestFiles(sourceDir, 7);
        
        var options = new RecoveryOptions
        {
            SourcePath = sourceDir,
            DestinationPath = destDir
        };
        
        // Act
        var result = await service.RecoverFilesAsync(options);
        
        // Assert
        result.SuccessfulFiles.Should().Be(7);
        result.TotalFiles.Should().Be(7);
    }
    
    [Fact]
    public async Task RecoverFilesAsync_CountsFailedFiles_Correctly()
    {
        // Arrange
        var service = new FileRecoveryService(_logger);
        var sourceDir = Path.Combine(_testDirectory, "source");
        var destDir = Path.Combine(_testDirectory, "dest");
        Directory.CreateDirectory(sourceDir);
        Directory.CreateDirectory(destDir);
        
        // Create a locked file
        var lockedFile = Path.Combine(sourceDir, "locked.txt");
        using var stream = TestFileGenerator.CreateLockedFile(lockedFile);
        
        var options = new RecoveryOptions
        {
            SourcePath = sourceDir,
            DestinationPath = destDir
        };
        
        // Act
        var result = await service.RecoverFilesAsync(options);
        
        // Assert
        result.FailedFiles.Should().Be(1);
    }
    
    [Fact]
    public async Task RecoverFilesAsync_CountsSkippedFiles_Correctly()
    {
        // Arrange
        var service = new FileRecoveryService(_logger);
        var sourceDir = Path.Combine(_testDirectory, "source");
        var destDir = Path.Combine(_testDirectory, "dest");
        Directory.CreateDirectory(sourceDir);
        Directory.CreateDirectory(destDir);
        
        File.WriteAllText(Path.Combine(sourceDir, "file1.txt"), "new");
        File.WriteAllText(Path.Combine(sourceDir, "file2.txt"), "new");
        File.WriteAllText(Path.Combine(destDir, "file1.txt"), "old");
        
        var options = new RecoveryOptions
        {
            SourcePath = sourceDir,
            DestinationPath = destDir,
            OverwriteExisting = false
        };
        
        // Act
        var result = await service.RecoverFilesAsync(options);
        
        // Assert
        result.SkippedFiles.Should().Be(1);
        result.SuccessfulFiles.Should().Be(1);
    }
    
    [Fact]
    public async Task RecoverFilesAsync_CalculatesProgressPercentage_Correctly()
    {
        // Arrange
        var service = new FileRecoveryService(_logger);
        var sourceDir = Path.Combine(_testDirectory, "source");
        var destDir = Path.Combine(_testDirectory, "dest");
        Directory.CreateDirectory(sourceDir);
        Directory.CreateDirectory(destDir);
        TestFileGenerator.CreateTestFiles(sourceDir, 4);
        
        var options = new RecoveryOptions
        {
            SourcePath = sourceDir,
            DestinationPath = destDir
        };
        
        var progressValues = new List<int>();
        service.ProgressChanged += (sender, progress) => progressValues.Add(progress);
        
        // Act
        await service.RecoverFilesAsync(options);
        
        // Assert
        progressValues.Should().Contain(25); // After 1 of 4
        progressValues.Should().Contain(50); // After 2 of 4
        progressValues.Should().Contain(75); // After 3 of 4
        progressValues.Should().Contain(100); // After 4 of 4
    }
    
    [Fact]
    public async Task RecoverFilesAsync_EstimatesTimeRemaining_Correctly()
    {
        // Arrange
        var service = new FileRecoveryService(_logger);
        var sourceDir = Path.Combine(_testDirectory, "source");
        var destDir = Path.Combine(_testDirectory, "dest");
        Directory.CreateDirectory(sourceDir);
        Directory.CreateDirectory(destDir);
        TestFileGenerator.CreateTestFiles(sourceDir, 3);
        
        var options = new RecoveryOptions
        {
            SourcePath = sourceDir,
            DestinationPath = destDir
        };
        
        // Act
        var result = await service.RecoverFilesAsync(options);
        
        // Assert
        // At the end, estimated time remaining should be close to zero
        result.EstimatedTimeRemaining.Should().BeLessThan(TimeSpan.FromSeconds(5));
    }
    
    #endregion
    
    public void Dispose()
    {
        TestFileGenerator.Cleanup(_testDirectory);
    }
}
