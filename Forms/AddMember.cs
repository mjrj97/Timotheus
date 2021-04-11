using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Timotheus.Utility;
using Timotheus.Persons;

namespace Timotheus.Forms
{
    public partial class AddMember : Form
    {
        public bool Member_New;
        public int Member_Index;
        public string Member_Name;
        public string Member_Address;
        public DateTime Member_Birthday;
        public DateTime Member_Entry;

        /// <summary>
        /// Constructor. Loads localization.
        /// </summary>
        public AddMember(List<Person> list, int year)
        {
            InitializeComponent();
            AddMember_ComboBox.DataSource = list;
            AddMember_EntryPicker.Value = new DateTime(year, 1, 1);

            LocalizationLoader locale = new LocalizationLoader(Program.directory, Program.culture.Name);

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
        private void Add(object sender, EventArgs e)
        {
            try
            {
                if ((AddMember_NewPersonButton.Checked && AddMember_NameBox.Text.Trim() == string.Empty) || (!AddMember_NewPersonButton.Checked && AddMember_ComboBox.SelectedItem == null))
                    throw new Exception("Name cannot be empty.");
                
                if (AddMember_AddressBox.Text.Trim() == string.Empty)
                    throw new Exception("Address cannot be empty.");

                Member_New = AddMember_NewPersonButton.Checked;
                Member_Name = AddMember_NameBox.Text;
                Member_Address = AddMember_AddressBox.Text;
                Member_Birthday = AddMember_BirthdayPicker.Value.Date;
                Member_Entry = AddMember_EntryPicker.Value.Date;

                if (!AddMember_NewPersonButton.Checked)
                    Member_Index = AddMember_ComboBox.SelectedIndex;

                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                Program.Error(ex.Message, "Invalid input");
            }
        }

        /// <summary>
        /// Closes the dialog.
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