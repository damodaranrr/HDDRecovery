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
            this.btnSelectSource = new System.Windows.Forms.Button();
            this.txtSourcePath = new System.Windows.Forms.TextBox();
            this.lblSource = new System.Windows.Forms.Label();
            this.grpDestination = new System.Windows.Forms.GroupBox();
            this.btnSelectDestination = new System.Windows.Forms.Button();
            this.txtDestinationPath = new System.Windows.Forms.TextBox();
            this.lblDestination = new System.Windows.Forms.Label();
            this.btnStartRecovery = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnViewFiles = new System.Windows.Forms.Button();
            this.grpSource.SuspendLayout();
            this.grpDestination.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpSource
            // 
            this.grpSource.Controls.Add(this.btnSelectSource);
            this.grpSource.Controls.Add(this.txtSourcePath);
            this.grpSource.Controls.Add(this.lblSource);
            this.grpSource.Location = new System.Drawing.Point(12, 12);
            this.grpSource.Name = "grpSource";
            this.grpSource.Size = new System.Drawing.Size(560, 80);
            this.grpSource.TabIndex = 0;
            this.grpSource.TabStop = false;
            this.grpSource.Text = "Source";
            // 
            // btnSelectSource
            // 
            this.btnSelectSource.Location = new System.Drawing.Point(460, 40);
            this.btnSelectSource.Name = "btnSelectSource";
            this.btnSelectSource.Size = new System.Drawing.Size(85, 23);
            this.btnSelectSource.TabIndex = 2;
            this.btnSelectSource.Text = "Browse...";
            this.btnSelectSource.UseVisualStyleBackColor = true;
            this.btnSelectSource.Click += new System.EventHandler(this.btnSelectSource_Click);
            // 
            // txtSourcePath
            // 
            this.txtSourcePath.Location = new System.Drawing.Point(15, 41);
            this.txtSourcePath.Name = "txtSourcePath";
            this.txtSourcePath.ReadOnly = true;
            this.txtSourcePath.Size = new System.Drawing.Size(430, 23);
            this.txtSourcePath.TabIndex = 1;
            // 
            // lblSource
            // 
            this.lblSource.AutoSize = true;
            this.lblSource.Location = new System.Drawing.Point(15, 20);
            this.lblSource.Name = "lblSource";
            this.lblSource.Size = new System.Drawing.Size(144, 15);
            this.lblSource.TabIndex = 0;
            this.lblSource.Text = "Select folder to recover from:";
            // 
            // grpDestination
            // 
            this.grpDestination.Controls.Add(this.btnSelectDestination);
            this.grpDestination.Controls.Add(this.txtDestinationPath);
            this.grpDestination.Controls.Add(this.lblDestination);
            this.grpDestination.Location = new System.Drawing.Point(12, 98);
            this.grpDestination.Name = "grpDestination";
            this.grpDestination.Size = new System.Drawing.Size(560, 80);
            this.grpDestination.TabIndex = 1;
            this.grpDestination.TabStop = false;
            this.grpDestination.Text = "Destination";
            // 
            // btnSelectDestination
            // 
            this.btnSelectDestination.Location = new System.Drawing.Point(460, 40);
            this.btnSelectDestination.Name = "btnSelectDestination";
            this.btnSelectDestination.Size = new System.Drawing.Size(85, 23);
            this.btnSelectDestination.TabIndex = 2;
            this.btnSelectDestination.Text = "Browse...";
            this.btnSelectDestination.UseVisualStyleBackColor = true;
            this.btnSelectDestination.Click += new System.EventHandler(this.btnSelectDestination_Click);
            // 
            // txtDestinationPath
            // 
            this.txtDestinationPath.Location = new System.Drawing.Point(15, 41);
            this.txtDestinationPath.Name = "txtDestinationPath";
            this.txtDestinationPath.ReadOnly = true;
            this.txtDestinationPath.Size = new System.Drawing.Size(430, 23);
            this.txtDestinationPath.TabIndex = 1;
            // 
            // lblDestination
            // 
            this.lblDestination.AutoSize = true;
            this.lblDestination.Location = new System.Drawing.Point(15, 20);
            this.lblDestination.Name = "lblDestination";
            this.lblDestination.Size = new System.Drawing.Size(170, 15);
            this.lblDestination.TabIndex = 0;
            this.lblDestination.Text = "Select destination for recovered files:";
            // 
            // btnStartRecovery
            // 
            this.btnStartRecovery.Location = new System.Drawing.Point(12, 194);
            this.btnStartRecovery.Name = "btnStartRecovery";
            this.btnStartRecovery.Size = new System.Drawing.Size(150, 30);
            this.btnStartRecovery.TabIndex = 2;
            this.btnStartRecovery.Text = "Start Recovery";
            this.btnStartRecovery.UseVisualStyleBackColor = true;
            this.btnStartRecovery.Click += new System.EventHandler(this.btnStartRecovery_Click);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(12, 240);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(560, 23);
            this.progressBar.TabIndex = 3;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(12, 275);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(39, 15);
            this.lblStatus.TabIndex = 4;
            this.lblStatus.Text = "Ready";
            // 
            // btnViewFiles
            // 
            this.btnViewFiles.Location = new System.Drawing.Point(180, 194);
            this.btnViewFiles.Name = "btnViewFiles";
            this.btnViewFiles.Size = new System.Drawing.Size(150, 30);
            this.btnViewFiles.TabIndex = 5;
            this.btnViewFiles.Text = "View Recovered Files";
            this.btnViewFiles.UseVisualStyleBackColor = true;
            this.btnViewFiles.Click += new System.EventHandler(this.btnViewFiles_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 311);
            this.Controls.Add(this.btnViewFiles);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.btnStartRecovery);
            this.Controls.Add(this.grpDestination);
            this.Controls.Add(this.grpSource);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HDD Recovery";
            this.grpSource.ResumeLayout(false);
            this.grpSource.PerformLayout();
            this.grpDestination.ResumeLayout(false);
            this.grpDestination.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.GroupBox grpSource;
        private System.Windows.Forms.Button btnSelectSource;
        private System.Windows.Forms.TextBox txtSourcePath;
        private System.Windows.Forms.Label lblSource;
        private System.Windows.Forms.GroupBox grpDestination;
        private System.Windows.Forms.Button btnSelectDestination;
        private System.Windows.Forms.TextBox txtDestinationPath;
        private System.Windows.Forms.Label lblDestination;
        private System.Windows.Forms.Button btnStartRecovery;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnViewFiles;
    }
}
