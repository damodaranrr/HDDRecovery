using FluentAssertions;
using HDDRecovery.Models;
using Xunit;

namespace HDDRecovery.Tests.Models;

public class RecoveryOptionsTests
{
    [Fact]
    public void Constructor_DefaultValues_SetCorrectly()
    {
        // Act
        var options = new RecoveryOptions();
        
        // Assert
        options.SourcePath.Should().Be(string.Empty);
        options.DestinationPath.Should().Be(string.Empty);
        options.PreserveFolderStructure.Should().BeTrue();
        options.OverwriteExisting.Should().BeFalse();
        options.FileFilter.Should().Be("*.*");
    }
    
    [Fact]
    public void PreserveFolderStructure_DefaultValue_IsTrue()
    {
        // Act
        var options = new RecoveryOptions();
        
        // Assert
        options.PreserveFolderStructure.Should().BeTrue();
    }
    
    [Fact]
    public void OverwriteExisting_DefaultValue_IsFalse()
    {
        // Act
        var options = new RecoveryOptions();
        
        // Assert
        options.OverwriteExisting.Should().BeFalse();
    }
    
    [Fact]
    public void FileFilter_DefaultValue_IsAllFiles()
    {
        // Act
        var options = new RecoveryOptions();
        
        // Assert
        options.FileFilter.Should().Be("*.*");
    }
    
    [Fact]
    public void Properties_CanBeSetAndGet()
    {
        // Arrange
        var options = new RecoveryOptions();
        
        // Act
        options.SourcePath = "/source";
        options.DestinationPath = "/dest";
        options.PreserveFolderStructure = false;
        options.OverwriteExisting = true;
        options.FileFilter = "*.txt";
        
        // Assert
        options.SourcePath.Should().Be("/source");
        options.DestinationPath.Should().Be("/dest");
        options.PreserveFolderStructure.Should().BeFalse();
        options.OverwriteExisting.Should().BeTrue();
        options.FileFilter.Should().Be("*.txt");
    }
}
