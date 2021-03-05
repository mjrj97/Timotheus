
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
            this.SyncButton = new System.Windows.Forms.Button();
            this.UseExistingButton = new System.Windows.Forms.RadioButton();
            this.SyncDestinationBox = new System.Windows.Forms.GroupBox();
            this.PasswordBox = new System.Windows.Forms.TextBox();
            this.UsernameBox = new System.Windows.Forms.TextBox();
            this.CalDAVBox = new System.Windows.Forms.TextBox();
            this.PasswordLabel = new System.Windows.Forms.Label();
            this.UsernameLabel = new System.Windows.Forms.Label();
            this.CalDAVLabel = new System.Windows.Forms.Label();
            this.NewCalendarButton = new System.Windows.Forms.RadioButton();
            this.CloseButton = new System.Windows.Forms.Button();
            this.SyncSettingsBox = new System.Windows.Forms.GroupBox();
            this.bTimePicker = new System.Windows.Forms.DateTimePicker();
            this.aTimePicker = new System.Windows.Forms.DateTimePicker();
            this.CustomCalendarButton = new System.Windows.Forms.RadioButton();
            this.PeriodCalendarButton = new System.Windows.Forms.RadioButton();
            this.EntireCalendarButton = new System.Windows.Forms.RadioButton();
            this.SyncDestinationBox.SuspendLayout();
            this.SyncSettingsBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // SyncButton
            // 
            this.SyncButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SyncButton.Location = new System.Drawing.Point(212, 414);
            this.SyncButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SyncButton.Name = "SyncButton";
            this.SyncButton.Size = new System.Drawing.Size(86, 31);
            this.SyncButton.TabIndex = 0;
            this.SyncButton.Text = "Sync";
            this.SyncButton.UseVisualStyleBackColor = true;
            this.SyncButton.Click += new System.EventHandler(this.Sync);
            // 
            // UseExistingButton
            // 
            this.UseExistingButton.AutoSize = true;
            this.UseExistingButton.Enabled = false;
            this.UseExistingButton.Location = new System.Drawing.Point(11, 27);
            this.UseExistingButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.UseExistingButton.Name = "UseExistingButton";
            this.UseExistingButton.Size = new System.Drawing.Size(139, 24);
            this.UseExistingButton.TabIndex = 1;
            this.UseExistingButton.TabStop = true;
            this.UseExistingButton.Text = "Current calendar";
            this.UseExistingButton.UseVisualStyleBackColor = true;
            // 
            // SyncDestinationBox
            // 
            this.SyncDestinationBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SyncDestinationBox.Controls.Add(this.PasswordBox);
            this.SyncDestinationBox.Controls.Add(this.UsernameBox);
            this.SyncDestinationBox.Controls.Add(this.CalDAVBox);
            this.SyncDestinationBox.Controls.Add(this.PasswordLabel);
            this.SyncDestinationBox.Controls.Add(this.UsernameLabel);
            this.SyncDestinationBox.Controls.Add(this.CalDAVLabel);
            this.SyncDestinationBox.Controls.Add(this.NewCalendarButton);
            this.SyncDestinationBox.Controls.Add(this.UseExistingButton);
            this.SyncDestinationBox.Location = new System.Drawing.Point(11, 7);
            this.SyncDestinationBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SyncDestinationBox.Name = "SyncDestinationBox";
            this.SyncDestinationBox.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SyncDestinationBox.Size = new System.Drawing.Size(377, 221);
            this.SyncDestinationBox.TabIndex = 2;
            this.SyncDestinationBox.TabStop = false;
            this.SyncDestinationBox.Text = "Destination";
            // 
            // PasswordBox
            // 
            this.PasswordBox.Enabled = false;
            this.PasswordBox.Location = new System.Drawing.Point(103, 173);
            this.PasswordBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.PasswordBox.Name = "PasswordBox";
            this.PasswordBox.Size = new System.Drawing.Size(262, 27);
            this.PasswordBox.TabIndex = 8;
            // 
            // UsernameBox
            // 
            this.UsernameBox.Enabled = false;
            this.UsernameBox.Location = new System.Drawing.Point(103, 135);
            this.UsernameBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.UsernameBox.Name = "UsernameBox";
            this.UsernameBox.Size = new System.Drawing.Size(262, 27);
            this.UsernameBox.TabIndex = 7;
            // 
            // CalDAVBox
            // 
            this.CalDAVBox.Enabled = false;
            this.CalDAVBox.Location = new System.Drawing.Point(103, 96);
            this.CalDAVBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.CalDAVBox.Name = "CalDAVBox";
            this.CalDAVBox.Size = new System.Drawing.Size(262, 27);
            this.CalDAVBox.TabIndex = 6;
            // 
            // PasswordLabel
            // 
            this.PasswordLabel.AutoSize = true;
            this.PasswordLabel.Enabled = false;
            this.PasswordLabel.Location = new System.Drawing.Point(11, 177);
            this.PasswordLabel.Name = "PasswordLabel";
            this.PasswordLabel.Size = new System.Drawing.Size(70, 20);
            this.PasswordLabel.TabIndex = 5;
            this.PasswordLabel.Text = "Password";
            // 
            // UsernameLabel
            // 
            this.UsernameLabel.AutoSize = true;
            this.UsernameLabel.Enabled = false;
            this.UsernameLabel.Location = new System.Drawing.Point(11, 139);
            this.UsernameLabel.Name = "UsernameLabel";
            this.UsernameLabel.Size = new System.Drawing.Size(75, 20);
            this.UsernameLabel.TabIndex = 4;
            this.UsernameLabel.Text = "Username";
            // 
            // CalDAVLabel
            // 
            this.CalDAVLabel.AutoSize = true;
            this.CalDAVLabel.Enabled = false;
            this.CalDAVLabel.Location = new System.Drawing.Point(11, 100);
            this.CalDAVLabel.Name = "CalDAVLabel";
            this.CalDAVLabel.Size = new System.Drawing.Size(59, 20);
            this.CalDAVLabel.TabIndex = 3;
            this.CalDAVLabel.Text = "CalDAV";
            // 
            // NewCalendarButton
            // 
            this.NewCalendarButton.AutoSize = true;
            this.NewCalendarButton.Location = new System.Drawing.Point(11, 60);
            this.NewCalendarButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.NewCalendarButton.Name = "NewCalendarButton";
            this.NewCalendarButton.Size = new System.Drawing.Size(144, 24);
            this.NewCalendarButton.TabIndex = 2;
            this.NewCalendarButton.TabStop = true;
            this.NewCalendarButton.Text = "Another calendar";
            this.NewCalendarButton.UseVisualStyleBackColor = true;
            this.NewCalendarButton.CheckedChanged += new System.EventHandler(this.NewCalendarButton_CheckedChanged);
            // 
            // CloseButton
            // 
            this.CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseButton.Location = new System.Drawing.Point(304, 414);
            this.CloseButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(86, 31);
            this.CloseButton.TabIndex = 3;
            this.CloseButton.Text = "Close";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.Close);
            // 
            // SyncSettingsBox
            // 
            this.SyncSettingsBox.Controls.Add(this.bTimePicker);
            this.SyncSettingsBox.Controls.Add(this.aTimePicker);
            this.SyncSettingsBox.Controls.Add(this.CustomCalendarButton);
            this.SyncSettingsBox.Controls.Add(this.PeriodCalendarButton);
            this.SyncSettingsBox.Controls.Add(this.EntireCalendarButton);
            this.SyncSettingsBox.Location = new System.Drawing.Point(11, 235);
            this.SyncSettingsBox.Name = "SyncSettingsBox";
            this.SyncSettingsBox.Size = new System.Drawing.Size(377, 168);
            this.SyncSettingsBox.TabIndex = 4;
            this.SyncSettingsBox.TabStop = false;
            this.SyncSettingsBox.Text = "Settings";
            // 
            // bTimePicker
            // 
            this.bTimePicker.Enabled = false;
            this.bTimePicker.Location = new System.Drawing.Point(195, 126);
            this.bTimePicker.Name = "bTimePicker";
            this.bTimePicker.Size = new System.Drawing.Size(170, 27);
            this.bTimePicker.TabIndex = 4;
            // 
            // aTimePicker
            // 
            this.aTimePicker.Enabled = false;
            this.aTimePicker.Location = new System.Drawing.Point(11, 126);
            this.aTimePicker.Name = "aTimePicker";
            this.aTimePicker.Size = new System.Drawing.Size(170, 27);
            this.aTimePicker.TabIndex = 3;
            // 
            // CustomCalendarButton
            // 
            this.CustomCalendarButton.AutoSize = true;
            this.CustomCalendarButton.Location = new System.Drawing.Point(11, 86);
            this.CustomCalendarButton.Name = "CustomCalendarButton";
            this.CustomCalendarButton.Size = new System.Drawing.Size(160, 24);
            this.CustomCalendarButton.TabIndex = 2;
            this.CustomCalendarButton.TabStop = true;
            this.CustomCalendarButton.Text = "Sync custom period";
            this.CustomCalendarButton.UseVisualStyleBackColor = true;
            this.CustomCalendarButton.CheckedChanged += new System.EventHandler(this.CustomCalendarButton_CheckedChanged);
            // 
            // PeriodCalendarButton
            // 
            this.PeriodCalendarButton.AutoSize = true;
            this.PeriodCalendarButton.Location = new System.Drawing.Point(11, 56);
            this.PeriodCalendarButton.Name = "PeriodCalendarButton";
            this.PeriodCalendarButton.Size = new System.Drawing.Size(133, 24);
            this.PeriodCalendarButton.TabIndex = 1;
            this.PeriodCalendarButton.TabStop = true;
            this.PeriodCalendarButton.Text = "Sync the period";
            this.PeriodCalendarButton.UseVisualStyleBackColor = true;
            // 
            // EntireCalendarButton
            // 
            this.EntireCalendarButton.AutoSize = true;
            this.EntireCalendarButton.Checked = true;
            this.EntireCalendarButton.Location = new System.Drawing.Point(11, 26);
            this.EntireCalendarButton.Name = "EntireCalendarButton";
            this.EntireCalendarButton.Size = new System.Drawing.Size(188, 24);
            this.EntireCalendarButton.TabIndex = 0;
            this.EntireCalendarButton.TabStop = true;
            this.EntireCalendarButton.Text = "Sync the entire calendar";
            this.EntireCalendarButton.UseVisualStyleBackColor = true;
            // 
            // SyncCalendar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(402, 458);
            this.Controls.Add(this.SyncSettingsBox);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.SyncDestinationBox);
            this.Controls.Add(this.SyncButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SyncCalendar";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Sync";
            this.SyncDestinationBox.ResumeLayout(false);
            this.SyncDestinationBox.PerformLayout();
            this.SyncSettingsBox.ResumeLayout(false);
            this.SyncSettingsBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button SyncButton;
        private System.Windows.Forms.RadioButton UseExistingButton;
        private System.Windows.Forms.GroupBox SyncDestinationBox;
        private System.Windows.Forms.RadioButton NewCalendarButton;
        private System.Windows.Forms.Label PasswordLabel;
        private System.Windows.Forms.Label UsernameLabel;
        private System.Windows.Forms.Label CalDAVLabel;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.TextBox PasswordBox;
        private System.Windows.Forms.TextBox UsernameBox;
        private System.Windows.Forms.TextBox CalDAVBox;
        private System.Windows.Forms.GroupBox SyncSettingsBox;
        private System.Windows.Forms.RadioButton PeriodCalendarButton;
        private System.Windows.Forms.RadioButton EntireCalendarButton;
        private System.Windows.Forms.DateTimePicker bTimePicker;
        private System.Windows.Forms.DateTimePicker aTimePicker;
        private System.Windows.Forms.RadioButton CustomCalendarButton;
    }
}