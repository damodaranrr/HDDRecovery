using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using HDDRecovery.Helpers;
using HDDRecovery.Models;

namespace HDDRecovery.Services
{
    /// <summary>
    /// Service for file preview operations
    /// </summary>
    public class FilePreviewService
    {
        /// <summary>
        /// Scan a directory and build a list of recovered files
        /// </summary>
        public List<RecoveredFileInfo> ScanRecoveredFiles(string destinationPath)
        {
            var files = new List<RecoveredFileInfo>();

            if (!Directory.Exists(destinationPath))
                return files;

            try
            {
                var allFiles = Directory.GetFiles(destinationPath, "*.*", SearchOption.AllDirectories);

                foreach (var filePath in allFiles)
                {
                    try
                    {
                        var fileInfo = new FileInfo(filePath);
                        files.Add(new RecoveredFileInfo
                        {
                            FullPath = filePath,
                            FileName = fileInfo.Name,
                            RelativePath = Path.GetRelativePath(destinationPath, filePath),
                            FileType = fileInfo.Extension,
                            FileSize = fileInfo.Length,
                            ModifiedDate = fileInfo.LastWriteTime,
                            CreatedDate = fileInfo.CreationTime,
                            Status = RecoveryStatus.Success
                        });
                    }
                    catch (Exception ex)
                    {
                        // Add failed file entry
                        files.Add(new RecoveredFileInfo
                        {
                            FullPath = filePath,
                            FileName = Path.GetFileName(filePath),
                            RelativePath = Path.GetRelativePath(destinationPath, filePath),
                            Status = RecoveryStatus.Failed,
                            ErrorMessage = ex.Message
                        });
                    }
                }
            }
            catch
            {
                // Return empty list on error
            }

            return files;
        }

        /// <summary>
        /// Load and resize an image for preview
        /// </summary>
        public Bitmap? LoadImagePreview(string filePath, int maxWidth, int maxHeight)
        {
            try
            {
                var original = FileHelper.LoadImageSafe(filePath);
                if (original == null)
                    return null;

                // If image is smaller than max dimensions, return as-is
                if (original.Width <= maxWidth && original.Height <= maxHeight)
                    return original;

                // Resize the image
                var resized = FileHelper.ResizeImage(original, maxWidth, maxHeight);
                original.Dispose();
                return resized;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Get system icon for a file
        /// </summary>
        public Icon? GetFileIcon(string filePath)
        {
            try
            {
                var extension = Path.GetExtension(filePath);
                return FileHelper.GetFileTypeIcon(extension);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Build a folder tree structure from file list
        /// </summary>
        public Dictionary<string, List<RecoveredFileInfo>> BuildFolderTree(List<RecoveredFileInfo> files)
        {
            var folderTree = new Dictionary<string, List<RecoveredFileInfo>>();

            foreach (var file in files)
            {
                var directory = Path.GetDirectoryName(file.FullPath) ?? string.Empty;

                if (!folderTree.ContainsKey(directory))
                {
                    folderTree[directory] = new List<RecoveredFileInfo>();
                }

                folderTree[directory].Add(file);
            }

            return folderTree;
        }

        /// <summary>
        /// Get unique folder paths from file list
        /// </summary>
        public List<string> GetUniqueFolders(List<RecoveredFileInfo> files)
        {
            return files
                .Select(f => Path.GetDirectoryName(f.FullPath) ?? string.Empty)
                .Distinct()
                .OrderBy(f => f)
                .ToList();
        }
    }
}
