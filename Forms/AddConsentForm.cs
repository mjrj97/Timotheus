using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Timotheus.Utility;
using Timotheus.Persons;

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
        public AddConsentForm(List<Person> list)
        {
            InitializeComponent();
            AddConsentForm_ComboBox.DataSource = list;

            Text = Localization.Get(this);
            AddConsentForm_AddButton.Text = Localization.Get(AddConsentForm_AddButton);
            AddConsentForm_CancelButton.Text = Localization.Get(AddConsentForm_CancelButton);
            AddConsentForm_NameLabel.Text = Localization.Get(AddConsentForm_NameLabel);
            AddConsentForm_SignedLabel.Text = Localization.Get(AddConsentForm_SignedLabel);
            AddConsentForm_VersionLabel.Text = Localization.Get(AddConsentForm_VersionLabel);
            AddConsentForm_CommentLabel.Text = Localization.Get(AddConsentForm_CommentLabel);
        }

        /// <summary>
        /// Adds the consent form to the consentForms list in MainWindow.
        /// </summary>
        private void Add(object sender, EventArgs e)
        {
            try
            {
                if (AddConsentForm_NameBox.Text.Trim() == string.Empty)
                    throw new Exception("Exception_EmptyName");

                if (AddConsentForm_NewButton.Checked)
                    new Person(AddConsentForm_NameBox.Text, AddConsentForm_SignedDate.Value.Date, AddConsentForm_VersionDate.Value.Date, AddConsentForm_CommentBox.Text);
                else
                {
                    Person person = Person.list[AddConsentForm_ComboBox.SelectedIndex];
                    person.Name = AddConsentForm_NameBox.Text;
                    person.Signed = AddConsentForm_SignedDate.Value.Date;
                    person.Version = AddConsentForm_VersionDate.Value.Date;
                    person.Comment = AddConsentForm_CommentBox.Text;
                }

                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                Program.Error("Exception_InvalidInput", ex.Message);
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
        /// Updates fields if value in ComboBox is changed.
        /// </summary>
        private void ComboBoxChange(object sender, EventArgs e)
        {
            if (AddConsentForm_ComboBox.SelectedItem != null && AddConsentForm_ExistingButton.Checked)
            {
                Person person = (Person)AddConsentForm_ComboBox.SelectedItem;
                AddConsentForm_NameBox.Text = person.Address;
                if (person.Signed != DateTime.MinValue)
                    AddConsentForm_SignedDate.Value = person.Signed;
                if (person.Version != DateTime.MinValue)
                    AddConsentForm_VersionDate.Value = person.Version;
                AddConsentForm_CommentBox.Text = person.Comment;
            }
        }

        /// <summary>
        /// Updates content if radio buttons changes.
        /// </summary>
        private void RadioButtonsChanged(object sender, EventArgs e)
        {
            AddConsentForm_ComboBox.Visible = AddConsentForm_ExistingButton.Checked;
            AddConsentForm_NameBox.Visible = !AddConsentForm_ExistingButton.Checked;

            if (AddConsentForm_ExistingButton.Checked)
                ComboBoxChange(null, null);
            else
            {
                AddConsentForm_NameBox.Text = string.Empty;
                AddConsentForm_CommentBox.Text = string.Empty;
                AddConsentForm_SignedDate.Value = DateTime.Now;
                AddConsentForm_VersionDate.Value = DateTime.Now;
            }
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