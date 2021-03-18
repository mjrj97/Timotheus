
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
            this.SuspendLayout();
            // 
            // AddTransaction_CancelButton
            // 
            this.AddTransaction_CancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.AddTransaction_CancelButton.Location = new System.Drawing.Point(254, 253);
            this.AddTransaction_CancelButton.Name = "AddTransaction_CancelButton";
            this.AddTransaction_CancelButton.Size = new System.Drawing.Size(75, 23);
            this.AddTransaction_CancelButton.TabIndex = 0;
            this.AddTransaction_CancelButton.Text = "Cancel";
            this.AddTransaction_CancelButton.UseVisualStyleBackColor = true;
            this.AddTransaction_CancelButton.Click += new System.EventHandler(this.Cancel);
            // 
            // AddTransaction_AddButton
            // 
            this.AddTransaction_AddButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.AddTransaction_AddButton.Location = new System.Drawing.Point(173, 253);
            this.AddTransaction_AddButton.Name = "AddTransaction_AddButton";
            this.AddTransaction_AddButton.Size = new System.Drawing.Size(75, 23);
            this.AddTransaction_AddButton.TabIndex = 1;
            this.AddTransaction_AddButton.Text = "Add";
            this.AddTransaction_AddButton.UseVisualStyleBackColor = true;
            this.AddTransaction_AddButton.Click += new System.EventHandler(this.Add);
            // 
            // AddTransaction
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(341, 288);
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

        }

        #endregion

        private System.Windows.Forms.Button AddTransaction_CancelButton;
        private System.Windows.Forms.Button AddTransaction_AddButton;
    }
}