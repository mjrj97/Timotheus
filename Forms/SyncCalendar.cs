using System;
using System.IO;
using System.Windows.Forms;

namespace Timotheus.Forms
{
    public partial class SyncCalendar : Form
    {
        public SyncCalendar()
        {
            InitializeComponent();
            PasswordBox.PasswordChar = '*';

            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string fullName = Path.Combine(desktopPath, "Data.txt");
            if (File.Exists(fullName))
            {
                StreamReader steamReader = new StreamReader(fullName);
                string[] content = steamReader.ReadToEnd().Split("\n");
                steamReader.Close();

                UsernameBox.Text = content[0].Trim();
                PasswordBox.Text = content[1].Trim();
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

        private void Sync(object sender, EventArgs e)
        {
            if (NewCalendarButton.Checked)
            {
                MainWindow.window.calendar.SetupSync(UsernameBox.Text, PasswordBox.Text, CalDAVBox.Text);
            }

            try
            {
                MainWindow.window.calendar.Sync();
                MainWindow.window.UpdateTable();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Sync error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Close(object sender, EventArgs e)
        {
            Close();
        }

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
    }
}
