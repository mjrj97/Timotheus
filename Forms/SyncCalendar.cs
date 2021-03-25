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
        /// <summary>
        /// Constructor. Loads initial data and loads localization based on culture and directory set by MainWindow.
        /// </summary>
        public SyncCalendar()
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

            if (MainWindow.window.calendar.IsSetup())
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
            SyncCalendar_PeriodCalendarButton.Text = locale.GetLocalization(SyncCalendar_PeriodCalendarButton) + ": " + MainWindow.window.Calendar_PeriodBox.Text;
            SyncCalendar_EntireCalendarButton.Text = locale.GetLocalization(SyncCalendar_EntireCalendarButton);
            SyncCalendar_CustomCalendarButton.Text = locale.GetLocalization(SyncCalendar_CustomCalendarButton);
        }

        /// <summary>
        /// Syncs the calendar using selected settings and closes the dialog.
        /// </summary>
        private void Sync(object sender, EventArgs e)
        {
            if (SyncCalendar_NewCalendarButton.Checked)
            {
                MainWindow.window.calendar.SetupSync(SyncCalendar_UsernameBox.Text, SyncCalendar_PasswordBox.Text, SyncCalendar_CalDAVBox.Text);
            }

            try
            {
                if (SyncCalendar_EntireCalendarButton.Checked)
                    MainWindow.window.calendar.Sync();
                else if (SyncCalendar_PeriodCalendarButton.Checked)
                    MainWindow.window.calendar.Sync(MainWindow.window.StartPeriod, MainWindow.window.EndPeriod);
                else if (SyncCalendar_CustomCalendarButton.Checked)
                {
                    DateTime a = SyncCalendar_aTimePicker.Value;
                    DateTime b = SyncCalendar_bTimePicker.Value.AddDays(1);

                    MainWindow.window.calendar.Sync(new DateTime(a.Year, a.Month, a.Day), new DateTime(b.Year, b.Month, b.Day));
                }

                MainWindow.window.UpdateCalendarTable();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Sync error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Closes the dialog without syncing.
        /// </summary>
        private void Close(object sender, EventArgs e)
        {
            Close();
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