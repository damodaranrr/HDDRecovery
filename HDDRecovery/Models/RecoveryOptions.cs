using System;

namespace HDDRecovery.Models
{
    /// <summary>
    /// Represents configuration options for the file recovery operation.
    /// </summary>
    public class RecoveryOptions
    {
        /// <summary>
        /// Gets or sets the source directory path to recover files from.
        /// </summary>
        public string SourcePath { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the destination directory path where files will be recovered.
        /// </summary>
        public string DestinationPath { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether to preserve the original folder structure.
        /// </summary>
        public bool PreserveFolderStructure { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether to overwrite existing files at the destination.
        /// </summary>
        public bool OverwriteExisting { get; set; } = false;

        /// <summary>
        /// Gets or sets the file filter pattern (e.g., "*.*" for all files, "*.jpg;*.png" for specific types).
        /// </summary>
        public string FileFilter { get; set; } = "*.*";
    }
}
