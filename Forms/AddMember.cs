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
            Person person = new Person(AddMember_NameBox.Text, AddMember_AddressBox.Text, Addmember_BirthdayCalendar.SelectionStart, AddMember_MemberSinceCalender.SelectionStart);
            MainWindow.window.Persons.Add(person);
            MainWindow.window.UpdateMemberTable();
            Close();
        }

        private void AddMember_CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Addmember_BirthdayCalendar_DateChanged(object sender, DateRangeEventArgs e)
        {
            AddMember_BirthdayDateLabel.Text = Addmember_BirthdayCalendar.SelectionStart.ToString("d-M-yyyy", CultureInfo.CreateSpecificCulture(Program.culture));
        }

        private void AddMember_MemberSinceCalender_DateChanged(object sender, DateRangeEventArgs e)
        {
            AddMember_MemberSinceDateLabel.Text = AddMember_MemberSinceCalender.SelectionStart.ToString("d-M-yyyy", CultureInfo.CreateSpecificCulture(Program.culture));
        }
    }
}
