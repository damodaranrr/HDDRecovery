namespace HDDRecovery.Models;

/// <summary>
/// Options for file recovery operations
/// </summary>
public class RecoveryOptions
{
    public string SourcePath { get; set; } = string.Empty;
    public string DestinationPath { get; set; } = string.Empty;
    public bool PreserveFolderStructure { get; set; } = true;
    public bool OverwriteExisting { get; set; } = false;
    public string FileFilter { get; set; } = "*.*";
}
