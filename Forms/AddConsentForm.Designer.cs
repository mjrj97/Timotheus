
namespace Timotheus.Forms
{
    partial class AddConsentForm
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
            this.AddConsentForm_CancelButton = new System.Windows.Forms.Button();
            this.AddConsentForm_AddButton = new System.Windows.Forms.Button();
            this.AddConsentForm_NameBox = new System.Windows.Forms.TextBox();
            this.AddConsentForm_NameLabel = new System.Windows.Forms.Label();
            this.AddConsentForm_SignedDate = new System.Windows.Forms.DateTimePicker();
            this.AddConsentForm_VersionDate = new System.Windows.Forms.DateTimePicker();
            this.AddConsentForm_SignedLabel = new System.Windows.Forms.Label();
            this.AddConsentForm_VersionLabel = new System.Windows.Forms.Label();
            this.AddConsentForm_CommentBox = new System.Windows.Forms.RichTextBox();
            this.AddConsentForm_CommentLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // AddConsentForm_CancelButton
            // 
            this.AddConsentForm_CancelButton.Location = new System.Drawing.Point(182, 176);
            this.AddConsentForm_CancelButton.Name = "AddConsentForm_CancelButton";
            this.AddConsentForm_CancelButton.Size = new System.Drawing.Size(90, 23);
            this.AddConsentForm_CancelButton.TabIndex = 0;
            this.AddConsentForm_CancelButton.Text = "Cancel";
            this.AddConsentForm_CancelButton.UseVisualStyleBackColor = true;
            this.AddConsentForm_CancelButton.Click += new System.EventHandler(this.Cancel);
            // 
            // AddConsentForm_AddButton
            // 
            this.AddConsentForm_AddButton.Location = new System.Drawing.Point(88, 176);
            this.AddConsentForm_AddButton.Name = "AddConsentForm_AddButton";
            this.AddConsentForm_AddButton.Size = new System.Drawing.Size(90, 23);
            this.AddConsentForm_AddButton.TabIndex = 1;
            this.AddConsentForm_AddButton.Text = "Add";
            this.AddConsentForm_AddButton.UseVisualStyleBackColor = true;
            this.AddConsentForm_AddButton.Click += new System.EventHandler(this.Add);
            // 
            // AddConsentForm_NameBox
            // 
            this.AddConsentForm_NameBox.Location = new System.Drawing.Point(88, 12);
            this.AddConsentForm_NameBox.Name = "AddConsentForm_NameBox";
            this.AddConsentForm_NameBox.Size = new System.Drawing.Size(184, 23);
            this.AddConsentForm_NameBox.TabIndex = 2;
            // 
            // AddConsentForm_NameLabel
            // 
            this.AddConsentForm_NameLabel.AutoSize = true;
            this.AddConsentForm_NameLabel.Location = new System.Drawing.Point(12, 15);
            this.AddConsentForm_NameLabel.Name = "AddConsentForm_NameLabel";
            this.AddConsentForm_NameLabel.Size = new System.Drawing.Size(39, 15);
            this.AddConsentForm_NameLabel.TabIndex = 3;
            this.AddConsentForm_NameLabel.Text = "Name";
            // 
            // AddConsentForm_SignedDate
            // 
            this.AddConsentForm_SignedDate.Location = new System.Drawing.Point(88, 41);
            this.AddConsentForm_SignedDate.Name = "AddConsentForm_SignedDate";
            this.AddConsentForm_SignedDate.Size = new System.Drawing.Size(184, 23);
            this.AddConsentForm_SignedDate.TabIndex = 4;
            // 
            // AddConsentForm_VersionDate
            // 
            this.AddConsentForm_VersionDate.Location = new System.Drawing.Point(88, 70);
            this.AddConsentForm_VersionDate.Name = "AddConsentForm_VersionDate";
            this.AddConsentForm_VersionDate.Size = new System.Drawing.Size(184, 23);
            this.AddConsentForm_VersionDate.TabIndex = 5;
            // 
            // AddConsentForm_SignedLabel
            // 
            this.AddConsentForm_SignedLabel.AutoSize = true;
            this.AddConsentForm_SignedLabel.Location = new System.Drawing.Point(13, 47);
            this.AddConsentForm_SignedLabel.Name = "AddConsentForm_SignedLabel";
            this.AddConsentForm_SignedLabel.Size = new System.Drawing.Size(43, 15);
            this.AddConsentForm_SignedLabel.TabIndex = 6;
            this.AddConsentForm_SignedLabel.Text = "Signed";
            // 
            // AddConsentForm_VersionLabel
            // 
            this.AddConsentForm_VersionLabel.AutoSize = true;
            this.AddConsentForm_VersionLabel.Location = new System.Drawing.Point(13, 76);
            this.AddConsentForm_VersionLabel.Name = "AddConsentForm_VersionLabel";
            this.AddConsentForm_VersionLabel.Size = new System.Drawing.Size(45, 15);
            this.AddConsentForm_VersionLabel.TabIndex = 7;
            this.AddConsentForm_VersionLabel.Text = "Version";
            // 
            // AddConsentForm_CommentBox
            // 
            this.AddConsentForm_CommentBox.Location = new System.Drawing.Point(88, 99);
            this.AddConsentForm_CommentBox.Name = "AddConsentForm_CommentBox";
            this.AddConsentForm_CommentBox.Size = new System.Drawing.Size(184, 71);
            this.AddConsentForm_CommentBox.TabIndex = 8;
            this.AddConsentForm_CommentBox.Text = "";
            // 
            // AddConsentForm_CommentLabel
            // 
            this.AddConsentForm_CommentLabel.AutoSize = true;
            this.AddConsentForm_CommentLabel.Location = new System.Drawing.Point(12, 102);
            this.AddConsentForm_CommentLabel.Name = "AddConsentForm_CommentLabel";
            this.AddConsentForm_CommentLabel.Size = new System.Drawing.Size(61, 15);
            this.AddConsentForm_CommentLabel.TabIndex = 9;
            this.AddConsentForm_CommentLabel.Text = "Comment";
            // 
            // AddConsentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 211);
            this.Controls.Add(this.AddConsentForm_CommentLabel);
            this.Controls.Add(this.AddConsentForm_CommentBox);
            this.Controls.Add(this.AddConsentForm_VersionLabel);
            this.Controls.Add(this.AddConsentForm_SignedLabel);
            this.Controls.Add(this.AddConsentForm_VersionDate);
            this.Controls.Add(this.AddConsentForm_SignedDate);
            this.Controls.Add(this.AddConsentForm_NameLabel);
            this.Controls.Add(this.AddConsentForm_NameBox);
            this.Controls.Add(this.AddConsentForm_AddButton);
            this.Controls.Add(this.AddConsentForm_CancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddConsentForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button AddConsentForm_CancelButton;
        private System.Windows.Forms.Button AddConsentForm_AddButton;
        private System.Windows.Forms.TextBox AddConsentForm_NameBox;
        private System.Windows.Forms.Label AddConsentForm_NameLabel;
        private System.Windows.Forms.DateTimePicker AddConsentForm_SignedDate;
        private System.Windows.Forms.DateTimePicker AddConsentForm_VersionDate;
        private System.Windows.Forms.Label AddConsentForm_SignedLabel;
        private System.Windows.Forms.Label AddConsentForm_VersionLabel;
        private System.Windows.Forms.RichTextBox AddConsentForm_CommentBox;
        private System.Windows.Forms.Label AddConsentForm_CommentLabel;
    }
}