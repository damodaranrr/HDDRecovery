using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using HDDRecovery.Helpers;
using HDDRecovery.Models;
using HDDRecovery.Services;

namespace HDDRecovery.Forms
{
    /// <summary>
    /// File preview form with explorer-style interface
    /// </summary>
    public partial class FilePreviewForm : Form
    {
        private readonly string _destinationPath;
        private readonly FilePreviewService _previewService;
        private List<RecoveredFileInfo> _allFiles;
        private List<RecoveredFileInfo> _filteredFiles;
        private Bitmap? _currentImage;
        private float _zoomFactor = 1.0f;
        private Point _panOffset;
        private Point _lastMousePos;
        private bool _isPanning;
        private readonly Dictionary<string, Bitmap> _imageCache;
        private const int MaxCacheSize = 50;

        public FilePreviewForm(string destinationPath)
        {
            InitializeComponent();
            _destinationPath = destinationPath;
            _previewService = new FilePreviewService();
            _allFiles = new List<RecoveredFileInfo>();
            _filteredFiles = new List<RecoveredFileInfo>();
            _imageCache = new Dictionary<string, Bitmap>();
            
            InitializeControls();
            LoadFilesAsync();
        }

        private void InitializeControls()
        {
            // Set up ListView
            lvFiles.View = View.Details;
            lvFiles.FullRowSelect = true;
            lvFiles.MultiSelect = true;
            lvFiles.GridLines = true;
            
            // Set up PictureBox
            picPreview.SizeMode = PictureBoxSizeMode.Zoom;
            picPreview.MouseWheel += PicPreview_MouseWheel;
            picPreview.MouseDown += PicPreview_MouseDown;
            picPreview.MouseMove += PicPreview_MouseMove;
            picPreview.MouseUp += PicPreview_MouseUp;
            
            // Set up TreeView
            tvFolders.ImageList = new ImageList();
            tvFolders.ImageList.Images.Add("folder", SystemIcons.Application.ToBitmap());
        }

        private async void LoadFilesAsync()
        {
            try
            {
                lblStatus.Text = "Loading files...";
                toolStripProgressBar.Visible = true;

                await Task.Run(() =>
                {
                    _allFiles = _previewService.ScanRecoveredFiles(_destinationPath);
                    _filteredFiles = new List<RecoveredFileInfo>(_allFiles);
                });

                BuildFolderTree();
                UpdateStatusBar();
                
                lblStatus.Text = $"Loaded {_allFiles.Count} files";
                toolStripProgressBar.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading files: {ex.Message}", "Load Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Error loading files";
                toolStripProgressBar.Visible = false;
            }
        }

        private void BuildFolderTree()
        {
            tvFolders.Nodes.Clear();
            
            var rootNode = new TreeNode(_destinationPath)
            {
                Tag = _destinationPath,
                ImageIndex = 0,
                SelectedImageIndex = 0
            };
            tvFolders.Nodes.Add(rootNode);

            var folders = _previewService.GetUniqueFolders(_allFiles);
            var folderNodes = new Dictionary<string, TreeNode>();
            folderNodes[_destinationPath] = rootNode;

            foreach (var folder in folders)
            {
                if (folder == _destinationPath)
                    continue;

                var parts = folder.Substring(_destinationPath.Length).TrimStart(Path.DirectorySeparatorChar).Split(Path.DirectorySeparatorChar);
                var currentPath = _destinationPath;
                TreeNode? parentNode = rootNode;

                foreach (var part in parts)
                {
                    currentPath = Path.Combine(currentPath, part);
                    
                    if (!folderNodes.ContainsKey(currentPath))
                    {
                        var fileCount = _allFiles.Count(f => Path.GetDirectoryName(f.FullPath) == currentPath);
                        var newNode = new TreeNode($"{part} ({fileCount})")
                        {
                            Tag = currentPath,
                            ImageIndex = 0,
                            SelectedImageIndex = 0
                        };
                        parentNode?.Nodes.Add(newNode);
                        folderNodes[currentPath] = newNode;
                        parentNode = newNode;
                    }
                    else
                    {
                        parentNode = folderNodes[currentPath];
                    }
                }
            }

            rootNode.Expand();
        }

