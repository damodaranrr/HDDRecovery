using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HDDRecovery.Models;
using HDDRecovery.Utilities;

namespace HDDRecovery.Services
{
    /// <summary>
    /// Provides core file recovery functionality with progress reporting.
    /// </summary>
    public class FileRecoveryService
    {
        private readonly LoggingService _logger;

        /// <summary>
        /// Initializes a new instance of the FileRecoveryService class.
        /// </summary>
        /// <param name="logger">The logging service to use for operation logging.</param>
        public FileRecoveryService(LoggingService logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Occurs when recovery progress is updated.
        /// </summary>
        public event EventHandler<RecoveryProgressEventArgs>? ProgressChanged;

        /// <summary>
        /// Occurs when a status message needs to be displayed.
        /// </summary>
        public event EventHandler<string>? StatusChanged;

        /// <summary>
        /// Starts the file recovery operation asynchronously.
        /// </summary>
        /// <param name="options">The recovery options specifying source, destination, and preferences.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation with recovery statistics.</returns>
        public async Task<RecoveryResult> RecoverFilesAsync(RecoveryOptions options, CancellationToken cancellationToken)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            if (string.IsNullOrWhiteSpace(options.SourcePath))
                throw new ArgumentException("Source path cannot be empty.", nameof(options));

            if (string.IsNullOrWhiteSpace(options.DestinationPath))
                throw new ArgumentException("Destination path cannot be empty.", nameof(options));

            if (!Directory.Exists(options.SourcePath))
                throw new DirectoryNotFoundException($"Source directory not found: {options.SourcePath}");

            _logger.LogInfo($"Starting recovery - Source: {options.SourcePath}, Destination: {options.DestinationPath}");
            OnStatusChanged("Scanning source directory...");

            var result = new RecoveryResult
            {
                StartTime = DateTime.Now
            };

            try
            {
                // Ensure destination directory exists
                Directory.CreateDirectory(options.DestinationPath);

                // Get all files from source
                var files = await Task.Run(() => GetFilesToRecover(options.SourcePath, options.FileFilter), cancellationToken);
                result.TotalFiles = files.Count;

                _logger.LogInfo($"Found {files.Count} files to recover");
                OnStatusChanged($"Found {files.Count} files. Starting recovery...");

                // Process files
                int processedCount = 0;
                var startTime = DateTime.Now;

                foreach (var sourceFile in files)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        _logger.LogWarning("Recovery cancelled by user");
                        result.WasCancelled = true;
                        break;
                    }

                    try
                    {
                        string destinationFile = GetDestinationPath(sourceFile, options);

                        // Check if file should be copied
                        bool shouldCopy = options.OverwriteExisting || !File.Exists(destinationFile);

                        if (shouldCopy)
                        {
                            // Ensure destination directory exists
                            string? destDir = Path.GetDirectoryName(destinationFile);
                            if (!string.IsNullOrEmpty(destDir))
                            {
                                Directory.CreateDirectory(destDir);
                            }

                            // Copy file and preserve attributes
                            await Task.Run(() => FileHelper.CopyFileWithAttributes(sourceFile, destinationFile), cancellationToken);

                            result.SuccessfulFiles++;
                            _logger.LogInfo($"Recovered: {sourceFile}");
                        }
                        else
                        {
                            result.SkippedFiles++;
                            _logger.LogWarning($"Skipped (already exists): {sourceFile}");
                        }
                    }
                    catch (Exception ex)
                    {
                        result.FailedFiles++;
                        _logger.LogError($"Failed to recover: {sourceFile}", ex);
                        OnStatusChanged($"Error: {ex.Message}");
                    }

                    processedCount++;

                    // Calculate progress and estimated time
                    double progressPercentage = (double)processedCount / result.TotalFiles * 100;
                    var elapsed = DateTime.Now - startTime;
                    TimeSpan? estimatedRemaining = null;

                    if (processedCount > 0)
                    {
                        double avgTimePerFile = elapsed.TotalSeconds / processedCount;
                        int remainingFiles = result.TotalFiles - processedCount;
                        estimatedRemaining = TimeSpan.FromSeconds(avgTimePerFile * remainingFiles);
                    }

                    // Report progress
                    OnProgressChanged(new RecoveryProgressEventArgs
                    {
                        TotalFiles = result.TotalFiles,
                        ProcessedFiles = processedCount,
                        CurrentFile = Path.GetFileName(sourceFile),
                        ProgressPercentage = (int)progressPercentage,
                        EstimatedTimeRemaining = estimatedRemaining
                    });
                }

