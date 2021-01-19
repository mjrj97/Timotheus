
namespace Manager
{
    partial class AddEvent
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
            this.Cancel = new System.Windows.Forms.Button();
            this.Add = new System.Windows.Forms.Button();
            this.NameText = new System.Windows.Forms.TextBox();
            this.NameLabel = new System.Windows.Forms.Label();
            this.StartTimePicker = new System.Windows.Forms.DateTimePicker();
            this.EndTimePicker = new System.Windows.Forms.DateTimePicker();
            this.StartTimeBox = new System.Windows.Forms.TextBox();
            this.EndTimeBox = new System.Windows.Forms.TextBox();
            this.StartLabel = new System.Windows.Forms.Label();
            this.EndLabel = new System.Windows.Forms.Label();
            this.DescriptionBox = new System.Windows.Forms.RichTextBox();
            this.DescriptionLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Cancel
            // 
            this.Cancel.Location = new System.Drawing.Point(197, 205);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(125, 25);
            this.Cancel.TabIndex = 0;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // Add
            // 
            this.Add.Location = new System.Drawing.Point(62, 205);
            this.Add.Name = "Add";
            this.Add.Size = new System.Drawing.Size(125, 25);
            this.Add.TabIndex = 1;
            this.Add.Text = "Add";
            this.Add.UseVisualStyleBackColor = true;
            this.Add.Click += new System.EventHandler(this.Add_Click);
            // 
            // NameText
            // 
            this.NameText.Location = new System.Drawing.Point(62, 15);
            this.NameText.Name = "NameText";
            this.NameText.Size = new System.Drawing.Size(260, 23);
            this.NameText.TabIndex = 2;
            // 
            // NameLabel
            // 
            this.NameLabel.AutoSize = true;
            this.NameLabel.Location = new System.Drawing.Point(12, 18);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Size = new System.Drawing.Size(39, 15);
            this.NameLabel.TabIndex = 3;
            this.NameLabel.Text = "Name";
            // 
            // StartTimePicker
            // 
            this.StartTimePicker.Location = new System.Drawing.Point(164, 50);
            this.StartTimePicker.Name = "StartTimePicker";
            this.StartTimePicker.Size = new System.Drawing.Size(158, 23);
            this.StartTimePicker.TabIndex = 4;
            // 
            // EndTimePicker
            // 
            this.EndTimePicker.Location = new System.Drawing.Point(164, 85);
            this.EndTimePicker.Name = "EndTimePicker";
            this.EndTimePicker.Size = new System.Drawing.Size(158, 23);
            this.EndTimePicker.TabIndex = 5;
            // 
            // StartTimeBox
            // 
            this.StartTimeBox.Location = new System.Drawing.Point(62, 50);
            this.StartTimeBox.Name = "StartTimeBox";
            this.StartTimeBox.Size = new System.Drawing.Size(96, 23);
            this.StartTimeBox.TabIndex = 6;
            // 
            // EndTimeBox
            // 
            this.EndTimeBox.Location = new System.Drawing.Point(62, 85);
            this.EndTimeBox.Name = "EndTimeBox";
            this.EndTimeBox.Size = new System.Drawing.Size(96, 23);
            this.EndTimeBox.TabIndex = 7;
            // 
            // StartLabel
            // 
            this.StartLabel.AutoSize = true;
            this.StartLabel.Location = new System.Drawing.Point(12, 53);
            this.StartLabel.Name = "StartLabel";
            this.StartLabel.Size = new System.Drawing.Size(31, 15);
            this.StartLabel.TabIndex = 8;
            this.StartLabel.Text = "Start";
            // 
            // EndLabel
            // 
            this.EndLabel.AutoSize = true;
            this.EndLabel.Location = new System.Drawing.Point(12, 88);
            this.EndLabel.Name = "EndLabel";
            this.EndLabel.Size = new System.Drawing.Size(27, 15);
            this.EndLabel.TabIndex = 9;
            this.EndLabel.Text = "End";
            // 
            // DescriptionBox
            // 
            this.DescriptionBox.Location = new System.Drawing.Point(62, 120);
            this.DescriptionBox.Name = "DescriptionBox";
            this.DescriptionBox.Size = new System.Drawing.Size(260, 75);
            this.DescriptionBox.TabIndex = 10;
            this.DescriptionBox.Text = "";
            // 
            // DescriptionLabel
            // 
            this.DescriptionLabel.AutoSize = true;
            this.DescriptionLabel.Location = new System.Drawing.Point(12, 123);
            this.DescriptionLabel.Name = "DescriptionLabel";
            this.DescriptionLabel.Size = new System.Drawing.Size(35, 15);
            this.DescriptionLabel.TabIndex = 11;
            this.DescriptionLabel.Text = "Desc.";
            // 
            // AddEvent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 241);
            this.Controls.Add(this.DescriptionLabel);
            this.Controls.Add(this.DescriptionBox);
            this.Controls.Add(this.EndLabel);
            this.Controls.Add(this.StartLabel);
            this.Controls.Add(this.EndTimeBox);
            this.Controls.Add(this.StartTimeBox);
            this.Controls.Add(this.EndTimePicker);
            this.Controls.Add(this.StartTimePicker);
            this.Controls.Add(this.NameLabel);
            this.Controls.Add(this.NameText);
            this.Controls.Add(this.Add);
            this.Controls.Add(this.Cancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddEvent";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Button Add;
        private System.Windows.Forms.TextBox NameText;
        private System.Windows.Forms.Label NameLabel;
        private System.Windows.Forms.DateTimePicker StartTimePicker;
        private System.Windows.Forms.DateTimePicker EndTimePicker;
        private System.Windows.Forms.TextBox StartTimeBox;
        private System.Windows.Forms.TextBox EndTimeBox;
        private System.Windows.Forms.Label StartLabel;
        private System.Windows.Forms.Label EndLabel;
        private System.Windows.Forms.RichTextBox DescriptionBox;
        private System.Windows.Forms.Label DescriptionLabel;
    }
}