using System;
using System.Windows.Forms;
using System.Globalization;
using Timotheus.Utility;
using Timotheus.Persons;

namespace Timotheus.Forms
{
    public partial class AddMember : Form
    {
        public AddMember()
        {
            InitializeComponent();

            LocalizationLoader locale = new LocalizationLoader(Program.directory, Program.culture);

            AddMember_NameLabel.Text = locale.GetLocalization(AddMember_NameLabel);
            AddMember_AddressLabel.Text = locale.GetLocalization(AddMember_AddressLabel);
            AddMember_BirthdayLabel.Text = locale.GetLocalization(AddMember_BirthdayLabel);
            AddMember_AddButton.Text = locale.GetLocalization(AddMember_AddButton);
            AddMember_CancelButton.Text = locale.GetLocalization(AddMember_CancelButton);
        }

        private void AddMember_AddButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (AddMember_NameBox.Text.Trim() == String.Empty)
                    throw new Exception("Name cannot be empty.");

                if (AddMember_AddressBox.Text.Trim() == String.Empty)
                    throw new Exception("Address cannot be empty.");

                Person person = new Person(AddMember_NameBox.Text, AddMember_AddressBox.Text, Addmember_BirthdayPicker.Value.Date, AddMember_MemberSincePicker.Value.Date);
                MainWindow.window.Persons.Add(person);
                MainWindow.window.UpdateMemberTable();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddMember_CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Addmember_BirthdayCalendar_DateChanged(object sender, DateRangeEventArgs e)
        {
            AddMember_BirthdayDateLabel.Text = Addmember_BirthdayPicker.Value.ToString("d-M-yyyy", CultureInfo.CreateSpecificCulture(Program.culture));
        }

        private void AddMember_MemberSinceCalender_DateChanged(object sender, DateRangeEventArgs e)
        {
            AddMember_MemberSinceDateLabel.Text = AddMember_MemberSincePicker.Value.ToString("d-M-yyyy", CultureInfo.CreateSpecificCulture(Program.culture));
        }
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (ModifierKeys == Keys.None)
            {
                if (keyData == Keys.Escape)
                {
                    AddMember_CancelButton_Click(null, null);
                    return true;
                }
                else if (keyData == Keys.Enter)
                {
                    AddMember_AddButton_Click(null, null);
                    return true;
                }
            }
            return base.ProcessDialogKey(keyData);
        }
    }
}
