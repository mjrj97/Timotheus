using System;
using System.Windows.Forms;

namespace Timotheus.Forms
{
    /// <summary>
    /// Dialog used to input a password.
    /// </summary>
    public partial class PasswordDialog : Form
    {
        /// <summary>
        /// The password put into the text field. Only updated on Continue/OK.
        /// </summary>
        public string Password;
        /// <summary>
        /// Check if the password should be saved. Only updated on Continue/OK.
        /// </summary>
        public bool Check;

        /// <summary>
        /// Opens a password dialog where the user can input a password.
        /// </summary>
        public PasswordDialog()
        {
            InitializeComponent();
            PasswordDialog_Field.PasswordChar = '*';

            Text = Program.Localization.Get(this);
            PasswordDialog_Label.Text = Program.Localization.Get(PasswordDialog_Label);
            PasswordDialog_SaveBox.Text = Program.Localization.Get(PasswordDialog_SaveBox);
            PasswordDialog_OKButton.Text = Program.Localization.Get(PasswordDialog_OKButton);
            PasswordDialog_CancelButton.Text = Program.Localization.Get(PasswordDialog_CancelButton);
        }

        /// <summary>
        /// The dialog closes and the values are assigned.
        /// </summary>
        private void Continue(object sender, EventArgs e)
        {
            Check = PasswordDialog_SaveBox.Checked;
            Password = PasswordDialog_Field.Text;
            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// The dialog is closed without using the values.
        /// </summary>
        private void Close(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// Processes the hotkeys. Escape closes the dialog. Enter opens with the inputted data.
        /// </summary>
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (ModifierKeys == Keys.None)
            {
                if (keyData == Keys.Enter)
                {
                    Continue(null, null);
                    return true;
                }
                else if (keyData == Keys.Escape)
                {
                    Close(null, null);
                    return true;
                }
            }
            return base.ProcessDialogKey(keyData);
        }
    }
}