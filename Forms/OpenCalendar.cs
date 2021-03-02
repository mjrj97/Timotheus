using System;
using System.IO;
using System.Windows.Forms;
using Timotheus.Schedule;

namespace Timotheus.Forms
{
    public partial class OpenCalendar : Form
    {
        //Constructor
        public OpenCalendar()
        {
            InitializeComponent();
            PasswordText.PasswordChar = '*';

            string fullName = Path.Combine(Application.StartupPath, "Data.txt");
            if (File.Exists(fullName))
            {
                StreamReader steamReader = new StreamReader(fullName);
                string[] content = steamReader.ReadToEnd().Split("\n");
                steamReader.Close();

                if (content.Length > 0)
                    UsernameText.Text = content[0].Trim();
                if (content.Length > 1)
                    PasswordText.Text = content[1].Trim();
                if (content.Length > 2)
                    CalDAVText.Text = content[2].Trim();
            }
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
                ICSText.Text = openFileDialog.FileName;
            }
        }

        //Loads the calendar from a .ics file or CalDAV link
        private void LoadCalendar(object sender, EventArgs e)
        {
            try
            {
                if (CalDAVButton.Checked)
                    MainWindow.window.calendar = new Calendar(UsernameText.Text, PasswordText.Text, CalDAVText.Text);
                else
                    MainWindow.window.calendar = new Calendar(ICSText.Text);

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
            if (CalDAVButton.Checked)
            {
                CalDAVText.Enabled = true;
                UsernameLabel.Enabled = true;
                UsernameText.Enabled = true;
                PasswordLabel.Enabled = true;
                PasswordText.Enabled = true;

                BrowseButton.Enabled = false;
                ICSText.Enabled = false;
            }
            else
            {
                CalDAVText.Enabled = false;
                UsernameLabel.Enabled = false;
                UsernameText.Enabled = false;
                PasswordLabel.Enabled = false;
                PasswordText.Enabled = false;

                BrowseButton.Enabled = true;
                ICSText.Enabled = true;
            }
        }
    }
}