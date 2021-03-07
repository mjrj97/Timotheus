
namespace Timotheus.Forms
{
    partial class SyncCalendar
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SyncCalendar));
            this.Sync_SyncButton = new System.Windows.Forms.Button();
            this.Sync_UseExistingButton = new System.Windows.Forms.RadioButton();
            this.Sync_DestinationBox = new System.Windows.Forms.GroupBox();
            this.Sync_PasswordBox = new System.Windows.Forms.TextBox();
            this.Sync_UsernameBox = new System.Windows.Forms.TextBox();
            this.Sync_CalDAVBox = new System.Windows.Forms.TextBox();
            this.Sync_PasswordLabel = new System.Windows.Forms.Label();
            this.Sync_UsernameLabel = new System.Windows.Forms.Label();
            this.Sync_CalDAVLabel = new System.Windows.Forms.Label();
            this.Sync_NewCalendarButton = new System.Windows.Forms.RadioButton();
            this.Sync_CancelButton = new System.Windows.Forms.Button();
            this.Sync_SettingsBox = new System.Windows.Forms.GroupBox();
            this.Sync_bTimePicker = new System.Windows.Forms.DateTimePicker();
            this.Sync_aTimePicker = new System.Windows.Forms.DateTimePicker();
            this.Sync_CustomCalendarButton = new System.Windows.Forms.RadioButton();
            this.Sync_PeriodCalendarButton = new System.Windows.Forms.RadioButton();
            this.Sync_EntireCalendarButton = new System.Windows.Forms.RadioButton();
            this.Sync_DestinationBox.SuspendLayout();
            this.Sync_SettingsBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // Sync_SyncButton
            // 
            this.Sync_SyncButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Sync_SyncButton.Location = new System.Drawing.Point(212, 414);
            this.Sync_SyncButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Sync_SyncButton.Name = "Sync_SyncButton";
            this.Sync_SyncButton.Size = new System.Drawing.Size(86, 31);
            this.Sync_SyncButton.TabIndex = 0;
            this.Sync_SyncButton.Text = "Sync";
            this.Sync_SyncButton.UseVisualStyleBackColor = true;
            this.Sync_SyncButton.Click += new System.EventHandler(this.Sync);
            // 
            // Sync_UseExistingButton
            // 
            this.Sync_UseExistingButton.AutoSize = true;
            this.Sync_UseExistingButton.Enabled = false;
            this.Sync_UseExistingButton.Location = new System.Drawing.Point(11, 27);
            this.Sync_UseExistingButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Sync_UseExistingButton.Name = "Sync_UseExistingButton";
            this.Sync_UseExistingButton.Size = new System.Drawing.Size(139, 24);
            this.Sync_UseExistingButton.TabIndex = 1;
            this.Sync_UseExistingButton.TabStop = true;
            this.Sync_UseExistingButton.Text = "Current calendar";
            this.Sync_UseExistingButton.UseVisualStyleBackColor = true;
            // 
            // Sync_DestinationBox
            // 
            this.Sync_DestinationBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Sync_DestinationBox.Controls.Add(this.Sync_PasswordBox);
            this.Sync_DestinationBox.Controls.Add(this.Sync_UsernameBox);
            this.Sync_DestinationBox.Controls.Add(this.Sync_CalDAVBox);
            this.Sync_DestinationBox.Controls.Add(this.Sync_PasswordLabel);
            this.Sync_DestinationBox.Controls.Add(this.Sync_UsernameLabel);
            this.Sync_DestinationBox.Controls.Add(this.Sync_CalDAVLabel);
            this.Sync_DestinationBox.Controls.Add(this.Sync_NewCalendarButton);
            this.Sync_DestinationBox.Controls.Add(this.Sync_UseExistingButton);
            this.Sync_DestinationBox.Location = new System.Drawing.Point(11, 7);
            this.Sync_DestinationBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Sync_DestinationBox.Name = "Sync_DestinationBox";
            this.Sync_DestinationBox.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Sync_DestinationBox.Size = new System.Drawing.Size(377, 221);
            this.Sync_DestinationBox.TabIndex = 2;
            this.Sync_DestinationBox.TabStop = false;
            this.Sync_DestinationBox.Text = "Destination";
            // 
            // Sync_PasswordBox
            // 
            this.Sync_PasswordBox.Enabled = false;
            this.Sync_PasswordBox.Location = new System.Drawing.Point(103, 173);
            this.Sync_PasswordBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Sync_PasswordBox.Name = "Sync_PasswordBox";
            this.Sync_PasswordBox.Size = new System.Drawing.Size(262, 27);
            this.Sync_PasswordBox.TabIndex = 8;
            // 
            // Sync_UsernameBox
            // 
            this.Sync_UsernameBox.Enabled = false;
            this.Sync_UsernameBox.Location = new System.Drawing.Point(103, 135);
            this.Sync_UsernameBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Sync_UsernameBox.Name = "Sync_UsernameBox";
            this.Sync_UsernameBox.Size = new System.Drawing.Size(262, 27);
            this.Sync_UsernameBox.TabIndex = 7;
            // 
            // Sync_CalDAVBox
            // 
            this.Sync_CalDAVBox.Enabled = false;
            this.Sync_CalDAVBox.Location = new System.Drawing.Point(103, 96);
            this.Sync_CalDAVBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Sync_CalDAVBox.Name = "Sync_CalDAVBox";
            this.Sync_CalDAVBox.Size = new System.Drawing.Size(262, 27);
            this.Sync_CalDAVBox.TabIndex = 6;
            // 
            // Sync_PasswordLabel
            // 
            this.Sync_PasswordLabel.AutoSize = true;
            this.Sync_PasswordLabel.Enabled = false;
            this.Sync_PasswordLabel.Location = new System.Drawing.Point(11, 177);
            this.Sync_PasswordLabel.Name = "Sync_PasswordLabel";
            this.Sync_PasswordLabel.Size = new System.Drawing.Size(70, 20);
            this.Sync_PasswordLabel.TabIndex = 5;
            this.Sync_PasswordLabel.Text = "Password";
            // 
            // Sync_UsernameLabel
            // 
            this.Sync_UsernameLabel.AutoSize = true;
            this.Sync_UsernameLabel.Enabled = false;
            this.Sync_UsernameLabel.Location = new System.Drawing.Point(11, 139);
            this.Sync_UsernameLabel.Name = "Sync_UsernameLabel";
            this.Sync_UsernameLabel.Size = new System.Drawing.Size(75, 20);
            this.Sync_UsernameLabel.TabIndex = 4;
            this.Sync_UsernameLabel.Text = "Username";
            // 
            // Sync_CalDAVLabel
            // 
            this.Sync_CalDAVLabel.AutoSize = true;
            this.Sync_CalDAVLabel.Enabled = false;
            this.Sync_CalDAVLabel.Location = new System.Drawing.Point(11, 100);
            this.Sync_CalDAVLabel.Name = "Sync_CalDAVLabel";
            this.Sync_CalDAVLabel.Size = new System.Drawing.Size(59, 20);
            this.Sync_CalDAVLabel.TabIndex = 3;
            this.Sync_CalDAVLabel.Text = "CalDAV";
            // 
            // Sync_NewCalendarButton
            // 
            this.Sync_NewCalendarButton.AutoSize = true;
            this.Sync_NewCalendarButton.Location = new System.Drawing.Point(11, 60);
            this.Sync_NewCalendarButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Sync_NewCalendarButton.Name = "Sync_NewCalendarButton";
            this.Sync_NewCalendarButton.Size = new System.Drawing.Size(144, 24);
            this.Sync_NewCalendarButton.TabIndex = 2;
            this.Sync_NewCalendarButton.TabStop = true;
            this.Sync_NewCalendarButton.Text = "Another calendar";
            this.Sync_NewCalendarButton.UseVisualStyleBackColor = true;
            this.Sync_NewCalendarButton.CheckedChanged += new System.EventHandler(this.NewCalendarButton_CheckedChanged);
            // 
            // Sync_CancelButton
            // 
            this.Sync_CancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Sync_CancelButton.Location = new System.Drawing.Point(304, 414);
            this.Sync_CancelButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Sync_CancelButton.Name = "Sync_CancelButton";
            this.Sync_CancelButton.Size = new System.Drawing.Size(86, 31);
            this.Sync_CancelButton.TabIndex = 3;
            this.Sync_CancelButton.Text = "Cancel";
            this.Sync_CancelButton.UseVisualStyleBackColor = true;
            this.Sync_CancelButton.Click += new System.EventHandler(this.Close);
            // 
            // Sync_SettingsBox
            // 
            this.Sync_SettingsBox.Controls.Add(this.Sync_bTimePicker);
            this.Sync_SettingsBox.Controls.Add(this.Sync_aTimePicker);
            this.Sync_SettingsBox.Controls.Add(this.Sync_CustomCalendarButton);
            this.Sync_SettingsBox.Controls.Add(this.Sync_PeriodCalendarButton);
            this.Sync_SettingsBox.Controls.Add(this.Sync_EntireCalendarButton);
            this.Sync_SettingsBox.Location = new System.Drawing.Point(11, 235);
            this.Sync_SettingsBox.Name = "Sync_SettingsBox";
            this.Sync_SettingsBox.Size = new System.Drawing.Size(377, 168);
            this.Sync_SettingsBox.TabIndex = 4;
            this.Sync_SettingsBox.TabStop = false;
            this.Sync_SettingsBox.Text = "Settings";
            // 
            // Sync_bTimePicker
            // 
            this.Sync_bTimePicker.Enabled = false;
            this.Sync_bTimePicker.Location = new System.Drawing.Point(195, 126);
            this.Sync_bTimePicker.Name = "Sync_bTimePicker";
            this.Sync_bTimePicker.Size = new System.Drawing.Size(170, 27);
            this.Sync_bTimePicker.TabIndex = 4;
            // 
            // Sync_aTimePicker
            // 
            this.Sync_aTimePicker.Enabled = false;
            this.Sync_aTimePicker.Location = new System.Drawing.Point(11, 126);
            this.Sync_aTimePicker.Name = "Sync_aTimePicker";
            this.Sync_aTimePicker.Size = new System.Drawing.Size(170, 27);
            this.Sync_aTimePicker.TabIndex = 3;
            // 
            // Sync_CustomCalendarButton
            // 
            this.Sync_CustomCalendarButton.AutoSize = true;
            this.Sync_CustomCalendarButton.Location = new System.Drawing.Point(11, 86);
            this.Sync_CustomCalendarButton.Name = "Sync_CustomCalendarButton";
            this.Sync_CustomCalendarButton.Size = new System.Drawing.Size(160, 24);
            this.Sync_CustomCalendarButton.TabIndex = 2;
            this.Sync_CustomCalendarButton.TabStop = true;
            this.Sync_CustomCalendarButton.Text = "Sync custom period";
            this.Sync_CustomCalendarButton.UseVisualStyleBackColor = true;
            this.Sync_CustomCalendarButton.CheckedChanged += new System.EventHandler(this.CustomCalendarButton_CheckedChanged);
            // 
            // Sync_PeriodCalendarButton
            // 
            this.Sync_PeriodCalendarButton.AutoSize = true;
            this.Sync_PeriodCalendarButton.Location = new System.Drawing.Point(11, 56);
            this.Sync_PeriodCalendarButton.Name = "Sync_PeriodCalendarButton";
            this.Sync_PeriodCalendarButton.Size = new System.Drawing.Size(133, 24);
            this.Sync_PeriodCalendarButton.TabIndex = 1;
            this.Sync_PeriodCalendarButton.TabStop = true;
            this.Sync_PeriodCalendarButton.Text = "Sync the period";
            this.Sync_PeriodCalendarButton.UseVisualStyleBackColor = true;
            // 
            // Sync_EntireCalendarButton
            // 
            this.Sync_EntireCalendarButton.AutoSize = true;
            this.Sync_EntireCalendarButton.Checked = true;
            this.Sync_EntireCalendarButton.Location = new System.Drawing.Point(11, 26);
            this.Sync_EntireCalendarButton.Name = "Sync_EntireCalendarButton";
            this.Sync_EntireCalendarButton.Size = new System.Drawing.Size(188, 24);
            this.Sync_EntireCalendarButton.TabIndex = 0;
            this.Sync_EntireCalendarButton.TabStop = true;
            this.Sync_EntireCalendarButton.Text = "Sync the entire calendar";
            this.Sync_EntireCalendarButton.UseVisualStyleBackColor = true;
            // 
            // SyncCalendar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(402, 458);
            this.Controls.Add(this.Sync_SettingsBox);
            this.Controls.Add(this.Sync_CancelButton);
            this.Controls.Add(this.Sync_DestinationBox);
            this.Controls.Add(this.Sync_SyncButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SyncCalendar";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Sync";
            this.Sync_DestinationBox.ResumeLayout(false);
            this.Sync_DestinationBox.PerformLayout();
            this.Sync_SettingsBox.ResumeLayout(false);
            this.Sync_SettingsBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Sync_SyncButton;
        private System.Windows.Forms.RadioButton Sync_UseExistingButton;
        private System.Windows.Forms.GroupBox Sync_DestinationBox;
        private System.Windows.Forms.RadioButton Sync_NewCalendarButton;
        private System.Windows.Forms.Label Sync_PasswordLabel;
        private System.Windows.Forms.Label Sync_UsernameLabel;
        private System.Windows.Forms.Label Sync_CalDAVLabel;
        private System.Windows.Forms.Button Sync_CancelButton;
        private System.Windows.Forms.TextBox Sync_PasswordBox;
        private System.Windows.Forms.TextBox Sync_UsernameBox;
        private System.Windows.Forms.TextBox Sync_CalDAVBox;
        private System.Windows.Forms.GroupBox Sync_SettingsBox;
        private System.Windows.Forms.RadioButton Sync_PeriodCalendarButton;
        private System.Windows.Forms.RadioButton Sync_EntireCalendarButton;
        private System.Windows.Forms.DateTimePicker Sync_bTimePicker;
        private System.Windows.Forms.DateTimePicker Sync_aTimePicker;
        private System.Windows.Forms.RadioButton Sync_CustomCalendarButton;
    }
}