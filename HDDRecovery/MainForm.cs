using System;
using System.Threading;
using System.Windows.Forms;
using HDDRecovery.Models;
using HDDRecovery.Services;
using HDDRecovery.Utilities;

namespace HDDRecovery
{
    /// <summary>
    /// Main form for the HDD Recovery application.
    /// </summary>
    public partial class MainForm : Form
    {
        private readonly LoggingService _logger;
        private readonly FileRecoveryService _recoveryService;
        private CancellationTokenSource? _cancellationTokenSource;
        private bool _isRecovering = false;

        /// <summary>
        /// Initializes a new instance of the MainForm class.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            
            _logger = new LoggingService();
            _recoveryService = new FileRecoveryService(_logger);

            // Subscribe to recovery service events
            _recoveryService.ProgressChanged += OnRecoveryProgressChanged;
            _recoveryService.StatusChanged += OnRecoveryStatusChanged;

            // Initialize UI state
            UpdateUIState();
            LogStatus("Application started. Ready to recover files.");
            _logger.LogInfo("Application started");
        }

        /// <summary>
        /// Handles the Browse Source button click event.
        /// </summary>
        private void btnBrowseSource_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Select the source drive or folder to recover files from";
                dialog.ShowNewFolderButton = false;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    txtSourcePath.Text = dialog.SelectedPath;
                    UpdateSourceDriveInfo();
                    UpdateUIState();
                    LogStatus($"Source selected: {dialog.SelectedPath}");
                }
            }
        }

        /// <summary>
        /// Handles the Browse Destination button click event.
        /// </summary>
        private void btnBrowseDestination_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Select the destination folder for recovered files";
                dialog.ShowNewFolderButton = true;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    txtDestinationPath.Text = dialog.SelectedPath;
                    UpdateUIState();
                    LogStatus($"Destination selected: {dialog.SelectedPath}");
                }
            }
        }

        /// <summary>
        /// Handles the Start Recovery button click event.
        /// </summary>
        private async void btnStartRecovery_Click(object sender, EventArgs e)
        {
            if (_isRecovering)
                return;

            // Validate inputs
            if (string.IsNullOrWhiteSpace(txtSourcePath.Text))
            {
                MessageBox.Show("Please select a source path.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDestinationPath.Text))
            {
                MessageBox.Show("Please select a destination path.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtSourcePath.Text.Equals(txtDestinationPath.Text, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Source and destination paths cannot be the same.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Prepare recovery options
            var options = new RecoveryOptions
            {
                SourcePath = txtSourcePath.Text,
                DestinationPath = txtDestinationPath.Text,
                PreserveFolderStructure = chkPreserveFolderStructure.Checked,
                OverwriteExisting = chkOverwriteExisting.Checked,
                FileFilter = GetFileFilter()
            };

            // Confirm with user
            var confirmMessage = $"Ready to start recovery:\n\n" +
                               $"Source: {options.SourcePath}\n" +
                               $"Destination: {options.DestinationPath}\n\n" +
                               $"Preserve folder structure: {options.PreserveFolderStructure}\n" +
                               $"Overwrite existing: {options.OverwriteExisting}\n\n" +
                               $"Do you want to continue?";

            if (MessageBox.Show(confirmMessage, "Confirm Recovery", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            // Start recovery
            _isRecovering = true;
            _cancellationTokenSource = new CancellationTokenSource();
            UpdateUIState();

            try
            {
                LogStatus("Starting file recovery...");
                progressBar.Value = 0;
                lblProgress.Text = "0%";
                lblFileCount.Text = "0/0 files recovered";
                lblCurrentFile.Text = "Initializing...";
                lblTimeRemaining.Text = "Estimating...";

                var result = await _recoveryService.RecoverFilesAsync(options, _cancellationTokenSource.Token);

                // Show completion message
                if (result.WasCancelled)
                {
                    MessageBox.Show($"Recovery was cancelled.\n\n" +
                                  $"Files recovered: {result.SuccessfulFiles}\n" +
                                  $"Files failed: {result.FailedFiles}\n" +
                                  $"Files skipped: {result.SkippedFiles}",
                                  "Recovery Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show($"Recovery completed successfully!\n\n" +
                                  $"Total files: {result.TotalFiles}\n" +
                                  $"Successfully recovered: {result.SuccessfulFiles}\n" +
                                  $"Failed: {result.FailedFiles}\n" +
                                  $"Skipped: {result.SkippedFiles}\n" +
                                  $"Duration: {result.Duration:hh\\:mm\\:ss}",
                                  "Recovery Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                LogStatus($"Error: {ex.Message}");
                MessageBox.Show($"An error occurred during recovery:\n\n{ex.Message}", 
                    "Recovery Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _logger.LogError("Recovery operation failed", ex);
            }
            finally
            {
                _isRecovering = false;
                _cancellationTokenSource?.Dispose();
                _cancellationTokenSource = null;
                UpdateUIState();
            }
        }

        /// <summary>
        /// Handles the Cancel button click event.
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (_isRecovering && _cancellationTokenSource != null)
            {
                if (MessageBox.Show("Are you sure you want to cancel the recovery operation?", 
                    "Confirm Cancellation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    LogStatus("Cancelling recovery...");
                    _cancellationTokenSource.Cancel();
                }
            }
        }

        /// <summary>
        /// Handles the Clear button click event.
        /// </summary>
        private void btnClear_Click(object sender, EventArgs e)
        {
            if (_isRecovering)
            {
                MessageBox.Show("Cannot clear while recovery is in progress.", "Operation Not Allowed", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            txtSourcePath.Clear();
            txtDestinationPath.Clear();
            lblDriveInfo.Text = "";
            progressBar.Value = 0;
            lblProgress.Text = "0%";
            lblFileCount.Text = "0/0 files recovered";
            lblCurrentFile.Text = "Ready";
            lblTimeRemaining.Text = "";
            txtLog.Clear();
            
            LogStatus("Form cleared. Ready to recover files.");
            UpdateUIState();
        }

        /// <summary>
        /// Handles recovery progress updates.
        /// </summary>
        private void OnRecoveryProgressChanged(object? sender, RecoveryProgressEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new EventHandler<RecoveryProgressEventArgs>(OnRecoveryProgressChanged), sender, e);
                return;
            }

            progressBar.Value = Math.Min(e.ProgressPercentage, 100);
            lblProgress.Text = $"{e.ProgressPercentage}%";
            lblFileCount.Text = $"{e.ProcessedFiles}/{e.TotalFiles} files recovered";
            lblCurrentFile.Text = $"Current: {e.CurrentFile}";
            
            if (e.EstimatedTimeRemaining.HasValue)
            {
                var timeRemaining = e.EstimatedTimeRemaining.Value;
                lblTimeRemaining.Text = $"Time remaining: {timeRemaining:hh\\:mm\\:ss}";
            }
        }

        /// <summary>
        /// Handles recovery status updates.
        /// </summary>
        private void OnRecoveryStatusChanged(object? sender, string status)
        {
            if (InvokeRequired)
            {
                Invoke(new EventHandler<string>(OnRecoveryStatusChanged), sender, status);
                return;
            }

            LogStatus(status);
        }

        /// <summary>
        /// Updates the source drive information display.
        /// </summary>
        private void UpdateSourceDriveInfo()
        {
            var driveInfo = FileHelper.GetDriveInfo(txtSourcePath.Text);
            if (driveInfo != null && driveInfo.IsReady)
            {
                lblDriveInfo.Text = $"Drive: {driveInfo.Name} | " +
                                  $"Total: {FileHelper.FormatFileSize(driveInfo.TotalSize)} | " +
                                  $"Free: {FileHelper.FormatFileSize(driveInfo.AvailableFreeSpace)}";
            }
            else
            {
                lblDriveInfo.Text = "Drive information not available";
            }
        }

        /// <summary>
        /// Updates the UI state based on current operation status.
        /// </summary>
        private void UpdateUIState()
        {
            bool hasSource = !string.IsNullOrWhiteSpace(txtSourcePath.Text);
            bool hasDestination = !string.IsNullOrWhiteSpace(txtDestinationPath.Text);

            btnStartRecovery.Enabled = !_isRecovering && hasSource && hasDestination;
            btnCancel.Enabled = _isRecovering;
            btnClear.Enabled = !_isRecovering;
            btnBrowseSource.Enabled = !_isRecovering;
            btnBrowseDestination.Enabled = !_isRecovering;
            chkPreserveFolderStructure.Enabled = !_isRecovering;
            chkOverwriteExisting.Enabled = !_isRecovering;
            cmbFileFilter.Enabled = !_isRecovering;
        }

        /// <summary>
        /// Gets the selected file filter pattern.
        /// </summary>
        /// <returns>The file filter pattern string.</returns>
        private string GetFileFilter()
        {
            return cmbFileFilter.SelectedItem?.ToString() ?? "*.*";
        }

        /// <summary>
        /// Logs a status message to the log display.
        /// </summary>
        /// <param name="message">The message to log.</param>
        private void LogStatus(string message)
        {
            string timestamp = DateTime.Now.ToString("HH:mm:ss");
            txtLog.AppendText($"[{timestamp}] {message}{Environment.NewLine}");
            
            // Auto-scroll to bottom
            txtLog.SelectionStart = txtLog.Text.Length;
            txtLog.ScrollToCaret();
        }

        /// <summary>
        /// Handles form closing event.
        /// </summary>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (_isRecovering)
            {
                if (MessageBox.Show("Recovery is in progress. Are you sure you want to exit?", 
                    "Confirm Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                {
                    e.Cancel = true;
                    return;
                }

                _cancellationTokenSource?.Cancel();
            }

            _logger.LogInfo("Application closing");
            base.OnFormClosing(e);
        }
    }
}