        private void tvFolders_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node?.Tag is string folderPath)
            {
                DisplayFilesInFolder(folderPath);
            }
        }

        private void DisplayFilesInFolder(string folderPath)
        {
            lvFiles.Items.Clear();
            
            var filesInFolder = _filteredFiles.Where(f => 
                Path.GetDirectoryName(f.FullPath) == folderPath).ToList();

            foreach (var file in filesInFolder)
            {
                var item = new ListViewItem(file.FileName);
                item.SubItems.Add(file.FileType);
                item.SubItems.Add(FileHelper.FormatFileSize(file.FileSize));
                item.SubItems.Add(file.ModifiedDate.ToString("g"));
                item.SubItems.Add(file.Status.ToString());
                item.Tag = file;
                
                // Try to get file icon
                var icon = _previewService.GetFileIcon(file.FullPath);
                if (icon != null)
                {
                    if (lvFiles.SmallImageList == null)
                    {
                        lvFiles.SmallImageList = new ImageList();
                    }
                    var key = file.FileType;
                    if (!lvFiles.SmallImageList.Images.ContainsKey(key))
                    {
                        lvFiles.SmallImageList.Images.Add(key, icon);
                    }
                    item.ImageKey = key;
                }
                
                lvFiles.Items.Add(item);
            }
        }

        private void lvFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvFiles.SelectedItems.Count > 0)
            {
                var selectedFile = lvFiles.SelectedItems[0].Tag as RecoveredFileInfo;
                if (selectedFile != null)
                {
                    DisplayFilePreview(selectedFile);
                    UpdateStatusBar();
                }
            }
        }

        private async void DisplayFilePreview(RecoveredFileInfo file)
        {
            // Clear current image
            if (_currentImage != null && !_imageCache.ContainsValue(_currentImage))
            {
                _currentImage.Dispose();
                _currentImage = null;
            }

            if (!FileHelper.IsImageFile(file.FullPath))
            {
                picPreview.Image = null;
                lblImageInfo.Text = "Not an image file";
                return;
            }

            try
            {
                lblImageInfo.Text = "Loading...";

                // Check cache first
                if (_imageCache.TryGetValue(file.FullPath, out var cachedImage))
                {
                    _currentImage = cachedImage;
                    picPreview.Image = _currentImage;
                    UpdateImageInfo(file);
                    return;
                }

                // Load image asynchronously
                var image = await Task.Run(() => FileHelper.LoadImageSafe(file.FullPath));

                if (image != null)
                {
                    _currentImage = image;
                    picPreview.Image = _currentImage;
                    
                    // Add to cache
                    if (_imageCache.Count >= MaxCacheSize)
                    {
                        var firstKey = _imageCache.Keys.First();
                        _imageCache[firstKey].Dispose();
                        _imageCache.Remove(firstKey);
                    }
                    _imageCache[file.FullPath] = image;
                    
                    UpdateImageInfo(file);
                }
                else
                {
                    picPreview.Image = null;
                    lblImageInfo.Text = "Failed to load image";
                }
            }
            catch (Exception ex)
            {
                picPreview.Image = null;
                lblImageInfo.Text = $"Error: {ex.Message}";
            }
        }

        private void UpdateImageInfo(RecoveredFileInfo file)
        {
            if (_currentImage != null)
            {
                lblImageInfo.Text = $"{_currentImage.Width} x {_currentImage.Height} - {FileHelper.FormatFileSize(file.FileSize)}";
            }
        }

        private void UpdateStatusBar()
        {
            var selectedCount = lvFiles.SelectedItems.Count;
            long totalSize = 0;

            foreach (ListViewItem item in lvFiles.SelectedItems)
            {
                if (item.Tag is RecoveredFileInfo file)
                {
                    totalSize += file.FileSize;
                }
            }

            if (selectedCount > 0)
            {
                lblStatus.Text = $"Selected: {selectedCount} file(s) - {FileHelper.FormatFileSize(totalSize)}";
            }
            else
            {
                lblStatus.Text = $"Total: {_allFiles.Count} file(s)";
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            var searchText = txtSearch.Text.ToLower();
            
            if (string.IsNullOrWhiteSpace(searchText))
            {
                _filteredFiles = new List<RecoveredFileInfo>(_allFiles);
            }
            else
            {
                _filteredFiles = _allFiles.Where(f => 
                    f.FileName.ToLower().Contains(searchText)).ToList();
            }

            BuildFolderTree();
            
            if (tvFolders.SelectedNode?.Tag is string folderPath)
            {
                DisplayFilesInFolder(folderPath);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadFilesAsync();
        }

        private void btnFitToWindow_Click(object sender, EventArgs e)
        {
            picPreview.SizeMode = PictureBoxSizeMode.Zoom;
            _zoomFactor = 1.0f;
        }

        private void btnActualSize_Click(object sender, EventArgs e)
        {
            picPreview.SizeMode = PictureBoxSizeMode.CenterImage;
            _zoomFactor = 1.0f;
        }

        private void btnZoomIn_Click(object sender, EventArgs e)
        {
            _zoomFactor *= 1.2f;
            ApplyZoom();
        }

        private void btnZoomOut_Click(object sender, EventArgs e)
        {
            _zoomFactor /= 1.2f;
            ApplyZoom();
        }

        private void PicPreview_MouseWheel(object? sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                _zoomFactor *= 1.1f;
            }
            else
            {
                _zoomFactor /= 1.1f;
            }
            ApplyZoom();
        }

        private void ApplyZoom()
        {
            if (_currentImage == null) return;

            picPreview.SizeMode = PictureBoxSizeMode.CenterImage;
            var newWidth = (int)(_currentImage.Width * _zoomFactor);
            var newHeight = (int)(_currentImage.Height * _zoomFactor);
            
            var resized = new Bitmap(newWidth, newHeight);
            using (var g = Graphics.FromImage(resized))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(_currentImage, 0, 0, newWidth, newHeight);
            }
            
            picPreview.Image = resized;
        }

        private void PicPreview_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _isPanning = true;
                _lastMousePos = e.Location;
            }
        }

        private void PicPreview_MouseMove(object? sender, MouseEventArgs e)
        {
            if (_isPanning)
            {
                var deltaX = e.X - _lastMousePos.X;
                var deltaY = e.Y - _lastMousePos.Y;
                _panOffset.X += deltaX;
                _panOffset.Y += deltaY;
                _lastMousePos = e.Location;
                picPreview.Invalidate();
            }
        }

        private void PicPreview_MouseUp(object? sender, MouseEventArgs e)
        {
            _isPanning = false;
        }

        private void lvFiles_DoubleClick(object sender, EventArgs e)
        {
            if (lvFiles.SelectedItems.Count > 0)
            {
                var file = lvFiles.SelectedItems[0].Tag as RecoveredFileInfo;
                if (file != null)
                {
                    OpenFile(file.FullPath);
                }
            }
        }

        private void OpenFile(string filePath)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening file: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvFiles.SelectedItems.Count > 0)
            {
                var file = lvFiles.SelectedItems[0].Tag as RecoveredFileInfo;
                if (file != null)
                {
                    OpenFile(file.FullPath);
                }
            }
        }

        private void openFileLocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvFiles.SelectedItems.Count > 0)
            {
                var file = lvFiles.SelectedItems[0].Tag as RecoveredFileInfo;
                if (file != null)
                {
                    try
                    {
                        Process.Start("explorer.exe", $"/select,\"{file.FullPath}\"");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error opening location: {ex.Message}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void copyPathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvFiles.SelectedItems.Count > 0)
            {
                var file = lvFiles.SelectedItems[0].Tag as RecoveredFileInfo;
                if (file != null)
                {
                    Clipboard.SetText(file.FullPath);
                }
            }
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvFiles.SelectedItems.Count > 0)
            {
                var file = lvFiles.SelectedItems[0].Tag as RecoveredFileInfo;
                if (file != null)
                {
                    var info = $"File: {file.FileName}\n" +
                              $"Path: {file.FullPath}\n" +
                              $"Type: {file.FileType}\n" +
                              $"Size: {FileHelper.FormatFileSize(file.FileSize)}\n" +
                              $"Created: {file.CreatedDate}\n" +
                              $"Modified: {file.ModifiedDate}\n" +
                              $"Status: {file.Status}";
                    
                    if (!string.IsNullOrEmpty(file.ErrorMessage))
                    {
                        info += $"\nError: {file.ErrorMessage}";
                    }
                    
                    MessageBox.Show(info, "File Properties", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void openInExplorerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tvFolders.SelectedNode?.Tag is string folderPath)
            {
                try
                {
                    Process.Start("explorer.exe", folderPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error opening folder: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void lvFiles_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Simple sorting implementation
            var items = lvFiles.Items.Cast<ListViewItem>().ToList();
            
            items.Sort((a, b) =>
            {
                return string.Compare(a.SubItems[e.Column].Text, b.SubItems[e.Column].Text);
            });

            lvFiles.Items.Clear();
            lvFiles.Items.AddRange(items.ToArray());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
                
                // Dispose current image if not in cache
                if (_currentImage != null && !_imageCache.ContainsValue(_currentImage))
                {
                    _currentImage.Dispose();
                }
                
                // Dispose cached images
                foreach (var image in _imageCache.Values)
                {
                    image.Dispose();
                }
                _imageCache.Clear();
            }
            base.Dispose(disposing);
        }
    }
}
