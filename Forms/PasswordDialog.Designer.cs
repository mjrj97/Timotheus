
namespace Timotheus.Forms
{
    partial class PasswordDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PasswordDialog));
            this.PasswordDialog_Field = new System.Windows.Forms.TextBox();
            this.PasswordDialog_Label = new System.Windows.Forms.Label();
            this.PasswordDialog_SaveBox = new System.Windows.Forms.CheckBox();
            this.PasswordDialog_OKButton = new System.Windows.Forms.Button();
            this.PasswordDialog_CancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // PasswordDialog_Field
            // 
            this.PasswordDialog_Field.Location = new System.Drawing.Point(97, 12);
            this.PasswordDialog_Field.Name = "PasswordDialog_Field";
            this.PasswordDialog_Field.Size = new System.Drawing.Size(175, 23);
            this.PasswordDialog_Field.TabIndex = 0;
            // 
            // PasswordDialog_Label
            // 
            this.PasswordDialog_Label.AutoSize = true;
            this.PasswordDialog_Label.Location = new System.Drawing.Point(12, 15);
            this.PasswordDialog_Label.Name = "PasswordDialog_Label";
            this.PasswordDialog_Label.Size = new System.Drawing.Size(57, 15);
            this.PasswordDialog_Label.TabIndex = 1;
            this.PasswordDialog_Label.Text = "Password";
            // 
            // PasswordDialog_SaveBox
            // 
            this.PasswordDialog_SaveBox.AutoSize = true;
            this.PasswordDialog_SaveBox.Location = new System.Drawing.Point(12, 45);
            this.PasswordDialog_SaveBox.Name = "PasswordDialog_SaveBox";
            this.PasswordDialog_SaveBox.Size = new System.Drawing.Size(215, 19);
            this.PasswordDialog_SaveBox.TabIndex = 2;
            this.PasswordDialog_SaveBox.Text = "Save password (Not recommended)";
            this.PasswordDialog_SaveBox.UseVisualStyleBackColor = true;
            // 
            // PasswordDialog_OKButton
            // 
            this.PasswordDialog_OKButton.Location = new System.Drawing.Point(12, 76);
            this.PasswordDialog_OKButton.Name = "PasswordDialog_OKButton";
            this.PasswordDialog_OKButton.Size = new System.Drawing.Size(125, 23);
            this.PasswordDialog_OKButton.TabIndex = 3;
            this.PasswordDialog_OKButton.Text = "Ok";
            this.PasswordDialog_OKButton.UseVisualStyleBackColor = true;
            this.PasswordDialog_OKButton.Click += new System.EventHandler(this.Continue);
            // 
            // PasswordDialog_CancelButton
            // 
            this.PasswordDialog_CancelButton.Location = new System.Drawing.Point(147, 76);
            this.PasswordDialog_CancelButton.Name = "PasswordDialog_CancelButton";
            this.PasswordDialog_CancelButton.Size = new System.Drawing.Size(125, 23);
            this.PasswordDialog_CancelButton.TabIndex = 4;
            this.PasswordDialog_CancelButton.Text = "Cancel";
            this.PasswordDialog_CancelButton.UseVisualStyleBackColor = true;
            this.PasswordDialog_CancelButton.Click += new System.EventHandler(this.Close);
            // 
            // PasswordDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 111);
            this.Controls.Add(this.PasswordDialog_CancelButton);
            this.Controls.Add(this.PasswordDialog_OKButton);
            this.Controls.Add(this.PasswordDialog_SaveBox);
            this.Controls.Add(this.PasswordDialog_Label);
            this.Controls.Add(this.PasswordDialog_Field);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PasswordDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Password required";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox PasswordDialog_Field;
        private System.Windows.Forms.Label PasswordDialog_Label;
        private System.Windows.Forms.CheckBox PasswordDialog_SaveBox;
        private System.Windows.Forms.Button PasswordDialog_OKButton;
        private System.Windows.Forms.Button PasswordDialog_CancelButton;
    }
}