
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
            this.SuspendLayout();
            // 
            // AddConsentForm_CancelButton
            // 
            this.AddConsentForm_CancelButton.Location = new System.Drawing.Point(197, 176);
            this.AddConsentForm_CancelButton.Name = "AddConsentForm_CancelButton";
            this.AddConsentForm_CancelButton.Size = new System.Drawing.Size(75, 23);
            this.AddConsentForm_CancelButton.TabIndex = 0;
            this.AddConsentForm_CancelButton.Text = "Cancel";
            this.AddConsentForm_CancelButton.UseVisualStyleBackColor = true;
            this.AddConsentForm_CancelButton.Click += new System.EventHandler(this.Cancel);
            // 
            // AddConsentForm_AddButton
            // 
            this.AddConsentForm_AddButton.Location = new System.Drawing.Point(116, 176);
            this.AddConsentForm_AddButton.Name = "AddConsentForm_AddButton";
            this.AddConsentForm_AddButton.Size = new System.Drawing.Size(75, 23);
            this.AddConsentForm_AddButton.TabIndex = 1;
            this.AddConsentForm_AddButton.Text = "Add";
            this.AddConsentForm_AddButton.UseVisualStyleBackColor = true;
            this.AddConsentForm_AddButton.Click += new System.EventHandler(this.Add);
            // 
            // AddConsentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 211);
            this.Controls.Add(this.AddConsentForm_AddButton);
            this.Controls.Add(this.AddConsentForm_CancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddConsentForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button AddConsentForm_CancelButton;
        private System.Windows.Forms.Button AddConsentForm_AddButton;
    }
}