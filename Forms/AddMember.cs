using System;
using System.Windows.Forms;
using Timotheus.Utility;
using Timotheus.Persons;

namespace Timotheus.Forms
{
    public partial class AddMember : Form
    {
        /// <summary>
        /// Constructor. Loads localization.
        /// </summary>
        public AddMember()
        {
            InitializeComponent();
            LocalizationLoader locale = new LocalizationLoader(Program.directory, Program.culture.Name);
            AddMember_ComboBox.DataSource = Person.list;

            AddMember_AddExistingButton.Text = locale.GetLocalization(AddMember_AddExistingButton);
            AddMember_NewPersonButton.Text = locale.GetLocalization(AddMember_NewPersonButton);
            AddMember_NameLabel.Text = locale.GetLocalization(AddMember_NameLabel);
            AddMember_AddressLabel.Text = locale.GetLocalization(AddMember_AddressLabel);
            AddMember_BirthdayLabel.Text = locale.GetLocalization(AddMember_BirthdayLabel);
            AddMember_EntryLabel.Text = locale.GetLocalization(AddMember_EntryLabel);
            AddMember_AddButton.Text = locale.GetLocalization(AddMember_AddButton);
            AddMember_CancelButton.Text = locale.GetLocalization(AddMember_CancelButton);
        }

        /// <summary>
        /// Adds the member to the persons list and updates the view in Members tab.
        /// </summary>
        private void AddButton(object sender, EventArgs e)
        {
            try
            {
                if (AddMember_AddressBox.Text.Trim() == string.Empty)
                    throw new Exception("Address cannot be empty.");

                if (AddMember_NewPersonButton.Checked)
                {
                    if (AddMember_NameBox.Text.Trim() == string.Empty)
                        throw new Exception("Name cannot be empty.");

                    new Person(AddMember_NameBox.Text, AddMember_AddressBox.Text, AddMember_BirthdayPicker.Value.Date, AddMember_EntryPicker.Value.Date);
                }
                else
                {
                    if (AddMember_ComboBox.SelectedItem == null)
                        throw new Exception("Name cannot be empty.");

                    Person person = (Person)AddMember_ComboBox.SelectedItem;
                    person.Address = AddMember_AddressBox.Text;
                    person.Birthday = AddMember_BirthdayPicker.Value.Date;
                    person.Entry = AddMember_EntryPicker.Value.Date;
                }

                MainWindow.window.UpdateMemberTable();
                CloseButton(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Closes the dialog.
        /// </summary>
        private void CloseButton(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Updates fields if value in ComboBox is changed.
        /// </summary>
        private void ComboBoxChange(object sender, EventArgs e)
        {
            if (AddMember_ComboBox.SelectedItem != null && AddMember_AddExistingButton.Checked)
            {
                Person person = (Person)AddMember_ComboBox.SelectedItem;
                AddMember_AddressBox.Text = person.Address;
                AddMember_BirthdayPicker.Value = person.Birthday;
                AddMember_EntryPicker.Value = person.Entry;
            }
        }

        /// <summary>
        /// Updates content if radio buttons changes.
        /// </summary>
        private void RadioButtonsChanged(object sender, EventArgs e)
        {
            AddMember_ComboBox.Visible = AddMember_AddExistingButton.Checked;
            AddMember_NameBox.Visible = !AddMember_AddExistingButton.Checked;

            if (AddMember_AddExistingButton.Checked)
                ComboBoxChange(null, null);
            else
            {
                AddMember_AddressBox.Text = string.Empty;
                AddMember_EntryPicker.Value = DateTime.Now;
                AddMember_BirthdayPicker.Value = DateTime.Now;
            }
        }

        /// <summary>
        /// Processes the hotkeys. Escape closes the dialog. Enter adds the member.
        /// </summary>
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (ModifierKeys == Keys.None)
            {
                if (keyData == Keys.Escape)
                {
                    CloseButton(null, null);
                    return true;
                }
                else if (keyData == Keys.Enter)
                {
                    AddButton(null, null);
                    return true;
                }
            }
            return base.ProcessDialogKey(keyData);
        }
    }
}