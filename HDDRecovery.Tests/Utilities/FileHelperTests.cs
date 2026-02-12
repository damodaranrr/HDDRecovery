using FluentAssertions;
using HDDRecovery.Tests.TestHelpers;
using HDDRecovery.Utilities;
using Xunit;

namespace HDDRecovery.Tests.Utilities;

public class FileHelperTests : IDisposable
{
    private readonly string _testDirectory;
    
    public FileHelperTests()
    {
        _testDirectory = TestFileGenerator.CreateTestDirectory();
    }
    
    #region CopyFileWithAttributes Tests
    
    [Fact]
    public void CopyFileWithAttributes_ValidFile_CopiesSuccessfully()
    {
        // Arrange
        var sourceFile = Path.Combine(_testDirectory, "source.txt");
        var destFile = Path.Combine(_testDirectory, "dest.txt");
        File.WriteAllText(sourceFile, "test content");
        
        // Act
        FileHelper.CopyFileWithAttributes(sourceFile, destFile);
        
        // Assert
        File.Exists(destFile).Should().BeTrue();
        File.ReadAllText(destFile).Should().Be("test content");
    }
    
    [Fact]
    public void CopyFileWithAttributes_PreservesCreationTime()
    {
        // Arrange
        var sourceFile = Path.Combine(_testDirectory, "source.txt");
        var destFile = Path.Combine(_testDirectory, "dest.txt");
        File.WriteAllText(sourceFile, "test");
        var creationTime = new DateTime(2020, 1, 1);
        File.SetCreationTime(sourceFile, creationTime);
        
        // Act
        FileHelper.CopyFileWithAttributes(sourceFile, destFile);
        
        // Assert
        File.GetCreationTime(destFile).Should().BeCloseTo(creationTime, TimeSpan.FromSeconds(1));
    }
    
    [Fact]
    public void CopyFileWithAttributes_PreservesModifiedTime()
    {
        // Arrange
        var sourceFile = Path.Combine(_testDirectory, "source.txt");
        var destFile = Path.Combine(_testDirectory, "dest.txt");
        File.WriteAllText(sourceFile, "test");
        var modifiedTime = new DateTime(2021, 6, 15);
        File.SetLastWriteTime(sourceFile, modifiedTime);
        
        // Act
        FileHelper.CopyFileWithAttributes(sourceFile, destFile);
        
        // Assert
        File.GetLastWriteTime(destFile).Should().BeCloseTo(modifiedTime, TimeSpan.FromSeconds(1));
    }
    
    [Fact]
    public void CopyFileWithAttributes_PreservesFileAttributes()
    {
        // Arrange
        var sourceFile = Path.Combine(_testDirectory, "source.txt");
        var destFile = Path.Combine(_testDirectory, "dest.txt");
        File.WriteAllText(sourceFile, "test");
        File.SetAttributes(sourceFile, FileAttributes.ReadOnly);
        
        // Act
        FileHelper.CopyFileWithAttributes(sourceFile, destFile);
        
        // Assert
        var destAttributes = File.GetAttributes(destFile);
        destAttributes.Should().HaveFlag(FileAttributes.ReadOnly);
        
        // Cleanup - remove read-only
        File.SetAttributes(destFile, FileAttributes.Normal);
    }
    
    [Fact]
    public void CopyFileWithAttributes_NullSourcePath_ThrowsArgumentException()
    {
        // Act & Assert
        var act = () => FileHelper.CopyFileWithAttributes(null!, "dest.txt");
        act.Should().Throw<ArgumentException>()
            .WithParameterName("sourcePath");
    }
    
    [Fact]
    public void CopyFileWithAttributes_EmptySourcePath_ThrowsArgumentException()
    {
        // Act & Assert
        var act = () => FileHelper.CopyFileWithAttributes(string.Empty, "dest.txt");
        act.Should().Throw<ArgumentException>()
            .WithParameterName("sourcePath");
    }
    
    [Fact]
    public void CopyFileWithAttributes_NullDestinationPath_ThrowsArgumentException()
    {
        // Act & Assert
        var act = () => FileHelper.CopyFileWithAttributes("source.txt", null!);
        act.Should().Throw<ArgumentException>()
            .WithParameterName("destinationPath");
    }
    
    [Fact]
    public void CopyFileWithAttributes_FileNotFound_ThrowsFileNotFoundException()
    {
        // Arrange
        var sourceFile = Path.Combine(_testDirectory, "nonexistent.txt");
        var destFile = Path.Combine(_testDirectory, "dest.txt");
        
        // Act & Assert
        var act = () => FileHelper.CopyFileWithAttributes(sourceFile, destFile);
        act.Should().Throw<FileNotFoundException>();
    }
    
    [Fact]
    public void CopyFileWithAttributes_OverwritesExistingFile()
    {
        // Arrange
        var sourceFile = Path.Combine(_testDirectory, "source.txt");
        var destFile = Path.Combine(_testDirectory, "dest.txt");
        File.WriteAllText(sourceFile, "new content");
        File.WriteAllText(destFile, "old content");
        
        // Act
        FileHelper.CopyFileWithAttributes(sourceFile, destFile);
        
        // Assert
        File.ReadAllText(destFile).Should().Be("new content");
    }
    
