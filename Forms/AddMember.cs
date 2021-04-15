using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Timotheus.Utility;
using Timotheus.Persons;

namespace Timotheus.Forms
{
    /// <summary>
    /// Dialog for adding a new member in association.
    /// </summary>
    public partial class AddMember : Form
    {
        /// <summary>
        /// Constructor. Loads localization.
        /// </summary>
        public AddMember(List<Person> list, int year)
        {
            InitializeComponent();
            AddMember_ComboBox.DataSource = list;
            AddMember_EntryPicker.Value = new DateTime(year, 1, 1);

            AddMember_AddExistingButton.Text = Localization.Get(AddMember_AddExistingButton);
            AddMember_NewPersonButton.Text = Localization.Get(AddMember_NewPersonButton);
            AddMember_NameLabel.Text = Localization.Get(AddMember_NameLabel);
            AddMember_AddressLabel.Text = Localization.Get(AddMember_AddressLabel);
            AddMember_BirthdayLabel.Text = Localization.Get(AddMember_BirthdayLabel);
            AddMember_EntryLabel.Text = Localization.Get(AddMember_EntryLabel);
            AddMember_AddButton.Text = Localization.Get(AddMember_AddButton);
            AddMember_CancelButton.Text = Localization.Get(AddMember_CancelButton);
        }

        /// <summary>
        /// Adds the member to the persons list and updates the view in Members tab.
        /// </summary>
        private void Add(object sender, EventArgs e)
        {
            try
            {
                if ((AddMember_NewPersonButton.Checked && AddMember_NameBox.Text.Trim() == string.Empty) || (!AddMember_NewPersonButton.Checked && AddMember_ComboBox.SelectedItem == null))
                    throw new Exception("Exception_EmptyName");
                
                if (AddMember_AddressBox.Text.Trim() == string.Empty)
                    throw new Exception("Exception_EmptyAddress");

                if (AddMember_NewPersonButton.Checked)
                    new Person(AddMember_NameBox.Text, AddMember_AddressBox.Text, AddMember_BirthdayPicker.Value.Date, AddMember_EntryPicker.Value.Date);
                else
                {
                    Person person = Person.list[AddMember_ComboBox.SelectedIndex];
                    person.Address = AddMember_AddressBox.Text;
                    person.Birthday = AddMember_BirthdayPicker.Value.Date;
                    person.Entry = AddMember_EntryPicker.Value.Date;
                }

                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                Program.Error("Exception_InvalidInput", ex.Message);
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