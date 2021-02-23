namespace Timotheus.Forms
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
            this.NameText = new System.Windows.Forms.TextBox();
            this.NameLabel = new System.Windows.Forms.Label();
            this.StartTimeBox = new System.Windows.Forms.TextBox();
            this.StartTimePicker = new System.Windows.Forms.DateTimePicker();
            this.EndTimeBox = new System.Windows.Forms.TextBox();
            this.EndTimePicker = new System.Windows.Forms.DateTimePicker();
            this.StartLabel = new System.Windows.Forms.Label();
            this.EndLabel = new System.Windows.Forms.Label();
            this.LocationBox = new System.Windows.Forms.TextBox();
            this.LocationLabel = new System.Windows.Forms.Label();
            this.DescriptionBox = new System.Windows.Forms.RichTextBox();
            this.DescriptionLabel = new System.Windows.Forms.Label();
            this.Add = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.AllDayBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // NameText
            // 
            this.NameText.Location = new System.Drawing.Point(85, 15);
            this.NameText.Name = "NameText";
            this.NameText.Size = new System.Drawing.Size(237, 23);
            this.NameText.TabIndex = 0;
            // 
            // NameLabel
            // 
            this.NameLabel.AutoSize = true;
            this.NameLabel.Location = new System.Drawing.Point(12, 18);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Size = new System.Drawing.Size(39, 15);
            this.NameLabel.TabIndex = 9;
            this.NameLabel.Text = "Name";
            // 
            // StartTimeBox
            // 
            this.StartTimeBox.Location = new System.Drawing.Point(85, 50);
            this.StartTimeBox.Name = "StartTimeBox";
            this.StartTimeBox.Size = new System.Drawing.Size(73, 23);
            this.StartTimeBox.TabIndex = 1;
            // 
            // StartTimePicker
            // 
            this.StartTimePicker.Location = new System.Drawing.Point(164, 50);
            this.StartTimePicker.Name = "StartTimePicker";
            this.StartTimePicker.Size = new System.Drawing.Size(158, 23);
            this.StartTimePicker.TabIndex = 2;
            // 
            // EndTimeBox
            // 
            this.EndTimeBox.Location = new System.Drawing.Point(85, 85);
            this.EndTimeBox.Name = "EndTimeBox";
            this.EndTimeBox.Size = new System.Drawing.Size(73, 23);
            this.EndTimeBox.TabIndex = 3;
            // 
            // EndTimePicker
            // 
            this.EndTimePicker.Location = new System.Drawing.Point(164, 85);
            this.EndTimePicker.Name = "EndTimePicker";
            this.EndTimePicker.Size = new System.Drawing.Size(158, 23);
            this.EndTimePicker.TabIndex = 4;
            // 
            // StartLabel
            // 
            this.StartLabel.AutoSize = true;
            this.StartLabel.Location = new System.Drawing.Point(12, 53);
            this.StartLabel.Name = "StartLabel";
            this.StartLabel.Size = new System.Drawing.Size(31, 15);
            this.StartLabel.TabIndex = 10;
            this.StartLabel.Text = "Start";
            // 
            // EndLabel
            // 
            this.EndLabel.AutoSize = true;
            this.EndLabel.Location = new System.Drawing.Point(12, 88);
            this.EndLabel.Name = "EndLabel";
            this.EndLabel.Size = new System.Drawing.Size(27, 15);
            this.EndLabel.TabIndex = 11;
            this.EndLabel.Text = "End";
            // 
            // LocationBox
            // 
            this.LocationBox.Location = new System.Drawing.Point(86, 150);
            this.LocationBox.Name = "LocationBox";
            this.LocationBox.Size = new System.Drawing.Size(236, 23);
            this.LocationBox.TabIndex = 5;
            // 
            // LocationLabel
            // 
            this.LocationLabel.AutoSize = true;
            this.LocationLabel.Location = new System.Drawing.Point(12, 153);
            this.LocationLabel.Name = "LocationLabel";
            this.LocationLabel.Size = new System.Drawing.Size(53, 15);
            this.LocationLabel.TabIndex = 12;
            this.LocationLabel.Text = "Location";
            // 
            // DescriptionBox
            // 
            this.DescriptionBox.Location = new System.Drawing.Point(85, 185);
            this.DescriptionBox.Name = "DescriptionBox";
            this.DescriptionBox.Size = new System.Drawing.Size(237, 75);
            this.DescriptionBox.TabIndex = 6;
            this.DescriptionBox.Text = "";
            // 
            // DescriptionLabel
            // 
            this.DescriptionLabel.AutoSize = true;
            this.DescriptionLabel.Location = new System.Drawing.Point(12, 188);
            this.DescriptionLabel.Name = "DescriptionLabel";
            this.DescriptionLabel.Size = new System.Drawing.Size(67, 15);
            this.DescriptionLabel.TabIndex = 13;
            this.DescriptionLabel.Text = "Description";
            // 
            // Add
            // 
            this.Add.Location = new System.Drawing.Point(85, 270);
            this.Add.Name = "Add";
            this.Add.Size = new System.Drawing.Size(115, 25);
            this.Add.TabIndex = 7;
            this.Add.Text = "Add";
            this.Add.UseVisualStyleBackColor = true;
            this.Add.Click += new System.EventHandler(this.Add_Click);
            // 
            // Cancel
            // 
            this.Cancel.Location = new System.Drawing.Point(207, 270);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(115, 25);
            this.Cancel.TabIndex = 8;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // AllDayBox
            // 
            this.AllDayBox.AutoSize = true;
            this.AllDayBox.Location = new System.Drawing.Point(85, 120);
            this.AllDayBox.Name = "AllDayBox";
            this.AllDayBox.Size = new System.Drawing.Size(94, 19);
            this.AllDayBox.TabIndex = 14;
            this.AllDayBox.Text = "All day event";
            this.AllDayBox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.AllDayBox.UseVisualStyleBackColor = true;
            this.AllDayBox.CheckedChanged += new System.EventHandler(this.AllDayBox_CheckedChanged);
            // 
            // AddEvent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 307);
            this.Controls.Add(this.AllDayBox);
            this.Controls.Add(this.DescriptionLabel);
            this.Controls.Add(this.DescriptionBox);
            this.Controls.Add(this.LocationLabel);
            this.Controls.Add(this.LocationBox);
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
        private System.Windows.Forms.TextBox LocationBox;
        private System.Windows.Forms.Label LocationLabel;
        private System.Windows.Forms.RichTextBox DescriptionBox;
        private System.Windows.Forms.Label DescriptionLabel;
        private System.Windows.Forms.CheckBox AllDayBox;
    }
}