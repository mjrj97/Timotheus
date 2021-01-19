
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.calendarPage = new System.Windows.Forms.TabPage();
            this.saveButton = new System.Windows.Forms.Button();
            this.openButton = new System.Windows.Forms.Button();
            this.updateWebsiteButton = new System.Windows.Forms.Button();
            this.exportButton = new System.Windows.Forms.Button();
            this.Remove = new System.Windows.Forms.Button();
            this.Add = new System.Windows.Forms.Button();
            this.CalendarView = new System.Windows.Forms.DataGridView();
            this.StartColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EndColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DescriptionColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Year = new System.Windows.Forms.TextBox();
            this.AddYear = new System.Windows.Forms.Button();
            this.SubtractYear = new System.Windows.Forms.Button();
            this.helpPage = new System.Windows.Forms.TabPage();
            this.TrayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.TrayContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.TrayOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.TrayClose = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl.SuspendLayout();
            this.calendarPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CalendarView)).BeginInit();
            this.TrayContextMenu.SuspendLayout();
            this.SuspendLayout();
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
            this.calendarPage.Controls.Add(this.updateWebsiteButton);
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
            // updateWebsiteButton
            // 
            this.updateWebsiteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.updateWebsiteButton.Enabled = false;
            this.updateWebsiteButton.Location = new System.Drawing.Point(549, 378);
            this.updateWebsiteButton.Name = "updateWebsiteButton";
            this.updateWebsiteButton.Size = new System.Drawing.Size(125, 23);
            this.updateWebsiteButton.TabIndex = 7;
            this.updateWebsiteButton.Text = "Update website";
            this.updateWebsiteButton.UseVisualStyleBackColor = true;
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
            this.helpPage.Location = new System.Drawing.Point(4, 24);
            this.helpPage.Name = "helpPage";
            this.helpPage.Padding = new System.Windows.Forms.Padding(3);
            this.helpPage.Size = new System.Drawing.Size(794, 409);
            this.helpPage.TabIndex = 1;
            this.helpPage.Text = "Help";
            this.helpPage.UseVisualStyleBackColor = true;
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
            this.tabControl.ResumeLayout(false);
            this.calendarPage.ResumeLayout(false);
            this.calendarPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CalendarView)).EndInit();
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
        private System.Windows.Forms.DataGridView CalendarView;
        private System.Windows.Forms.Button Remove;
        private System.Windows.Forms.Button Add;
        private System.Windows.Forms.Button updateWebsiteButton;
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
    }
}

