using System;

namespace HDDRecovery.Models
{
    /// <summary>
    /// Represents information about a recovered file
    /// </summary>
    public class RecoveredFileInfo
    {
        /// <summary>
        /// Full path to the file
        /// </summary>
        public string FullPath { get; set; } = string.Empty;
        
        /// <summary>
        /// File name without path
        /// </summary>
        public string FileName { get; set; } = string.Empty;
        
        /// <summary>
        /// Relative path from root
        /// </summary>
        public string RelativePath { get; set; } = string.Empty;
        
        /// <summary>
        /// File extension/type
        /// </summary>
        public string FileType { get; set; } = string.Empty;
        
        /// <summary>
        /// File size in bytes
        /// </summary>
        public long FileSize { get; set; }
        
        /// <summary>
        /// Last modified date
        /// </summary>
        public DateTime ModifiedDate { get; set; }
        
        /// <summary>
        /// File creation date
        /// </summary>
        public DateTime CreatedDate { get; set; }
        
        /// <summary>
        /// Recovery status
        /// </summary>
        public RecoveryStatus Status { get; set; }
        
        /// <summary>
        /// Error message if recovery failed
        /// </summary>
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
