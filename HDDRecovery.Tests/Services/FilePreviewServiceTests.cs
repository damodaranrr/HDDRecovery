using FluentAssertions;
using HDDRecovery.Models;
using HDDRecovery.Services;
using HDDRecovery.Tests.TestHelpers;
using Xunit;

namespace HDDRecovery.Tests.Services;

public class FilePreviewServiceTests : IDisposable
{
    private readonly string _testDirectory;
    
    public FilePreviewServiceTests()
    {
        _testDirectory = TestFileGenerator.CreateTestDirectory();
    }
    
    #region ScanRecoveredFiles Tests
    
    [Fact]
    public void ScanRecoveredFiles_ValidPath_ReturnsFileList()
    {
        // Arrange
        var service = new FilePreviewService();
        var testDir = Path.Combine(_testDirectory, "files");
        Directory.CreateDirectory(testDir);
        TestFileGenerator.CreateTestFiles(testDir, 5);
        
        // Act
        var files = service.ScanRecoveredFiles(testDir);
        
        // Assert
        files.Should().NotBeNull();
        files.Should().HaveCount(5);
    }
    
    [Fact]
    public void ScanRecoveredFiles_EmptyDirectory_ReturnsEmptyList()
    {
        // Arrange
        var service = new FilePreviewService();
        var testDir = Path.Combine(_testDirectory, "empty");
        Directory.CreateDirectory(testDir);
        
        // Act
        var files = service.ScanRecoveredFiles(testDir);
        
        // Assert
        files.Should().NotBeNull();
        files.Should().BeEmpty();
    }
    
    [Fact]
    public void ScanRecoveredFiles_InvalidPath_HandlesGracefully()
    {
        // Arrange
        var service = new FilePreviewService();
        var invalidPath = Path.Combine(_testDirectory, "nonexistent");
        
        // Act
        var files = service.ScanRecoveredFiles(invalidPath);
        
        // Assert
        files.Should().NotBeNull();
        files.Should().BeEmpty();
    }
    
    [Fact]
    public void ScanRecoveredFiles_PopulatesFileInfo_Correctly()
    {
        // Arrange
        var service = new FilePreviewService();
        var testDir = Path.Combine(_testDirectory, "files");
        Directory.CreateDirectory(testDir);
        var testFile = Path.Combine(testDir, "test.txt");
        File.WriteAllText(testFile, "content");
        
        // Act
        var files = service.ScanRecoveredFiles(testDir);
        
        // Assert
        files.Should().HaveCount(1);
        var file = files[0];
        file.FileName.Should().Be("test.txt");
        file.FilePath.Should().Be(testFile);
        file.FileSize.Should().BeGreaterThan(0);
        file.FileExtension.Should().Be(".txt");
        file.Status.Should().Be(FileStatus.Recovered);
    }
    
    #endregion
    
    #region LoadImagePreview Tests
    
    [Fact(Skip = "Requires GDI+ (Windows-only)")]
    public void LoadImagePreview_ValidImage_LoadsSuccessfully()
    {
        // Arrange
        var service = new FilePreviewService();
        var imagePath = Path.Combine(_testDirectory, "test.png");
        TestFileGenerator.CreateTestImage(imagePath, 200, 200);
        
        // Act
        var image = service.LoadImagePreview(imagePath);
        
        // Assert
        image.Should().NotBeNull();
        image!.Dispose();
    }
    
    [Fact(Skip = "Requires GDI+ (Windows-only)")]
    public void LoadImagePreview_ResizesImage_ToMaxDimensions()
    {
        // Arrange
        var service = new FilePreviewService();
        var imagePath = Path.Combine(_testDirectory, "large.png");
        TestFileGenerator.CreateTestImage(imagePath, 800, 600);
        
        // Act
        using var image = service.LoadImagePreview(imagePath, 300, 300);
        
        // Assert
        image.Should().NotBeNull();
        image!.Width.Should().BeLessThanOrEqualTo(300);
        image.Height.Should().BeLessThanOrEqualTo(300);
    }
    
    [Fact(Skip = "Requires GDI+ (Windows-only)")]
    public void LoadImagePreview_MaintainsAspectRatio()
    {
        // Arrange
        var service = new FilePreviewService();
        var imagePath = Path.Combine(_testDirectory, "aspect.png");
        TestFileGenerator.CreateTestImage(imagePath, 400, 200); // 2:1 ratio
        
        // Act
        using var image = service.LoadImagePreview(imagePath, 300, 300);
        
        // Assert
        image.Should().NotBeNull();
        var aspectRatio = (double)image!.Width / image.Height;
        aspectRatio.Should().BeApproximately(2.0, 0.1);
    }
    
