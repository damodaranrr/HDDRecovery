using System;
using System.IO;
using System.Windows.Forms;
using HDDRecovery.Forms;

namespace HDDRecovery
{
    /// <summary>
    /// Main form for HDD Recovery application
    /// </summary>
    public partial class MainForm : Form
    {
        private string? _destinationPath;

        public MainForm()
        {
            InitializeComponent();
            btnViewFiles.Enabled = false;
        }

        private void btnSelectSource_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Select source folder to recover from";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    txtSourcePath.Text = dialog.SelectedPath;
                }
            }
        }

        private void btnSelectDestination_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Select destination folder for recovered files";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    txtDestinationPath.Text = dialog.SelectedPath;
                    _destinationPath = dialog.SelectedPath;
                }
            }
        }

        private async void btnStartRecovery_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSourcePath.Text))
            {
                MessageBox.Show("Please select a source folder.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDestinationPath.Text))
            {
                MessageBox.Show("Please select a destination folder.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                btnStartRecovery.Enabled = false;
                progressBar.Value = 0;
                lblStatus.Text = "Starting recovery...";

                await Task.Run(() => PerformRecovery(txtSourcePath.Text, txtDestinationPath.Text));

                progressBar.Value = 100;
                lblStatus.Text = "Recovery completed successfully!";
                _destinationPath = txtDestinationPath.Text;
                btnViewFiles.Enabled = true;

                MessageBox.Show("File recovery completed successfully!", "Recovery Complete",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during recovery: {ex.Message}", "Recovery Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Recovery failed.";
            }
            finally
            {
                btnStartRecovery.Enabled = true;
            }
        }

        private void PerformRecovery(string source, string destination)
        {
            // Simulate file recovery process
            if (!Directory.Exists(source))
                throw new DirectoryNotFoundException($"Source directory not found: {source}");

            if (!Directory.Exists(destination))
                Directory.CreateDirectory(destination);

            // Get all files from source
            var files = Directory.GetFiles(source, "*.*", SearchOption.AllDirectories);
            int total = files.Length;
            int current = 0;

            foreach (var file in files)
            {
                try
                {
                    var relativePath = Path.GetRelativePath(source, file);
                    var destFile = Path.Combine(destination, relativePath);
                    var destDir = Path.GetDirectoryName(destFile);

                    if (!string.IsNullOrEmpty(destDir) && !Directory.Exists(destDir))
                        Directory.CreateDirectory(destDir);

                    File.Copy(file, destFile, true);

                    current++;
                    var progress = (int)((double)current / total * 100);
                    Invoke((MethodInvoker)delegate
                    {
                        progressBar.Value = progress;
                        lblStatus.Text = $"Recovering files... {current}/{total}";
                    });
                }
                catch
                {
                    // Continue with next file on error
                }
            }
        }

        private void btnViewFiles_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_destinationPath))
            {
                MessageBox.Show("No recovery destination path available.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (var previewForm = new FilePreviewForm(_destinationPath))
            {
                previewForm.ShowDialog();
            }
        }
    }
}
