using System;
using System.Windows.Forms;
using Timotheus.Utility;

namespace Timotheus.Forms
{
    /// <summary>
    /// Add consent form dialog which contains fields that specifies the values of the form.
    /// </summary>
    public partial class AddConsentForm : Form
    {
        public string ConsentForm_Name;
        public DateTime ConsentForm_Signed;
        public DateTime ConsentForm_Version;
        public string ConsentForm_Comment;

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
                if (AddConsentForm_NameBox.Text.Trim() == string.Empty)
                    throw new Exception("Name cannot be empty.");

                ConsentForm_Name = AddConsentForm_NameBox.Text;
                ConsentForm_Signed = AddConsentForm_SignedDate.Value.Date;
                ConsentForm_Version = AddConsentForm_VersionDate.Value.Date;
                ConsentForm_Comment = AddConsentForm_CommentBox.Text;
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                Program.Error(ex.Message, "Exception_InvalidInput");
            }
        }

        /// <summary>
        /// Closes the dialog without adding a consent form.
        /// </summary>
        private void Close(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
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
                    Close(null, null);
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