                result.EndTime = DateTime.Now;
                _logger.LogInfo($"Recovery completed - Success: {result.SuccessfulFiles}, Failed: {result.FailedFiles}, Skipped: {result.SkippedFiles}");
                OnStatusChanged($"Recovery completed! Success: {result.SuccessfulFiles}, Failed: {result.FailedFiles}, Skipped: {result.SkippedFiles}");

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Recovery failed", ex);
                OnStatusChanged($"Recovery failed: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Gets the list of files to recover from the source directory.
        /// </summary>
        /// <param name="sourcePath">The source directory path.</param>
        /// <param name="fileFilter">The file filter pattern.</param>
        /// <returns>List of file paths to recover.</returns>
        private List<string> GetFilesToRecover(string sourcePath, string fileFilter)
        {
            var files = new List<string>();

            try
            {
                // Parse file filter (supports patterns like "*.jpg;*.png")
                var patterns = fileFilter.Split(';', StringSplitOptions.RemoveEmptyEntries);
                
                foreach (var pattern in patterns)
                {
                    var matchingFiles = Directory.GetFiles(sourcePath, pattern.Trim(), SearchOption.AllDirectories);
                    files.AddRange(matchingFiles);
                }

                // Remove duplicates
                files = files.Distinct().ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error scanning directory: {sourcePath}", ex);
            }

            return files;
        }

        /// <summary>
        /// Determines the destination path for a file based on recovery options.
        /// </summary>
        /// <param name="sourceFile">The source file path.</param>
        /// <param name="options">The recovery options.</param>
        /// <returns>The destination file path.</returns>
        private string GetDestinationPath(string sourceFile, RecoveryOptions options)
        {
            if (options.PreserveFolderStructure)
            {
                // Preserve directory structure
                string relativePath = Path.GetRelativePath(options.SourcePath, sourceFile);
                return Path.Combine(options.DestinationPath, relativePath);
            }
            else
            {
                // Flatten to destination root
                return Path.Combine(options.DestinationPath, Path.GetFileName(sourceFile));
            }
        }

        /// <summary>
        /// Raises the ProgressChanged event.
        /// </summary>
        /// <param name="e">Event arguments containing progress information.</param>
        protected virtual void OnProgressChanged(RecoveryProgressEventArgs e)
        {
            ProgressChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the StatusChanged event.
        /// </summary>
        /// <param name="status">The status message.</param>
        protected virtual void OnStatusChanged(string status)
        {
            StatusChanged?.Invoke(this, status);
        }
    }

    /// <summary>
    /// Represents the result of a file recovery operation.
    /// </summary>
    public class RecoveryResult
    {
        /// <summary>
        /// Gets or sets the total number of files found.
        /// </summary>
        public int TotalFiles { get; set; }

        /// <summary>
        /// Gets or sets the number of successfully recovered files.
        /// </summary>
        public int SuccessfulFiles { get; set; }

        /// <summary>
        /// Gets or sets the number of failed file recoveries.
        /// </summary>
        public int FailedFiles { get; set; }

        /// <summary>
        /// Gets or sets the number of skipped files.
        /// </summary>
        public int SkippedFiles { get; set; }

        /// <summary>
        /// Gets or sets the operation start time.
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or sets the operation end time.
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the operation was cancelled.
        /// </summary>
        public bool WasCancelled { get; set; }

        /// <summary>
        /// Gets the total duration of the recovery operation.
        /// </summary>
        public TimeSpan Duration => (EndTime ?? DateTime.Now) - StartTime;
    }

    /// <summary>
    /// Provides data for recovery progress events.
    /// </summary>
    public class RecoveryProgressEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the total number of files to process.
        /// </summary>
        public int TotalFiles { get; set; }

        /// <summary>
        /// Gets or sets the number of files processed so far.
        /// </summary>
        public int ProcessedFiles { get; set; }

        /// <summary>
        /// Gets or sets the name of the file currently being processed.
        /// </summary>
        public string CurrentFile { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the progress percentage (0-100).
        /// </summary>
        public int ProgressPercentage { get; set; }

        /// <summary>
        /// Gets or sets the estimated time remaining for the operation.
        /// </summary>
        public TimeSpan? EstimatedTimeRemaining { get; set; }
    }
}