    #endregion
    
    #region FormatFileSize Tests
    
    [Fact]
    public void FormatFileSize_ZeroBytes_ReturnsCorrectFormat()
    {
        // Act
        var result = FileHelper.FormatFileSize(0);
        
        // Assert
        result.Should().Be("0 B");
    }
    
    [Fact]
    public void FormatFileSize_Bytes_ReturnsCorrectFormat()
    {
        // Act
        var result = FileHelper.FormatFileSize(500);
        
        // Assert
        result.Should().Be("500 B");
    }
    
    [Fact]
    public void FormatFileSize_Kilobytes_ReturnsCorrectFormat()
    {
        // Act
        var result = FileHelper.FormatFileSize(1536); // 1.5 KB
        
        // Assert
        result.Should().Be("1.5 KB");
    }
    
    [Fact]
    public void FormatFileSize_Megabytes_ReturnsCorrectFormat()
    {
        // Act
        var result = FileHelper.FormatFileSize(5242880); // 5 MB
        
        // Assert
        result.Should().Be("5 MB");
    }
    
    [Fact]
    public void FormatFileSize_Gigabytes_ReturnsCorrectFormat()
    {
        // Act
        var result = FileHelper.FormatFileSize(3221225472); // 3 GB
        
        // Assert
        result.Should().Be("3 GB");
    }
    
    [Fact]
    public void FormatFileSize_Terabytes_ReturnsCorrectFormat()
    {
        // Act
        var result = FileHelper.FormatFileSize(2199023255552); // 2 TB
        
        // Assert
        result.Should().Be("2 TB");
    }
    
    [Fact]
    public void FormatFileSize_NegativeValue_HandlesCorrectly()
    {
        // Act
        var result = FileHelper.FormatFileSize(-1024);
        
        // Assert
        result.Should().Be("0 B");
    }
    
    #endregion
    
    #region GetDriveInfo Tests
    
    [Fact]
    public void GetDriveInfo_ValidPath_ReturnsDriveInfo()
    {
        // Arrange
        var path = _testDirectory;
        
        // Act
        var driveInfo = FileHelper.GetDriveInfo(path);
        
        // Assert
        driveInfo.Should().NotBeNull();
        driveInfo!.Name.Should().NotBeNullOrEmpty();
    }
    
    [Fact]
    public void GetDriveInfo_NullPath_ReturnsNull()
    {
        // Act
        var driveInfo = FileHelper.GetDriveInfo(null!);
        
        // Assert
        driveInfo.Should().BeNull();
    }
    
    [Fact]
    public void GetDriveInfo_EmptyPath_ReturnsNull()
    {
        // Act
        var driveInfo = FileHelper.GetDriveInfo(string.Empty);
        
        // Assert
        driveInfo.Should().BeNull();
    }
    
    [Fact]
    public void GetDriveInfo_InvalidPath_ReturnsNull()
    {
        // Act
        var driveInfo = FileHelper.GetDriveInfo("invalid:::path");
        
        // Assert
        driveInfo.Should().BeNull();
    }
    
    #endregion
    
    #region IsPathAccessible Tests
    
    [Fact]
    public void IsPathAccessible_ExistingDirectory_ReturnsTrue()
    {
        // Arrange
        var directory = Path.Combine(_testDirectory, "testdir");
        Directory.CreateDirectory(directory);
        
        // Act
        var result = FileHelper.IsPathAccessible(directory);
        
        // Assert
        result.Should().BeTrue();
    }
    
    [Fact]
    public void IsPathAccessible_ExistingFile_ReturnsTrue()
    {
        // Arrange
        var file = Path.Combine(_testDirectory, "testfile.txt");
        File.WriteAllText(file, "test");
        
        // Act
        var result = FileHelper.IsPathAccessible(file);
        
        // Assert
        result.Should().BeTrue();
    }
    
    [Fact]
    public void IsPathAccessible_NonExistentPath_ReturnsFalse()
    {
        // Arrange
        var path = Path.Combine(_testDirectory, "nonexistent");
        
        // Act
        var result = FileHelper.IsPathAccessible(path);
        
        // Assert
        result.Should().BeFalse();
    }
    
    [Fact]
    public void IsPathAccessible_NullPath_ReturnsFalse()
    {
        // Act
        var result = FileHelper.IsPathAccessible(null!);
        
        // Assert
        result.Should().BeFalse();
    }
    
    [Fact]
    public void IsPathAccessible_LockedFile_ReturnsFalse()
    {
        // Arrange
        var file = Path.Combine(_testDirectory, "locked.txt");
        using var lockedStream = TestFileGenerator.CreateLockedFile(file);
        
        // Act
        var result = FileHelper.IsPathAccessible(file);
        
        // Assert
        result.Should().BeFalse();
    }
    
    #endregion
    
    public void Dispose()
    {
        TestFileGenerator.Cleanup(_testDirectory);
    }
}
