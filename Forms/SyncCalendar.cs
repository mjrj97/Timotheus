using System;
using System.IO;
using System.Windows.Forms;
using Timotheus.Utility;

namespace Timotheus.Forms
{
    /// <summary>
    /// Synchronize dialog that allows the user to specify how the calendar should be synced and with which remote calendar.
    /// </summary>
    public partial class SyncCalendar : Form
    {
        public bool Calendar_New;
        public string Username;
        public string Password;
        public string CalDAV;
        public int SyncType;
        public DateTime a;
        public DateTime b;

        /// <summary>
        /// Constructor. Loads initial data and loads localization based on culture and directory set by MainWindow.
        /// </summary>
        public SyncCalendar(bool isSetup, string period)
        {
            InitializeComponent();
            SyncCalendar_PasswordBox.PasswordChar = '*';

            string fullName = Path.Combine(Application.StartupPath, "Data.txt");
            if (File.Exists(fullName))
            {
                StreamReader steamReader = new StreamReader(fullName);
                string[] content = steamReader.ReadToEnd().Split("\n");
                steamReader.Close();

                if (content.Length > 0)
                    SyncCalendar_UsernameBox.Text = content[0].Trim();
                if (content.Length > 1)
                    SyncCalendar_PasswordBox.Text = content[1].Trim();
                if (content.Length > 2)
                    SyncCalendar_CalDAVBox.Text = content[2].Trim();
            }

            if (isSetup)
            {
                SyncCalendar_UseExistingButton.Enabled = true;
                SyncCalendar_UseExistingButton.Checked = true;
            }
            else
            {
                SyncCalendar_NewCalendarButton.Checked = true;
                SyncCalendar_CalDAVLabel.Enabled = true;
                SyncCalendar_CalDAVBox.Enabled = true;
                SyncCalendar_UsernameLabel.Enabled = true;
                SyncCalendar_UsernameBox.Enabled = true;
                SyncCalendar_PasswordLabel.Enabled = true;
                SyncCalendar_PasswordBox.Enabled = true;
            }

            LocalizationLoader locale = new LocalizationLoader(Program.directory, Program.culture.Name);
            
            Text = locale.GetLocalization(this);
            SyncCalendar_SyncButton.Text = locale.GetLocalization(SyncCalendar_SyncButton);
            SyncCalendar_CancelButton.Text = locale.GetLocalization(SyncCalendar_CancelButton);
            SyncCalendar_UseExistingButton.Text = locale.GetLocalization(SyncCalendar_UseExistingButton);
            SyncCalendar_NewCalendarButton.Text = locale.GetLocalization(SyncCalendar_NewCalendarButton);
            SyncCalendar_PasswordLabel.Text = locale.GetLocalization(SyncCalendar_PasswordLabel);
            SyncCalendar_UsernameLabel.Text = locale.GetLocalization(SyncCalendar_UsernameLabel);
            SyncCalendar_CalDAVLabel.Text = locale.GetLocalization(SyncCalendar_CalDAVLabel);
            SyncCalendar_PeriodCalendarButton.Text = locale.GetLocalization(SyncCalendar_PeriodCalendarButton) + ": " + period;
            SyncCalendar_EntireCalendarButton.Text = locale.GetLocalization(SyncCalendar_EntireCalendarButton);
            SyncCalendar_CustomCalendarButton.Text = locale.GetLocalization(SyncCalendar_CustomCalendarButton);
        }

        /// <summary>
        /// Syncs the calendar using selected settings and closes the dialog.
        /// </summary>
        private void Sync(object sender, EventArgs e)
        {
            try
            {
                Calendar_New = SyncCalendar_NewCalendarButton.Checked;
                Username = SyncCalendar_UsernameBox.Text;
                Password = SyncCalendar_PasswordBox.Text;
                CalDAV = SyncCalendar_CalDAVBox.Text;

                a = SyncCalendar_aTimePicker.Value.Date;
                b = SyncCalendar_bTimePicker.Value.Date.AddDays(1);

                if (SyncCalendar_EntireCalendarButton.Checked)
                    SyncType = 0;
                else if (SyncCalendar_PeriodCalendarButton.Checked)
                    SyncType = 1;
                else if (SyncCalendar_CustomCalendarButton.Checked)
                    SyncType = 2;

                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                Program.Error(ex.Message, "Exception_Sync");
            }
        }

        /// <summary>
        /// Closes the dialog without syncing.
        /// </summary>
        private void Close(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// Processes the hotkeys. Escape closes the dialog. Enter sends a sync request.
        /// </summary>
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (ModifierKeys == Keys.None)
            {
                if (keyData == Keys.Enter)
                {
                    Sync(null, null);
                    return true;
                }
                else if (keyData == Keys.Escape)
                {
                    Close(null, null);
                    return true;
                }
            }
            return base.ProcessDialogKey(keyData);
        }

        /// <summary>
        /// Enables or disables relevant textboxes when the radio buttons are checked.
        /// </summary>
        private void NewCalendarButton_CheckedChanged(object sender, EventArgs e)
        {
            if (SyncCalendar_NewCalendarButton.Checked)
            {
                SyncCalendar_CalDAVLabel.Enabled = true;
                SyncCalendar_CalDAVBox.Enabled = true;
                SyncCalendar_UsernameLabel.Enabled = true;
                SyncCalendar_UsernameBox.Enabled = true;
                SyncCalendar_PasswordLabel.Enabled = true;
                SyncCalendar_PasswordBox.Enabled = true;
            }
            else
            {
                SyncCalendar_CalDAVLabel.Enabled = false;
                SyncCalendar_CalDAVBox.Enabled = false;
                SyncCalendar_UsernameLabel.Enabled = false;
                SyncCalendar_UsernameBox.Enabled = false;
                SyncCalendar_PasswordLabel.Enabled = false;
                SyncCalendar_PasswordBox.Enabled = false;
            }
        }

        /// <summary>
        /// Enables or disables the DateTimePickers when the radio button is (un)checked.
        /// </summary>
        private void CustomCalendarButton_CheckedChanged(object sender, EventArgs e)
        {
            SyncCalendar_aTimePicker.Enabled = SyncCalendar_CustomCalendarButton.Checked;
            SyncCalendar_bTimePicker.Enabled = SyncCalendar_CustomCalendarButton.Checked;
        }
    }
}