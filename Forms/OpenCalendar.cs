using System;
using System.IO;
using System.Windows.Forms;
using Timotheus.Schedule;
using Timotheus.Utility;

namespace Timotheus.Forms
{
    public partial class OpenCalendar : Form
    {
        //Constructor
        public OpenCalendar()
        {
            InitializeComponent();
            Open_PasswordBox.PasswordChar = '*';

            string fullName = Path.Combine(Application.StartupPath, "Data.txt");
            if (File.Exists(fullName))
            {
                StreamReader steamReader = new StreamReader(fullName);
                string[] content = steamReader.ReadToEnd().Split("\n");
                steamReader.Close();

                if (content.Length > 0)
                    Open_UsernameBox.Text = content[0].Trim();
                if (content.Length > 1)
                    Open_PasswordBox.Text = content[1].Trim();
                if (content.Length > 2)
                    Open_CalDAVBox.Text = content[2].Trim();
            }

            LocalizationLoader locale = new LocalizationLoader(MainWindow.directory, MainWindow.culture);

            Text = locale.GetLocalization(this);
            Open_OpenButton.Text = locale.GetLocalization(Open_OpenButton);
            Open_CancelButton.Text = locale.GetLocalization(Open_CancelButton);
            Open_ICSButton.Text = locale.GetLocalization(Open_ICSButton);
            Open_CalDAVButton.Text = locale.GetLocalization(Open_CalDAVButton);
            Open_BrowseButton.Text = locale.GetLocalization(Open_BrowseButton);
            Open_UsernameLabel.Text = locale.GetLocalization(Open_UsernameLabel);
            Open_PasswordLabel.Text = locale.GetLocalization(Open_PasswordLabel);
        }

        //Opens dialog where the user can find a .ics file
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
                Open_ICSBox.Text = openFileDialog.FileName;
            }
        }

        //Loads the calendar from a .ics file or CalDAV link
        private void LoadCalendar(object sender, EventArgs e)
        {
            try
            {
                if (Open_CalDAVButton.Checked)
                    MainWindow.window.calendar = new Calendar(Open_UsernameBox.Text, Open_PasswordBox.Text, Open_CalDAVBox.Text);
                else
                    MainWindow.window.calendar = new Calendar(Open_ICSBox.Text);

                MainWindow.window.UpdateTable();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Close();
        }

        //Close the dialog without loading a calendar
        private void CloseDialog(object sender, EventArgs e)
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
                    LoadCalendar(null, null);
                    return true;
                }
                else if (keyData == Keys.Escape)
                {
                    CloseDialog(null, null);
                    return true;
                }
            }
            return base.ProcessDialogKey(keyData);
        }

        //Enables or disables relevant UI when the radio buttons are checked
        private void CalDAVButton_CheckedChanged(object sender, EventArgs e)
        {
            if (Open_CalDAVButton.Checked)
            {
                Open_CalDAVBox.Enabled = true;
                Open_UsernameLabel.Enabled = true;
                Open_UsernameBox.Enabled = true;
                Open_PasswordLabel.Enabled = true;
                Open_PasswordBox.Enabled = true;

                Open_BrowseButton.Enabled = false;
                Open_ICSBox.Enabled = false;
            }
            else
            {
                Open_CalDAVBox.Enabled = false;
                Open_UsernameLabel.Enabled = false;
                Open_UsernameBox.Enabled = false;
                Open_PasswordLabel.Enabled = false;
                Open_PasswordBox.Enabled = false;

                Open_BrowseButton.Enabled = true;
                Open_ICSBox.Enabled = true;
            }
        }
    }
}