
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
            this.AddMember_NameLabel = new System.Windows.Forms.Label();
            this.AddMember_NameBox = new System.Windows.Forms.TextBox();
            this.AddMember_AddressLabel = new System.Windows.Forms.Label();
            this.AddMember_AddressBox = new System.Windows.Forms.TextBox();
            this.AddMember_BirthdayLabel = new System.Windows.Forms.Label();
            this.Addmember_BirthdayCalendar = new System.Windows.Forms.MonthCalendar();
            this.AddMember_MemberSinceLabel = new System.Windows.Forms.Label();
            this.AddMember_MemberSinceCalender = new System.Windows.Forms.MonthCalendar();
            this.AddMember_AddButton = new System.Windows.Forms.Button();
            this.AddMember_CancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // AddMember_NameLabel
            // 
            this.AddMember_NameLabel.AutoSize = true;
            this.AddMember_NameLabel.Location = new System.Drawing.Point(25, 31);
            this.AddMember_NameLabel.Name = "AddMember_NameLabel";
            this.AddMember_NameLabel.Size = new System.Drawing.Size(39, 15);
            this.AddMember_NameLabel.TabIndex = 0;
            this.AddMember_NameLabel.Text = "Name";
            // 
            // AddMember_NameBox
            // 
            this.AddMember_NameBox.Location = new System.Drawing.Point(127, 28);
            this.AddMember_NameBox.Name = "AddMember_NameBox";
            this.AddMember_NameBox.Size = new System.Drawing.Size(171, 23);
            this.AddMember_NameBox.TabIndex = 1;
            // 
            // AddMember_AddressLabel
            // 
            this.AddMember_AddressLabel.AutoSize = true;
            this.AddMember_AddressLabel.Location = new System.Drawing.Point(25, 72);
            this.AddMember_AddressLabel.Name = "AddMember_AddressLabel";
            this.AddMember_AddressLabel.Size = new System.Drawing.Size(49, 15);
            this.AddMember_AddressLabel.TabIndex = 2;
            this.AddMember_AddressLabel.Text = "Address";
            // 
            // AddMember_AddressBox
            // 
            this.AddMember_AddressBox.Location = new System.Drawing.Point(127, 72);
            this.AddMember_AddressBox.Name = "AddMember_AddressBox";
            this.AddMember_AddressBox.Size = new System.Drawing.Size(171, 23);
            this.AddMember_AddressBox.TabIndex = 3;
            this.AddMember_AddressBox.TextChanged += new System.EventHandler(this.AddMember_AddressBox_TextChanged);
            // 
            // AddMember_BirthdayLabel
            // 
            this.AddMember_BirthdayLabel.AutoSize = true;
            this.AddMember_BirthdayLabel.Location = new System.Drawing.Point(25, 114);
            this.AddMember_BirthdayLabel.Name = "AddMember_BirthdayLabel";
            this.AddMember_BirthdayLabel.Size = new System.Drawing.Size(51, 15);
            this.AddMember_BirthdayLabel.TabIndex = 4;
            this.AddMember_BirthdayLabel.Text = "Birthday";
            // 
            // Addmember_BirthdayCalendar
            // 
            this.Addmember_BirthdayCalendar.Location = new System.Drawing.Point(127, 114);
            this.Addmember_BirthdayCalendar.Name = "Addmember_BirthdayCalendar";
            this.Addmember_BirthdayCalendar.TabIndex = 5;
            // 
            // AddMember_MemberSinceLabel
            // 
            this.AddMember_MemberSinceLabel.AutoSize = true;
            this.AddMember_MemberSinceLabel.Location = new System.Drawing.Point(25, 302);
            this.AddMember_MemberSinceLabel.Name = "AddMember_MemberSinceLabel";
            this.AddMember_MemberSinceLabel.Size = new System.Drawing.Size(82, 15);
            this.AddMember_MemberSinceLabel.TabIndex = 6;
            this.AddMember_MemberSinceLabel.Text = "Member since";
            // 
            // AddMember_MemberSinceCalender
            // 
            this.AddMember_MemberSinceCalender.Location = new System.Drawing.Point(127, 294);
            this.AddMember_MemberSinceCalender.Name = "AddMember_MemberSinceCalender";
            this.AddMember_MemberSinceCalender.TabIndex = 7;
            // 
            // AddMember_AddButton
            // 
            this.AddMember_AddButton.Location = new System.Drawing.Point(127, 468);
            this.AddMember_AddButton.Name = "AddMember_AddButton";
            this.AddMember_AddButton.Size = new System.Drawing.Size(77, 23);
            this.AddMember_AddButton.TabIndex = 8;
            this.AddMember_AddButton.Text = "Add";
            this.AddMember_AddButton.UseVisualStyleBackColor = true;
            this.AddMember_AddButton.Click += new System.EventHandler(this.AddMember_AddButton_Click);
            // 
            // AddMember_CancelButton
            // 
            this.AddMember_CancelButton.Location = new System.Drawing.Point(223, 468);
            this.AddMember_CancelButton.Name = "AddMember_CancelButton";
            this.AddMember_CancelButton.Size = new System.Drawing.Size(75, 23);
            this.AddMember_CancelButton.TabIndex = 9;
            this.AddMember_CancelButton.Text = "Cancel";
            this.AddMember_CancelButton.UseVisualStyleBackColor = true;
            this.AddMember_CancelButton.Click += new System.EventHandler(this.AddMember_CancelButton_Click);
            // 
            // AddMember
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(324, 507);
            this.Controls.Add(this.AddMember_CancelButton);
            this.Controls.Add(this.AddMember_AddButton);
            this.Controls.Add(this.AddMember_MemberSinceCalender);
            this.Controls.Add(this.AddMember_MemberSinceLabel);
            this.Controls.Add(this.Addmember_BirthdayCalendar);
            this.Controls.Add(this.AddMember_BirthdayLabel);
            this.Controls.Add(this.AddMember_AddressBox);
            this.Controls.Add(this.AddMember_AddressLabel);
            this.Controls.Add(this.AddMember_NameBox);
            this.Controls.Add(this.AddMember_NameLabel);
            this.Name = "AddMember";
            this.Text = "Add member";
            this.Load += new System.EventHandler(this.AddMember_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label AddMember_NameLabel;
        private System.Windows.Forms.TextBox AddMember_NameBox;
        private System.Windows.Forms.Label AddMember_addressLabel;
        private System.Windows.Forms.Label AddMember_AddressLabel;
        private System.Windows.Forms.Label AddMember_AddressLabel1;
        private System.Windows.Forms.TextBox AddMember_AddressBox;
        private System.Windows.Forms.Label AddMember_BirthdayLabel;
        private System.Windows.Forms.MonthCalendar Addmember_BirtCalendar;
        private System.Windows.Forms.Label AddMember_MemberSinceLabel;
        private System.Windows.Forms.MonthCalendar Addmember_birthdayCalendar;
        private System.Windows.Forms.MonthCalendar Addmember_BirthdayCalendar;
        private System.Windows.Forms.MonthCalendar AddMember_MemberSinceCalender;
        private System.Windows.Forms.Button AddMember_AddButton;
        private System.Windows.Forms.Button AddMember_CancelButton;
    }
}