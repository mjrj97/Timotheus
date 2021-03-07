using System;
using System.IO;
using System.Windows.Forms;

namespace Timotheus.Forms
{
    public partial class SyncCalendar : Form
    {
        //Constructor
        public SyncCalendar()
        {
            InitializeComponent();
            PasswordBox.PasswordChar = '*';
            PeriodCalendarButton.Text = "Sync the period: " + MainWindow.window.Calendar_PeriodBox.Text;

            string fullName = Path.Combine(Application.StartupPath, "Data.txt");
            if (File.Exists(fullName))
            {
                StreamReader steamReader = new StreamReader(fullName);
                string[] content = steamReader.ReadToEnd().Split("\n");
                steamReader.Close();

                if (content.Length > 0)
                    UsernameBox.Text = content[0].Trim();
                if (content.Length > 1)
                    PasswordBox.Text = content[1].Trim();
                if (content.Length > 2)
                    CalDAVBox.Text = content[2].Trim();
            }

            if (MainWindow.window.calendar.IsSetup())
            {
                UseExistingButton.Enabled = true;
                UseExistingButton.Checked = true;
            }
            else
            {
                NewCalendarButton.Checked = true;
                CalDAVLabel.Enabled = true;
                CalDAVBox.Enabled = true;
                UsernameLabel.Enabled = true;
                UsernameBox.Enabled = true;
                PasswordLabel.Enabled = true;
                PasswordBox.Enabled = true;
            }
        }

        //Syncs the calendar using selected settings
        private void Sync(object sender, EventArgs e)
        {
            if (NewCalendarButton.Checked)
            {
                MainWindow.window.calendar.SetupSync(UsernameBox.Text, PasswordBox.Text, CalDAVBox.Text);
            }

            try
            {
                if (EntireCalendarButton.Checked)
                    MainWindow.window.calendar.Sync();
                else if (PeriodCalendarButton.Checked)
                    MainWindow.window.calendar.Sync(MainWindow.window.a, MainWindow.window.b);
                else if (CustomCalendarButton.Checked)
                {
                    DateTime a = aTimePicker.Value;
                    DateTime b = bTimePicker.Value.AddDays(1);

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

        //Closes the dialog without syncing
        private void Close(object sender, EventArgs e)
        {
            Close();
        }

		//Processes the hotkeys
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

        //Enables or disables relevant UI when the radio buttons are checked
        private void NewCalendarButton_CheckedChanged(object sender, EventArgs e)
        {
            if (NewCalendarButton.Checked)
            {
                CalDAVLabel.Enabled = true;
                CalDAVBox.Enabled = true;
                UsernameLabel.Enabled = true;
                UsernameBox.Enabled = true;
                PasswordLabel.Enabled = true;
                PasswordBox.Enabled = true;
            }
            else
            {
                CalDAVLabel.Enabled = false;
                CalDAVBox.Enabled = false;
                UsernameLabel.Enabled = false;
                UsernameBox.Enabled = false;
                PasswordLabel.Enabled = false;
                PasswordBox.Enabled = false;
            }
        }

        private void CustomCalendarButton_CheckedChanged(object sender, EventArgs e)
        {
            aTimePicker.Enabled = CustomCalendarButton.Checked;
            bTimePicker.Enabled = CustomCalendarButton.Checked;
        }
    }
}