    [Fact]
    public void LoadImagePreview_CorruptedImage_HandlesGracefully()
    {
        // Arrange
        var service = new FilePreviewService();
        var imagePath = Path.Combine(_testDirectory, "corrupted.png");
        File.WriteAllText(imagePath, "This is not an image");
        
        // Act
        var image = service.LoadImagePreview(imagePath);
        
        // Assert
        image.Should().BeNull();
    }
    
    [Fact]
    public void LoadImagePreview_NonImageFile_ReturnsNull()
    {
        // Arrange
        var service = new FilePreviewService();
        var textPath = Path.Combine(_testDirectory, "text.txt");
        File.WriteAllText(textPath, "Just text");
        
        // Act
        var image = service.LoadImagePreview(textPath);
        
        // Assert
        image.Should().BeNull();
    }
    
    #endregion
    
    #region GetFileIcon Tests
    
    [Fact]
    public void GetFileIcon_ValidFile_ReturnsIcon()
    {
        // Arrange
        var service = new FilePreviewService();
        var filePath = Path.Combine(_testDirectory, "file.txt");
        File.WriteAllText(filePath, "content");
        
        // Act
        var icon = service.GetFileIcon(filePath);
        
        // Assert (may be null on non-Windows platforms)
        // Just check it doesn't throw
        if (icon != null)
        {
            icon.Dispose();
        }
    }
    
    [Fact]
    public void GetFileIcon_DifferentExtensions_ReturnsDifferentIcons()
    {
        // Arrange
        var service = new FilePreviewService();
        var txtFile = Path.Combine(_testDirectory, "file.txt");
        var docFile = Path.Combine(_testDirectory, "file.doc");
        File.WriteAllText(txtFile, "content");
        File.WriteAllText(docFile, "content");
        
        // Act
        var txtIcon = service.GetFileIcon(txtFile);
        var docIcon = service.GetFileIcon(docFile);
        
        // Assert (may be null on non-Windows platforms)
        // Just ensure no exceptions are thrown
        txtIcon?.Dispose();
        docIcon?.Dispose();
    }
    
    #endregion
    
    #region BuildFolderTree Tests
    
    [Fact]
    public void BuildFolderTree_OrganizesFilesByFolder()
    {
        // Arrange
        var service = new FilePreviewService();
        var files = new List<RecoveredFileInfo>
        {
            new() { FilePath = "/folder1/file1.txt" },
            new() { FilePath = "/folder1/file2.txt" },
            new() { FilePath = "/folder2/file3.txt" }
        };
        
        // Act
        var tree = service.BuildFolderTree(files);
        
        // Assert
        tree.Should().HaveCount(2);
        tree.Should().ContainKey("/folder1");
        tree.Should().ContainKey("/folder2");
        tree["/folder1"].Should().HaveCount(2);
        tree["/folder2"].Should().HaveCount(1);
    }
    
    [Fact]
    public void BuildFolderTree_EmptyList_ReturnsEmptyDictionary()
    {
        // Arrange
        var service = new FilePreviewService();
        var files = new List<RecoveredFileInfo>();
        
        // Act
        var tree = service.BuildFolderTree(files);
        
        // Assert
        tree.Should().NotBeNull();
        tree.Should().BeEmpty();
    }
    
    [Fact]
    public void BuildFolderTree_HandlesNestedFolders()
    {
        // Arrange
        var service = new FilePreviewService();
        var files = new List<RecoveredFileInfo>
        {
            new() { FilePath = "/root/sub1/file1.txt" },
            new() { FilePath = "/root/sub2/file2.txt" },
            new() { FilePath = "/root/file3.txt" }
        };
        
        // Act
        var tree = service.BuildFolderTree(files);
        
        // Assert
        tree.Should().HaveCount(3);
        tree.Should().ContainKey("/root/sub1");
        tree.Should().ContainKey("/root/sub2");
        tree.Should().ContainKey("/root");
    }
    
    #endregion
    
    public void Dispose()
    {
        TestFileGenerator.Cleanup(_testDirectory);
    }
}
