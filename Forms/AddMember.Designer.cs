
namespace Timotheus.Forms
{
    partial class AddMember
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddMember));
            this.AddMember_NameLabel = new System.Windows.Forms.Label();
            this.AddMember_NameBox = new System.Windows.Forms.TextBox();
            this.AddMember_AddressLabel = new System.Windows.Forms.Label();
            this.AddMember_AddressBox = new System.Windows.Forms.TextBox();
            this.AddMember_BirthdayLabel = new System.Windows.Forms.Label();
            this.AddMember_EntryLabel = new System.Windows.Forms.Label();
            this.AddMember_AddButton = new System.Windows.Forms.Button();
            this.AddMember_CancelButton = new System.Windows.Forms.Button();
            this.AddMember_BirthdayDateLabel = new System.Windows.Forms.Label();
            this.AddMember_MemberSinceDateLabel = new System.Windows.Forms.Label();
            this.Addmember_BirthdayPicker = new System.Windows.Forms.DateTimePicker();
            this.AddMember_EntryPicker = new System.Windows.Forms.DateTimePicker();
            this.SuspendLayout();
            // 
            // AddMember_NameLabel
            // 
            this.AddMember_NameLabel.AutoSize = true;
            this.AddMember_NameLabel.Location = new System.Drawing.Point(16, 27);
            this.AddMember_NameLabel.Name = "AddMember_NameLabel";
            this.AddMember_NameLabel.Size = new System.Drawing.Size(39, 15);
            this.AddMember_NameLabel.TabIndex = 0;
            this.AddMember_NameLabel.Text = "Name";
            // 
            // AddMember_NameBox
            // 
            this.AddMember_NameBox.Location = new System.Drawing.Point(104, 19);
            this.AddMember_NameBox.Name = "AddMember_NameBox";
            this.AddMember_NameBox.Size = new System.Drawing.Size(200, 23);
            this.AddMember_NameBox.TabIndex = 1;
            // 
            // AddMember_AddressLabel
            // 
            this.AddMember_AddressLabel.AutoSize = true;
            this.AddMember_AddressLabel.Location = new System.Drawing.Point(16, 56);
            this.AddMember_AddressLabel.Name = "AddMember_AddressLabel";
            this.AddMember_AddressLabel.Size = new System.Drawing.Size(49, 15);
            this.AddMember_AddressLabel.TabIndex = 2;
            this.AddMember_AddressLabel.Text = "Address";
            // 
            // AddMember_AddressBox
            // 
            this.AddMember_AddressBox.Location = new System.Drawing.Point(104, 48);
            this.AddMember_AddressBox.Name = "AddMember_AddressBox";
            this.AddMember_AddressBox.Size = new System.Drawing.Size(200, 23);
            this.AddMember_AddressBox.TabIndex = 3;
            // 
            // AddMember_BirthdayLabel
            // 
            this.AddMember_BirthdayLabel.AutoSize = true;
            this.AddMember_BirthdayLabel.Location = new System.Drawing.Point(16, 83);
            this.AddMember_BirthdayLabel.Name = "AddMember_BirthdayLabel";
            this.AddMember_BirthdayLabel.Size = new System.Drawing.Size(51, 15);
            this.AddMember_BirthdayLabel.TabIndex = 4;
            this.AddMember_BirthdayLabel.Text = "Birthday";
            // 
            // AddMember_EntryLabel
            // 
            this.AddMember_EntryLabel.AutoSize = true;
            this.AddMember_EntryLabel.Location = new System.Drawing.Point(16, 110);
            this.AddMember_EntryLabel.Name = "AddMember_EntryLabel";
            this.AddMember_EntryLabel.Size = new System.Drawing.Size(34, 15);
            this.AddMember_EntryLabel.TabIndex = 6;
            this.AddMember_EntryLabel.Text = "Entry";
            // 
            // AddMember_AddButton
            // 
            this.AddMember_AddButton.Location = new System.Drawing.Point(146, 145);
            this.AddMember_AddButton.Name = "AddMember_AddButton";
            this.AddMember_AddButton.Size = new System.Drawing.Size(77, 23);
            this.AddMember_AddButton.TabIndex = 8;
            this.AddMember_AddButton.Text = "Add";
            this.AddMember_AddButton.UseVisualStyleBackColor = true;
            this.AddMember_AddButton.Click += new System.EventHandler(this.AddButton);
            // 
            // AddMember_CancelButton
            // 
            this.AddMember_CancelButton.Location = new System.Drawing.Point(229, 145);
            this.AddMember_CancelButton.Name = "AddMember_CancelButton";
            this.AddMember_CancelButton.Size = new System.Drawing.Size(75, 23);
            this.AddMember_CancelButton.TabIndex = 9;
            this.AddMember_CancelButton.Text = "Cancel";
            this.AddMember_CancelButton.UseVisualStyleBackColor = true;
            this.AddMember_CancelButton.Click += new System.EventHandler(this.CloseButton);
            // 
            // AddMember_BirthdayDateLabel
            // 
            this.AddMember_BirthdayDateLabel.Location = new System.Drawing.Point(0, 0);
            this.AddMember_BirthdayDateLabel.Name = "AddMember_BirthdayDateLabel";
            this.AddMember_BirthdayDateLabel.Size = new System.Drawing.Size(100, 23);
            this.AddMember_BirthdayDateLabel.TabIndex = 15;
            // 
            // AddMember_MemberSinceDateLabel
            // 
            this.AddMember_MemberSinceDateLabel.Location = new System.Drawing.Point(0, 0);
            this.AddMember_MemberSinceDateLabel.Name = "AddMember_MemberSinceDateLabel";
            this.AddMember_MemberSinceDateLabel.Size = new System.Drawing.Size(100, 23);
            this.AddMember_MemberSinceDateLabel.TabIndex = 14;
            // 
            // Addmember_BirthdayPicker
            // 
            this.Addmember_BirthdayPicker.Location = new System.Drawing.Point(104, 77);
            this.Addmember_BirthdayPicker.Name = "Addmember_BirthdayPicker";
            this.Addmember_BirthdayPicker.Size = new System.Drawing.Size(200, 23);
            this.Addmember_BirthdayPicker.TabIndex = 12;
            // 
            // AddMember_EntryPicker
            // 
            this.AddMember_EntryPicker.Location = new System.Drawing.Point(104, 106);
            this.AddMember_EntryPicker.Name = "AddMember_EntryPicker";
            this.AddMember_EntryPicker.Size = new System.Drawing.Size(200, 23);
            this.AddMember_EntryPicker.TabIndex = 13;
            // 
            // AddMember
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(316, 183);
            this.Controls.Add(this.AddMember_EntryPicker);
            this.Controls.Add(this.Addmember_BirthdayPicker);
            this.Controls.Add(this.AddMember_MemberSinceDateLabel);
            this.Controls.Add(this.AddMember_BirthdayDateLabel);
            this.Controls.Add(this.AddMember_CancelButton);
            this.Controls.Add(this.AddMember_AddButton);
            this.Controls.Add(this.AddMember_EntryLabel);
            this.Controls.Add(this.AddMember_BirthdayLabel);
            this.Controls.Add(this.AddMember_AddressBox);
            this.Controls.Add(this.AddMember_AddressLabel);
            this.Controls.Add(this.AddMember_NameBox);
            this.Controls.Add(this.AddMember_NameLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddMember";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add member";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label AddMember_NameLabel;
        private System.Windows.Forms.TextBox AddMember_NameBox;
        private System.Windows.Forms.Label AddMember_AddressLabel;
        private System.Windows.Forms.TextBox AddMember_AddressBox;
        private System.Windows.Forms.Label AddMember_BirthdayLabel;
        private System.Windows.Forms.Label AddMember_EntryLabel;
        private System.Windows.Forms.Button AddMember_AddButton;
        private System.Windows.Forms.Button AddMember_CancelButton;
        private System.Windows.Forms.Label AddMember_BirthdayDateLabel;
        private System.Windows.Forms.Label AddMember_MemberSinceDateLabel;
        private System.Windows.Forms.DateTimePicker Addmember_BirthdayPicker;
        private System.Windows.Forms.DateTimePicker AddMember_EntryPicker;
    }
}