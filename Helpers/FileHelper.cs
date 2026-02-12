using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;

namespace HDDRecovery.Helpers
{
    /// <summary>
    /// Helper class for file operations
    /// </summary>
    public static class FileHelper
    {
        private static readonly string[] ImageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".ico" };

        /// <summary>
        /// Check if a file is an image based on extension
        /// </summary>
        public static bool IsImageFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return false;

            var extension = Path.GetExtension(filePath).ToLowerInvariant();
            return Array.IndexOf(ImageExtensions, extension) >= 0;
        }

        /// <summary>
        /// Load an image safely with error handling
        /// </summary>
        public static Bitmap? LoadImageSafe(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                    return null;

                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    return new Bitmap(stream);
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Resize an image maintaining aspect ratio
        /// </summary>
        public static Bitmap ResizeImage(Bitmap original, int maxWidth, int maxHeight)
        {
            if (original == null)
                throw new ArgumentNullException(nameof(original));

            var ratioX = (double)maxWidth / original.Width;
            var ratioY = (double)maxHeight / original.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(original.Width * ratio);
            var newHeight = (int)(original.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);
            using (var graphics = Graphics.FromImage(newImage))
            {
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.DrawImage(original, 0, 0, newWidth, newHeight);
            }

            return newImage;
        }

        /// <summary>
        /// Get file type icon using Windows Shell
        /// </summary>
        public static Icon? GetFileTypeIcon(string extension)
        {
            try
            {
                SHFILEINFO shinfo = new SHFILEINFO();
                IntPtr hIcon = SHGetFileInfo(
                    extension,
                    FILE_ATTRIBUTE_NORMAL,
                    ref shinfo,
                    (uint)Marshal.SizeOf(shinfo),
                    SHGFI_ICON | SHGFI_SMALLICON | SHGFI_USEFILEATTRIBUTES);

                if (hIcon != IntPtr.Zero && shinfo.hIcon != IntPtr.Zero)
                {
                    Icon icon = Icon.FromHandle(shinfo.hIcon);
                    Icon clonedIcon = (Icon)icon.Clone();
                    DestroyIcon(shinfo.hIcon);
                    return clonedIcon;
                }
            }
            catch
            {
                // Return null on failure
            }

            return null;
        }

        /// <summary>
        /// Format file size to human readable string
        /// </summary>
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

        #region Windows API Imports

        [DllImport("shell32.dll")]
        private static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes,
            ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool DestroyIcon(IntPtr hIcon);

        private const uint FILE_ATTRIBUTE_NORMAL = 0x80;
        private const uint SHGFI_ICON = 0x100;
        private const uint SHGFI_SMALLICON = 0x1;
        private const uint SHGFI_USEFILEATTRIBUTES = 0x10;

        [StructLayout(LayoutKind.Sequential)]
        private struct SHFILEINFO
        {
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        }

        #endregion
    }
}
