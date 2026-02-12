using System.Drawing;
using System.Drawing.Imaging;

namespace HDDRecovery.Tests.TestHelpers;

/// <summary>
/// Helper class for generating test files and folders
/// </summary>
public static class TestFileGenerator
{
    /// <summary>
    /// Creates a temporary test directory
    /// </summary>
    public static string CreateTestDirectory()
    {
        var tempPath = Path.Combine(Path.GetTempPath(), $"HDDRecoveryTest_{Guid.NewGuid()}");
        Directory.CreateDirectory(tempPath);
        return tempPath;
    }
    
    /// <summary>
    /// Creates test files in a directory
    /// </summary>
    public static void CreateTestFiles(string path, int count, string extension = ".txt")
    {
        for (int i = 0; i < count; i++)
        {
            var filePath = Path.Combine(path, $"testfile{i}{extension}");
            File.WriteAllText(filePath, $"Test content for file {i}");
        }
    }
    
    /// <summary>
    /// Creates nested folders with files
    /// </summary>
    public static void CreateNestedFolders(string path, int depth, int filesPerFolder = 2)
    {
        if (depth <= 0) return;
        
        for (int i = 0; i < 2; i++)
        {
            var folderPath = Path.Combine(path, $"folder{depth}_{i}");
            Directory.CreateDirectory(folderPath);
            CreateTestFiles(folderPath, filesPerFolder);
            
            if (depth > 1)
            {
                CreateNestedFolders(folderPath, depth - 1, filesPerFolder);
            }
        }
    }
    
    /// <summary>
    /// Creates a test image file
    /// </summary>
    public static void CreateTestImage(string path, int width = 100, int height = 100)
    {
        using var bitmap = new Bitmap(width, height);
        using var graphics = Graphics.FromImage(bitmap);
        
        // Fill with a color
        graphics.Clear(Color.Blue);
        
        // Draw some shapes
        graphics.DrawLine(Pens.Red, 0, 0, width, height);
        graphics.DrawRectangle(Pens.Green, 10, 10, width - 20, height - 20);
        
        bitmap.Save(path, ImageFormat.Png);
    }
    
    /// <summary>
    /// Creates a locked file (file that's currently open)
    /// </summary>
    public static FileStream CreateLockedFile(string path)
    {
        var stream = File.Open(path, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
        stream.WriteByte(0);
        stream.Flush();
        return stream;
    }
    
    /// <summary>
    /// Cleans up a test directory
    /// </summary>
    public static void Cleanup(string path)
    {
        if (Directory.Exists(path))
        {
            try
            {
                // Remove read-only attributes
                var dirInfo = new DirectoryInfo(path);
                SetAttributesNormal(dirInfo);
                
                Directory.Delete(path, true);
            }
            catch
            {
                // Ignore cleanup errors
            }
        }
    }
    
    private static void SetAttributesNormal(DirectoryInfo dir)
    {
        foreach (var subDir in dir.GetDirectories())
        {
            SetAttributesNormal(subDir);
        }
        
        foreach (var file in dir.GetFiles())
        {
            file.Attributes = FileAttributes.Normal;
        }
        
        dir.Attributes = FileAttributes.Normal;
    }
}
