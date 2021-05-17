
namespace CreateFolderFromCreationDate
{
    partial class FrmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.openToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.helpToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabelInfo = new System.Windows.Forms.ToolStripLabel();
            this.dgvFiles = new System.Windows.Forms.DataGridView();
            this.btnGenerateFolder = new System.Windows.Forms.Button();
            this.txtLocationToGenerate = new System.Windows.Forms.TextBox();
            this.lblLocationToGenerate = new System.Windows.Forms.Label();
            this.txtOriginLocation = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSelect = new System.Windows.Forms.Button();
            this.chkCheckExtensions = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFiles)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.SystemColors.Info;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripButton,
            this.toolStripSeparator,
            this.helpToolStripButton,
            this.toolStripProgressBar,
            this.toolStripSeparator1,
            this.toolStripLabelInfo});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(804, 27);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // openToolStripButton
            // 
            this.openToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.openToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripButton.Image")));
            this.openToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openToolStripButton.Name = "openToolStripButton";
            this.openToolStripButton.Size = new System.Drawing.Size(24, 24);
            this.openToolStripButton.Text = "&Open";
            this.openToolStripButton.Click += new System.EventHandler(this.OpenToolStripButton_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(6, 27);
            // 
            // helpToolStripButton
            // 
            this.helpToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.helpToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("helpToolStripButton.Image")));
            this.helpToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.helpToolStripButton.Name = "helpToolStripButton";
            this.helpToolStripButton.Size = new System.Drawing.Size(24, 24);
            this.helpToolStripButton.Text = "He&lp";
            this.helpToolStripButton.Click += new System.EventHandler(this.HelpToolStripButton_Click);
            // 
            // toolStripProgressBar
            // 
            this.toolStripProgressBar.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripProgressBar.Name = "toolStripProgressBar";
            this.toolStripProgressBar.Padding = new System.Windows.Forms.Padding(0, 0, 15, 0);
            this.toolStripProgressBar.Size = new System.Drawing.Size(150, 24);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // toolStripLabelInfo
            // 
            this.toolStripLabelInfo.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripLabelInfo.Name = "toolStripLabelInfo";
            this.toolStripLabelInfo.Size = new System.Drawing.Size(0, 24);
            // 
            // dgvFiles
            // 
            this.dgvFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvFiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFiles.Location = new System.Drawing.Point(9, 106);
            this.dgvFiles.Margin = new System.Windows.Forms.Padding(2);
            this.dgvFiles.Name = "dgvFiles";
            this.dgvFiles.RowHeadersWidth = 51;
            this.dgvFiles.RowTemplate.Height = 24;
            this.dgvFiles.Size = new System.Drawing.Size(785, 408);
            this.dgvFiles.TabIndex = 3;
            // 
            // btnGenerateFolder
            // 
            this.btnGenerateFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGenerateFolder.Location = new System.Drawing.Point(638, 67);
            this.btnGenerateFolder.Margin = new System.Windows.Forms.Padding(2);
            this.btnGenerateFolder.Name = "btnGenerateFolder";
            this.btnGenerateFolder.Size = new System.Drawing.Size(156, 31);
            this.btnGenerateFolder.TabIndex = 4;
            this.btnGenerateFolder.Text = "Generate Folders";
            this.btnGenerateFolder.UseVisualStyleBackColor = true;
            this.btnGenerateFolder.Click += new System.EventHandler(this.BtnGenerateFolder_Click);
            // 
            // txtLocationToGenerate
            // 
            this.txtLocationToGenerate.Location = new System.Drawing.Point(86, 57);
            this.txtLocationToGenerate.Margin = new System.Windows.Forms.Padding(2);
            this.txtLocationToGenerate.Name = "txtLocationToGenerate";
            this.txtLocationToGenerate.ReadOnly = true;
            this.txtLocationToGenerate.Size = new System.Drawing.Size(324, 20);
            this.txtLocationToGenerate.TabIndex = 5;
            // 
            // lblLocationToGenerate
            // 
            this.lblLocationToGenerate.AutoSize = true;
            this.lblLocationToGenerate.Location = new System.Drawing.Point(7, 59);
            this.lblLocationToGenerate.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblLocationToGenerate.Name = "lblLocationToGenerate";
            this.lblLocationToGenerate.Size = new System.Drawing.Size(68, 13);
            this.lblLocationToGenerate.TabIndex = 6;
            this.lblLocationToGenerate.Text = "Output folder";
            // 
            // txtOriginLocation
            // 
            this.txtOriginLocation.Location = new System.Drawing.Point(86, 32);
            this.txtOriginLocation.Margin = new System.Windows.Forms.Padding(2);
            this.txtOriginLocation.Name = "txtOriginLocation";
            this.txtOriginLocation.ReadOnly = true;
            this.txtOriginLocation.Size = new System.Drawing.Size(324, 20);
            this.txtOriginLocation.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 36);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Origin location";
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(414, 56);
            this.btnSelect.Margin = new System.Windows.Forms.Padding(2);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(111, 20);
            this.btnSelect.TabIndex = 7;
            this.btnSelect.Text = "Select";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.BtnSelect_Click);
            // 
            // chkCheckExtensions
            // 
            this.chkCheckExtensions.AutoSize = true;
            this.chkCheckExtensions.Location = new System.Drawing.Point(415, 34);
            this.chkCheckExtensions.Name = "chkCheckExtensions";
            this.chkCheckExtensions.Size = new System.Drawing.Size(111, 17);
            this.chkCheckExtensions.TabIndex = 10;
            this.chkCheckExtensions.Text = "Check Extensions";
            this.chkCheckExtensions.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(638, 32);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(156, 31);
            this.button1.TabIndex = 12;
            this.button1.Text = "Delete repeated files";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.DeleteRepeatedFiles_Click);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Info;
            this.ClientSize = new System.Drawing.Size(804, 523);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.chkCheckExtensions);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtOriginLocation);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.lblLocationToGenerate);
            this.Controls.Add(this.txtLocationToGenerate);
            this.Controls.Add(this.btnGenerateFolder);
            this.Controls.Add(this.dgvFiles);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MinimumSize = new System.Drawing.Size(820, 562);
            this.Name = "FrmMain";
            this.Text = "Create Folder From Metadatas";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFiles)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripButton helpToolStripButton;
        private System.Windows.Forms.DataGridView dgvFiles;
        private System.Windows.Forms.Button btnGenerateFolder;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel toolStripLabelInfo;
        private System.Windows.Forms.TextBox txtLocationToGenerate;
        private System.Windows.Forms.Label lblLocationToGenerate;
        private System.Windows.Forms.TextBox txtOriginLocation;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolStripButton openToolStripButton;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.CheckBox chkCheckExtensions;
        private System.Windows.Forms.Button button1;
    }
}

