using System;
using System.IO;
using System.Windows.Forms;
using Timotheus.Utility;

namespace Timotheus.Forms
{
    /// <summary>
    /// Open calendar dialog with which the user can load calendar data from either an .ics file or a remote calendar.
    /// </summary>
    public partial class OpenCalendar : Form
    {
        /// <summary>
        /// Username/email for the CalDAV.
        /// </summary>
        public string Username;
        /// <summary>
        /// Password to the CalDAV link.
        /// </summary>
        public string Password;
        /// <summary>
        /// Whether the calendar should be loaded from link or .ics file.
        /// </summary>
        public bool Online;
        /// <summary>
        /// CalDAV link to the calendar.
        /// </summary>
        public string CalDAV;
        /// <summary>
        /// Path to the .ics file.
        /// </summary>
        public string ICS;

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

            Text = Localization.Get(this);
            OpenCalendar_OpenButton.Text = Localization.Get(OpenCalendar_OpenButton);
            OpenCalendar_CancelButton.Text = Localization.Get(OpenCalendar_CancelButton);
            OpenCalendar_ICSButton.Text = Localization.Get(OpenCalendar_ICSButton);
            OpenCalendar_CalDAVButton.Text = Localization.Get(OpenCalendar_CalDAVButton);
            OpenCalendar_BrowseButton.Text = Localization.Get(OpenCalendar_BrowseButton);
            OpenCalendar_UsernameLabel.Text = Localization.Get(OpenCalendar_UsernameLabel);
            OpenCalendar_PasswordLabel.Text = Localization.Get(OpenCalendar_PasswordLabel);
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
                Online = OpenCalendar_CalDAVButton.Checked;
                Username = OpenCalendar_UsernameBox.Text;
                Password = OpenCalendar_PasswordBox.Text;
                CalDAV = OpenCalendar_CalDAVBox.Text;
                ICS = OpenCalendar_ICSBox.Text;

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