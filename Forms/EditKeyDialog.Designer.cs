
namespace Timotheus.Forms
{
    partial class EditKeyDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditKeyDialog));
            this.EditKeyDialog_TextBox = new System.Windows.Forms.RichTextBox();
            this.EditKeyDialog_CancelButton = new System.Windows.Forms.Button();
            this.EditKeyDialog_OKButton = new System.Windows.Forms.Button();
            this.EditKeyDialog_AddStdButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // EditKeyDialog_TextBox
            // 
            this.EditKeyDialog_TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.EditKeyDialog_TextBox.Location = new System.Drawing.Point(12, 12);
            this.EditKeyDialog_TextBox.Name = "EditKeyDialog_TextBox";
            this.EditKeyDialog_TextBox.Size = new System.Drawing.Size(575, 325);
            this.EditKeyDialog_TextBox.TabIndex = 0;
            this.EditKeyDialog_TextBox.Text = "";
            // 
            // EditKeyDialog_CancelButton
            // 
            this.EditKeyDialog_CancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.EditKeyDialog_CancelButton.Location = new System.Drawing.Point(512, 343);
            this.EditKeyDialog_CancelButton.Name = "EditKeyDialog_CancelButton";
            this.EditKeyDialog_CancelButton.Size = new System.Drawing.Size(75, 23);
            this.EditKeyDialog_CancelButton.TabIndex = 1;
            this.EditKeyDialog_CancelButton.Text = "Cancel";
            this.EditKeyDialog_CancelButton.UseVisualStyleBackColor = true;
            this.EditKeyDialog_CancelButton.Click += new System.EventHandler(this.Close);
            // 
            // EditKeyDialog_OKButton
            // 
            this.EditKeyDialog_OKButton.Location = new System.Drawing.Point(431, 343);
            this.EditKeyDialog_OKButton.Name = "EditKeyDialog_OKButton";
            this.EditKeyDialog_OKButton.Size = new System.Drawing.Size(75, 23);
            this.EditKeyDialog_OKButton.TabIndex = 2;
            this.EditKeyDialog_OKButton.Text = "Ok";
            this.EditKeyDialog_OKButton.UseVisualStyleBackColor = true;
            this.EditKeyDialog_OKButton.Click += new System.EventHandler(this.Continue);
            // 
            // EditKeyDialog_AddStdButton
            // 
            this.EditKeyDialog_AddStdButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.EditKeyDialog_AddStdButton.Location = new System.Drawing.Point(12, 343);
            this.EditKeyDialog_AddStdButton.Name = "EditKeyDialog_AddStdButton";
            this.EditKeyDialog_AddStdButton.Size = new System.Drawing.Size(150, 23);
            this.EditKeyDialog_AddStdButton.TabIndex = 3;
            this.EditKeyDialog_AddStdButton.Text = "Add standard keys";
            this.EditKeyDialog_AddStdButton.UseVisualStyleBackColor = true;
            this.EditKeyDialog_AddStdButton.Click += new System.EventHandler(this.AddStandardKeys);
            // 
            // EditKeyDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(599, 378);
            this.Controls.Add(this.EditKeyDialog_AddStdButton);
            this.Controls.Add(this.EditKeyDialog_OKButton);
            this.Controls.Add(this.EditKeyDialog_CancelButton);
            this.Controls.Add(this.EditKeyDialog_TextBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditKeyDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox EditKeyDialog_TextBox;
        private System.Windows.Forms.Button EditKeyDialog_CancelButton;
        private System.Windows.Forms.Button EditKeyDialog_OKButton;
        private System.Windows.Forms.Button EditKeyDialog_AddStdButton;
    }
}