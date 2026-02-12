namespace HDDRecovery
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.grpSource = new System.Windows.Forms.GroupBox();
            this.lblDriveInfo = new System.Windows.Forms.Label();
            this.btnBrowseSource = new System.Windows.Forms.Button();
            this.txtSourcePath = new System.Windows.Forms.TextBox();
            this.lblSource = new System.Windows.Forms.Label();
            this.grpDestination = new System.Windows.Forms.GroupBox();
            this.btnBrowseDestination = new System.Windows.Forms.Button();
            this.txtDestinationPath = new System.Windows.Forms.TextBox();
            this.lblDestination = new System.Windows.Forms.Label();
            this.grpOptions = new System.Windows.Forms.GroupBox();
            this.cmbFileFilter = new System.Windows.Forms.ComboBox();
            this.lblFileFilter = new System.Windows.Forms.Label();
            this.chkOverwriteExisting = new System.Windows.Forms.CheckBox();
            this.chkPreserveFolderStructure = new System.Windows.Forms.CheckBox();
            this.grpActions = new System.Windows.Forms.GroupBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnStartRecovery = new System.Windows.Forms.Button();
            this.grpProgress = new System.Windows.Forms.GroupBox();
            this.lblTimeRemaining = new System.Windows.Forms.Label();
            this.lblCurrentFile = new System.Windows.Forms.Label();
            this.lblFileCount = new System.Windows.Forms.Label();
            this.lblProgress = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.grpLog = new System.Windows.Forms.GroupBox();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.grpSource.SuspendLayout();
            this.grpDestination.SuspendLayout();
            this.grpOptions.SuspendLayout();
            this.grpActions.SuspendLayout();
            this.grpProgress.SuspendLayout();
            this.grpLog.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpSource
            // 
            this.grpSource.Controls.Add(this.lblDriveInfo);
            this.grpSource.Controls.Add(this.btnBrowseSource);
            this.grpSource.Controls.Add(this.txtSourcePath);
            this.grpSource.Controls.Add(this.lblSource);
            this.grpSource.Location = new System.Drawing.Point(12, 12);
            this.grpSource.Name = "grpSource";
            this.grpSource.Size = new System.Drawing.Size(760, 100);
            this.grpSource.TabIndex = 0;
            this.grpSource.TabStop = false;
            this.grpSource.Text = "Source Path";
            // 
            // lblDriveInfo
            // 
            this.lblDriveInfo.AutoSize = true;
            this.lblDriveInfo.ForeColor = System.Drawing.Color.Blue;
            this.lblDriveInfo.Location = new System.Drawing.Point(15, 75);
            this.lblDriveInfo.Name = "lblDriveInfo";
            this.lblDriveInfo.Size = new System.Drawing.Size(0, 15);
            this.lblDriveInfo.TabIndex = 3;
            // 
            // btnBrowseSource
            // 
            this.btnBrowseSource.Location = new System.Drawing.Point(650, 40);
            this.btnBrowseSource.Name = "btnBrowseSource";
            this.btnBrowseSource.Size = new System.Drawing.Size(100, 25);
            this.btnBrowseSource.TabIndex = 2;
            this.btnBrowseSource.Text = "Browse...";
            this.btnBrowseSource.UseVisualStyleBackColor = true;
            this.btnBrowseSource.Click += new System.EventHandler(this.btnBrowseSource_Click);
            // 
            // txtSourcePath
            // 
            this.txtSourcePath.Location = new System.Drawing.Point(15, 42);
            this.txtSourcePath.Name = "txtSourcePath";
            this.txtSourcePath.ReadOnly = true;
            this.txtSourcePath.Size = new System.Drawing.Size(625, 23);
            this.txtSourcePath.TabIndex = 1;
            // 
            // lblSource
            // 
            this.lblSource.AutoSize = true;
            this.lblSource.Location = new System.Drawing.Point(15, 22);
            this.lblSource.Name = "lblSource";
            this.lblSource.Size = new System.Drawing.Size(269, 15);
            this.lblSource.TabIndex = 0;
            this.lblSource.Text = "Select source drive or folder to recover files from:";
            // 
            // grpDestination
            // 
            this.grpDestination.Controls.Add(this.btnBrowseDestination);
            this.grpDestination.Controls.Add(this.txtDestinationPath);
            this.grpDestination.Controls.Add(this.lblDestination);
            this.grpDestination.Location = new System.Drawing.Point(12, 118);
            this.grpDestination.Name = "grpDestination";
            this.grpDestination.Size = new System.Drawing.Size(760, 80);
            this.grpDestination.TabIndex = 1;
            this.grpDestination.TabStop = false;
            this.grpDestination.Text = "Destination Path";
            // 
            // btnBrowseDestination
            // 
            this.btnBrowseDestination.Location = new System.Drawing.Point(650, 40);
            this.btnBrowseDestination.Name = "btnBrowseDestination";
            this.btnBrowseDestination.Size = new System.Drawing.Size(100, 25);
            this.btnBrowseDestination.TabIndex = 2;
            this.btnBrowseDestination.Text = "Browse...";
            this.btnBrowseDestination.UseVisualStyleBackColor = true;
            this.btnBrowseDestination.Click += new System.EventHandler(this.btnBrowseDestination_Click);
            // 
            // txtDestinationPath
            // 
            this.txtDestinationPath.Location = new System.Drawing.Point(15, 42);
            this.txtDestinationPath.Name = "txtDestinationPath";
            this.txtDestinationPath.ReadOnly = true;
            this.txtDestinationPath.Size = new System.Drawing.Size(625, 23);
            this.txtDestinationPath.TabIndex = 1;
            // 
            // lblDestination
            // 
            this.lblDestination.AutoSize = true;
            this.lblDestination.Location = new System.Drawing.Point(15, 22);
            this.lblDestination.Name = "lblDestination";
            this.lblDestination.Size = new System.Drawing.Size(246, 15);
            this.lblDestination.TabIndex = 0;
            this.lblDestination.Text = "Select destination folder for recovered files:";
            // 
            // grpOptions
            // 
            this.grpOptions.Controls.Add(this.cmbFileFilter);
            this.grpOptions.Controls.Add(this.lblFileFilter);
            this.grpOptions.Controls.Add(this.chkOverwriteExisting);
            this.grpOptions.Controls.Add(this.chkPreserveFolderStructure);
            this.grpOptions.Location = new System.Drawing.Point(12, 204);
            this.grpOptions.Name = "grpOptions";
            this.grpOptions.Size = new System.Drawing.Size(760, 100);
            this.grpOptions.TabIndex = 2;
            this.grpOptions.TabStop = false;
            this.grpOptions.Text = "Recovery Options";
            // 
            // cmbFileFilter
            // 
            this.cmbFileFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFileFilter.FormattingEnabled = true;
            this.cmbFileFilter.Items.AddRange(new object[] {
            "*.*",
            "*.jpg;*.jpeg;*.png;*.gif;*.bmp",
            "*.doc;*.docx;*.pdf;*.txt",
            "*.mp3;*.wav;*.flac",
            "*.mp4;*.avi;*.mkv;*.mov"});
            this.cmbFileFilter.Location = new System.Drawing.Point(400, 25);
            this.cmbFileFilter.Name = "cmbFileFilter";
            this.cmbFileFilter.Size = new System.Drawing.Size(350, 23);
            this.cmbFileFilter.TabIndex = 3;
            // 
            // lblFileFilter
            // 
            this.lblFileFilter.AutoSize = true;
            this.lblFileFilter.Location = new System.Drawing.Point(400, 7);
            this.lblFileFilter.Name = "lblFileFilter";
            this.lblFileFilter.Size = new System.Drawing.Size(59, 15);
            this.lblFileFilter.TabIndex = 2;
            this.lblFileFilter.Text = "File Filter:";
            // 
            // chkOverwriteExisting
            // 
            this.chkOverwriteExisting.AutoSize = true;
            this.chkOverwriteExisting.Location = new System.Drawing.Point(15, 52);
            this.chkOverwriteExisting.Name = "chkOverwriteExisting";
            this.chkOverwriteExisting.Size = new System.Drawing.Size(138, 19);
            this.chkOverwriteExisting.TabIndex = 1;
            this.chkOverwriteExisting.Text = "Overwrite existing files";
            this.chkOverwriteExisting.UseVisualStyleBackColor = true;
            // 
            // chkPreserveFolderStructure
            // 
            this.chkPreserveFolderStructure.AutoSize = true;
            this.chkPreserveFolderStructure.Checked = true;
            this.chkPreserveFolderStructure.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPreserveFolderStructure.Location = new System.Drawing.Point(15, 27);
            this.chkPreserveFolderStructure.Name = "chkPreserveFolderStructure";
            this.chkPreserveFolderStructure.Size = new System.Drawing.Size(162, 19);
            this.chkPreserveFolderStructure.TabIndex = 0;
            this.chkPreserveFolderStructure.Text = "Preserve folder structure";
            this.chkPreserveFolderStructure.UseVisualStyleBackColor = true;
            // 
            // grpActions
            // 
            this.grpActions.Controls.Add(this.btnClear);
            this.grpActions.Controls.Add(this.btnCancel);
            this.grpActions.Controls.Add(this.btnStartRecovery);
            this.grpActions.Location = new System.Drawing.Point(12, 310);
            this.grpActions.Name = "grpActions";
            this.grpActions.Size = new System.Drawing.Size(760, 70);
            this.grpActions.TabIndex = 3;
            this.grpActions.TabStop = false;
            this.grpActions.Text = "Actions";
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(360, 25);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(150, 35);
            this.btnClear.TabIndex = 2;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Enabled = false;
            this.btnCancel.Location = new System.Drawing.Point(190, 25);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(150, 35);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel Recovery";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnStartRecovery
            // 
            this.btnStartRecovery.Enabled = false;
            this.btnStartRecovery.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnStartRecovery.Location = new System.Drawing.Point(20, 25);
            this.btnStartRecovery.Name = "btnStartRecovery";
            this.btnStartRecovery.Size = new System.Drawing.Size(150, 35);
            this.btnStartRecovery.TabIndex = 0;
            this.btnStartRecovery.Text = "Start Recovery";
            this.btnStartRecovery.UseVisualStyleBackColor = true;
            this.btnStartRecovery.Click += new System.EventHandler(this.btnStartRecovery_Click);
            // 
            // grpProgress
            // 
            this.grpProgress.Controls.Add(this.lblTimeRemaining);
            this.grpProgress.Controls.Add(this.lblCurrentFile);
            this.grpProgress.Controls.Add(this.lblFileCount);
            this.grpProgress.Controls.Add(this.lblProgress);
            this.grpProgress.Controls.Add(this.progressBar);
            this.grpProgress.Location = new System.Drawing.Point(12, 386);
            this.grpProgress.Name = "grpProgress";
            this.grpProgress.Size = new System.Drawing.Size(760, 120);
            this.grpProgress.TabIndex = 4;
            this.grpProgress.TabStop = false;
            this.grpProgress.Text = "Progress";
            // 
            // lblTimeRemaining
            // 
            this.lblTimeRemaining.AutoSize = true;
            this.lblTimeRemaining.Location = new System.Drawing.Point(15, 95);
            this.lblTimeRemaining.Name = "lblTimeRemaining";
            this.lblTimeRemaining.Size = new System.Drawing.Size(0, 15);
            this.lblTimeRemaining.TabIndex = 4;
            // 
            // lblCurrentFile
            // 
            this.lblCurrentFile.AutoSize = true;
            this.lblCurrentFile.Location = new System.Drawing.Point(15, 75);
            this.lblCurrentFile.Name = "lblCurrentFile";
            this.lblCurrentFile.Size = new System.Drawing.Size(42, 15);
            this.lblCurrentFile.TabIndex = 3;
            this.lblCurrentFile.Text = "Ready";
            // 
            // lblFileCount
            // 
            this.lblFileCount.AutoSize = true;
            this.lblFileCount.Location = new System.Drawing.Point(15, 55);
            this.lblFileCount.Name = "lblFileCount";
            this.lblFileCount.Size = new System.Drawing.Size(103, 15);
            this.lblFileCount.TabIndex = 2;
            this.lblFileCount.Text = "0/0 files recovered";
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblProgress.Location = new System.Drawing.Point(720, 28);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(25, 15);
            this.lblProgress.TabIndex = 1;
            this.lblProgress.Text = "0%";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(15, 25);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(700, 23);
            this.progressBar.TabIndex = 0;
            // 
            // grpLog
            // 
            this.grpLog.Controls.Add(this.txtLog);
            this.grpLog.Location = new System.Drawing.Point(12, 512);
            this.grpLog.Name = "grpLog";
            this.grpLog.Size = new System.Drawing.Size(760, 150);
            this.grpLog.TabIndex = 5;
            this.grpLog.TabStop = false;
            this.grpLog.Text = "Status Log";
            // 
            // txtLog
            // 
            this.txtLog.BackColor = System.Drawing.SystemColors.Window;
            this.txtLog.Font = new System.Drawing.Font("Consolas", 9F);
            this.txtLog.Location = new System.Drawing.Point(15, 22);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(735, 115);
            this.txtLog.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 671);
            this.Controls.Add(this.grpLog);
            this.Controls.Add(this.grpProgress);
            this.Controls.Add(this.grpActions);
            this.Controls.Add(this.grpOptions);
            this.Controls.Add(this.grpDestination);
            this.Controls.Add(this.grpSource);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HDD File Recovery Application";
            this.grpSource.ResumeLayout(false);
            this.grpSource.PerformLayout();
            this.grpDestination.ResumeLayout(false);
            this.grpDestination.PerformLayout();
            this.grpOptions.ResumeLayout(false);
            this.grpOptions.PerformLayout();
            this.grpActions.ResumeLayout(false);
            this.grpProgress.ResumeLayout(false);
            this.grpProgress.PerformLayout();
            this.grpLog.ResumeLayout(false);
            this.grpLog.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpSource;
        private System.Windows.Forms.Label lblDriveInfo;
        private System.Windows.Forms.Button btnBrowseSource;
        private System.Windows.Forms.TextBox txtSourcePath;
        private System.Windows.Forms.Label lblSource;
        private System.Windows.Forms.GroupBox grpDestination;
        private System.Windows.Forms.Button btnBrowseDestination;
        private System.Windows.Forms.TextBox txtDestinationPath;
        private System.Windows.Forms.Label lblDestination;
        private System.Windows.Forms.GroupBox grpOptions;
        private System.Windows.Forms.ComboBox cmbFileFilter;
        private System.Windows.Forms.Label lblFileFilter;
        private System.Windows.Forms.CheckBox chkOverwriteExisting;
        private System.Windows.Forms.CheckBox chkPreserveFolderStructure;
        private System.Windows.Forms.GroupBox grpActions;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnStartRecovery;
        private System.Windows.Forms.GroupBox grpProgress;
        private System.Windows.Forms.Label lblTimeRemaining;
        private System.Windows.Forms.Label lblCurrentFile;
        private System.Windows.Forms.Label lblFileCount;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.GroupBox grpLog;
        private System.Windows.Forms.TextBox txtLog;
    }
}
