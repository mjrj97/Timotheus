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
            Sync_PasswordBox.PasswordChar = '*';

            string fullName = Path.Combine(Application.StartupPath, "Data.txt");
            if (File.Exists(fullName))
            {
                StreamReader steamReader = new StreamReader(fullName);
                string[] content = steamReader.ReadToEnd().Split("\n");
                steamReader.Close();

                if (content.Length > 0)
                    Sync_UsernameBox.Text = content[0].Trim();
                if (content.Length > 1)
                    Sync_PasswordBox.Text = content[1].Trim();
                if (content.Length > 2)
                    Sync_CalDAVBox.Text = content[2].Trim();
            }

            if (MainWindow.window.calendar.IsSetup())
            {
                Sync_UseExistingButton.Enabled = true;
                Sync_UseExistingButton.Checked = true;
            }
            else
            {
                Sync_NewCalendarButton.Checked = true;
                Sync_CalDAVLabel.Enabled = true;
                Sync_CalDAVBox.Enabled = true;
                Sync_UsernameLabel.Enabled = true;
                Sync_UsernameBox.Enabled = true;
                Sync_PasswordLabel.Enabled = true;
                Sync_PasswordBox.Enabled = true;
            }

            LocalizationLoader locale = new LocalizationLoader(MainWindow.directory, MainWindow.culture);
            
            Text = locale.GetLocalization(this);
            Sync_SyncButton.Text = locale.GetLocalization(Sync_SyncButton);
            Sync_CancelButton.Text = locale.GetLocalization(Sync_CancelButton);
            Sync_UseExistingButton.Text = locale.GetLocalization(Sync_UseExistingButton);
            Sync_NewCalendarButton.Text = locale.GetLocalization(Sync_NewCalendarButton);
            Sync_PasswordLabel.Text = locale.GetLocalization(Sync_PasswordLabel);
            Sync_UsernameLabel.Text = locale.GetLocalization(Sync_UsernameLabel);
            Sync_CalDAVLabel.Text = locale.GetLocalization(Sync_CalDAVLabel);
            Sync_PeriodCalendarButton.Text = locale.GetLocalization(Sync_PeriodCalendarButton) + ": " + MainWindow.window.Calendar_PeriodBox.Text;
            Sync_EntireCalendarButton.Text = locale.GetLocalization(Sync_EntireCalendarButton);
            Sync_CustomCalendarButton.Text = locale.GetLocalization(Sync_CustomCalendarButton);
        }

        /// <summary>
        /// Syncs the calendar using selected settings and closes the dialog.
        /// </summary>
        private void Sync(object sender, EventArgs e)
        {
            if (Sync_NewCalendarButton.Checked)
            {
                MainWindow.window.calendar.SetupSync(Sync_UsernameBox.Text, Sync_PasswordBox.Text, Sync_CalDAVBox.Text);
            }

            try
            {
                if (Sync_EntireCalendarButton.Checked)
                    MainWindow.window.calendar.Sync();
                else if (Sync_PeriodCalendarButton.Checked)
                    MainWindow.window.calendar.Sync(MainWindow.window.a, MainWindow.window.b);
                else if (Sync_CustomCalendarButton.Checked)
                {
                    DateTime a = Sync_aTimePicker.Value;
                    DateTime b = Sync_bTimePicker.Value.AddDays(1);

                    MainWindow.window.calendar.Sync(new DateTime(a.Year, a.Month, a.Day), new DateTime(b.Year, b.Month, b.Day));
                }

                MainWindow.window.UpdateTable();
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
            if (Sync_NewCalendarButton.Checked)
            {
                Sync_CalDAVLabel.Enabled = true;
                Sync_CalDAVBox.Enabled = true;
                Sync_UsernameLabel.Enabled = true;
                Sync_UsernameBox.Enabled = true;
                Sync_PasswordLabel.Enabled = true;
                Sync_PasswordBox.Enabled = true;
            }
            else
            {
                Sync_CalDAVLabel.Enabled = false;
                Sync_CalDAVBox.Enabled = false;
                Sync_UsernameLabel.Enabled = false;
                Sync_UsernameBox.Enabled = false;
                Sync_PasswordLabel.Enabled = false;
                Sync_PasswordBox.Enabled = false;
            }
        }

        /// <summary>
        /// Enables or disables the DateTimePickers when the radio button is (un)checked.
        /// </summary>
        private void CustomCalendarButton_CheckedChanged(object sender, EventArgs e)
        {
            Sync_aTimePicker.Enabled = Sync_CustomCalendarButton.Checked;
            Sync_bTimePicker.Enabled = Sync_CustomCalendarButton.Checked;
        }
    }
}