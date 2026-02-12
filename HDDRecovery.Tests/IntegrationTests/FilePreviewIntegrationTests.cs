using FluentAssertions;
using HDDRecovery.Services;
using HDDRecovery.Tests.TestHelpers;
using Xunit;

namespace HDDRecovery.Tests.IntegrationTests;

/// <summary>
/// Integration tests for file preview functionality
/// </summary>
public class FilePreviewIntegrationTests : IDisposable
{
    private readonly string _testDirectory;
    
    public FilePreviewIntegrationTests()
    {
        _testDirectory = TestFileGenerator.CreateTestDirectory();
    }
    
    [Fact]
    public void CompletePreviewWorkflow_ScanAndOrganize_WorksCorrectly()
    {
        // Arrange
        var service = new FilePreviewService();
        var testDir = Path.Combine(_testDirectory, "recovered");
        Directory.CreateDirectory(testDir);
        
        // Create folder structure with files
        var folder1 = Path.Combine(testDir, "folder1");
        var folder2 = Path.Combine(testDir, "folder2");
        Directory.CreateDirectory(folder1);
        Directory.CreateDirectory(folder2);
        
        TestFileGenerator.CreateTestFiles(folder1, 3);
        TestFileGenerator.CreateTestFiles(folder2, 2);
        
        // Act
        var files = service.ScanRecoveredFiles(testDir);
        var folderTree = service.BuildFolderTree(files);
        
        // Assert
        files.Should().HaveCount(5);
        folderTree.Should().HaveCount(2);
        folderTree[folder1].Should().HaveCount(3);
        folderTree[folder2].Should().HaveCount(2);
    }
    
    [Fact(Skip = "Requires GDI+ (Windows-only)")]
    public void CompletePreviewWorkflow_WithImages_WorksCorrectly()
    {
        // Arrange
        var service = new FilePreviewService();
        var testDir = Path.Combine(_testDirectory, "images");
        Directory.CreateDirectory(testDir);
        
        // Create test images
        var image1 = Path.Combine(testDir, "image1.png");
        var image2 = Path.Combine(testDir, "image2.png");
        TestFileGenerator.CreateTestImage(image1, 500, 400);
        TestFileGenerator.CreateTestImage(image2, 300, 300);
        
        // Act
        var files = service.ScanRecoveredFiles(testDir);
        using var preview1 = service.LoadImagePreview(image1, 200, 200);
        using var preview2 = service.LoadImagePreview(image2, 200, 200);
        
        // Assert
        files.Should().HaveCount(2);
        preview1.Should().NotBeNull();
        preview2.Should().NotBeNull();
        preview1!.Width.Should().BeLessThanOrEqualTo(200);
        preview1.Height.Should().BeLessThanOrEqualTo(200);
    }
    
    [Fact(Skip = "Requires GDI+ (Windows-only)")]
    public void CompletePreviewWorkflow_MixedFileTypes_WorksCorrectly()
    {
        // Arrange
        var service = new FilePreviewService();
        var testDir = Path.Combine(_testDirectory, "mixed");
        Directory.CreateDirectory(testDir);
        
        // Create mixed content
        TestFileGenerator.CreateTestFiles(testDir, 3, ".txt");
        TestFileGenerator.CreateTestFiles(testDir, 2, ".doc");
        TestFileGenerator.CreateTestImage(Path.Combine(testDir, "image.png"));
        
        // Act
        var files = service.ScanRecoveredFiles(testDir);
        var folderTree = service.BuildFolderTree(files);
        
        // Assert
        files.Should().HaveCount(6);
        files.Should().Contain(f => f.FileExtension == ".txt");
        files.Should().Contain(f => f.FileExtension == ".doc");
        files.Should().Contain(f => f.FileExtension == ".png");
        
        folderTree.Should().HaveCount(1);
        folderTree[testDir].Should().HaveCount(6);
    }
    
    public void Dispose()
    {
        TestFileGenerator.Cleanup(_testDirectory);
    }
}
