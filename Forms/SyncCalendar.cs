﻿using System;
using System.IO;
using System.Windows.Forms;
using Timotheus.Schedule;
using Timotheus.Utility;

namespace Timotheus.Forms
{
    /// <summary>
    /// Synchronize dialog that allows the user to specify how the calendar should be synced and with which remote calendar.
    /// </summary>
    public partial class SyncCalendar : Form
    {
        /// <summary>
        /// Calendar to sync. Is assigned by the constructor.
        /// </summary>
        private readonly Calendar calendar;

        /// <summary>
        /// Constructor. Loads initial data and loads localization based on culture and directory set by MainWindow.
        /// </summary>
        public SyncCalendar(Calendar calendar, string period)
        {
            this.calendar = calendar;
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

            if (calendar.IsSetup())
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

           Text = Localization.Get(this);
            SyncCalendar_SyncButton.Text = Localization.Get(SyncCalendar_SyncButton);
            SyncCalendar_CancelButton.Text = Localization.Get(SyncCalendar_CancelButton);
            SyncCalendar_UseExistingButton.Text = Localization.Get(SyncCalendar_UseExistingButton);
            SyncCalendar_NewCalendarButton.Text = Localization.Get(SyncCalendar_NewCalendarButton);
            SyncCalendar_PasswordLabel.Text = Localization.Get(SyncCalendar_PasswordLabel);
            SyncCalendar_UsernameLabel.Text = Localization.Get(SyncCalendar_UsernameLabel);
            SyncCalendar_CalDAVLabel.Text = Localization.Get(SyncCalendar_CalDAVLabel);
            SyncCalendar_PeriodCalendarButton.Text = Localization.Get(SyncCalendar_PeriodCalendarButton) + ": " + period;
            SyncCalendar_EntireCalendarButton.Text = Localization.Get(SyncCalendar_EntireCalendarButton);
            SyncCalendar_CustomCalendarButton.Text = Localization.Get(SyncCalendar_CustomCalendarButton);
        }

        /// <summary>
        /// Syncs the calendar using selected settings and closes the dialog.
        /// </summary>
        private void Sync(object sender, EventArgs e)
        {
            try
            {
                if (SyncCalendar_NewCalendarButton.Checked)
                    calendar.SetupSync(SyncCalendar_UsernameBox.Text, SyncCalendar_PasswordBox.Text, SyncCalendar_CalDAVBox.Text);

                if (SyncCalendar_EntireCalendarButton.Checked)
                    calendar.Sync();
                else if (SyncCalendar_PeriodCalendarButton.Checked)
                    calendar.Sync(DateTime.Now, DateTime.Now);
                else if (SyncCalendar_CustomCalendarButton.Checked)
                    calendar.Sync(SyncCalendar_aTimePicker.Value.Date, SyncCalendar_bTimePicker.Value.Date.AddDays(1));

                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                Program.Error("Exception_Sync", ex.Message);
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