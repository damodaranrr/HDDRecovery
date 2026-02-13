using HDDRecovery.Models;

namespace HDDRecovery.Services;

/// <summary>
/// Service for file recovery operations
/// </summary>
public class FileRecoveryService
{
    private readonly LoggingService _logger;
    
    public event EventHandler<int>? ProgressChanged;
    public event EventHandler<string>? StatusChanged;
    
    public FileRecoveryService(LoggingService logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    public async Task<RecoveryResult> RecoverFilesAsync(
        RecoveryOptions options,
        CancellationToken cancellationToken = default)
    {
        if (options == null)
            throw new ArgumentNullException(nameof(options));
        
        if (string.IsNullOrEmpty(options.SourcePath))
            throw new ArgumentException("Source path cannot be empty", nameof(options));
        
        if (string.IsNullOrEmpty(options.DestinationPath))
            throw new ArgumentException("Destination path cannot be empty", nameof(options));
        
        if (!Directory.Exists(options.SourcePath))
            throw new DirectoryNotFoundException($"Source directory not found: {options.SourcePath}");
        
        var result = new RecoveryResult
        {
            StartTime = DateTime.Now
        };
        
        try
        {
            _logger.LogInfo($"Starting recovery from {options.SourcePath} to {options.DestinationPath}");
            StatusChanged?.Invoke(this, "Scanning files...");
            
            var files = Directory.GetFiles(options.SourcePath, options.FileFilter, SearchOption.AllDirectories);
            var totalFiles = files.Length;
            var processedFiles = 0;
            var startTime = DateTime.Now;
            
            foreach (var sourceFile in files)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    _logger.LogWarning("Recovery cancelled by user");
                    result.Cancelled = true;
                    break;
                }
                
                try
                {
                    var destinationFile = GetDestinationPath(sourceFile, options);
                    
                    if (File.Exists(destinationFile) && !options.OverwriteExisting)
                    {
                        result.SkippedFiles++;
                        _logger.LogInfo($"Skipped existing file: {destinationFile}");
                    }
                    else
                    {
                        var destDir = Path.GetDirectoryName(destinationFile);
                        if (!string.IsNullOrEmpty(destDir) && !Directory.Exists(destDir))
                        {
                            Directory.CreateDirectory(destDir);
                        }
                        
                        await Task.Run(() => File.Copy(sourceFile, destinationFile, true), cancellationToken);
                        result.SuccessfulFiles++;
                        _logger.LogInfo($"Recovered: {sourceFile} -> {destinationFile}");
                    }
                }
                catch (Exception ex)
                {
                    result.FailedFiles++;
                    _logger.LogError($"Failed to recover {sourceFile}", ex);
                }
                
                processedFiles++;
                var progress = (int)((double)processedFiles / totalFiles * 100);
                ProgressChanged?.Invoke(this, progress);
                
                // Calculate estimated time remaining
                var elapsed = DateTime.Now - startTime;
                if (processedFiles > 0)
                {
                    var avgTimePerFile = elapsed.TotalSeconds / processedFiles;
                    var remainingFiles = totalFiles - processedFiles;
                    var estimatedSeconds = avgTimePerFile * remainingFiles;
                    result.EstimatedTimeRemaining = TimeSpan.FromSeconds(estimatedSeconds);
                }
                
                StatusChanged?.Invoke(this, $"Processing file {processedFiles} of {totalFiles}");
            }
            
            result.EndTime = DateTime.Now;
            result.TotalFiles = totalFiles;
            _logger.LogInfo($"Recovery completed: {result.SuccessfulFiles} successful, {result.FailedFiles} failed, {result.SkippedFiles} skipped");
        }
        catch (Exception ex)
        {
            _logger.LogError("Recovery failed", ex);
            throw;
        }
        
        return result;
    }
    
    private string GetDestinationPath(string sourceFile, RecoveryOptions options)
    {
        if (options.PreserveFolderStructure)
        {
            var relativePath = Path.GetRelativePath(options.SourcePath, sourceFile);
            return Path.Combine(options.DestinationPath, relativePath);
        }
        else
        {
            var fileName = Path.GetFileName(sourceFile);
            return Path.Combine(options.DestinationPath, fileName);
        }
    }
}

/// <summary>
/// Result of a recovery operation
/// </summary>
public class RecoveryResult
{
    public int TotalFiles { get; set; }
    public int SuccessfulFiles { get; set; }
    public int FailedFiles { get; set; }
    public int SkippedFiles { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public bool Cancelled { get; set; }
    public TimeSpan EstimatedTimeRemaining { get; set; }
    
    public TimeSpan Duration => EndTime - StartTime;
}
