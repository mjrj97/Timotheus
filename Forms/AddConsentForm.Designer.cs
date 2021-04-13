
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddConsentForm));
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
            this.AddConsentForm_NewButton = new System.Windows.Forms.RadioButton();
            this.AddConsentForm_ExistingButton = new System.Windows.Forms.RadioButton();
            this.AddConsentForm_ComboBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // AddConsentForm_CancelButton
            // 
            this.AddConsentForm_CancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AddConsentForm_CancelButton.Location = new System.Drawing.Point(182, 206);
            this.AddConsentForm_CancelButton.Name = "AddConsentForm_CancelButton";
            this.AddConsentForm_CancelButton.Size = new System.Drawing.Size(95, 23);
            this.AddConsentForm_CancelButton.TabIndex = 0;
            this.AddConsentForm_CancelButton.Text = "Cancel";
            this.AddConsentForm_CancelButton.UseVisualStyleBackColor = true;
            this.AddConsentForm_CancelButton.Click += new System.EventHandler(this.Close);
            // 
            // AddConsentForm_AddButton
            // 
            this.AddConsentForm_AddButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AddConsentForm_AddButton.Location = new System.Drawing.Point(88, 206);
            this.AddConsentForm_AddButton.Name = "AddConsentForm_AddButton";
            this.AddConsentForm_AddButton.Size = new System.Drawing.Size(88, 23);
            this.AddConsentForm_AddButton.TabIndex = 1;
            this.AddConsentForm_AddButton.Text = "Add";
            this.AddConsentForm_AddButton.UseVisualStyleBackColor = true;
            this.AddConsentForm_AddButton.Click += new System.EventHandler(this.Add);
            // 
            // AddConsentForm_NameBox
            // 
            this.AddConsentForm_NameBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AddConsentForm_NameBox.Location = new System.Drawing.Point(88, 42);
            this.AddConsentForm_NameBox.Name = "AddConsentForm_NameBox";
            this.AddConsentForm_NameBox.Size = new System.Drawing.Size(189, 23);
            this.AddConsentForm_NameBox.TabIndex = 2;
            // 
            // AddConsentForm_NameLabel
            // 
            this.AddConsentForm_NameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AddConsentForm_NameLabel.AutoSize = true;
            this.AddConsentForm_NameLabel.Location = new System.Drawing.Point(12, 45);
            this.AddConsentForm_NameLabel.Name = "AddConsentForm_NameLabel";
            this.AddConsentForm_NameLabel.Size = new System.Drawing.Size(39, 15);
            this.AddConsentForm_NameLabel.TabIndex = 3;
            this.AddConsentForm_NameLabel.Text = "Name";
            // 
            // AddConsentForm_SignedDate
            // 
            this.AddConsentForm_SignedDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AddConsentForm_SignedDate.Location = new System.Drawing.Point(88, 71);
            this.AddConsentForm_SignedDate.Name = "AddConsentForm_SignedDate";
            this.AddConsentForm_SignedDate.Size = new System.Drawing.Size(189, 23);
            this.AddConsentForm_SignedDate.TabIndex = 4;
            // 
            // AddConsentForm_VersionDate
            // 
            this.AddConsentForm_VersionDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AddConsentForm_VersionDate.Location = new System.Drawing.Point(88, 100);
            this.AddConsentForm_VersionDate.Name = "AddConsentForm_VersionDate";
            this.AddConsentForm_VersionDate.Size = new System.Drawing.Size(189, 23);
            this.AddConsentForm_VersionDate.TabIndex = 5;
            // 
            // AddConsentForm_SignedLabel
            // 
            this.AddConsentForm_SignedLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AddConsentForm_SignedLabel.AutoSize = true;
            this.AddConsentForm_SignedLabel.Location = new System.Drawing.Point(13, 77);
            this.AddConsentForm_SignedLabel.Name = "AddConsentForm_SignedLabel";
            this.AddConsentForm_SignedLabel.Size = new System.Drawing.Size(43, 15);
            this.AddConsentForm_SignedLabel.TabIndex = 6;
            this.AddConsentForm_SignedLabel.Text = "Signed";
            // 
            // AddConsentForm_VersionLabel
            // 
            this.AddConsentForm_VersionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AddConsentForm_VersionLabel.AutoSize = true;
            this.AddConsentForm_VersionLabel.Location = new System.Drawing.Point(13, 106);
            this.AddConsentForm_VersionLabel.Name = "AddConsentForm_VersionLabel";
            this.AddConsentForm_VersionLabel.Size = new System.Drawing.Size(45, 15);
            this.AddConsentForm_VersionLabel.TabIndex = 7;
            this.AddConsentForm_VersionLabel.Text = "Version";
            // 
            // AddConsentForm_CommentBox
            // 
            this.AddConsentForm_CommentBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AddConsentForm_CommentBox.Location = new System.Drawing.Point(88, 129);
            this.AddConsentForm_CommentBox.Name = "AddConsentForm_CommentBox";
            this.AddConsentForm_CommentBox.Size = new System.Drawing.Size(189, 71);
            this.AddConsentForm_CommentBox.TabIndex = 8;
            this.AddConsentForm_CommentBox.Text = "";
            // 
            // AddConsentForm_CommentLabel
            // 
            this.AddConsentForm_CommentLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AddConsentForm_CommentLabel.AutoSize = true;
            this.AddConsentForm_CommentLabel.Location = new System.Drawing.Point(12, 132);
            this.AddConsentForm_CommentLabel.Name = "AddConsentForm_CommentLabel";
            this.AddConsentForm_CommentLabel.Size = new System.Drawing.Size(61, 15);
            this.AddConsentForm_CommentLabel.TabIndex = 9;
            this.AddConsentForm_CommentLabel.Text = "Comment";
            // 
            // AddConsentForm_NewButton
            // 
            this.AddConsentForm_NewButton.AutoSize = true;
            this.AddConsentForm_NewButton.Checked = true;
            this.AddConsentForm_NewButton.Location = new System.Drawing.Point(12, 12);
            this.AddConsentForm_NewButton.Name = "AddConsentForm_NewButton";
            this.AddConsentForm_NewButton.Size = new System.Drawing.Size(49, 19);
            this.AddConsentForm_NewButton.TabIndex = 10;
            this.AddConsentForm_NewButton.TabStop = true;
            this.AddConsentForm_NewButton.Text = "New";
            this.AddConsentForm_NewButton.UseVisualStyleBackColor = true;
            this.AddConsentForm_NewButton.CheckedChanged += new System.EventHandler(this.RadioButtonsChanged);
            // 
            // AddConsentForm_ExistingButton
            // 
            this.AddConsentForm_ExistingButton.AutoSize = true;
            this.AddConsentForm_ExistingButton.Location = new System.Drawing.Point(88, 12);
            this.AddConsentForm_ExistingButton.Name = "AddConsentForm_ExistingButton";
            this.AddConsentForm_ExistingButton.Size = new System.Drawing.Size(71, 19);
            this.AddConsentForm_ExistingButton.TabIndex = 11;
            this.AddConsentForm_ExistingButton.Text = "From list";
            this.AddConsentForm_ExistingButton.UseVisualStyleBackColor = true;
            // 
            // AddConsentForm_ComboBox
            // 
            this.AddConsentForm_ComboBox.DisplayMember = "Name";
            this.AddConsentForm_ComboBox.FormattingEnabled = true;
            this.AddConsentForm_ComboBox.Location = new System.Drawing.Point(88, 42);
            this.AddConsentForm_ComboBox.Name = "AddConsentForm_ComboBox";
            this.AddConsentForm_ComboBox.Size = new System.Drawing.Size(189, 23);
            this.AddConsentForm_ComboBox.TabIndex = 12;
            this.AddConsentForm_ComboBox.Visible = false;
            this.AddConsentForm_ComboBox.SelectedValueChanged += new System.EventHandler(this.ComboBoxChange);
            // 
            // AddConsentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(289, 241);
            this.Controls.Add(this.AddConsentForm_ComboBox);
            this.Controls.Add(this.AddConsentForm_ExistingButton);
            this.Controls.Add(this.AddConsentForm_NewButton);
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
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
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
        private System.Windows.Forms.RadioButton AddConsentForm_NewButton;
        private System.Windows.Forms.RadioButton AddConsentForm_ExistingButton;
        private System.Windows.Forms.ComboBox AddConsentForm_ComboBox;
    }
}