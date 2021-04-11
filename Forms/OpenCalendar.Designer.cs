
namespace Timotheus.Forms

{
    partial class OpenCalendar
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OpenCalendar));
            this.OpenCalendar_CancelButton = new System.Windows.Forms.Button();
            this.OpenCalendar_ICSButton = new System.Windows.Forms.RadioButton();
            this.OpenCalendar_CalDAVButton = new System.Windows.Forms.RadioButton();
            this.OpenCalendar_ICSBox = new System.Windows.Forms.TextBox();
            this.OpenCalendar_BrowseButton = new System.Windows.Forms.Button();
            this.OpenCalendar_CalDAVBox = new System.Windows.Forms.TextBox();
            this.OpenCalendar_OpenButton = new System.Windows.Forms.Button();
            this.OpenCalendar_UsernameBox = new System.Windows.Forms.TextBox();
            this.OpenCalendar_UsernameLabel = new System.Windows.Forms.Label();
            this.OpenCalendar_PasswordBox = new System.Windows.Forms.TextBox();
            this.OpenCalendar_PasswordLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // OpenCalendar_CancelButton
            // 
            this.OpenCalendar_CancelButton.Location = new System.Drawing.Point(310, 160);
            this.OpenCalendar_CancelButton.Name = "OpenCalendar_CancelButton";
            this.OpenCalendar_CancelButton.Size = new System.Drawing.Size(75, 23);
            this.OpenCalendar_CancelButton.TabIndex = 0;
            this.OpenCalendar_CancelButton.Text = "Cancel";
            this.OpenCalendar_CancelButton.UseVisualStyleBackColor = true;
            this.OpenCalendar_CancelButton.Click += new System.EventHandler(this.Close);
            // 
            // OpenCalendar_ICSButton
            // 
            this.OpenCalendar_ICSButton.AutoSize = true;
            this.OpenCalendar_ICSButton.Checked = true;
            this.OpenCalendar_ICSButton.Location = new System.Drawing.Point(20, 20);
            this.OpenCalendar_ICSButton.Name = "OpenCalendar_ICSButton";
            this.OpenCalendar_ICSButton.Size = new System.Drawing.Size(42, 19);
            this.OpenCalendar_ICSButton.TabIndex = 1;
            this.OpenCalendar_ICSButton.TabStop = true;
            this.OpenCalendar_ICSButton.Text = ".ics";
            this.OpenCalendar_ICSButton.UseVisualStyleBackColor = true;
            // 
            // OpenCalendar_CalDAVButton
            // 
            this.OpenCalendar_CalDAVButton.AutoSize = true;
            this.OpenCalendar_CalDAVButton.Location = new System.Drawing.Point(20, 55);
            this.OpenCalendar_CalDAVButton.Name = "OpenCalendar_CalDAVButton";
            this.OpenCalendar_CalDAVButton.Size = new System.Drawing.Size(64, 19);
            this.OpenCalendar_CalDAVButton.TabIndex = 2;
            this.OpenCalendar_CalDAVButton.Text = "CalDAV";
            this.OpenCalendar_CalDAVButton.UseVisualStyleBackColor = true;
            this.OpenCalendar_CalDAVButton.CheckedChanged += new System.EventHandler(this.CalDAVButton_CheckedChanged);
            // 
            // OpenCalendar_ICSBox
            // 
            this.OpenCalendar_ICSBox.Location = new System.Drawing.Point(100, 20);
            this.OpenCalendar_ICSBox.Name = "OpenCalendar_ICSBox";
            this.OpenCalendar_ICSBox.Size = new System.Drawing.Size(200, 23);
            this.OpenCalendar_ICSBox.TabIndex = 3;
            // 
            // OpenCalendar_BrowseButton
            // 
            this.OpenCalendar_BrowseButton.Location = new System.Drawing.Point(310, 19);
            this.OpenCalendar_BrowseButton.Name = "OpenCalendar_BrowseButton";
            this.OpenCalendar_BrowseButton.Size = new System.Drawing.Size(75, 23);
            this.OpenCalendar_BrowseButton.TabIndex = 4;
            this.OpenCalendar_BrowseButton.Text = "Browse";
            this.OpenCalendar_BrowseButton.UseVisualStyleBackColor = true;
            this.OpenCalendar_BrowseButton.Click += new System.EventHandler(this.BrowseLocalDirectories);
            // 
            // OpenCalendar_CalDAVBox
            // 
            this.OpenCalendar_CalDAVBox.Enabled = false;
            this.OpenCalendar_CalDAVBox.Location = new System.Drawing.Point(100, 55);
            this.OpenCalendar_CalDAVBox.Name = "OpenCalendar_CalDAVBox";
            this.OpenCalendar_CalDAVBox.Size = new System.Drawing.Size(285, 23);
            this.OpenCalendar_CalDAVBox.TabIndex = 5;
            // 
            // OpenCalendar_OpenButton
            // 
            this.OpenCalendar_OpenButton.Location = new System.Drawing.Point(225, 160);
            this.OpenCalendar_OpenButton.Name = "OpenCalendar_OpenButton";
            this.OpenCalendar_OpenButton.Size = new System.Drawing.Size(75, 23);
            this.OpenCalendar_OpenButton.TabIndex = 6;
            this.OpenCalendar_OpenButton.Text = "Open";
            this.OpenCalendar_OpenButton.UseVisualStyleBackColor = true;
            this.OpenCalendar_OpenButton.Click += new System.EventHandler(this.LoadCalendar);
            // 
            // OpenCalendar_UsernameBox
            // 
            this.OpenCalendar_UsernameBox.Enabled = false;
            this.OpenCalendar_UsernameBox.Location = new System.Drawing.Point(100, 90);
            this.OpenCalendar_UsernameBox.Name = "OpenCalendar_UsernameBox";
            this.OpenCalendar_UsernameBox.Size = new System.Drawing.Size(285, 23);
            this.OpenCalendar_UsernameBox.TabIndex = 7;
            // 
            // OpenCalendar_UsernameLabel
            // 
            this.OpenCalendar_UsernameLabel.AutoSize = true;
            this.OpenCalendar_UsernameLabel.Enabled = false;
            this.OpenCalendar_UsernameLabel.Location = new System.Drawing.Point(20, 92);
            this.OpenCalendar_UsernameLabel.Name = "OpenCalendar_UsernameLabel";
            this.OpenCalendar_UsernameLabel.Size = new System.Drawing.Size(60, 15);
            this.OpenCalendar_UsernameLabel.TabIndex = 8;
            this.OpenCalendar_UsernameLabel.Text = "Username";
            // 
            // OpenCalendar_PasswordBox
            // 
            this.OpenCalendar_PasswordBox.Enabled = false;
            this.OpenCalendar_PasswordBox.Location = new System.Drawing.Point(100, 125);
            this.OpenCalendar_PasswordBox.Name = "OpenCalendar_PasswordBox";
            this.OpenCalendar_PasswordBox.Size = new System.Drawing.Size(285, 23);
            this.OpenCalendar_PasswordBox.TabIndex = 9;
            // 
            // OpenCalendar_PasswordLabel
            // 
            this.OpenCalendar_PasswordLabel.AutoSize = true;
            this.OpenCalendar_PasswordLabel.Enabled = false;
            this.OpenCalendar_PasswordLabel.Location = new System.Drawing.Point(20, 128);
            this.OpenCalendar_PasswordLabel.Name = "OpenCalendar_PasswordLabel";
            this.OpenCalendar_PasswordLabel.Size = new System.Drawing.Size(57, 15);
            this.OpenCalendar_PasswordLabel.TabIndex = 10;
            this.OpenCalendar_PasswordLabel.Text = "Password";
            // 
            // OpenCalendar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 201);
            this.Controls.Add(this.OpenCalendar_PasswordLabel);
            this.Controls.Add(this.OpenCalendar_PasswordBox);
            this.Controls.Add(this.OpenCalendar_UsernameLabel);
            this.Controls.Add(this.OpenCalendar_UsernameBox);
            this.Controls.Add(this.OpenCalendar_OpenButton);
            this.Controls.Add(this.OpenCalendar_CalDAVBox);
            this.Controls.Add(this.OpenCalendar_BrowseButton);
            this.Controls.Add(this.OpenCalendar_ICSBox);
            this.Controls.Add(this.OpenCalendar_CalDAVButton);
            this.Controls.Add(this.OpenCalendar_ICSButton);
            this.Controls.Add(this.OpenCalendar_CancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OpenCalendar";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Open";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OpenCalendar_CancelButton;
        private System.Windows.Forms.RadioButton OpenCalendar_ICSButton;
        private System.Windows.Forms.RadioButton OpenCalendar_CalDAVButton;
        private System.Windows.Forms.TextBox OpenCalendar_ICSBox;
        private System.Windows.Forms.Button OpenCalendar_BrowseButton;
        private System.Windows.Forms.TextBox OpenCalendar_CalDAVBox;
        private System.Windows.Forms.Button OpenCalendar_OpenButton;
        private System.Windows.Forms.TextBox OpenCalendar_UsernameBox;
        private System.Windows.Forms.Label OpenCalendar_UsernameLabel;
        private System.Windows.Forms.TextBox OpenCalendar_PasswordBox;
        private System.Windows.Forms.Label OpenCalendar_PasswordLabel;
    }
}