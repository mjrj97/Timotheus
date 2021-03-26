using System;
using System.Windows.Forms;
using Timotheus.Persons;
using Timotheus.Utility;

namespace Timotheus.Forms
{
    /// <summary>
    /// Add consent form dialog which contains fields that specifies the values of the form.
    /// </summary>
    public partial class AddConsentForm : Form
    {
        /// <summary>
        /// Constructor. Loads the localization for the dialog.
        /// </summary>
        public AddConsentForm()
        {
            InitializeComponent();

            LocalizationLoader locale = new LocalizationLoader(Program.directory, Program.culture.Name);

            Text = locale.GetLocalization(this);
            AddConsentForm_AddButton.Text = locale.GetLocalization(AddConsentForm_AddButton);
            AddConsentForm_CancelButton.Text = locale.GetLocalization(AddConsentForm_CancelButton);
            AddConsentForm_NameLabel.Text = locale.GetLocalization(AddConsentForm_NameLabel);
            AddConsentForm_SignedLabel.Text = locale.GetLocalization(AddConsentForm_SignedLabel);
            AddConsentForm_VersionLabel.Text = locale.GetLocalization(AddConsentForm_VersionLabel);
            AddConsentForm_CommentLabel.Text = locale.GetLocalization(AddConsentForm_CommentLabel);
        }

        /// <summary>
        /// Adds the consent form to the consentForms list in MainWindow.
        /// </summary>
        private void Add(object sender, EventArgs e)
        {
            try
            {
                new Person(AddConsentForm_NameBox.Text, AddConsentForm_SignedDate.Value, AddConsentForm_VersionDate.Value, AddConsentForm_CommentBox.Text);
                MainWindow.window.UpdateConsentFormsTable();
                Close();
            }
            catch (Exception ex)
            {
                Program.Error(ex.Message, "Exception_InvalidInput");
            }
        }

        /// <summary>
        /// Closes the dialog without adding a consent form.
        /// </summary>
        private void Cancel(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Processes the hotkeys. Escape closes the dialog. Enter adds the consent form.
        /// </summary>
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (ModifierKeys == Keys.None)
            {
                if (keyData == Keys.Escape)
                {
                    Cancel(null, null);
                    return true;
                }
                else if (keyData == Keys.Enter)
                {
                    Add(null, null);
                    return true;
                }
            }
            return base.ProcessDialogKey(keyData);
        }
    }
}