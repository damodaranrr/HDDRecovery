using HDDRecovery.Models;
using System.Drawing;

namespace HDDRecovery.Services;

/// <summary>
/// Service for file preview operations
/// </summary>
public class FilePreviewService
{
    /// <summary>
    /// Scans a directory for recovered files
    /// </summary>
    public List<RecoveredFileInfo> ScanRecoveredFiles(string path)
    {
        var files = new List<RecoveredFileInfo>();
        
        if (string.IsNullOrEmpty(path) || !Directory.Exists(path))
            return files;
        
        try
        {
            var fileEntries = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
            
            foreach (var file in fileEntries)
            {
                try
                {
                    var fileInfo = new FileInfo(file);
                    files.Add(new RecoveredFileInfo
                    {
                        FilePath = file,
                        FileName = fileInfo.Name,
                        FileSize = fileInfo.Length,
                        CreatedDate = fileInfo.CreationTime,
                        ModifiedDate = fileInfo.LastWriteTime,
                        FileExtension = fileInfo.Extension,
                        Status = FileStatus.Recovered
                    });
                }
                catch
                {
                    // Skip files that can't be accessed
                }
            }
        }
        catch
        {
            // Return empty list if directory can't be accessed
        }
        
        return files;
    }
    
    /// <summary>
    /// Loads an image preview with resizing
    /// </summary>
    public Image? LoadImagePreview(string filePath, int maxWidth = 300, int maxHeight = 300)
    {
        if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            return null;
        
        try
        {
            using var originalImage = Image.FromFile(filePath);
            
            // Calculate aspect ratio
            double ratioX = (double)maxWidth / originalImage.Width;
            double ratioY = (double)maxHeight / originalImage.Height;
            double ratio = Math.Min(ratioX, ratioY);
            
            int newWidth = (int)(originalImage.Width * ratio);
            int newHeight = (int)(originalImage.Height * ratio);
            
            var resizedImage = new Bitmap(newWidth, newHeight);
            using (var graphics = Graphics.FromImage(resizedImage))
            {
                graphics.DrawImage(originalImage, 0, 0, newWidth, newHeight);
            }
            
            return resizedImage;
        }
        catch
        {
            return null;
        }
    }
    
    /// <summary>
    /// Gets the icon for a file based on its extension
    /// </summary>
    public Icon? GetFileIcon(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
            return null;
        
        try
        {
            return Icon.ExtractAssociatedIcon(filePath);
        }
        catch
        {
            return null;
        }
    }
    
    /// <summary>
    /// Builds a folder tree from a list of files
    /// </summary>
    public Dictionary<string, List<RecoveredFileInfo>> BuildFolderTree(List<RecoveredFileInfo> files)
    {
        var folderTree = new Dictionary<string, List<RecoveredFileInfo>>();
        
        foreach (var file in files)
        {
            var folder = Path.GetDirectoryName(file.FilePath) ?? string.Empty;
            
            if (!folderTree.ContainsKey(folder))
            {
                folderTree[folder] = new List<RecoveredFileInfo>();
            }
            
            folderTree[folder].Add(file);
        }
        
        return folderTree;
    }
}
