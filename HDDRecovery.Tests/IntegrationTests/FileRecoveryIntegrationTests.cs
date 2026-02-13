using FluentAssertions;
using HDDRecovery.Models;
using HDDRecovery.Services;
using HDDRecovery.Tests.TestHelpers;
using Xunit;

namespace HDDRecovery.Tests.IntegrationTests;

/// <summary>
/// Integration tests for the complete file recovery workflow
/// </summary>
public class FileRecoveryIntegrationTests : IDisposable
{
    private readonly string _testDirectory;
    
    public FileRecoveryIntegrationTests()
    {
        _testDirectory = TestFileGenerator.CreateTestDirectory();
    }
    
    [Fact]
    public async Task CompleteRecoveryWorkflow_WithNestedFolders_WorksCorrectly()
    {
        // Arrange
        var logDir = Path.Combine(_testDirectory, "logs");
        var logger = new LoggingService(logDir);
        var service = new FileRecoveryService(logger);
        
        var sourceDir = Path.Combine(_testDirectory, "source");
        var destDir = Path.Combine(_testDirectory, "dest");
        Directory.CreateDirectory(sourceDir);
        Directory.CreateDirectory(destDir);
        
        // Create nested folder structure with files
        TestFileGenerator.CreateNestedFolders(sourceDir, 3, 2);
        
        var options = new RecoveryOptions
        {
            SourcePath = sourceDir,
            DestinationPath = destDir,
            PreserveFolderStructure = true
        };
        
        // Act
        var result = await service.RecoverFilesAsync(options);
        
        // Assert
        result.Should().NotBeNull();
        result.SuccessfulFiles.Should().BeGreaterThan(0);
        result.FailedFiles.Should().Be(0);
        
        // Verify folder structure is preserved
        var sourceFiles = Directory.GetFiles(sourceDir, "*.*", SearchOption.AllDirectories);
        var destFiles = Directory.GetFiles(destDir, "*.*", SearchOption.AllDirectories);
        destFiles.Length.Should().Be(sourceFiles.Length);
    }
    
    [Fact]
    public async Task CompleteRecoveryWorkflow_WithFileFilters_WorksCorrectly()
    {
        // Arrange
        var logDir = Path.Combine(_testDirectory, "logs");
        var logger = new LoggingService(logDir);
        var service = new FileRecoveryService(logger);
        
        var sourceDir = Path.Combine(_testDirectory, "source");
        var destDir = Path.Combine(_testDirectory, "dest");
        Directory.CreateDirectory(sourceDir);
        Directory.CreateDirectory(destDir);
        
        // Create files with different extensions
        TestFileGenerator.CreateTestFiles(sourceDir, 5, ".txt");
        TestFileGenerator.CreateTestFiles(sourceDir, 3, ".doc");
        TestFileGenerator.CreateTestFiles(sourceDir, 2, ".pdf");
        
        var options = new RecoveryOptions
        {
            SourcePath = sourceDir,
            DestinationPath = destDir,
            FileFilter = "*.txt"
        };
        
        // Act
        var result = await service.RecoverFilesAsync(options);
        
        // Assert
        result.SuccessfulFiles.Should().Be(5);
        Directory.GetFiles(destDir, "*.txt").Length.Should().Be(5);
        Directory.GetFiles(destDir, "*.doc").Length.Should().Be(0);
    }
    
    [Fact]
    public async Task CompleteRecoveryWorkflow_WithProgressTracking_WorksCorrectly()
    {
        // Arrange
        var logDir = Path.Combine(_testDirectory, "logs");
        var logger = new LoggingService(logDir);
        var service = new FileRecoveryService(logger);
        
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
        
        var progressUpdates = new List<int>();
        var statusUpdates = new List<string>();
        
        service.ProgressChanged += (sender, progress) => progressUpdates.Add(progress);
        service.StatusChanged += (sender, status) => statusUpdates.Add(status);
        
        // Act
        var result = await service.RecoverFilesAsync(options);
        
        // Assert
        result.SuccessfulFiles.Should().Be(10);
        progressUpdates.Should().NotBeEmpty();
        progressUpdates.Should().Contain(100);
        statusUpdates.Should().NotBeEmpty();
        
        // Verify log file was created
        File.Exists(logger.GetLogFilePath()).Should().BeTrue();
    }
    
    public void Dispose()
    {
        TestFileGenerator.Cleanup(_testDirectory);
    }
}
