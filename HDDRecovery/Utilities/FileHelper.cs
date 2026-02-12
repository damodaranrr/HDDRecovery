namespace HDDRecovery.Utilities;

/// <summary>
/// Helper methods for file operations
/// </summary>
public static class FileHelper
{
    /// <summary>
    /// Copies a file and preserves its attributes
    /// </summary>
    public static void CopyFileWithAttributes(string sourcePath, string destinationPath)
    {
        if (string.IsNullOrEmpty(sourcePath))
            throw new ArgumentException("Source path cannot be null or empty", nameof(sourcePath));
        
        if (string.IsNullOrEmpty(destinationPath))
            throw new ArgumentException("Destination path cannot be null or empty", nameof(destinationPath));
        
        if (!File.Exists(sourcePath))
            throw new FileNotFoundException("Source file not found", sourcePath);
        
        File.Copy(sourcePath, destinationPath, true);
        
        // Preserve attributes
        var fileInfo = new FileInfo(sourcePath);
        var destInfo = new FileInfo(destinationPath);
        
        destInfo.CreationTime = fileInfo.CreationTime;
        destInfo.LastWriteTime = fileInfo.LastWriteTime;
        destInfo.Attributes = fileInfo.Attributes;
    }
    
    /// <summary>
    /// Formats file size in human-readable format
    /// </summary>
    public static string FormatFileSize(long bytes)
    {
        if (bytes < 0)
            return "0 B";
        
        string[] sizes = { "B", "KB", "MB", "GB", "TB" };
        int order = 0;
        double size = bytes;
        
        while (size >= 1024 && order < sizes.Length - 1)
        {
            order++;
            size /= 1024;
        }
        
        return $"{size:0.##} {sizes[order]}";
    }
    
    /// <summary>
    /// Gets drive information for a path
    /// </summary>
    public static DriveInfo? GetDriveInfo(string path)
    {
        if (string.IsNullOrEmpty(path))
            return null;
        
        try
        {
            var root = Path.GetPathRoot(path);
            if (string.IsNullOrEmpty(root))
                return null;
            
            return new DriveInfo(root);
        }
        catch
        {
            return null;
        }
    }
    
    /// <summary>
    /// Checks if a path is accessible
    /// </summary>
    public static bool IsPathAccessible(string path)
    {
        if (string.IsNullOrEmpty(path))
            return false;
        
        try
        {
            if (Directory.Exists(path))
            {
                Directory.GetFiles(path);
                return true;
            }
            
            if (File.Exists(path))
            {
                using var stream = File.OpenRead(path);
                return true;
            }
            
            return false;
        }
        catch
        {
            return false;
        }
    }
}
