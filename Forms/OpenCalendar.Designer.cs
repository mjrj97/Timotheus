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
            this.CloseButton = new System.Windows.Forms.Button();
            this.ICSButton = new System.Windows.Forms.RadioButton();
            this.CalDAVButton = new System.Windows.Forms.RadioButton();
            this.ICSText = new System.Windows.Forms.TextBox();
            this.BrowseButton = new System.Windows.Forms.Button();
            this.CalDAVText = new System.Windows.Forms.TextBox();
            this.OpenButton = new System.Windows.Forms.Button();
            this.UsernameText = new System.Windows.Forms.TextBox();
            this.UsernameLabel = new System.Windows.Forms.Label();
            this.PasswordText = new System.Windows.Forms.TextBox();
            this.PasswordLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // CloseButton
            // 
            this.CloseButton.Location = new System.Drawing.Point(310, 160);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(75, 23);
            this.CloseButton.TabIndex = 0;
            this.CloseButton.Text = "Close";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseDialog);
            // 
            // ICSButton
            // 
            this.ICSButton.AutoSize = true;
            this.ICSButton.Checked = true;
            this.ICSButton.Location = new System.Drawing.Point(20, 20);
            this.ICSButton.Name = "ICSButton";
            this.ICSButton.Size = new System.Drawing.Size(42, 19);
            this.ICSButton.TabIndex = 1;
            this.ICSButton.TabStop = true;
            this.ICSButton.Text = ".ics";
            this.ICSButton.UseVisualStyleBackColor = true;
            // 
            // CalDAVButton
            // 
            this.CalDAVButton.AutoSize = true;
            this.CalDAVButton.Location = new System.Drawing.Point(20, 55);
            this.CalDAVButton.Name = "CalDAVButton";
            this.CalDAVButton.Size = new System.Drawing.Size(64, 19);
            this.CalDAVButton.TabIndex = 2;
            this.CalDAVButton.Text = "CalDAV";
            this.CalDAVButton.UseVisualStyleBackColor = true;
            this.CalDAVButton.CheckedChanged += new System.EventHandler(this.CalDAVButton_CheckedChanged);
            // 
            // ICSText
            // 
            this.ICSText.Location = new System.Drawing.Point(100, 20);
            this.ICSText.Name = "ICSText";
            this.ICSText.Size = new System.Drawing.Size(200, 23);
            this.ICSText.TabIndex = 3;
            // 
            // BrowseButton
            // 
            this.BrowseButton.Location = new System.Drawing.Point(310, 20);
            this.BrowseButton.Name = "BrowseButton";
            this.BrowseButton.Size = new System.Drawing.Size(75, 23);
            this.BrowseButton.TabIndex = 4;
            this.BrowseButton.Text = "Browse";
            this.BrowseButton.UseVisualStyleBackColor = true;
            this.BrowseButton.Click += new System.EventHandler(this.BrowseLocalDirectories);
            // 
            // CalDAVText
            // 
            this.CalDAVText.Enabled = false;
            this.CalDAVText.Location = new System.Drawing.Point(100, 55);
            this.CalDAVText.Name = "CalDAVText";
            this.CalDAVText.Size = new System.Drawing.Size(285, 23);
            this.CalDAVText.TabIndex = 5;
            // 
            // OpenButton
            // 
            this.OpenButton.Location = new System.Drawing.Point(225, 160);
            this.OpenButton.Name = "OpenButton";
            this.OpenButton.Size = new System.Drawing.Size(75, 23);
            this.OpenButton.TabIndex = 6;
            this.OpenButton.Text = "Open";
            this.OpenButton.UseVisualStyleBackColor = true;
            this.OpenButton.Click += new System.EventHandler(this.LoadCalendar);
            // 
            // UsernameText
            // 
            this.UsernameText.Enabled = false;
            this.UsernameText.Location = new System.Drawing.Point(100, 90);
            this.UsernameText.Name = "UsernameText";
            this.UsernameText.Size = new System.Drawing.Size(285, 23);
            this.UsernameText.TabIndex = 7;
            // 
            // UsernameLabel
            // 
            this.UsernameLabel.AutoSize = true;
            this.UsernameLabel.Enabled = false;
            this.UsernameLabel.Location = new System.Drawing.Point(20, 90);
            this.UsernameLabel.Name = "UsernameLabel";
            this.UsernameLabel.Size = new System.Drawing.Size(60, 15);
            this.UsernameLabel.TabIndex = 8;
            this.UsernameLabel.Text = "Username";
            // 
            // PasswordText
            // 
            this.PasswordText.Enabled = false;
            this.PasswordText.Location = new System.Drawing.Point(100, 125);
            this.PasswordText.Name = "PasswordText";
            this.PasswordText.Size = new System.Drawing.Size(285, 23);
            this.PasswordText.TabIndex = 9;
            // 
            // PasswordLabel
            // 
            this.PasswordLabel.AutoSize = true;
            this.PasswordLabel.Enabled = false;
            this.PasswordLabel.Location = new System.Drawing.Point(20, 125);
            this.PasswordLabel.Name = "PasswordLabel";
            this.PasswordLabel.Size = new System.Drawing.Size(57, 15);
            this.PasswordLabel.TabIndex = 10;
            this.PasswordLabel.Text = "Password";
            // 
            // OpenCalendar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 201);
            this.Controls.Add(this.PasswordLabel);
            this.Controls.Add(this.PasswordText);
            this.Controls.Add(this.UsernameLabel);
            this.Controls.Add(this.UsernameText);
            this.Controls.Add(this.OpenButton);
            this.Controls.Add(this.CalDAVText);
            this.Controls.Add(this.BrowseButton);
            this.Controls.Add(this.ICSText);
            this.Controls.Add(this.CalDAVButton);
            this.Controls.Add(this.ICSButton);
            this.Controls.Add(this.CloseButton);
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

        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.RadioButton ICSButton;
        private System.Windows.Forms.RadioButton CalDAVButton;
        private System.Windows.Forms.TextBox ICSText;
        private System.Windows.Forms.Button BrowseButton;
        private System.Windows.Forms.TextBox CalDAVText;
        private System.Windows.Forms.Button OpenButton;
        private System.Windows.Forms.TextBox UsernameText;
        private System.Windows.Forms.Label UsernameLabel;
        private System.Windows.Forms.TextBox PasswordText;
        private System.Windows.Forms.Label PasswordLabel;
    }
}