
namespace Manager
{
    partial class MainWindow
    {
        private System.ComponentModel.IContainer components = null;

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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.CalendarView = new System.Windows.Forms.DataGridView();
            this.StartColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EndColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DescriptionColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.calendarPage = new System.Windows.Forms.TabPage();
            this.saveButton = new System.Windows.Forms.Button();
            this.openButton = new System.Windows.Forms.Button();
            this.SyncCalendarButton = new System.Windows.Forms.Button();
            this.exportButton = new System.Windows.Forms.Button();
            this.Remove = new System.Windows.Forms.Button();
            this.Add = new System.Windows.Forms.Button();
            this.Year = new System.Windows.Forms.TextBox();
            this.AddYear = new System.Windows.Forms.Button();
            this.SubtractYear = new System.Windows.Forms.Button();
            this.helpPage = new System.Windows.Forms.TabPage();
            this.EmailLink = new System.Windows.Forms.LinkLabel();
            this.EmailLabel = new System.Windows.Forms.Label();
            this.AuthorLabel = new System.Windows.Forms.Label();
            this.LicenseLabel = new System.Windows.Forms.Label();
            this.SourceLink = new System.Windows.Forms.LinkLabel();
            this.SourceLabel = new System.Windows.Forms.Label();
            this.VersionLabel = new System.Windows.Forms.Label();
            this.iconBox = new System.Windows.Forms.PictureBox();
            this.TrayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.TrayContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.TrayOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.TrayClose = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.CalendarView)).BeginInit();
            this.tabControl.SuspendLayout();
            this.calendarPage.SuspendLayout();
            this.helpPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconBox)).BeginInit();
            this.TrayContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // CalendarView
            // 
            this.CalendarView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CalendarView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.CalendarView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.StartColumn,
            this.EndColumn,
            this.NameColumn,
            this.DescriptionColumn});
            this.CalendarView.Location = new System.Drawing.Point(10, 43);
            this.CalendarView.Name = "CalendarView";
            this.CalendarView.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.CalendarView.RowHeadersVisible = false;
            this.CalendarView.RowTemplate.Height = 25;
            this.CalendarView.Size = new System.Drawing.Size(774, 324);
            this.CalendarView.TabIndex = 3;
            // 
            // StartColumn
            // 
            this.StartColumn.DataPropertyName = "StartTime";
            this.StartColumn.FillWeight = 39.11343F;
            this.StartColumn.HeaderText = "Start";
            this.StartColumn.Name = "StartColumn";
            // 
            // EndColumn
            // 
            this.EndColumn.DataPropertyName = "EndTime";
            this.EndColumn.FillWeight = 44.62484F;
            this.EndColumn.HeaderText = "End";
            this.EndColumn.Name = "EndColumn";
            // 
            // NameColumn
            // 
            this.NameColumn.DataPropertyName = "Name";
            this.NameColumn.FillWeight = 132.2595F;
            this.NameColumn.HeaderText = "Name";
            this.NameColumn.Name = "NameColumn";
            this.NameColumn.Width = 253;
            // 
            // DescriptionColumn
            // 
            this.DescriptionColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.DescriptionColumn.DataPropertyName = "Description";
            this.DescriptionColumn.FillWeight = 184.0022F;
            this.DescriptionColumn.HeaderText = "Description";
            this.DescriptionColumn.Name = "DescriptionColumn";
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.calendarPage);
            this.tabControl.Controls.Add(this.helpPage);
            this.tabControl.Location = new System.Drawing.Point(2, 3);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(802, 437);
            this.tabControl.TabIndex = 0;
            // 
            // calendarPage
            // 
            this.calendarPage.Controls.Add(this.saveButton);
            this.calendarPage.Controls.Add(this.openButton);
            this.calendarPage.Controls.Add(this.SyncCalendarButton);
            this.calendarPage.Controls.Add(this.exportButton);
            this.calendarPage.Controls.Add(this.Remove);
            this.calendarPage.Controls.Add(this.Add);
            this.calendarPage.Controls.Add(this.CalendarView);
            this.calendarPage.Controls.Add(this.Year);
            this.calendarPage.Controls.Add(this.AddYear);
            this.calendarPage.Controls.Add(this.SubtractYear);
            this.calendarPage.Location = new System.Drawing.Point(4, 24);
            this.calendarPage.Name = "calendarPage";
            this.calendarPage.Padding = new System.Windows.Forms.Padding(3);
            this.calendarPage.Size = new System.Drawing.Size(794, 409);
            this.calendarPage.TabIndex = 0;
            this.calendarPage.Text = "Calendar";
            this.calendarPage.UseVisualStyleBackColor = true;
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButton.Enabled = false;
            this.saveButton.Location = new System.Drawing.Point(674, 10);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(50, 23);
            this.saveButton.TabIndex = 9;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            // 
            // openButton
            // 
            this.openButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.openButton.Enabled = false;
            this.openButton.Location = new System.Drawing.Point(734, 10);
            this.openButton.Name = "openButton";
            this.openButton.Size = new System.Drawing.Size(50, 23);
            this.openButton.TabIndex = 8;
            this.openButton.Text = "Open";
            this.openButton.UseVisualStyleBackColor = true;
            // 
            // SyncCalendarButton
            // 
            this.SyncCalendarButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SyncCalendarButton.Location = new System.Drawing.Point(549, 378);
            this.SyncCalendarButton.Name = "SyncCalendarButton";
            this.SyncCalendarButton.Size = new System.Drawing.Size(125, 23);
            this.SyncCalendarButton.TabIndex = 7;
            this.SyncCalendarButton.Text = "Sync Calendar";
            this.SyncCalendarButton.UseVisualStyleBackColor = true;
            this.SyncCalendarButton.Click += new System.EventHandler(this.SyncCalendarButton_Click);
            // 
            // exportButton
            // 
            this.exportButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.exportButton.Enabled = false;
            this.exportButton.Location = new System.Drawing.Point(684, 378);
            this.exportButton.Name = "exportButton";
            this.exportButton.Size = new System.Drawing.Size(100, 23);
            this.exportButton.TabIndex = 6;
            this.exportButton.Text = "Export PDF";
            this.exportButton.UseVisualStyleBackColor = true;
            // 
            // Remove
            // 
            this.Remove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Remove.Location = new System.Drawing.Point(95, 378);
            this.Remove.Name = "Remove";
            this.Remove.Size = new System.Drawing.Size(75, 23);
            this.Remove.TabIndex = 5;
            this.Remove.Text = "Remove";
            this.Remove.UseVisualStyleBackColor = true;
            this.Remove.Click += new System.EventHandler(this.Remove_Click);
            // 
            // Add
            // 
            this.Add.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Add.Location = new System.Drawing.Point(10, 378);
            this.Add.Name = "Add";
            this.Add.Size = new System.Drawing.Size(75, 23);
            this.Add.TabIndex = 4;
            this.Add.Text = "Add";
            this.Add.UseVisualStyleBackColor = true;
            this.Add.Click += new System.EventHandler(this.Add_Click);
            // 
            // Year
            // 
            this.Year.Location = new System.Drawing.Point(38, 10);
            this.Year.Name = "Year";
            this.Year.Size = new System.Drawing.Size(100, 23);
            this.Year.TabIndex = 2;
            this.Year.Text = "2020";
            this.Year.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // AddYear
            // 
            this.AddYear.Location = new System.Drawing.Point(143, 10);
            this.AddYear.Name = "AddYear";
            this.AddYear.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.AddYear.Size = new System.Drawing.Size(23, 23);
            this.AddYear.TabIndex = 1;
            this.AddYear.Text = "+";
            this.AddYear.UseVisualStyleBackColor = true;
            this.AddYear.Click += new System.EventHandler(this.AddYear_Click);
            // 
            // SubtractYear
            // 
            this.SubtractYear.Location = new System.Drawing.Point(10, 10);
            this.SubtractYear.Name = "SubtractYear";
            this.SubtractYear.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.SubtractYear.Size = new System.Drawing.Size(23, 23);
            this.SubtractYear.TabIndex = 0;
            this.SubtractYear.Text = "-";
            this.SubtractYear.UseVisualStyleBackColor = true;
            this.SubtractYear.Click += new System.EventHandler(this.SubtractYear_Click);
            // 
            // helpPage
            // 
            this.helpPage.Controls.Add(this.EmailLink);
            this.helpPage.Controls.Add(this.EmailLabel);
            this.helpPage.Controls.Add(this.AuthorLabel);
            this.helpPage.Controls.Add(this.LicenseLabel);
            this.helpPage.Controls.Add(this.SourceLink);
            this.helpPage.Controls.Add(this.SourceLabel);
            this.helpPage.Controls.Add(this.VersionLabel);
            this.helpPage.Controls.Add(this.iconBox);
            this.helpPage.Location = new System.Drawing.Point(4, 24);
            this.helpPage.Name = "helpPage";
            this.helpPage.Padding = new System.Windows.Forms.Padding(3);
            this.helpPage.Size = new System.Drawing.Size(794, 409);
            this.helpPage.TabIndex = 1;
            this.helpPage.Text = "Help";
            this.helpPage.UseVisualStyleBackColor = true;
            // 
            // EmailLink
            // 
            this.EmailLink.AutoSize = true;
            this.EmailLink.Location = new System.Drawing.Point(46, 160);
            this.EmailLink.Name = "EmailLink";
            this.EmailLink.Size = new System.Drawing.Size(185, 15);
            this.EmailLink.TabIndex = 7;
            this.EmailLink.TabStop = true;
            this.EmailLink.Text = "martin.jensen.1997@hotmail.com";
            this.EmailLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.EmailLink_LinkClicked);
            // 
            // EmailLabel
            // 
            this.EmailLabel.AutoSize = true;
            this.EmailLabel.Location = new System.Drawing.Point(10, 160);
            this.EmailLabel.Name = "EmailLabel";
            this.EmailLabel.Size = new System.Drawing.Size(39, 15);
            this.EmailLabel.TabIndex = 6;
            this.EmailLabel.Text = "Email:";
            // 
            // AuthorLabel
            // 
            this.AuthorLabel.AutoSize = true;
            this.AuthorLabel.Location = new System.Drawing.Point(10, 145);
            this.AuthorLabel.Name = "AuthorLabel";
            this.AuthorLabel.Size = new System.Drawing.Size(146, 15);
            this.AuthorLabel.TabIndex = 5;
            this.AuthorLabel.Text = "Author: Martin J. R. Jensen";
            // 
            // LicenseLabel
            // 
            this.LicenseLabel.AutoSize = true;
            this.LicenseLabel.Location = new System.Drawing.Point(10, 191);
            this.LicenseLabel.Name = "LicenseLabel";
            this.LicenseLabel.Size = new System.Drawing.Size(112, 15);
            this.LicenseLabel.TabIndex = 4;
            this.LicenseLabel.Text = "License: Apache-2.0";
            // 
            // SourceLink
            // 
            this.SourceLink.AutoSize = true;
            this.SourceLink.Location = new System.Drawing.Point(53, 206);
            this.SourceLink.Name = "SourceLink";
            this.SourceLink.Size = new System.Drawing.Size(199, 15);
            this.SourceLink.TabIndex = 3;
            this.SourceLink.TabStop = true;
            this.SourceLink.Text = "https://github.com/mjrj97/Manager";
            this.SourceLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.SourceLink_LinkClicked);
            // 
            // SourceLabel
            // 
            this.SourceLabel.AutoSize = true;
            this.SourceLabel.Location = new System.Drawing.Point(10, 206);
            this.SourceLabel.Name = "SourceLabel";
            this.SourceLabel.Size = new System.Drawing.Size(46, 15);
            this.SourceLabel.TabIndex = 2;
            this.SourceLabel.Text = "Source:";
            // 
            // VersionLabel
            // 
            this.VersionLabel.AutoSize = true;
            this.VersionLabel.Location = new System.Drawing.Point(10, 117);
            this.VersionLabel.Name = "VersionLabel";
            this.VersionLabel.Size = new System.Drawing.Size(93, 15);
            this.VersionLabel.TabIndex = 1;
            this.VersionLabel.Text = "Manager v. 0.0.0";
            // 
            // iconBox
            // 
            this.iconBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.iconBox.Image = ((System.Drawing.Image)(resources.GetObject("iconBox.Image")));
            this.iconBox.Location = new System.Drawing.Point(10, 10);
            this.iconBox.Name = "iconBox";
            this.iconBox.Size = new System.Drawing.Size(100, 100);
            this.iconBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.iconBox.TabIndex = 0;
            this.iconBox.TabStop = false;
            // 
            // TrayIcon
            // 
            this.TrayIcon.ContextMenuStrip = this.TrayContextMenu;
            this.TrayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("TrayIcon.Icon")));
            this.TrayIcon.Text = "Manager";
            this.TrayIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Open);
            // 
            // TrayContextMenu
            // 
            this.TrayContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TrayOpen,
            this.TrayClose});
            this.TrayContextMenu.Name = "TrayContextMenu";
            this.TrayContextMenu.Size = new System.Drawing.Size(104, 48);
            // 
            // TrayOpen
            // 
            this.TrayOpen.Name = "TrayOpen";
            this.TrayOpen.Size = new System.Drawing.Size(103, 22);
            this.TrayOpen.Text = "Open";
            this.TrayOpen.Click += new System.EventHandler(this.Open);
            // 
            // TrayClose
            // 
            this.TrayClose.Name = "TrayClose";
            this.TrayClose.Size = new System.Drawing.Size(103, 22);
            this.TrayClose.Text = "Close";
            this.TrayClose.Click += new System.EventHandler(this.Exit);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 441);
            this.Controls.Add(this.tabControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(500, 250);
            this.Name = "MainWindow";
            this.Text = "Manager";
            this.Resize += new System.EventHandler(this.Manager_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.CalendarView)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.calendarPage.ResumeLayout(false);
            this.calendarPage.PerformLayout();
            this.helpPage.ResumeLayout(false);
            this.helpPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconBox)).EndInit();
            this.TrayContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage calendarPage;
        private System.Windows.Forms.TabPage helpPage;
        private System.Windows.Forms.Button AddYear;
        private System.Windows.Forms.Button SubtractYear;
        private System.Windows.Forms.TextBox Year;
        private System.Windows.Forms.Button Remove;
        private System.Windows.Forms.Button Add;
        private System.Windows.Forms.Button SyncCalendarButton;
        private System.Windows.Forms.Button exportButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button openButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn StartColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn EndColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn NameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn DescriptionColumn;
        private System.Windows.Forms.NotifyIcon TrayIcon;
        private System.Windows.Forms.ContextMenuStrip TrayContextMenu;
        private System.Windows.Forms.ToolStripMenuItem TrayOpen;
        private System.Windows.Forms.ToolStripMenuItem TrayClose;
        private System.Windows.Forms.LinkLabel SourceLink;
        private System.Windows.Forms.Label SourceLabel;
        private System.Windows.Forms.Label VersionLabel;
        private System.Windows.Forms.PictureBox iconBox;
        private System.Windows.Forms.LinkLabel EmailLink;
        private System.Windows.Forms.Label EmailLabel;
        private System.Windows.Forms.Label AuthorLabel;
        private System.Windows.Forms.Label LicenseLabel;
        private System.Windows.Forms.DataGridView CalendarView;
    }
}

