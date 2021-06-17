using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Timotheus.IO;

namespace Timotheus.Forms
{
    /// <summary>
    /// Dialog that is used to edit a key file as a normal unencrypted text file.
    /// </summary>
    public partial class EditKeyDialog : Form
    {
        /// <summary>
        /// The register as text in the text box. Only updated on Continue/DialogResult.OK.
        /// </summary>
        public string text;

        /// <summary>
        /// The keys usually expected in a key file.
        /// </summary>
        private readonly static string[] std = { 
            "Calendar-Email",
            "Calendar-Password",
            "Calendar-URL",
            "SSH-URL",
            "SSH-Username",
            "SSH-Password",
            "SSH-RemoteDirectory",
            "SSH-LocalDirectory",
            "Settings-Name",
            "Settings-Address",
            "Settings-Image"
        };

        /// <summary>
        /// Opens the edit key dialog.
        /// </summary>
        /// <param name="register">The register to be changed.</param>
        public EditKeyDialog(Register register)
        {
            InitializeComponent();
            text = register.ToString();
            EditKeyDialog_TextBox.Text = text;

            Text = Program.Localization.Get(this) + " (" + register.Name + ")";
            EditKeyDialog_AddStdButton.Text = Program.Localization.Get(EditKeyDialog_AddStdButton);
            EditKeyDialog_OKButton.Text = Program.Localization.Get(EditKeyDialog_OKButton);
            EditKeyDialog_CancelButton.Text = Program.Localization.Get(EditKeyDialog_CancelButton);
        }

        /// <summary>
        /// The dialog closes and the values are assigned.
        /// </summary>
        private void Continue(object sender, EventArgs e)
        {
            text = EditKeyDialog_TextBox.Text;
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
        /// Checks if the text contains all of the standard keys, and if any are missing adds them to the text.
        /// </summary>
        private void AddStandardKeys(object sender, EventArgs e)
        {
            Register register = new Register(':', EditKeyDialog_TextBox.Text);
            List<Key> keys = register.Keys();

            for (int i = 0; i < std.Length; i++)
            {
                bool found = false;

                int j = 0;
                while (j < keys.Count && !found)
                {
                    if (keys[j].name == std[i])
                        found = true;
                    j++;
                }

                if (!found)
                    EditKeyDialog_TextBox.Text += '\n' + std[i] + ':';
            }
        }

        /// <summary>
        /// Processes the hotkeys. Escape closes the dialog.
        /// </summary>
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (ModifierKeys == Keys.None)
            {
                if (keyData == Keys.Escape)
                {
                    Close(null, null);
                    return true;
                }
            }
            return base.ProcessDialogKey(keyData);
        }
    }
}
