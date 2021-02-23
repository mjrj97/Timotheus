
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
            this.SyncDestinationBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // SyncButton
            // 
            this.SyncButton.Location = new System.Drawing.Point(184, 177);
            this.SyncButton.Name = "SyncButton";
            this.SyncButton.Size = new System.Drawing.Size(75, 23);
            this.SyncButton.TabIndex = 0;
            this.SyncButton.Text = "Sync";
            this.SyncButton.UseVisualStyleBackColor = true;
            this.SyncButton.Click += new System.EventHandler(this.Sync);
            // 
            // UseExistingButton
            // 
            this.UseExistingButton.AutoSize = true;
            this.UseExistingButton.Enabled = false;
            this.UseExistingButton.Location = new System.Drawing.Point(10, 20);
            this.UseExistingButton.Name = "UseExistingButton";
            this.UseExistingButton.Size = new System.Drawing.Size(113, 19);
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
            this.SyncDestinationBox.Location = new System.Drawing.Point(10, 5);
            this.SyncDestinationBox.Name = "SyncDestinationBox";
            this.SyncDestinationBox.Size = new System.Drawing.Size(330, 166);
            this.SyncDestinationBox.TabIndex = 2;
            this.SyncDestinationBox.TabStop = false;
            this.SyncDestinationBox.Text = "Destination";
            // 
            // PasswordBox
            // 
            this.PasswordBox.Enabled = false;
            this.PasswordBox.Location = new System.Drawing.Point(90, 130);
            this.PasswordBox.Name = "PasswordBox";
            this.PasswordBox.Size = new System.Drawing.Size(230, 23);
            this.PasswordBox.TabIndex = 8;
            // 
            // UsernameBox
            // 
            this.UsernameBox.Enabled = false;
            this.UsernameBox.Location = new System.Drawing.Point(90, 101);
            this.UsernameBox.Name = "UsernameBox";
            this.UsernameBox.Size = new System.Drawing.Size(230, 23);
            this.UsernameBox.TabIndex = 7;
            // 
            // CalDAVBox
            // 
            this.CalDAVBox.Enabled = false;
            this.CalDAVBox.Location = new System.Drawing.Point(90, 72);
            this.CalDAVBox.Name = "CalDAVBox";
            this.CalDAVBox.Size = new System.Drawing.Size(230, 23);
            this.CalDAVBox.TabIndex = 6;
            // 
            // PasswordLabel
            // 
            this.PasswordLabel.AutoSize = true;
            this.PasswordLabel.Enabled = false;
            this.PasswordLabel.Location = new System.Drawing.Point(10, 133);
            this.PasswordLabel.Name = "PasswordLabel";
            this.PasswordLabel.Size = new System.Drawing.Size(57, 15);
            this.PasswordLabel.TabIndex = 5;
            this.PasswordLabel.Text = "Password";
            // 
            // UsernameLabel
            // 
            this.UsernameLabel.AutoSize = true;
            this.UsernameLabel.Enabled = false;
            this.UsernameLabel.Location = new System.Drawing.Point(10, 104);
            this.UsernameLabel.Name = "UsernameLabel";
            this.UsernameLabel.Size = new System.Drawing.Size(60, 15);
            this.UsernameLabel.TabIndex = 4;
            this.UsernameLabel.Text = "Username";
            // 
            // CalDAVLabel
            // 
            this.CalDAVLabel.AutoSize = true;
            this.CalDAVLabel.Enabled = false;
            this.CalDAVLabel.Location = new System.Drawing.Point(10, 75);
            this.CalDAVLabel.Name = "CalDAVLabel";
            this.CalDAVLabel.Size = new System.Drawing.Size(46, 15);
            this.CalDAVLabel.TabIndex = 3;
            this.CalDAVLabel.Text = "CalDAV";
            // 
            // NewCalendarButton
            // 
            this.NewCalendarButton.AutoSize = true;
            this.NewCalendarButton.Location = new System.Drawing.Point(10, 45);
            this.NewCalendarButton.Name = "NewCalendarButton";
            this.NewCalendarButton.Size = new System.Drawing.Size(116, 19);
            this.NewCalendarButton.TabIndex = 2;
            this.NewCalendarButton.TabStop = true;
            this.NewCalendarButton.Text = "Another calendar";
            this.NewCalendarButton.UseVisualStyleBackColor = true;
            this.NewCalendarButton.CheckedChanged += new System.EventHandler(this.NewCalendarButton_CheckedChanged);
            // 
            // CloseButton
            // 
            this.CloseButton.Location = new System.Drawing.Point(265, 177);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(75, 23);
            this.CloseButton.TabIndex = 3;
            this.CloseButton.Text = "Close";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.Close);
            // 
            // SyncCalendar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(352, 209);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.SyncDestinationBox);
            this.Controls.Add(this.SyncButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SyncCalendar";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Sync";
            this.SyncDestinationBox.ResumeLayout(false);
            this.SyncDestinationBox.PerformLayout();
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
    }
}