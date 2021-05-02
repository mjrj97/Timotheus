using System;
using System.IO;
using System.Windows.Forms;
using Timotheus.Schedule;

namespace Timotheus.Forms
{
    /// <summary>
    /// Open calendar dialog with which the user can load calendar data from either an .ics file or a remote calendar.
    /// </summary>
    public partial class OpenCalendar : Form
    {
        /// <summary>
        /// Loaded calendar. Only changes if LoadCalendar is called.
        /// </summary>
        public Calendar calendar;

        /// <summary>
        /// Constructor. Loads initial data and loads localization based on culture and directory set by MainWindow.
        /// </summary>
        public OpenCalendar()
        {
            InitializeComponent();
            OpenCalendar_PasswordBox.PasswordChar = '*';

            string fullName = Path.Combine(Application.StartupPath, "Data.txt");
            if (File.Exists(fullName))
            {
                StreamReader steamReader = new StreamReader(fullName);
                string[] content = steamReader.ReadToEnd().Split("\n");
                steamReader.Close();

                if (content.Length > 0)
                    OpenCalendar_UsernameBox.Text = content[0].Trim();
                if (content.Length > 1)
                    OpenCalendar_PasswordBox.Text = content[1].Trim();
                if (content.Length > 2)
                    OpenCalendar_CalDAVBox.Text = content[2].Trim();
            }

            Text = Program.Localization.Get(this);
            OpenCalendar_OpenButton.Text = Program.Localization.Get(OpenCalendar_OpenButton);
            OpenCalendar_CancelButton.Text = Program.Localization.Get(OpenCalendar_CancelButton);
            OpenCalendar_ICSButton.Text = Program.Localization.Get(OpenCalendar_ICSButton);
            OpenCalendar_CalDAVButton.Text = Program.Localization.Get(OpenCalendar_CalDAVButton);
            OpenCalendar_BrowseButton.Text = Program.Localization.Get(OpenCalendar_BrowseButton);
            OpenCalendar_UsernameLabel.Text = Program.Localization.Get(OpenCalendar_UsernameLabel);
            OpenCalendar_PasswordLabel.Text = Program.Localization.Get(OpenCalendar_PasswordLabel);
        }

        /// <summary>
        /// Opens dialog where the user can find a .ics file in a local directory.
        /// </summary>
        private void BrowseLocalDirectories(object sender, EventArgs e)
        {
            using OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                Filter = "ics files (*.ics)|*.ics|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = true
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                OpenCalendar_ICSBox.Text = openFileDialog.FileName;
            }
        }

        /// <summary>
        /// Loads the calendar from a .ics file or CalDAV link.
        /// </summary>
        private void LoadCalendar(object sender, EventArgs e)
        {
            try
            {
                if (!OpenCalendar_CalDAVButton.Checked && OpenCalendar_ICSBox.Text == string.Empty)
                    throw new Exception("Exception_EmptyICS");

                if (OpenCalendar_CalDAVButton.Checked)
                    calendar = new Calendar(OpenCalendar_UsernameBox.Text, OpenCalendar_PasswordBox.Text, OpenCalendar_CalDAVBox.Text);
                else
                {
                    string[] lines = File.ReadAllText(OpenCalendar_ICSBox.Text).Replace("\r\n ", "").Split("\n");
                    calendar = new Calendar(lines);
                }

                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                Program.Error("Exception_LoadFailed", ex.Message);
            }
        }

        /// <summary>
        /// Close the dialog without loading a calendar.
        /// </summary>
        private void Close(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// Processes the hotkeys. Escape closes the dialog. Enter opens with the inputted data.
        /// </summary>
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (ModifierKeys == Keys.None)
            {
                if (keyData == Keys.Enter)
                {
                    LoadCalendar(null, null);
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
        /// Enables or disables relevant UI when the radio buttons are checked.
        /// </summary>
        private void CalDAVButton_CheckedChanged(object sender, EventArgs e)
        {
            if (OpenCalendar_CalDAVButton.Checked)
            {
                OpenCalendar_CalDAVBox.Enabled = true;
                OpenCalendar_UsernameLabel.Enabled = true;
                OpenCalendar_UsernameBox.Enabled = true;
                OpenCalendar_PasswordLabel.Enabled = true;
                OpenCalendar_PasswordBox.Enabled = true;

                OpenCalendar_BrowseButton.Enabled = false;
                OpenCalendar_ICSBox.Enabled = false;
            }
            else
            {
                OpenCalendar_CalDAVBox.Enabled = false;
                OpenCalendar_UsernameLabel.Enabled = false;
                OpenCalendar_UsernameBox.Enabled = false;
                OpenCalendar_PasswordLabel.Enabled = false;
                OpenCalendar_PasswordBox.Enabled = false;

                OpenCalendar_BrowseButton.Enabled = true;
                OpenCalendar_ICSBox.Enabled = true;
            }
        }
    }
}