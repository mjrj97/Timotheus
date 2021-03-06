namespace Timotheus.Forms
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.CalendarView = new System.Windows.Forms.DataGridView();
            this.StartColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EndColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DescriptionColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LocationColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.calendarPage = new System.Windows.Forms.TabPage();
            this.MonthButton = new System.Windows.Forms.RadioButton();
            this.HalfYearButton = new System.Windows.Forms.RadioButton();
            this.YearButton = new System.Windows.Forms.RadioButton();
            this.AllButton = new System.Windows.Forms.RadioButton();
            this.SaveButton = new System.Windows.Forms.Button();
            this.OpenButton = new System.Windows.Forms.Button();
            this.SyncCalendarButton = new System.Windows.Forms.Button();
            this.ExportButton = new System.Windows.Forms.Button();
            this.RemoveButton = new System.Windows.Forms.Button();
            this.AddButton = new System.Windows.Forms.Button();
            this.PeriodBox = new System.Windows.Forms.TextBox();
            this.AddYearButton = new System.Windows.Forms.Button();
            this.SubtractYearButton = new System.Windows.Forms.Button();
            this.sftpPage = new System.Windows.Forms.TabPage();
            this.SynchronizeButton = new System.Windows.Forms.Button();
            this.DownloadAllButton = new System.Windows.Forms.Button();
            this.ShowDirectoryButton = new System.Windows.Forms.Button();
            this.FileView = new System.Windows.Forms.DataGridView();
            this.FileNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FileSizeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BrowseButton = new System.Windows.Forms.Button();
            this.RemoteDirectoryBox = new System.Windows.Forms.TextBox();
            this.RemoteDirectoryLabel = new System.Windows.Forms.Label();
            this.LocalDirectoryLabel = new System.Windows.Forms.Label();
            this.LocalDirectoryBox = new System.Windows.Forms.TextBox();
            this.PasswordLabel = new System.Windows.Forms.Label();
            this.PasswordBox = new System.Windows.Forms.TextBox();
            this.UsernameLabel = new System.Windows.Forms.Label();
            this.UsernameBox = new System.Windows.Forms.TextBox();
            this.HostLabel = new System.Windows.Forms.Label();
            this.HostBox = new System.Windows.Forms.TextBox();
            this.consentFormPage = new System.Windows.Forms.TabPage();
            this.RemoveConsentFormButton = new System.Windows.Forms.Button();
            this.AddConsentFormButton = new System.Windows.Forms.Button();
            this.ConsentFormView = new System.Windows.Forms.DataGridView();
            this.ConsentForm_NameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ConsentForm_DateColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ConsentForm_VersionColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ConsentForm_CommentColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.settingsPage = new System.Windows.Forms.TabPage();
            this.infoBox = new System.Windows.Forms.GroupBox();
            this.LogoBox = new System.Windows.Forms.TextBox();
            this.LogoLabel = new System.Windows.Forms.Label();
            this.BrowseLogoButton = new System.Windows.Forms.Button();
            this.AddressLabel = new System.Windows.Forms.Label();
            this.AddressBox = new System.Windows.Forms.TextBox();
            this.NameLabel = new System.Windows.Forms.Label();
            this.NameBox = new System.Windows.Forms.TextBox();
            this.LogoPictureBox = new System.Windows.Forms.PictureBox();
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
            this.sftpPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FileView)).BeginInit();
            this.consentFormPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ConsentFormView)).BeginInit();
            this.settingsPage.SuspendLayout();
            this.infoBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LogoPictureBox)).BeginInit();
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
            this.DescriptionColumn,
            this.LocationColumn});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.CalendarView.DefaultCellStyle = dataGridViewCellStyle1;
            this.CalendarView.Location = new System.Drawing.Point(11, 52);
            this.CalendarView.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.CalendarView.Name = "CalendarView";
            this.CalendarView.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.CalendarView.RowHeadersVisible = false;
            this.CalendarView.RowHeadersWidth = 51;
            this.CalendarView.RowTemplate.Height = 25;
            this.CalendarView.Size = new System.Drawing.Size(885, 441);
            this.CalendarView.TabIndex = 3;
            // 
            // StartColumn
            // 
            this.StartColumn.DataPropertyName = "StartTime";
            this.StartColumn.FillWeight = 39.11343F;
            this.StartColumn.HeaderText = "Start";
            this.StartColumn.MinimumWidth = 6;
            this.StartColumn.Name = "StartColumn";
            this.StartColumn.Width = 125;
            // 
            // EndColumn
            // 
            this.EndColumn.DataPropertyName = "EndTime";
            this.EndColumn.FillWeight = 44.62484F;
            this.EndColumn.HeaderText = "End";
            this.EndColumn.MinimumWidth = 6;
            this.EndColumn.Name = "EndColumn";
            this.EndColumn.Width = 125;
            // 
            // NameColumn
            // 
            this.NameColumn.DataPropertyName = "Name";
            this.NameColumn.FillWeight = 132.2595F;
            this.NameColumn.HeaderText = "Name";
            this.NameColumn.MinimumWidth = 6;
            this.NameColumn.Name = "NameColumn";
            this.NameColumn.Width = 253;
            // 
            // DescriptionColumn
            // 
            this.DescriptionColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.DescriptionColumn.DataPropertyName = "Description";
            this.DescriptionColumn.FillWeight = 184.0022F;
            this.DescriptionColumn.HeaderText = "Description";
            this.DescriptionColumn.MinimumWidth = 6;
            this.DescriptionColumn.Name = "DescriptionColumn";
            // 
            // LocationColumn
            // 
            this.LocationColumn.DataPropertyName = "Location";
            this.LocationColumn.HeaderText = "Location";
            this.LocationColumn.MinimumWidth = 6;
            this.LocationColumn.Name = "LocationColumn";
            this.LocationColumn.Width = 125;
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.calendarPage);
            this.tabControl.Controls.Add(this.sftpPage);
            this.tabControl.Controls.Add(this.consentFormPage);
            this.tabControl.Controls.Add(this.settingsPage);
            this.tabControl.Controls.Add(this.helpPage);
            this.tabControl.Location = new System.Drawing.Point(2, 4);
            this.tabControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(917, 583);
            this.tabControl.TabIndex = 0;
            // 
            // calendarPage
            // 
            this.calendarPage.Controls.Add(this.MonthButton);
            this.calendarPage.Controls.Add(this.HalfYearButton);
            this.calendarPage.Controls.Add(this.YearButton);
            this.calendarPage.Controls.Add(this.AllButton);
            this.calendarPage.Controls.Add(this.SaveButton);
            this.calendarPage.Controls.Add(this.OpenButton);
            this.calendarPage.Controls.Add(this.SyncCalendarButton);
            this.calendarPage.Controls.Add(this.ExportButton);
            this.calendarPage.Controls.Add(this.RemoveButton);
            this.calendarPage.Controls.Add(this.AddButton);
            this.calendarPage.Controls.Add(this.CalendarView);
            this.calendarPage.Controls.Add(this.PeriodBox);
            this.calendarPage.Controls.Add(this.AddYearButton);
            this.calendarPage.Controls.Add(this.SubtractYearButton);
            this.calendarPage.Location = new System.Drawing.Point(4, 29);
            this.calendarPage.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.calendarPage.Name = "calendarPage";
            this.calendarPage.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.calendarPage.Size = new System.Drawing.Size(909, 550);
            this.calendarPage.TabIndex = 0;
            this.calendarPage.Text = "Calendar";
            this.calendarPage.UseVisualStyleBackColor = true;
            // 
            // MonthButton
            // 
            this.MonthButton.AutoSize = true;
            this.MonthButton.Location = new System.Drawing.Point(465, 15);
            this.MonthButton.Name = "MonthButton";
            this.MonthButton.Size = new System.Drawing.Size(73, 24);
            this.MonthButton.TabIndex = 13;
            this.MonthButton.TabStop = true;
            this.MonthButton.Text = "Month";
            this.MonthButton.UseVisualStyleBackColor = true;
            this.MonthButton.CheckedChanged += new System.EventHandler(this.PeriodChanged);
            // 
            // HalfYearButton
            // 
            this.HalfYearButton.AutoSize = true;
            this.HalfYearButton.Location = new System.Drawing.Point(360, 15);
            this.HalfYearButton.Name = "HalfYearButton";
            this.HalfYearButton.Size = new System.Drawing.Size(92, 24);
            this.HalfYearButton.TabIndex = 12;
            this.HalfYearButton.TabStop = true;
            this.HalfYearButton.Text = "Half-year";
            this.HalfYearButton.UseVisualStyleBackColor = true;
            this.HalfYearButton.CheckedChanged += new System.EventHandler(this.PeriodChanged);
            // 
            // YearButton
            // 
            this.YearButton.AutoSize = true;
            this.YearButton.Checked = true;
            this.YearButton.Location = new System.Drawing.Point(285, 15);
            this.YearButton.Name = "YearButton";
            this.YearButton.Size = new System.Drawing.Size(58, 24);
            this.YearButton.TabIndex = 11;
            this.YearButton.TabStop = true;
            this.YearButton.Text = "Year";
            this.YearButton.UseVisualStyleBackColor = true;
            this.YearButton.CheckedChanged += new System.EventHandler(this.PeriodChanged);
            // 
            // AllButton
            // 
            this.AllButton.AutoSize = true;
            this.AllButton.Location = new System.Drawing.Point(220, 15);
            this.AllButton.Name = "AllButton";
            this.AllButton.Size = new System.Drawing.Size(48, 24);
            this.AllButton.TabIndex = 10;
            this.AllButton.Text = "All";
            this.AllButton.UseVisualStyleBackColor = true;
            this.AllButton.CheckedChanged += new System.EventHandler(this.PeriodChanged);
            // 
            // SaveButton
            // 
            this.SaveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveButton.Location = new System.Drawing.Point(770, 13);
            this.SaveButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(57, 27);
            this.SaveButton.TabIndex = 9;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveCalendar);
            // 
            // OpenButton
            // 
            this.OpenButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.OpenButton.Location = new System.Drawing.Point(839, 13);
            this.OpenButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.OpenButton.Name = "OpenButton";
            this.OpenButton.Size = new System.Drawing.Size(57, 27);
            this.OpenButton.TabIndex = 8;
            this.OpenButton.Text = "Open";
            this.OpenButton.UseVisualStyleBackColor = true;
            this.OpenButton.Click += new System.EventHandler(this.OpenCalendar);
            // 
            // SyncCalendarButton
            // 
            this.SyncCalendarButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SyncCalendarButton.Location = new System.Drawing.Point(627, 504);
            this.SyncCalendarButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SyncCalendarButton.Name = "SyncCalendarButton";
            this.SyncCalendarButton.Size = new System.Drawing.Size(143, 31);
            this.SyncCalendarButton.TabIndex = 7;
            this.SyncCalendarButton.Text = "Sync Calendar";
            this.SyncCalendarButton.UseVisualStyleBackColor = true;
            this.SyncCalendarButton.Click += new System.EventHandler(this.SyncCalendar);
            // 
            // ExportButton
            // 
            this.ExportButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ExportButton.Location = new System.Drawing.Point(782, 504);
            this.ExportButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ExportButton.Name = "ExportButton";
            this.ExportButton.Size = new System.Drawing.Size(114, 31);
            this.ExportButton.TabIndex = 6;
            this.ExportButton.Text = "Export PDF";
            this.ExportButton.UseVisualStyleBackColor = true;
            this.ExportButton.Click += new System.EventHandler(this.ExportPDF);
            // 
            // RemoveButton
            // 
            this.RemoveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.RemoveButton.Location = new System.Drawing.Point(109, 504);
            this.RemoveButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.RemoveButton.Name = "RemoveButton";
            this.RemoveButton.Size = new System.Drawing.Size(86, 31);
            this.RemoveButton.TabIndex = 5;
            this.RemoveButton.Text = "Remove";
            this.RemoveButton.UseVisualStyleBackColor = true;
            this.RemoveButton.Click += new System.EventHandler(this.RemoveEvent);
            // 
            // AddButton
            // 
            this.AddButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AddButton.Location = new System.Drawing.Point(11, 504);
            this.AddButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(86, 31);
            this.AddButton.TabIndex = 4;
            this.AddButton.Text = "Add";
            this.AddButton.UseVisualStyleBackColor = true;
            this.AddButton.Click += new System.EventHandler(this.AddEvent);
            // 
            // PeriodBox
            // 
            this.PeriodBox.Location = new System.Drawing.Point(43, 13);
            this.PeriodBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.PeriodBox.Name = "PeriodBox";
            this.PeriodBox.Size = new System.Drawing.Size(114, 27);
            this.PeriodBox.TabIndex = 2;
            this.PeriodBox.Text = "2020";
            this.PeriodBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // AddYearButton
            // 
            this.AddYearButton.Location = new System.Drawing.Point(163, 13);
            this.AddYearButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.AddYearButton.Name = "AddYearButton";
            this.AddYearButton.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.AddYearButton.Size = new System.Drawing.Size(26, 27);
            this.AddYearButton.TabIndex = 1;
            this.AddYearButton.Text = "+";
            this.AddYearButton.UseVisualStyleBackColor = true;
            this.AddYearButton.Click += new System.EventHandler(this.UpdatePeriod);
            // 
            // SubtractYearButton
            // 
            this.SubtractYearButton.Location = new System.Drawing.Point(11, 13);
            this.SubtractYearButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SubtractYearButton.Name = "SubtractYearButton";
            this.SubtractYearButton.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.SubtractYearButton.Size = new System.Drawing.Size(26, 27);
            this.SubtractYearButton.TabIndex = 0;
            this.SubtractYearButton.Text = "-";
            this.SubtractYearButton.UseVisualStyleBackColor = true;
            this.SubtractYearButton.Click += new System.EventHandler(this.UpdatePeriod);
            // 
            // sftpPage
            // 
            this.sftpPage.Controls.Add(this.SynchronizeButton);
            this.sftpPage.Controls.Add(this.DownloadAllButton);
            this.sftpPage.Controls.Add(this.ShowDirectoryButton);
            this.sftpPage.Controls.Add(this.FileView);
            this.sftpPage.Controls.Add(this.BrowseButton);
            this.sftpPage.Controls.Add(this.RemoteDirectoryBox);
            this.sftpPage.Controls.Add(this.RemoteDirectoryLabel);
            this.sftpPage.Controls.Add(this.LocalDirectoryLabel);
            this.sftpPage.Controls.Add(this.LocalDirectoryBox);
            this.sftpPage.Controls.Add(this.PasswordLabel);
            this.sftpPage.Controls.Add(this.PasswordBox);
            this.sftpPage.Controls.Add(this.UsernameLabel);
            this.sftpPage.Controls.Add(this.UsernameBox);
            this.sftpPage.Controls.Add(this.HostLabel);
            this.sftpPage.Controls.Add(this.HostBox);
            this.sftpPage.Location = new System.Drawing.Point(4, 29);
            this.sftpPage.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.sftpPage.Name = "sftpPage";
            this.sftpPage.Size = new System.Drawing.Size(909, 550);
            this.sftpPage.TabIndex = 2;
            this.sftpPage.Text = "SFTP";
            this.sftpPage.UseVisualStyleBackColor = true;
            // 
            // SynchronizeButton
            // 
            this.SynchronizeButton.Location = new System.Drawing.Point(11, 264);
            this.SynchronizeButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SynchronizeButton.Name = "SynchronizeButton";
            this.SynchronizeButton.Size = new System.Drawing.Size(251, 51);
            this.SynchronizeButton.TabIndex = 14;
            this.SynchronizeButton.Text = "Synchronize";
            this.SynchronizeButton.UseVisualStyleBackColor = true;
            this.SynchronizeButton.Click += new System.EventHandler(this.SyncDirectories);
            // 
            // DownloadAllButton
            // 
            this.DownloadAllButton.Location = new System.Drawing.Point(11, 205);
            this.DownloadAllButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.DownloadAllButton.Name = "DownloadAllButton";
            this.DownloadAllButton.Size = new System.Drawing.Size(251, 51);
            this.DownloadAllButton.TabIndex = 13;
            this.DownloadAllButton.Text = "Download all files";
            this.DownloadAllButton.UseVisualStyleBackColor = true;
            this.DownloadAllButton.Click += new System.EventHandler(this.DownloadAll);
            // 
            // ShowDirectoryButton
            // 
            this.ShowDirectoryButton.Location = new System.Drawing.Point(11, 147);
            this.ShowDirectoryButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ShowDirectoryButton.Name = "ShowDirectoryButton";
            this.ShowDirectoryButton.Size = new System.Drawing.Size(251, 51);
            this.ShowDirectoryButton.TabIndex = 12;
            this.ShowDirectoryButton.Text = "Show directory";
            this.ShowDirectoryButton.UseVisualStyleBackColor = true;
            this.ShowDirectoryButton.Click += new System.EventHandler(this.ShowDirectory);
            // 
            // FileView
            // 
            this.FileView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FileView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.FileView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FileNameColumn,
            this.FileSizeColumn});
            this.FileView.Location = new System.Drawing.Point(286, 93);
            this.FileView.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.FileView.Name = "FileView";
            this.FileView.RowHeadersVisible = false;
            this.FileView.RowHeadersWidth = 51;
            this.FileView.RowTemplate.Height = 25;
            this.FileView.Size = new System.Drawing.Size(613, 443);
            this.FileView.TabIndex = 11;
            // 
            // FileNameColumn
            // 
            this.FileNameColumn.DataPropertyName = "Name";
            this.FileNameColumn.HeaderText = "File name";
            this.FileNameColumn.MinimumWidth = 6;
            this.FileNameColumn.Name = "FileNameColumn";
            this.FileNameColumn.Width = 400;
            // 
            // FileSizeColumn
            // 
            this.FileSizeColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.FileSizeColumn.DataPropertyName = "Length";
            this.FileSizeColumn.HeaderText = "File size";
            this.FileSizeColumn.MinimumWidth = 6;
            this.FileSizeColumn.Name = "FileSizeColumn";
            // 
            // BrowseButton
            // 
            this.BrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BrowseButton.Location = new System.Drawing.Point(813, 13);
            this.BrowseButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BrowseButton.Name = "BrowseButton";
            this.BrowseButton.Size = new System.Drawing.Size(86, 27);
            this.BrowseButton.TabIndex = 10;
            this.BrowseButton.Text = "Browse";
            this.BrowseButton.UseVisualStyleBackColor = true;
            this.BrowseButton.Click += new System.EventHandler(this.BrowseLocalDirectory);
            // 
            // RemoteDirectoryBox
            // 
            this.RemoteDirectoryBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RemoteDirectoryBox.Location = new System.Drawing.Point(415, 53);
            this.RemoteDirectoryBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.RemoteDirectoryBox.Name = "RemoteDirectoryBox";
            this.RemoteDirectoryBox.Size = new System.Drawing.Size(483, 27);
            this.RemoteDirectoryBox.TabIndex = 9;
            // 
            // RemoteDirectoryLabel
            // 
            this.RemoteDirectoryLabel.AutoSize = true;
            this.RemoteDirectoryLabel.Location = new System.Drawing.Point(286, 57);
            this.RemoteDirectoryLabel.Name = "RemoteDirectoryLabel";
            this.RemoteDirectoryLabel.Size = new System.Drawing.Size(124, 20);
            this.RemoteDirectoryLabel.TabIndex = 8;
            this.RemoteDirectoryLabel.Text = "Remote directory";
            // 
            // LocalDirectoryLabel
            // 
            this.LocalDirectoryLabel.AutoSize = true;
            this.LocalDirectoryLabel.Location = new System.Drawing.Point(286, 17);
            this.LocalDirectoryLabel.Name = "LocalDirectoryLabel";
            this.LocalDirectoryLabel.Size = new System.Drawing.Size(107, 20);
            this.LocalDirectoryLabel.TabIndex = 7;
            this.LocalDirectoryLabel.Text = "Local directory";
            // 
            // LocalDirectoryBox
            // 
            this.LocalDirectoryBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LocalDirectoryBox.Location = new System.Drawing.Point(415, 13);
            this.LocalDirectoryBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.LocalDirectoryBox.Name = "LocalDirectoryBox";
            this.LocalDirectoryBox.Size = new System.Drawing.Size(390, 27);
            this.LocalDirectoryBox.TabIndex = 6;
            // 
            // PasswordLabel
            // 
            this.PasswordLabel.AutoSize = true;
            this.PasswordLabel.Location = new System.Drawing.Point(11, 97);
            this.PasswordLabel.Name = "PasswordLabel";
            this.PasswordLabel.Size = new System.Drawing.Size(70, 20);
            this.PasswordLabel.TabIndex = 5;
            this.PasswordLabel.Text = "Password";
            // 
            // PasswordBox
            // 
            this.PasswordBox.Location = new System.Drawing.Point(91, 93);
            this.PasswordBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.PasswordBox.Name = "PasswordBox";
            this.PasswordBox.Size = new System.Drawing.Size(171, 27);
            this.PasswordBox.TabIndex = 4;
            // 
            // UsernameLabel
            // 
            this.UsernameLabel.AutoSize = true;
            this.UsernameLabel.Location = new System.Drawing.Point(11, 57);
            this.UsernameLabel.Name = "UsernameLabel";
            this.UsernameLabel.Size = new System.Drawing.Size(75, 20);
            this.UsernameLabel.TabIndex = 3;
            this.UsernameLabel.Text = "Username";
            // 
            // UsernameBox
            // 
            this.UsernameBox.Location = new System.Drawing.Point(91, 53);
            this.UsernameBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.UsernameBox.Name = "UsernameBox";
            this.UsernameBox.Size = new System.Drawing.Size(171, 27);
            this.UsernameBox.TabIndex = 2;
            // 
            // HostLabel
            // 
            this.HostLabel.AutoSize = true;
            this.HostLabel.Location = new System.Drawing.Point(11, 17);
            this.HostLabel.Name = "HostLabel";
            this.HostLabel.Size = new System.Drawing.Size(40, 20);
            this.HostLabel.TabIndex = 1;
            this.HostLabel.Text = "Host";
            // 
            // HostBox
            // 
            this.HostBox.Location = new System.Drawing.Point(91, 13);
            this.HostBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.HostBox.Name = "HostBox";
            this.HostBox.Size = new System.Drawing.Size(171, 27);
            this.HostBox.TabIndex = 0;
            // 
            // consentFormPage
            // 
            this.consentFormPage.Controls.Add(this.RemoveConsentFormButton);
            this.consentFormPage.Controls.Add(this.AddConsentFormButton);
            this.consentFormPage.Controls.Add(this.ConsentFormView);
            this.consentFormPage.Location = new System.Drawing.Point(4, 29);
            this.consentFormPage.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.consentFormPage.Name = "consentFormPage";
            this.consentFormPage.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.consentFormPage.Size = new System.Drawing.Size(909, 550);
            this.consentFormPage.TabIndex = 4;
            this.consentFormPage.Text = "Consent forms";
            this.consentFormPage.UseVisualStyleBackColor = true;
            // 
            // RemoveConsentFormButton
            // 
            this.RemoveConsentFormButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.RemoveConsentFormButton.Location = new System.Drawing.Point(109, 504);
            this.RemoveConsentFormButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.RemoveConsentFormButton.Name = "RemoveConsentFormButton";
            this.RemoveConsentFormButton.Size = new System.Drawing.Size(86, 31);
            this.RemoveConsentFormButton.TabIndex = 6;
            this.RemoveConsentFormButton.Text = "Remove";
            this.RemoveConsentFormButton.UseVisualStyleBackColor = false;
            this.RemoveConsentFormButton.Click += new System.EventHandler(this.RemoveConsentForm);
            // 
            // AddConsentFormButton
            // 
            this.AddConsentFormButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AddConsentFormButton.Location = new System.Drawing.Point(11, 504);
            this.AddConsentFormButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.AddConsentFormButton.Name = "AddConsentFormButton";
            this.AddConsentFormButton.Size = new System.Drawing.Size(86, 31);
            this.AddConsentFormButton.TabIndex = 5;
            this.AddConsentFormButton.Text = "Add";
            this.AddConsentFormButton.UseVisualStyleBackColor = true;
            this.AddConsentFormButton.Click += new System.EventHandler(this.AddConsentForm);
            // 
            // ConsentFormView
            // 
            this.ConsentFormView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ConsentFormView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ConsentFormView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ConsentForm_NameColumn,
            this.ConsentForm_DateColumn,
            this.ConsentForm_VersionColumn,
            this.ConsentForm_CommentColumn});
            this.ConsentFormView.Location = new System.Drawing.Point(11, 13);
            this.ConsentFormView.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ConsentFormView.Name = "ConsentFormView";
            this.ConsentFormView.RowHeadersVisible = false;
            this.ConsentFormView.RowHeadersWidth = 51;
            this.ConsentFormView.RowTemplate.Height = 25;
            this.ConsentFormView.Size = new System.Drawing.Size(887, 476);
            this.ConsentFormView.TabIndex = 0;
            // 
            // ConsentForm_NameColumn
            // 
            this.ConsentForm_NameColumn.DataPropertyName = "Name";
            this.ConsentForm_NameColumn.HeaderText = "Name";
            this.ConsentForm_NameColumn.MinimumWidth = 6;
            this.ConsentForm_NameColumn.Name = "ConsentForm_NameColumn";
            this.ConsentForm_NameColumn.Width = 300;
            // 
            // ConsentForm_DateColumn
            // 
            this.ConsentForm_DateColumn.DataPropertyName = "Signed";
            this.ConsentForm_DateColumn.HeaderText = "Date";
            this.ConsentForm_DateColumn.MinimumWidth = 6;
            this.ConsentForm_DateColumn.Name = "ConsentForm_DateColumn";
            this.ConsentForm_DateColumn.Width = 125;
            // 
            // ConsentForm_VersionColumn
            // 
            this.ConsentForm_VersionColumn.DataPropertyName = "Version";
            this.ConsentForm_VersionColumn.HeaderText = "Version";
            this.ConsentForm_VersionColumn.MinimumWidth = 6;
            this.ConsentForm_VersionColumn.Name = "ConsentForm_VersionColumn";
            this.ConsentForm_VersionColumn.Width = 125;
            // 
            // ConsentForm_CommentColumn
            // 
            this.ConsentForm_CommentColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ConsentForm_CommentColumn.DataPropertyName = "Comment";
            this.ConsentForm_CommentColumn.HeaderText = "Comment";
            this.ConsentForm_CommentColumn.MinimumWidth = 6;
            this.ConsentForm_CommentColumn.Name = "ConsentForm_CommentColumn";
            // 
            // settingsPage
            // 
            this.settingsPage.Controls.Add(this.infoBox);
            this.settingsPage.Location = new System.Drawing.Point(4, 29);
            this.settingsPage.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.settingsPage.Name = "settingsPage";
            this.settingsPage.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.settingsPage.Size = new System.Drawing.Size(909, 550);
            this.settingsPage.TabIndex = 3;
            this.settingsPage.Text = "Settings";
            this.settingsPage.UseVisualStyleBackColor = true;
            // 
            // infoBox
            // 
            this.infoBox.Controls.Add(this.LogoBox);
            this.infoBox.Controls.Add(this.LogoLabel);
            this.infoBox.Controls.Add(this.BrowseLogoButton);
            this.infoBox.Controls.Add(this.AddressLabel);
            this.infoBox.Controls.Add(this.AddressBox);
            this.infoBox.Controls.Add(this.NameLabel);
            this.infoBox.Controls.Add(this.NameBox);
            this.infoBox.Controls.Add(this.LogoPictureBox);
            this.infoBox.Location = new System.Drawing.Point(11, 13);
            this.infoBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.infoBox.Name = "infoBox";
            this.infoBox.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.infoBox.Size = new System.Drawing.Size(626, 213);
            this.infoBox.TabIndex = 1;
            this.infoBox.TabStop = false;
            this.infoBox.Text = "Association information";
            // 
            // LogoBox
            // 
            this.LogoBox.Location = new System.Drawing.Point(262, 105);
            this.LogoBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.LogoBox.Name = "LogoBox";
            this.LogoBox.Size = new System.Drawing.Size(258, 27);
            this.LogoBox.TabIndex = 8;
            // 
            // LogoLabel
            // 
            this.LogoLabel.AutoSize = true;
            this.LogoLabel.Location = new System.Drawing.Point(183, 111);
            this.LogoLabel.Name = "LogoLabel";
            this.LogoLabel.Size = new System.Drawing.Size(43, 20);
            this.LogoLabel.TabIndex = 6;
            this.LogoLabel.Text = "Logo";
            // 
            // BrowseLogoButton
            // 
            this.BrowseLogoButton.Location = new System.Drawing.Point(527, 105);
            this.BrowseLogoButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BrowseLogoButton.Name = "BrowseLogoButton";
            this.BrowseLogoButton.Size = new System.Drawing.Size(86, 27);
            this.BrowseLogoButton.TabIndex = 5;
            this.BrowseLogoButton.Text = "Browse";
            this.BrowseLogoButton.UseVisualStyleBackColor = true;
            this.BrowseLogoButton.Click += new System.EventHandler(this.BrowseLogo);
            // 
            // AddressLabel
            // 
            this.AddressLabel.AutoSize = true;
            this.AddressLabel.Location = new System.Drawing.Point(183, 71);
            this.AddressLabel.Name = "AddressLabel";
            this.AddressLabel.Size = new System.Drawing.Size(62, 20);
            this.AddressLabel.TabIndex = 4;
            this.AddressLabel.Text = "Address";
            // 
            // AddressBox
            // 
            this.AddressBox.Location = new System.Drawing.Point(262, 67);
            this.AddressBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.AddressBox.Name = "AddressBox";
            this.AddressBox.Size = new System.Drawing.Size(350, 27);
            this.AddressBox.TabIndex = 3;
            // 
            // NameLabel
            // 
            this.NameLabel.AutoSize = true;
            this.NameLabel.Location = new System.Drawing.Point(183, 31);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Size = new System.Drawing.Size(49, 20);
            this.NameLabel.TabIndex = 2;
            this.NameLabel.Text = "Name";
            // 
            // NameBox
            // 
            this.NameBox.Location = new System.Drawing.Point(262, 27);
            this.NameBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.NameBox.Name = "NameBox";
            this.NameBox.Size = new System.Drawing.Size(350, 27);
            this.NameBox.TabIndex = 1;
            // 
            // LogoPictureBox
            // 
            this.LogoPictureBox.BackColor = System.Drawing.Color.LightGray;
            this.LogoPictureBox.Location = new System.Drawing.Point(11, 27);
            this.LogoPictureBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.LogoPictureBox.Name = "LogoPictureBox";
            this.LogoPictureBox.Size = new System.Drawing.Size(146, 171);
            this.LogoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.LogoPictureBox.TabIndex = 0;
            this.LogoPictureBox.TabStop = false;
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
            this.helpPage.Location = new System.Drawing.Point(4, 29);
            this.helpPage.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.helpPage.Name = "helpPage";
            this.helpPage.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.helpPage.Size = new System.Drawing.Size(909, 550);
            this.helpPage.TabIndex = 1;
            this.helpPage.Text = "Help";
            this.helpPage.UseVisualStyleBackColor = true;
            // 
            // EmailLink
            // 
            this.EmailLink.AutoSize = true;
            this.EmailLink.Location = new System.Drawing.Point(53, 213);
            this.EmailLink.Name = "EmailLink";
            this.EmailLink.Size = new System.Drawing.Size(229, 20);
            this.EmailLink.TabIndex = 7;
            this.EmailLink.TabStop = true;
            this.EmailLink.Text = "martin.jensen.1997@hotmail.com";
            this.EmailLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.EmailLink_LinkClicked);
            // 
            // EmailLabel
            // 
            this.EmailLabel.AutoSize = true;
            this.EmailLabel.Location = new System.Drawing.Point(11, 213);
            this.EmailLabel.Name = "EmailLabel";
            this.EmailLabel.Size = new System.Drawing.Size(49, 20);
            this.EmailLabel.TabIndex = 6;
            this.EmailLabel.Text = "Email:";
            // 
            // AuthorLabel
            // 
            this.AuthorLabel.AutoSize = true;
            this.AuthorLabel.Location = new System.Drawing.Point(11, 193);
            this.AuthorLabel.Name = "AuthorLabel";
            this.AuthorLabel.Size = new System.Drawing.Size(179, 20);
            this.AuthorLabel.TabIndex = 5;
            this.AuthorLabel.Text = "Author: Martin J. R. Jensen";
            // 
            // LicenseLabel
            // 
            this.LicenseLabel.AutoSize = true;
            this.LicenseLabel.Location = new System.Drawing.Point(11, 255);
            this.LicenseLabel.Name = "LicenseLabel";
            this.LicenseLabel.Size = new System.Drawing.Size(139, 20);
            this.LicenseLabel.TabIndex = 4;
            this.LicenseLabel.Text = "License: Apache-2.0";
            // 
            // SourceLink
            // 
            this.SourceLink.AutoSize = true;
            this.SourceLink.Location = new System.Drawing.Point(61, 275);
            this.SourceLink.Name = "SourceLink";
            this.SourceLink.Size = new System.Drawing.Size(245, 20);
            this.SourceLink.TabIndex = 3;
            this.SourceLink.TabStop = true;
            this.SourceLink.Text = "https://github.com/mjrj97/Manager";
            this.SourceLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.SourceLink_LinkClicked);
            // 
            // SourceLabel
            // 
            this.SourceLabel.AutoSize = true;
            this.SourceLabel.Location = new System.Drawing.Point(11, 275);
            this.SourceLabel.Name = "SourceLabel";
            this.SourceLabel.Size = new System.Drawing.Size(57, 20);
            this.SourceLabel.TabIndex = 2;
            this.SourceLabel.Text = "Source:";
            // 
            // VersionLabel
            // 
            this.VersionLabel.AutoSize = true;
            this.VersionLabel.Location = new System.Drawing.Point(11, 156);
            this.VersionLabel.Name = "VersionLabel";
            this.VersionLabel.Size = new System.Drawing.Size(126, 20);
            this.VersionLabel.TabIndex = 1;
            this.VersionLabel.Text = "Timotheus v. 0.1.0";
            // 
            // iconBox
            // 
            this.iconBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.iconBox.Image = ((System.Drawing.Image)(resources.GetObject("iconBox.Image")));
            this.iconBox.Location = new System.Drawing.Point(11, 13);
            this.iconBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.iconBox.Name = "iconBox";
            this.iconBox.Size = new System.Drawing.Size(114, 133);
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
            this.TrayContextMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.TrayContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TrayOpen,
            this.TrayClose});
            this.TrayContextMenu.Name = "TrayContextMenu";
            this.TrayContextMenu.Size = new System.Drawing.Size(115, 52);
            // 
            // TrayOpen
            // 
            this.TrayOpen.Name = "TrayOpen";
            this.TrayOpen.Size = new System.Drawing.Size(114, 24);
            this.TrayOpen.Text = "Open";
            this.TrayOpen.Click += new System.EventHandler(this.Open);
            // 
            // TrayClose
            // 
            this.TrayClose.Name = "TrayClose";
            this.TrayClose.Size = new System.Drawing.Size(114, 24);
            this.TrayClose.Text = "Close";
            this.TrayClose.Click += new System.EventHandler(this.Exit);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(919, 588);
            this.Controls.Add(this.tabControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MinimumSize = new System.Drawing.Size(569, 318);
            this.Name = "MainWindow";
            this.Text = "Timotheus";
            this.Resize += new System.EventHandler(this.Manager_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.CalendarView)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.calendarPage.ResumeLayout(false);
            this.calendarPage.PerformLayout();
            this.sftpPage.ResumeLayout(false);
            this.sftpPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FileView)).EndInit();
            this.consentFormPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ConsentFormView)).EndInit();
            this.settingsPage.ResumeLayout(false);
            this.infoBox.ResumeLayout(false);
            this.infoBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LogoPictureBox)).EndInit();
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
        private System.Windows.Forms.TabPage sftpPage;
        private System.Windows.Forms.TabPage settingsPage;
        private System.Windows.Forms.TextBox PeriodBox;
        private System.Windows.Forms.Button SyncCalendarButton;
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
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button OpenButton;
        private System.Windows.Forms.Button ExportButton;
        private System.Windows.Forms.Button RemoveButton;
        private System.Windows.Forms.Button AddButton;
        private System.Windows.Forms.Button AddYearButton;
        private System.Windows.Forms.Button SubtractYearButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn StartColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn EndColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn NameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn DescriptionColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn LocationColumn;
        private System.Windows.Forms.TextBox HostBox;
        private System.Windows.Forms.TextBox UsernameBox;
        private System.Windows.Forms.Label PasswordLabel;
        private System.Windows.Forms.TextBox PasswordBox;
        private System.Windows.Forms.Label UsernameLabel;
        private System.Windows.Forms.TextBox RemoteDirectoryBox;
        private System.Windows.Forms.Label RemoteDirectoryLabel;
        private System.Windows.Forms.Label LocalDirectoryLabel;
        private System.Windows.Forms.TextBox LocalDirectoryBox;
        private System.Windows.Forms.Button BrowseButton;
        private System.Windows.Forms.DataGridView FileView;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileNameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileSizeColumn;
        private System.Windows.Forms.Button ShowDirectoryButton;
        private System.Windows.Forms.Button DownloadAllButton;
        private System.Windows.Forms.Button SynchronizeButton;
        private System.Windows.Forms.GroupBox infoBox;
        private System.Windows.Forms.Label AddressLabel;
        private System.Windows.Forms.TextBox AddressBox;
        private System.Windows.Forms.Label NameLabel;
        private System.Windows.Forms.Label LogoLabel;
        private System.Windows.Forms.TextBox NameBox;
        private System.Windows.Forms.Label HostLabel;
        private System.Windows.Forms.Button BrowseLogoButton;
        private System.Windows.Forms.PictureBox LogoPictureBox;
        private System.Windows.Forms.TextBox LogoBox;
        private System.Windows.Forms.TabPage consentFormPage;
        private System.Windows.Forms.DataGridView ConsentFormView;
        private System.Windows.Forms.Button AddConsentFormButton;
        private System.Windows.Forms.Button RemoveConsentFormButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn ConsentForm_NameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ConsentForm_DateColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ConsentForm_VersionColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ConsentForm_CommentColumn;
        private System.Windows.Forms.RadioButton MonthButton;
        private System.Windows.Forms.RadioButton HalfYearButton;
        private System.Windows.Forms.RadioButton YearButton;
        private System.Windows.Forms.RadioButton AllButton;
    }
}

