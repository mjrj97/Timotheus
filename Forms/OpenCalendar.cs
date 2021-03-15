﻿using System;
using System.IO;
using System.Windows.Forms;
using Timotheus.Schedule;

namespace Timotheus.Forms
{
    public partial class OpenCalendar : Form
    {
        public OpenCalendar()
        {
            InitializeComponent();
            PasswordText.PasswordChar = '*';

            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string fullName = Path.Combine(desktopPath, "Data.txt");
            if (File.Exists(fullName))
            {
                StreamReader steamReader = new StreamReader(fullName);
                string[] content = steamReader.ReadToEnd().Split("\n");
                steamReader.Close();

                UsernameText.Text = content[0].Trim();
                PasswordText.Text = content[1].Trim();
                CalDAVText.Text = content[2].Trim();
            }
        }

        private void BrowseButton_Click(object sender, EventArgs e)
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

        private void OpenButton_Click(object sender, EventArgs e)
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

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (ModifierKeys == Keys.None)
            {
                if (keyData == Keys.Enter)
                {
                    OpenButton_Click(null, null);
                    return true;
                }
                else if (keyData == Keys.Escape)
                {
                    CloseButton_Click(null, null);
                    return true;
                }
            }
            return base.ProcessDialogKey(keyData);
        }

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
