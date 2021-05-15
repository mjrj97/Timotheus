
namespace Timotheus.Forms
{
    partial class Help
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Help));
            this.Help_PictureBox = new System.Windows.Forms.PictureBox();
            this.Help_VersionLabel = new System.Windows.Forms.Label();
            this.Help_ContributorsLabel = new System.Windows.Forms.Label();
            this.Help_LicenseLabel = new System.Windows.Forms.Label();
            this.Help_SourceLabel = new System.Windows.Forms.Label();
            this.Help_EmailLabel = new System.Windows.Forms.Label();
            this.Help_SourceLink = new System.Windows.Forms.LinkLabel();
            this.Help_EmailLink = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.Help_PictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // Help_PictureBox
            // 
            this.Help_PictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Help_PictureBox.Image = global::Timotheus.Properties.Resources.Icon;
            this.Help_PictureBox.InitialImage = null;
            this.Help_PictureBox.Location = new System.Drawing.Point(10, 10);
            this.Help_PictureBox.Name = "Help_PictureBox";
            this.Help_PictureBox.Size = new System.Drawing.Size(100, 100);
            this.Help_PictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Help_PictureBox.TabIndex = 0;
            this.Help_PictureBox.TabStop = false;
            // 
            // Help_VersionLabel
            // 
            this.Help_VersionLabel.AutoSize = true;
            this.Help_VersionLabel.Location = new System.Drawing.Point(10, 115);
            this.Help_VersionLabel.Name = "Help_VersionLabel";
            this.Help_VersionLabel.Size = new System.Drawing.Size(102, 15);
            this.Help_VersionLabel.TabIndex = 1;
            this.Help_VersionLabel.Text = "Timotheus v. 0.1.0";
            // 
            // Help_ContributorsLabel
            // 
            this.Help_ContributorsLabel.AutoSize = true;
            this.Help_ContributorsLabel.Location = new System.Drawing.Point(10, 145);
            this.Help_ContributorsLabel.Name = "Help_ContributorsLabel";
            this.Help_ContributorsLabel.Size = new System.Drawing.Size(103, 45);
            this.Help_ContributorsLabel.TabIndex = 2;
            this.Help_ContributorsLabel.Text = "Contributors:\r\nMartin J. R. Jensen\r\nJesper Roager";
            // 
            // Help_LicenseLabel
            // 
            this.Help_LicenseLabel.AutoSize = true;
            this.Help_LicenseLabel.Location = new System.Drawing.Point(10, 205);
            this.Help_LicenseLabel.Name = "Help_LicenseLabel";
            this.Help_LicenseLabel.Size = new System.Drawing.Size(112, 15);
            this.Help_LicenseLabel.TabIndex = 3;
            this.Help_LicenseLabel.Text = "License: Apache-2.0";
            // 
            // Help_SourceLabel
            // 
            this.Help_SourceLabel.AutoSize = true;
            this.Help_SourceLabel.Location = new System.Drawing.Point(10, 225);
            this.Help_SourceLabel.Name = "Help_SourceLabel";
            this.Help_SourceLabel.Size = new System.Drawing.Size(46, 15);
            this.Help_SourceLabel.TabIndex = 4;
            this.Help_SourceLabel.Text = "Source:";
            // 
            // Help_EmailLabel
            // 
            this.Help_EmailLabel.AutoSize = true;
            this.Help_EmailLabel.Location = new System.Drawing.Point(10, 245);
            this.Help_EmailLabel.Name = "Help_EmailLabel";
            this.Help_EmailLabel.Size = new System.Drawing.Size(39, 15);
            this.Help_EmailLabel.TabIndex = 5;
            this.Help_EmailLabel.Text = "Email:";
            // 
            // Help_SourceLink
            // 
            this.Help_SourceLink.AutoSize = true;
            this.Help_SourceLink.Location = new System.Drawing.Point(55, 225);
            this.Help_SourceLink.Name = "Help_SourceLink";
            this.Help_SourceLink.Size = new System.Drawing.Size(199, 15);
            this.Help_SourceLink.TabIndex = 6;
            this.Help_SourceLink.TabStop = true;
            this.Help_SourceLink.Text = "https://github.com/mjrj97/Manager";
            this.Help_SourceLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.EmailLink_LinkClicked);
            // 
            // Help_EmailLink
            // 
            this.Help_EmailLink.AutoSize = true;
            this.Help_EmailLink.Location = new System.Drawing.Point(47, 245);
            this.Help_EmailLink.Name = "Help_EmailLink";
            this.Help_EmailLink.Size = new System.Drawing.Size(185, 15);
            this.Help_EmailLink.TabIndex = 7;
            this.Help_EmailLink.TabStop = true;
            this.Help_EmailLink.Text = "martin.jensen.1997@hotmail.com";
            this.Help_EmailLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.SourceLink_LinkClicked);
            // 
            // Help
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 271);
            this.Controls.Add(this.Help_EmailLink);
            this.Controls.Add(this.Help_SourceLink);
            this.Controls.Add(this.Help_EmailLabel);
            this.Controls.Add(this.Help_SourceLabel);
            this.Controls.Add(this.Help_LicenseLabel);
            this.Controls.Add(this.Help_ContributorsLabel);
            this.Controls.Add(this.Help_VersionLabel);
            this.Controls.Add(this.Help_PictureBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Help";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Help";
            ((System.ComponentModel.ISupportInitialize)(this.Help_PictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox Help_PictureBox;
        private System.Windows.Forms.Label Help_VersionLabel;
        private System.Windows.Forms.Label Help_ContributorsLabel;
        private System.Windows.Forms.Label Help_LicenseLabel;
        private System.Windows.Forms.Label Help_SourceLabel;
        private System.Windows.Forms.Label Help_EmailLabel;
        private System.Windows.Forms.LinkLabel Help_SourceLink;
        private System.Windows.Forms.LinkLabel Help_EmailLink;
    }
}