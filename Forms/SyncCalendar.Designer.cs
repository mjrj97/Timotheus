
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
            this.SyncCalendar_SyncButton = new System.Windows.Forms.Button();
            this.SyncCalendar_UseExistingButton = new System.Windows.Forms.RadioButton();
            this.SyncCalendar_DestinationBox = new System.Windows.Forms.GroupBox();
            this.SyncCalendar_PasswordBox = new System.Windows.Forms.TextBox();
            this.SyncCalendar_UsernameBox = new System.Windows.Forms.TextBox();
            this.SyncCalendar_CalDAVBox = new System.Windows.Forms.TextBox();
            this.SyncCalendar_PasswordLabel = new System.Windows.Forms.Label();
            this.SyncCalendar_UsernameLabel = new System.Windows.Forms.Label();
            this.SyncCalendar_CalDAVLabel = new System.Windows.Forms.Label();
            this.SyncCalendar_NewCalendarButton = new System.Windows.Forms.RadioButton();
            this.SyncCalendar_CancelButton = new System.Windows.Forms.Button();
            this.SyncCalendar_SettingsBox = new System.Windows.Forms.GroupBox();
            this.SyncCalendar_bTimePicker = new System.Windows.Forms.DateTimePicker();
            this.SyncCalendar_aTimePicker = new System.Windows.Forms.DateTimePicker();
            this.SyncCalendar_CustomCalendarButton = new System.Windows.Forms.RadioButton();
            this.SyncCalendar_PeriodCalendarButton = new System.Windows.Forms.RadioButton();
            this.SyncCalendar_EntireCalendarButton = new System.Windows.Forms.RadioButton();
            this.SyncCalendar_DestinationBox.SuspendLayout();
            this.SyncCalendar_SettingsBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // SyncCalendar_SyncButton
            // 
            this.SyncCalendar_SyncButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SyncCalendar_SyncButton.Location = new System.Drawing.Point(186, 310);
            this.SyncCalendar_SyncButton.Name = "SyncCalendar_SyncButton";
            this.SyncCalendar_SyncButton.Size = new System.Drawing.Size(75, 23);
            this.SyncCalendar_SyncButton.TabIndex = 0;
            this.SyncCalendar_SyncButton.Text = "Sync";
            this.SyncCalendar_SyncButton.UseVisualStyleBackColor = true;
            this.SyncCalendar_SyncButton.Click += new System.EventHandler(this.Sync);
            // 
            // SyncCalendar_UseExistingButton
            // 
            this.SyncCalendar_UseExistingButton.AutoSize = true;
            this.SyncCalendar_UseExistingButton.Enabled = false;
            this.SyncCalendar_UseExistingButton.Location = new System.Drawing.Point(10, 20);
            this.SyncCalendar_UseExistingButton.Name = "SyncCalendar_UseExistingButton";
            this.SyncCalendar_UseExistingButton.Size = new System.Drawing.Size(113, 19);
            this.SyncCalendar_UseExistingButton.TabIndex = 1;
            this.SyncCalendar_UseExistingButton.TabStop = true;
            this.SyncCalendar_UseExistingButton.Text = "Current calendar";
            this.SyncCalendar_UseExistingButton.UseVisualStyleBackColor = true;
            // 
            // SyncCalendar_DestinationBox
            // 
            this.SyncCalendar_DestinationBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SyncCalendar_DestinationBox.Controls.Add(this.SyncCalendar_PasswordBox);
            this.SyncCalendar_DestinationBox.Controls.Add(this.SyncCalendar_UsernameBox);
            this.SyncCalendar_DestinationBox.Controls.Add(this.SyncCalendar_CalDAVBox);
            this.SyncCalendar_DestinationBox.Controls.Add(this.SyncCalendar_PasswordLabel);
            this.SyncCalendar_DestinationBox.Controls.Add(this.SyncCalendar_UsernameLabel);
            this.SyncCalendar_DestinationBox.Controls.Add(this.SyncCalendar_CalDAVLabel);
            this.SyncCalendar_DestinationBox.Controls.Add(this.SyncCalendar_NewCalendarButton);
            this.SyncCalendar_DestinationBox.Controls.Add(this.SyncCalendar_UseExistingButton);
            this.SyncCalendar_DestinationBox.Location = new System.Drawing.Point(10, 5);
            this.SyncCalendar_DestinationBox.Name = "SyncCalendar_DestinationBox";
            this.SyncCalendar_DestinationBox.Size = new System.Drawing.Size(330, 166);
            this.SyncCalendar_DestinationBox.TabIndex = 2;
            this.SyncCalendar_DestinationBox.TabStop = false;
            this.SyncCalendar_DestinationBox.Text = "Destination";
            // 
            // SyncCalendar_PasswordBox
            // 
            this.SyncCalendar_PasswordBox.Enabled = false;
            this.SyncCalendar_PasswordBox.Location = new System.Drawing.Point(90, 130);
            this.SyncCalendar_PasswordBox.Name = "SyncCalendar_PasswordBox";
            this.SyncCalendar_PasswordBox.Size = new System.Drawing.Size(230, 23);
            this.SyncCalendar_PasswordBox.TabIndex = 8;
            // 
            // SyncCalendar_UsernameBox
            // 
            this.SyncCalendar_UsernameBox.Enabled = false;
            this.SyncCalendar_UsernameBox.Location = new System.Drawing.Point(90, 101);
            this.SyncCalendar_UsernameBox.Name = "SyncCalendar_UsernameBox";
            this.SyncCalendar_UsernameBox.Size = new System.Drawing.Size(230, 23);
            this.SyncCalendar_UsernameBox.TabIndex = 7;
            // 
            // SyncCalendar_CalDAVBox
            // 
            this.SyncCalendar_CalDAVBox.Enabled = false;
            this.SyncCalendar_CalDAVBox.Location = new System.Drawing.Point(90, 72);
            this.SyncCalendar_CalDAVBox.Name = "SyncCalendar_CalDAVBox";
            this.SyncCalendar_CalDAVBox.Size = new System.Drawing.Size(230, 23);
            this.SyncCalendar_CalDAVBox.TabIndex = 6;
            // 
            // SyncCalendar_PasswordLabel
            // 
            this.SyncCalendar_PasswordLabel.AutoSize = true;
            this.SyncCalendar_PasswordLabel.Enabled = false;
            this.SyncCalendar_PasswordLabel.Location = new System.Drawing.Point(10, 133);
            this.SyncCalendar_PasswordLabel.Name = "SyncCalendar_PasswordLabel";
            this.SyncCalendar_PasswordLabel.Size = new System.Drawing.Size(57, 15);
            this.SyncCalendar_PasswordLabel.TabIndex = 5;
            this.SyncCalendar_PasswordLabel.Text = "Password";
            // 
            // SyncCalendar_UsernameLabel
            // 
            this.SyncCalendar_UsernameLabel.AutoSize = true;
            this.SyncCalendar_UsernameLabel.Enabled = false;
            this.SyncCalendar_UsernameLabel.Location = new System.Drawing.Point(10, 104);
            this.SyncCalendar_UsernameLabel.Name = "SyncCalendar_UsernameLabel";
            this.SyncCalendar_UsernameLabel.Size = new System.Drawing.Size(60, 15);
            this.SyncCalendar_UsernameLabel.TabIndex = 4;
            this.SyncCalendar_UsernameLabel.Text = "Username";
            // 
            // SyncCalendar_CalDAVLabel
            // 
            this.SyncCalendar_CalDAVLabel.AutoSize = true;
            this.SyncCalendar_CalDAVLabel.Enabled = false;
            this.SyncCalendar_CalDAVLabel.Location = new System.Drawing.Point(10, 75);
            this.SyncCalendar_CalDAVLabel.Name = "SyncCalendar_CalDAVLabel";
            this.SyncCalendar_CalDAVLabel.Size = new System.Drawing.Size(46, 15);
            this.SyncCalendar_CalDAVLabel.TabIndex = 3;
            this.SyncCalendar_CalDAVLabel.Text = "CalDAV";
            // 
            // SyncCalendar_NewCalendarButton
            // 
            this.SyncCalendar_NewCalendarButton.AutoSize = true;
            this.SyncCalendar_NewCalendarButton.Location = new System.Drawing.Point(10, 45);
            this.SyncCalendar_NewCalendarButton.Name = "SyncCalendar_NewCalendarButton";
            this.SyncCalendar_NewCalendarButton.Size = new System.Drawing.Size(116, 19);
            this.SyncCalendar_NewCalendarButton.TabIndex = 2;
            this.SyncCalendar_NewCalendarButton.TabStop = true;
            this.SyncCalendar_NewCalendarButton.Text = "Another calendar";
            this.SyncCalendar_NewCalendarButton.UseVisualStyleBackColor = true;
            this.SyncCalendar_NewCalendarButton.CheckedChanged += new System.EventHandler(this.NewCalendarButton_CheckedChanged);
            // 
            // SyncCalendar_CancelButton
            // 
            this.SyncCalendar_CancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SyncCalendar_CancelButton.Location = new System.Drawing.Point(266, 310);
            this.SyncCalendar_CancelButton.Name = "SyncCalendar_CancelButton";
            this.SyncCalendar_CancelButton.Size = new System.Drawing.Size(75, 23);
            this.SyncCalendar_CancelButton.TabIndex = 3;
            this.SyncCalendar_CancelButton.Text = "Cancel";
            this.SyncCalendar_CancelButton.UseVisualStyleBackColor = true;
            this.SyncCalendar_CancelButton.Click += new System.EventHandler(this.Close);
            // 
            // SyncCalendar_SettingsBox
            // 
            this.SyncCalendar_SettingsBox.Controls.Add(this.SyncCalendar_bTimePicker);
            this.SyncCalendar_SettingsBox.Controls.Add(this.SyncCalendar_aTimePicker);
            this.SyncCalendar_SettingsBox.Controls.Add(this.SyncCalendar_CustomCalendarButton);
            this.SyncCalendar_SettingsBox.Controls.Add(this.SyncCalendar_PeriodCalendarButton);
            this.SyncCalendar_SettingsBox.Controls.Add(this.SyncCalendar_EntireCalendarButton);
            this.SyncCalendar_SettingsBox.Location = new System.Drawing.Point(10, 176);
            this.SyncCalendar_SettingsBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.SyncCalendar_SettingsBox.Name = "SyncCalendar_SettingsBox";
            this.SyncCalendar_SettingsBox.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.SyncCalendar_SettingsBox.Size = new System.Drawing.Size(330, 126);
            this.SyncCalendar_SettingsBox.TabIndex = 4;
            this.SyncCalendar_SettingsBox.TabStop = false;
            this.SyncCalendar_SettingsBox.Text = "Settings";
            // 
            // SyncCalendar_bTimePicker
            // 
            this.SyncCalendar_bTimePicker.Enabled = false;
            this.SyncCalendar_bTimePicker.Location = new System.Drawing.Point(171, 94);
            this.SyncCalendar_bTimePicker.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.SyncCalendar_bTimePicker.Name = "SyncCalendar_bTimePicker";
            this.SyncCalendar_bTimePicker.Size = new System.Drawing.Size(149, 23);
            this.SyncCalendar_bTimePicker.TabIndex = 4;
            // 
            // SyncCalendar_aTimePicker
            // 
            this.SyncCalendar_aTimePicker.Enabled = false;
            this.SyncCalendar_aTimePicker.Location = new System.Drawing.Point(10, 94);
            this.SyncCalendar_aTimePicker.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.SyncCalendar_aTimePicker.Name = "SyncCalendar_aTimePicker";
            this.SyncCalendar_aTimePicker.Size = new System.Drawing.Size(149, 23);
            this.SyncCalendar_aTimePicker.TabIndex = 3;
            // 
            // SyncCalendar_CustomCalendarButton
            // 
            this.SyncCalendar_CustomCalendarButton.AutoSize = true;
            this.SyncCalendar_CustomCalendarButton.Location = new System.Drawing.Point(10, 64);
            this.SyncCalendar_CustomCalendarButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.SyncCalendar_CustomCalendarButton.Name = "SyncCalendar_CustomCalendarButton";
            this.SyncCalendar_CustomCalendarButton.Size = new System.Drawing.Size(130, 19);
            this.SyncCalendar_CustomCalendarButton.TabIndex = 2;
            this.SyncCalendar_CustomCalendarButton.TabStop = true;
            this.SyncCalendar_CustomCalendarButton.Text = "Sync custom period";
            this.SyncCalendar_CustomCalendarButton.UseVisualStyleBackColor = true;
            this.SyncCalendar_CustomCalendarButton.CheckedChanged += new System.EventHandler(this.CustomCalendarButton_CheckedChanged);
            // 
            // SyncCalendar_PeriodCalendarButton
            // 
            this.SyncCalendar_PeriodCalendarButton.AutoSize = true;
            this.SyncCalendar_PeriodCalendarButton.Location = new System.Drawing.Point(10, 42);
            this.SyncCalendar_PeriodCalendarButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.SyncCalendar_PeriodCalendarButton.Name = "SyncCalendar_PeriodCalendarButton";
            this.SyncCalendar_PeriodCalendarButton.Size = new System.Drawing.Size(107, 19);
            this.SyncCalendar_PeriodCalendarButton.TabIndex = 1;
            this.SyncCalendar_PeriodCalendarButton.TabStop = true;
            this.SyncCalendar_PeriodCalendarButton.Text = "Sync the period";
            this.SyncCalendar_PeriodCalendarButton.UseVisualStyleBackColor = true;
            // 
            // SyncCalendar_EntireCalendarButton
            // 
            this.SyncCalendar_EntireCalendarButton.AutoSize = true;
            this.SyncCalendar_EntireCalendarButton.Checked = true;
            this.SyncCalendar_EntireCalendarButton.Location = new System.Drawing.Point(10, 20);
            this.SyncCalendar_EntireCalendarButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.SyncCalendar_EntireCalendarButton.Name = "SyncCalendar_EntireCalendarButton";
            this.SyncCalendar_EntireCalendarButton.Size = new System.Drawing.Size(151, 19);
            this.SyncCalendar_EntireCalendarButton.TabIndex = 0;
            this.SyncCalendar_EntireCalendarButton.TabStop = true;
            this.SyncCalendar_EntireCalendarButton.Text = "Sync the entire calendar";
            this.SyncCalendar_EntireCalendarButton.UseVisualStyleBackColor = true;
            // 
            // SyncCalendar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(352, 344);
            this.Controls.Add(this.SyncCalendar_SettingsBox);
            this.Controls.Add(this.SyncCalendar_CancelButton);
            this.Controls.Add(this.SyncCalendar_DestinationBox);
            this.Controls.Add(this.SyncCalendar_SyncButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SyncCalendar";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Sync";
            this.SyncCalendar_DestinationBox.ResumeLayout(false);
            this.SyncCalendar_DestinationBox.PerformLayout();
            this.SyncCalendar_SettingsBox.ResumeLayout(false);
            this.SyncCalendar_SettingsBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button SyncCalendar_SyncButton;
        private System.Windows.Forms.RadioButton SyncCalendar_UseExistingButton;
        private System.Windows.Forms.GroupBox SyncCalendar_DestinationBox;
        private System.Windows.Forms.RadioButton SyncCalendar_NewCalendarButton;
        private System.Windows.Forms.Label SyncCalendar_PasswordLabel;
        private System.Windows.Forms.Label SyncCalendar_UsernameLabel;
        private System.Windows.Forms.Label SyncCalendar_CalDAVLabel;
        private System.Windows.Forms.Button SyncCalendar_CancelButton;
        private System.Windows.Forms.TextBox SyncCalendar_PasswordBox;
        private System.Windows.Forms.TextBox SyncCalendar_UsernameBox;
        private System.Windows.Forms.TextBox SyncCalendar_CalDAVBox;
        private System.Windows.Forms.GroupBox SyncCalendar_SettingsBox;
        private System.Windows.Forms.RadioButton SyncCalendar_PeriodCalendarButton;
        private System.Windows.Forms.RadioButton SyncCalendar_EntireCalendarButton;
        private System.Windows.Forms.DateTimePicker SyncCalendar_bTimePicker;
        private System.Windows.Forms.DateTimePicker SyncCalendar_aTimePicker;
        private System.Windows.Forms.RadioButton SyncCalendar_CustomCalendarButton;
    }
}