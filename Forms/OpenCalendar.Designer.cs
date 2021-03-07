
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
            this.Open_CancelButton = new System.Windows.Forms.Button();
            this.Open_ICSButton = new System.Windows.Forms.RadioButton();
            this.Open_CalDAVButton = new System.Windows.Forms.RadioButton();
            this.Open_ICSBox = new System.Windows.Forms.TextBox();
            this.Open_BrowseButton = new System.Windows.Forms.Button();
            this.Open_CalDAVBox = new System.Windows.Forms.TextBox();
            this.Open_OpenButton = new System.Windows.Forms.Button();
            this.Open_UsernameBox = new System.Windows.Forms.TextBox();
            this.Open_UsernameLabel = new System.Windows.Forms.Label();
            this.Open_PasswordBox = new System.Windows.Forms.TextBox();
            this.Open_PasswordLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Open_CancelButton
            // 
            this.Open_CancelButton.Location = new System.Drawing.Point(354, 213);
            this.Open_CancelButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Open_CancelButton.Name = "Open_CancelButton";
            this.Open_CancelButton.Size = new System.Drawing.Size(86, 31);
            this.Open_CancelButton.TabIndex = 0;
            this.Open_CancelButton.Text = "Cancel";
            this.Open_CancelButton.UseVisualStyleBackColor = true;
            this.Open_CancelButton.Click += new System.EventHandler(this.CloseDialog);
            // 
            // Open_ICSButton
            // 
            this.Open_ICSButton.AutoSize = true;
            this.Open_ICSButton.Checked = true;
            this.Open_ICSButton.Location = new System.Drawing.Point(23, 27);
            this.Open_ICSButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Open_ICSButton.Name = "Open_ICSButton";
            this.Open_ICSButton.Size = new System.Drawing.Size(50, 24);
            this.Open_ICSButton.TabIndex = 1;
            this.Open_ICSButton.TabStop = true;
            this.Open_ICSButton.Text = ".ics";
            this.Open_ICSButton.UseVisualStyleBackColor = true;
            // 
            // Open_CalDAVButton
            // 
            this.Open_CalDAVButton.AutoSize = true;
            this.Open_CalDAVButton.Location = new System.Drawing.Point(23, 73);
            this.Open_CalDAVButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Open_CalDAVButton.Name = "Open_CalDAVButton";
            this.Open_CalDAVButton.Size = new System.Drawing.Size(80, 24);
            this.Open_CalDAVButton.TabIndex = 2;
            this.Open_CalDAVButton.Text = "CalDAV";
            this.Open_CalDAVButton.UseVisualStyleBackColor = true;
            this.Open_CalDAVButton.CheckedChanged += new System.EventHandler(this.CalDAVButton_CheckedChanged);
            // 
            // Open_ICSBox
            // 
            this.Open_ICSBox.Location = new System.Drawing.Point(114, 27);
            this.Open_ICSBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Open_ICSBox.Name = "Open_ICSBox";
            this.Open_ICSBox.Size = new System.Drawing.Size(228, 27);
            this.Open_ICSBox.TabIndex = 3;
            // 
            // Open_BrowseButton
            // 
            this.Open_BrowseButton.Location = new System.Drawing.Point(354, 25);
            this.Open_BrowseButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Open_BrowseButton.Name = "Open_BrowseButton";
            this.Open_BrowseButton.Size = new System.Drawing.Size(86, 31);
            this.Open_BrowseButton.TabIndex = 4;
            this.Open_BrowseButton.Text = "Browse";
            this.Open_BrowseButton.UseVisualStyleBackColor = true;
            this.Open_BrowseButton.Click += new System.EventHandler(this.BrowseLocalDirectories);
            // 
            // Open_CalDAVBox
            // 
            this.Open_CalDAVBox.Enabled = false;
            this.Open_CalDAVBox.Location = new System.Drawing.Point(114, 73);
            this.Open_CalDAVBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Open_CalDAVBox.Name = "Open_CalDAVBox";
            this.Open_CalDAVBox.Size = new System.Drawing.Size(325, 27);
            this.Open_CalDAVBox.TabIndex = 5;
            // 
            // Open_OpenButton
            // 
            this.Open_OpenButton.Location = new System.Drawing.Point(257, 213);
            this.Open_OpenButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Open_OpenButton.Name = "Open_OpenButton";
            this.Open_OpenButton.Size = new System.Drawing.Size(86, 31);
            this.Open_OpenButton.TabIndex = 6;
            this.Open_OpenButton.Text = "Open";
            this.Open_OpenButton.UseVisualStyleBackColor = true;
            this.Open_OpenButton.Click += new System.EventHandler(this.LoadCalendar);
            // 
            // Open_UsernameBox
            // 
            this.Open_UsernameBox.Enabled = false;
            this.Open_UsernameBox.Location = new System.Drawing.Point(114, 120);
            this.Open_UsernameBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Open_UsernameBox.Name = "Open_UsernameBox";
            this.Open_UsernameBox.Size = new System.Drawing.Size(325, 27);
            this.Open_UsernameBox.TabIndex = 7;
            // 
            // Open_UsernameLabel
            // 
            this.Open_UsernameLabel.AutoSize = true;
            this.Open_UsernameLabel.Enabled = false;
            this.Open_UsernameLabel.Location = new System.Drawing.Point(23, 123);
            this.Open_UsernameLabel.Name = "Open_UsernameLabel";
            this.Open_UsernameLabel.Size = new System.Drawing.Size(75, 20);
            this.Open_UsernameLabel.TabIndex = 8;
            this.Open_UsernameLabel.Text = "Username";
            // 
            // Open_PasswordBox
            // 
            this.Open_PasswordBox.Enabled = false;
            this.Open_PasswordBox.Location = new System.Drawing.Point(114, 167);
            this.Open_PasswordBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Open_PasswordBox.Name = "Open_PasswordBox";
            this.Open_PasswordBox.Size = new System.Drawing.Size(325, 27);
            this.Open_PasswordBox.TabIndex = 9;
            // 
            // Open_PasswordLabel
            // 
            this.Open_PasswordLabel.AutoSize = true;
            this.Open_PasswordLabel.Enabled = false;
            this.Open_PasswordLabel.Location = new System.Drawing.Point(23, 170);
            this.Open_PasswordLabel.Name = "Open_PasswordLabel";
            this.Open_PasswordLabel.Size = new System.Drawing.Size(70, 20);
            this.Open_PasswordLabel.TabIndex = 10;
            this.Open_PasswordLabel.Text = "Password";
            // 
            // OpenCalendar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(462, 268);
            this.Controls.Add(this.Open_PasswordLabel);
            this.Controls.Add(this.Open_PasswordBox);
            this.Controls.Add(this.Open_UsernameLabel);
            this.Controls.Add(this.Open_UsernameBox);
            this.Controls.Add(this.Open_OpenButton);
            this.Controls.Add(this.Open_CalDAVBox);
            this.Controls.Add(this.Open_BrowseButton);
            this.Controls.Add(this.Open_ICSBox);
            this.Controls.Add(this.Open_CalDAVButton);
            this.Controls.Add(this.Open_ICSButton);
            this.Controls.Add(this.Open_CancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OpenCalendar";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Open";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Open_CancelButton;
        private System.Windows.Forms.RadioButton Open_ICSButton;
        private System.Windows.Forms.RadioButton Open_CalDAVButton;
        private System.Windows.Forms.TextBox Open_ICSBox;
        private System.Windows.Forms.Button Open_BrowseButton;
        private System.Windows.Forms.TextBox Open_CalDAVBox;
        private System.Windows.Forms.Button Open_OpenButton;
        private System.Windows.Forms.TextBox Open_UsernameBox;
        private System.Windows.Forms.Label Open_UsernameLabel;
        private System.Windows.Forms.TextBox Open_PasswordBox;
        private System.Windows.Forms.Label Open_PasswordLabel;
    }
}