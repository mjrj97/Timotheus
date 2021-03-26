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

            AddMember_NameLabel.Text = locale.GetLocalization(AddMember_NameLabel);
            AddMember_AddressLabel.Text = locale.GetLocalization(AddMember_AddressLabel);
            AddMember_BirthdayLabel.Text = locale.GetLocalization(AddMember_BirthdayLabel);
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
                if (AddMember_NameBox.Text.Trim() == String.Empty)
                    throw new Exception("Name cannot be empty.");

                if (AddMember_AddressBox.Text.Trim() == String.Empty)
                    throw new Exception("Address cannot be empty.");

                new Person(AddMember_NameBox.Text, AddMember_AddressBox.Text, Addmember_BirthdayPicker.Value.Date, AddMember_MemberSincePicker.Value.Date);
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