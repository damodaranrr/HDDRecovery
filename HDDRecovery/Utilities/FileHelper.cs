using System;
using System.IO;

namespace HDDRecovery.Utilities
{
    /// <summary>
    /// Provides helper methods for file operations.
    /// </summary>
    public static class FileHelper
    {
        /// <summary>
        /// Copies a file and preserves its attributes (creation time, modified time, attributes).
        /// </summary>
        /// <param name="sourceFile">The source file path.</param>
        /// <param name="destinationFile">The destination file path.</param>
        public static void CopyFileWithAttributes(string sourceFile, string destinationFile)
        {
            if (string.IsNullOrWhiteSpace(sourceFile))
                throw new ArgumentException("Source file path cannot be empty.", nameof(sourceFile));

            if (string.IsNullOrWhiteSpace(destinationFile))
                throw new ArgumentException("Destination file path cannot be empty.", nameof(destinationFile));

            if (!File.Exists(sourceFile))
                throw new FileNotFoundException("Source file not found.", sourceFile);

            // Copy the file
            File.Copy(sourceFile, destinationFile, overwrite: true);

            // Preserve file attributes
            try
            {
                var sourceInfo = new FileInfo(sourceFile);
                var destInfo = new FileInfo(destinationFile);

                destInfo.CreationTime = sourceInfo.CreationTime;
                destInfo.LastWriteTime = sourceInfo.LastWriteTime;
                destInfo.LastAccessTime = sourceInfo.LastAccessTime;
                destInfo.Attributes = sourceInfo.Attributes;
            }
            catch (Exception ex)
            {
                // Log but don't fail if we can't preserve attributes
                System.Diagnostics.Debug.WriteLine($"Warning: Could not preserve all attributes for {destinationFile}: {ex.Message}");
            }
        }

        /// <summary>
        /// Formats a file size in bytes to a human-readable string.
        /// </summary>
        /// <param name="bytes">The size in bytes.</param>
        /// <returns>A formatted string (e.g., "1.5 GB", "256 MB").</returns>
        public static string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = bytes;
            int order = 0;

            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }

            return $"{len:0.##} {sizes[order]}";
        }

        /// <summary>
        /// Gets drive information for a given path.
        /// </summary>
        /// <param name="path">The path to get drive information for.</param>
        /// <returns>A DriveInfo object, or null if the drive cannot be found.</returns>
        public static DriveInfo? GetDriveInfo(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
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
        /// Validates that a path is accessible and not locked.
        /// </summary>
        /// <param name="path">The path to validate.</param>
        /// <returns>True if the path is accessible; otherwise, false.</returns>
        public static bool IsPathAccessible(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return false;

            try
            {
                if (Directory.Exists(path))
                {
                    // Try to enumerate files to check access
                    Directory.GetFiles(path);
                    return true;
                }
                else if (File.Exists(path))
                {
                    // Try to open the file
                    using (File.OpenRead(path)) { }
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
}
