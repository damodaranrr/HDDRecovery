namespace HDDRecovery.Models;

/// <summary>
/// Information about a recovered file
/// </summary>
public class RecoveredFileInfo
{
    public string FilePath { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public string FileExtension { get; set; } = string.Empty;
    public FileStatus Status { get; set; }
}

/// <summary>
/// Status of a recovered file
/// </summary>
public enum FileStatus
{
    Unknown,
    Recovered,
    Failed,
    Skipped
}
