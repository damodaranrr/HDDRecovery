using FluentAssertions;
using HDDRecovery.Models;
using Xunit;

namespace HDDRecovery.Tests.Models;

public class RecoveredFileInfoTests
{
    [Fact]
    public void Properties_SetAndGet_WorkCorrectly()
    {
        // Arrange
        var fileInfo = new RecoveredFileInfo();
        var createdDate = DateTime.Now.AddDays(-10);
        var modifiedDate = DateTime.Now.AddDays(-5);
        
        // Act
        fileInfo.FilePath = "/path/to/file.txt";
        fileInfo.FileName = "file.txt";
        fileInfo.FileSize = 1024;
        fileInfo.CreatedDate = createdDate;
        fileInfo.ModifiedDate = modifiedDate;
        fileInfo.FileExtension = ".txt";
        fileInfo.Status = FileStatus.Recovered;
        
        // Assert
        fileInfo.FilePath.Should().Be("/path/to/file.txt");
        fileInfo.FileName.Should().Be("file.txt");
        fileInfo.FileSize.Should().Be(1024);
        fileInfo.CreatedDate.Should().Be(createdDate);
        fileInfo.ModifiedDate.Should().Be(modifiedDate);
        fileInfo.FileExtension.Should().Be(".txt");
        fileInfo.Status.Should().Be(FileStatus.Recovered);
    }
    
    [Fact]
    public void DefaultValues_AreSetCorrectly()
    {
        // Act
        var fileInfo = new RecoveredFileInfo();
        
        // Assert
        fileInfo.FilePath.Should().Be(string.Empty);
        fileInfo.FileName.Should().Be(string.Empty);
        fileInfo.FileSize.Should().Be(0);
        fileInfo.FileExtension.Should().Be(string.Empty);
        fileInfo.Status.Should().Be(FileStatus.Unknown);
    }
    
    [Fact]
    public void Status_Enum_HasAllValues()
    {
        // Assert
        Enum.IsDefined(typeof(FileStatus), FileStatus.Unknown).Should().BeTrue();
        Enum.IsDefined(typeof(FileStatus), FileStatus.Recovered).Should().BeTrue();
        Enum.IsDefined(typeof(FileStatus), FileStatus.Failed).Should().BeTrue();
        Enum.IsDefined(typeof(FileStatus), FileStatus.Skipped).Should().BeTrue();
    }
}
