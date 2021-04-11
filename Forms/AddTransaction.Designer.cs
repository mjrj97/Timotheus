
namespace Timotheus.Forms
{
    partial class AddTransaction
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddTransaction));
            this.AddTransaction_CancelButton = new System.Windows.Forms.Button();
            this.AddTransaction_AddButton = new System.Windows.Forms.Button();
            this.AddTransaction_DescriptionBox = new System.Windows.Forms.TextBox();
            this.AddTransaction_DescriptionLabel = new System.Windows.Forms.Label();
            this.AddTransaction_DatePicker = new System.Windows.Forms.DateTimePicker();
            this.AddTransaction_DateLabel = new System.Windows.Forms.Label();
            this.AddTransaction_AccountPicker = new System.Windows.Forms.ComboBox();
            this.AddTransaction_AccountLabel = new System.Windows.Forms.Label();
            this.AddTransaction_InBox = new System.Windows.Forms.TextBox();
            this.AddTransaction_Currency1 = new System.Windows.Forms.Label();
            this.AddTransaction_InLabel = new System.Windows.Forms.Label();
            this.AddTransaction_OutBox = new System.Windows.Forms.TextBox();
            this.AddTransaction_Currency2 = new System.Windows.Forms.Label();
            this.AddTransaction_OutLabel = new System.Windows.Forms.Label();
            this.AddTransaction_AppendixBox = new System.Windows.Forms.TextBox();
            this.AddTransaction_AppendixLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // AddTransaction_CancelButton
            // 
            this.AddTransaction_CancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.AddTransaction_CancelButton.Location = new System.Drawing.Point(170, 190);
            this.AddTransaction_CancelButton.Name = "AddTransaction_CancelButton";
            this.AddTransaction_CancelButton.Size = new System.Drawing.Size(75, 23);
            this.AddTransaction_CancelButton.TabIndex = 18;
            this.AddTransaction_CancelButton.Text = "Cancel";
            this.AddTransaction_CancelButton.UseVisualStyleBackColor = true;
            this.AddTransaction_CancelButton.Click += new System.EventHandler(this.Close);
            // 
            // AddTransaction_AddButton
            // 
            this.AddTransaction_AddButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.AddTransaction_AddButton.Location = new System.Drawing.Point(85, 190);
            this.AddTransaction_AddButton.Name = "AddTransaction_AddButton";
            this.AddTransaction_AddButton.Size = new System.Drawing.Size(75, 23);
            this.AddTransaction_AddButton.TabIndex = 17;
            this.AddTransaction_AddButton.Text = "Add";
            this.AddTransaction_AddButton.UseVisualStyleBackColor = true;
            this.AddTransaction_AddButton.Click += new System.EventHandler(this.Add);
            // 
            // AddTransaction_DescriptionBox
            // 
            this.AddTransaction_DescriptionBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AddTransaction_DescriptionBox.Location = new System.Drawing.Point(85, 12);
            this.AddTransaction_DescriptionBox.Name = "AddTransaction_DescriptionBox";
            this.AddTransaction_DescriptionBox.Size = new System.Drawing.Size(160, 23);
            this.AddTransaction_DescriptionBox.TabIndex = 0;
            // 
            // AddTransaction_DescriptionLabel
            // 
            this.AddTransaction_DescriptionLabel.AutoSize = true;
            this.AddTransaction_DescriptionLabel.Location = new System.Drawing.Point(12, 15);
            this.AddTransaction_DescriptionLabel.Name = "AddTransaction_DescriptionLabel";
            this.AddTransaction_DescriptionLabel.Size = new System.Drawing.Size(67, 15);
            this.AddTransaction_DescriptionLabel.TabIndex = 3;
            this.AddTransaction_DescriptionLabel.Text = "Description";
            // 
            // AddTransaction_DatePicker
            // 
            this.AddTransaction_DatePicker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AddTransaction_DatePicker.Location = new System.Drawing.Point(85, 41);
            this.AddTransaction_DatePicker.Name = "AddTransaction_DatePicker";
            this.AddTransaction_DatePicker.Size = new System.Drawing.Size(160, 23);
            this.AddTransaction_DatePicker.TabIndex = 4;
            // 
            // AddTransaction_DateLabel
            // 
            this.AddTransaction_DateLabel.AutoSize = true;
            this.AddTransaction_DateLabel.Location = new System.Drawing.Point(12, 45);
            this.AddTransaction_DateLabel.Name = "AddTransaction_DateLabel";
            this.AddTransaction_DateLabel.Size = new System.Drawing.Size(31, 15);
            this.AddTransaction_DateLabel.TabIndex = 5;
            this.AddTransaction_DateLabel.Text = "Date";
            // 
            // AddTransaction_AccountPicker
            // 
            this.AddTransaction_AccountPicker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AddTransaction_AccountPicker.DisplayMember = "Name";
            this.AddTransaction_AccountPicker.FormattingEnabled = true;
            this.AddTransaction_AccountPicker.Location = new System.Drawing.Point(85, 70);
            this.AddTransaction_AccountPicker.Name = "AddTransaction_AccountPicker";
            this.AddTransaction_AccountPicker.Size = new System.Drawing.Size(160, 23);
            this.AddTransaction_AccountPicker.TabIndex = 7;
            this.AddTransaction_AccountPicker.ValueMember = "ID";
            // 
            // AddTransaction_AccountLabel
            // 
            this.AddTransaction_AccountLabel.AutoSize = true;
            this.AddTransaction_AccountLabel.Location = new System.Drawing.Point(12, 73);
            this.AddTransaction_AccountLabel.Name = "AddTransaction_AccountLabel";
            this.AddTransaction_AccountLabel.Size = new System.Drawing.Size(24, 15);
            this.AddTransaction_AccountLabel.TabIndex = 8;
            this.AddTransaction_AccountLabel.Text = "a/c";
            // 
            // AddTransaction_InBox
            // 
            this.AddTransaction_InBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AddTransaction_InBox.Location = new System.Drawing.Point(85, 99);
            this.AddTransaction_InBox.Name = "AddTransaction_InBox";
            this.AddTransaction_InBox.Size = new System.Drawing.Size(125, 23);
            this.AddTransaction_InBox.TabIndex = 9;
            this.AddTransaction_InBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnlyNumbersBox);
            // 
            // AddTransaction_Currency1
            // 
            this.AddTransaction_Currency1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AddTransaction_Currency1.AutoSize = true;
            this.AddTransaction_Currency1.Location = new System.Drawing.Point(216, 102);
            this.AddTransaction_Currency1.Name = "AddTransaction_Currency1";
            this.AddTransaction_Currency1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.AddTransaction_Currency1.Size = new System.Drawing.Size(13, 15);
            this.AddTransaction_Currency1.TabIndex = 10;
            this.AddTransaction_Currency1.Text = "$";
            this.AddTransaction_Currency1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // AddTransaction_InLabel
            // 
            this.AddTransaction_InLabel.AutoSize = true;
            this.AddTransaction_InLabel.Location = new System.Drawing.Point(12, 102);
            this.AddTransaction_InLabel.Name = "AddTransaction_InLabel";
            this.AddTransaction_InLabel.Size = new System.Drawing.Size(47, 15);
            this.AddTransaction_InLabel.TabIndex = 11;
            this.AddTransaction_InLabel.Text = "Income";
            // 
            // AddTransaction_OutBox
            // 
            this.AddTransaction_OutBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AddTransaction_OutBox.Location = new System.Drawing.Point(85, 128);
            this.AddTransaction_OutBox.Name = "AddTransaction_OutBox";
            this.AddTransaction_OutBox.Size = new System.Drawing.Size(125, 23);
            this.AddTransaction_OutBox.TabIndex = 12;
            this.AddTransaction_OutBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnlyNumbersBox);
            // 
            // AddTransaction_Currency2
            // 
            this.AddTransaction_Currency2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AddTransaction_Currency2.AutoSize = true;
            this.AddTransaction_Currency2.Location = new System.Drawing.Point(216, 131);
            this.AddTransaction_Currency2.Name = "AddTransaction_Currency2";
            this.AddTransaction_Currency2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.AddTransaction_Currency2.Size = new System.Drawing.Size(13, 15);
            this.AddTransaction_Currency2.TabIndex = 13;
            this.AddTransaction_Currency2.Text = "$";
            this.AddTransaction_Currency2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // AddTransaction_OutLabel
            // 
            this.AddTransaction_OutLabel.AutoSize = true;
            this.AddTransaction_OutLabel.Location = new System.Drawing.Point(12, 131);
            this.AddTransaction_OutLabel.Name = "AddTransaction_OutLabel";
            this.AddTransaction_OutLabel.Size = new System.Drawing.Size(50, 15);
            this.AddTransaction_OutLabel.TabIndex = 14;
            this.AddTransaction_OutLabel.Text = "Expense";
            // 
            // AddTransaction_AppendixBox
            // 
            this.AddTransaction_AppendixBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AddTransaction_AppendixBox.Location = new System.Drawing.Point(85, 157);
            this.AddTransaction_AppendixBox.Name = "AddTransaction_AppendixBox";
            this.AddTransaction_AppendixBox.Size = new System.Drawing.Size(160, 23);
            this.AddTransaction_AppendixBox.TabIndex = 15;
            this.AddTransaction_AppendixBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnlyNumbersBox);
            // 
            // AddTransaction_AppendixLabel
            // 
            this.AddTransaction_AppendixLabel.AutoSize = true;
            this.AddTransaction_AppendixLabel.Location = new System.Drawing.Point(12, 160);
            this.AddTransaction_AppendixLabel.Name = "AddTransaction_AppendixLabel";
            this.AddTransaction_AppendixLabel.Size = new System.Drawing.Size(58, 15);
            this.AddTransaction_AppendixLabel.TabIndex = 16;
            this.AddTransaction_AppendixLabel.Text = "Appendix";
            // 
            // AddTransaction
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(257, 225);
            this.Controls.Add(this.AddTransaction_AppendixLabel);
            this.Controls.Add(this.AddTransaction_AppendixBox);
            this.Controls.Add(this.AddTransaction_OutLabel);
            this.Controls.Add(this.AddTransaction_Currency2);
            this.Controls.Add(this.AddTransaction_OutBox);
            this.Controls.Add(this.AddTransaction_InLabel);
            this.Controls.Add(this.AddTransaction_Currency1);
            this.Controls.Add(this.AddTransaction_InBox);
            this.Controls.Add(this.AddTransaction_AccountLabel);
            this.Controls.Add(this.AddTransaction_AccountPicker);
            this.Controls.Add(this.AddTransaction_DateLabel);
            this.Controls.Add(this.AddTransaction_DatePicker);
            this.Controls.Add(this.AddTransaction_DescriptionLabel);
            this.Controls.Add(this.AddTransaction_DescriptionBox);
            this.Controls.Add(this.AddTransaction_AddButton);
            this.Controls.Add(this.AddTransaction_CancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddTransaction";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button AddTransaction_CancelButton;
        private System.Windows.Forms.Button AddTransaction_AddButton;
        private System.Windows.Forms.TextBox AddTransaction_DescriptionBox;
        private System.Windows.Forms.Label AddTransaction_DescriptionLabel;
        private System.Windows.Forms.DateTimePicker AddTransaction_DatePicker;
        private System.Windows.Forms.Label AddTransaction_DateLabel;
        private System.Windows.Forms.ComboBox AddTransaction_AccountPicker;
        private System.Windows.Forms.Label AddTransaction_AccountLabel;
        private System.Windows.Forms.TextBox AddTransaction_InBox;
        private System.Windows.Forms.Label AddTransaction_Currency1;
        private System.Windows.Forms.Label AddTransaction_InLabel;
        private System.Windows.Forms.TextBox AddTransaction_OutBox;
        private System.Windows.Forms.Label AddTransaction_Currency2;
        private System.Windows.Forms.Label AddTransaction_OutLabel;
        private System.Windows.Forms.TextBox AddTransaction_AppendixBox;
        private System.Windows.Forms.Label AddTransaction_AppendixLabel;
    }
}