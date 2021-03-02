using Timotheus.Schedule;
using Timotheus.Utility;
using System;
using System.Text;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Net;
using Renci.SshNet.Sftp;
using Renci.SshNet;

namespace Timotheus.Forms
{
    public partial class MainWindow : Form
    {
        public static MainWindow window;
        public SortableBindingList<Event> shownEvents = new SortableBindingList<Event>();
        public SortableBindingList<SftpFile> shownFiles = new SortableBindingList<SftpFile>();

        private int year;
        public Calendar calendar = new Calendar();
        
        //Constructor
        public MainWindow()
        {
            window = this;
            year = DateTime.Now.Year;
            InitializeComponent();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            Year.Text = year.ToString();
            CalendarView.DataSource = new BindingSource(shownEvents, null);
            FileView.AutoGenerateColumns = false;
            FileView.DataSource = new BindingSource(shownFiles, null);
            PasswordBox.PasswordChar = '*';

            string fullName = Path.Combine(Application.StartupPath, "Data.txt");
            if (File.Exists(fullName))
            {
                StreamReader steamReader = new StreamReader(fullName);
                string[] content = steamReader.ReadToEnd().Split("\n");
                steamReader.Close();

                if (content.Length > 4)
                    UsernameBox.Text = content[4].Trim();
                if (content.Length > 5)
                    PasswordBox.Text = content[5].Trim();
                if (content.Length > 3)
                    HostBox.Text = content[3].Trim();
                if (content.Length > 6)
                    RemoteDirectoryBox.Text = content[6].Trim();
                if (content.Length > 7)
                    LocalDirectoryBox.Text = content[7].Trim();
            }
        }

        #region Calendar

        //Changes the selected year and updates calls UpdateTable
        private void UpdateYear(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            if (button.Text == "+")
                year++;
            else if (button.Text == "-")
                year--;
            Year.Text = year.ToString();
            UpdateTable();
        }

        //Updates the contents of the event table
        public void UpdateTable()
        {
            shownEvents.Clear();
            for (int i = 0; i < calendar.events.Count; i++)
            {
                if (calendar.events[i].StartTime.Year == year && !calendar.events[i].Deleted)
                    shownEvents.Add(calendar.events[i]);
            }
            CalendarView.Sort(CalendarView.Columns[0], ListSortDirection.Ascending);
        }

        //Opens dialog where the user can define the attributes of the new event
        private void AddEvent(object sender, EventArgs e)
        {
            AddEvent addEvent = new AddEvent
            {
                Owner = this
            };
            addEvent.ShowDialog();
        }

        //Removes the selected event
        private void RemoveEvent(object sender, EventArgs e)
        {
            if (shownEvents.Count > 0)
            {
                Event ev = shownEvents[CalendarView.CurrentCell.OwningRow.Index];
                int index = 0;
                for (int i = 0; i < calendar.events.Count; i++)
                {
                    if (ev.Equals(calendar.events[i]))
                        index = i;
                }
                calendar.events[index].Deleted = true;
                UpdateTable();
            }
        }

        //Opens dialog and saves the current calendar as .ics
        private void SaveCalendar(object sender, EventArgs e)
        {
            Stream stream;
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "iCalendar files (*.ics)|*.ics"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                if ((stream = saveFileDialog.OpenFile()) != null)
                {
                    byte[] data = Encoding.UTF8.GetBytes(calendar.GetCalendarICS(Path.GetFileNameWithoutExtension(saveFileDialog.FileName)));
                    stream.Write(data);
                    stream.Close();
                }
            }
        }

        //Opens dialog where the user can open the calendar from a .ics file or CalDAV link
        private void OpenCalendar(object sender, EventArgs e)
        {
            OpenCalendar open = new OpenCalendar
            {
                Owner = this
            };
            open.ShowDialog();
        }

        //Opens dialog where the user can sync the current calendar with a remote calendar
        private void SyncCalendar(object sender, EventArgs e)
        {
            SyncCalendar sync = new SyncCalendar
            {
                Owner = this
            };
            sync.ShowDialog();
        }

        #endregion

        #region SFTP

        //Opens dialog so the user can chose the local directory SFTP should use.
        private void BrowseLocalDirectory(object sender, EventArgs e)
        {
            using FolderBrowserDialog openFolderDialog = new FolderBrowserDialog();
            if (openFolderDialog.ShowDialog() == DialogResult.OK)
            {
                LocalDirectoryBox.Text = openFolderDialog.SelectedPath;
            }
        }

        //Gets the list of files from the remote directory and displays them in FileView
        private void ShowDirectory(object sender, EventArgs e)
        {
            try
            {
                using SftpClient sftp = new SftpClient(HostBox.Text, UsernameBox.Text, PasswordBox.Text);
                sftp.Connect();
                List<SftpFile> files = SFTP.GetListOfFiles(sftp, RemoteDirectoryBox.Text);
                sftp.Disconnect();
                shownFiles.Clear();
                for (int i = 0; i < files.Count; i++)
                {
                    shownFiles.Add(files[i]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Downloads all files in remote directory and subfolders to local directory
        private void DownloadAll(object sender, EventArgs e)
        {
            try
            {
                using SftpClient sftp = new SftpClient(HostBox.Text, UsernameBox.Text, PasswordBox.Text);
                sftp.Connect();
                SFTP.DownloadDirectory(sftp, RemoteDirectoryBox.Text, LocalDirectoryBox.Text);
                sftp.Disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Help

        //Opens link to the GitHub page
        private void SourceLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SourceLink.LinkVisited = true;
            Process p = new Process();
            p.StartInfo.FileName = "cmd";
            p.StartInfo.Arguments = "/c start https://www.github.com/mjrj97/Manager";
            p.StartInfo.CreateNoWindow = true;
            p.Start();
        }

        //Send email to Martin
        private void EmailLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            EmailLink.LinkVisited = true;
            Process p = new Process();
            p.StartInfo.FileName = "cmd";
            p.StartInfo.Arguments = "/c start mailto:martin.jensen.1997@hotmail.com";
            p.StartInfo.CreateNoWindow = true;
            p.Start();
        }

        #endregion

        #region Tray icon

        //Reopen Timotheus window
        private void Open(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
            TrayIcon.Visible = false;
        }
        
        //Close application
        private void Exit(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //If the user minimizes the application, minimize it to the tray.
        private void Manager_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Hide();
                TrayIcon.Visible = true;
            }
        }

        #endregion

        //Processes hotkeys
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (ModifierKeys == Keys.None)
            {
                //If in Calendar tab and presses 'delete', it removes the selected event
                if (keyData == Keys.Delete && tabControl.SelectedIndex == 0)
                {
                    RemoveEvent(null, null);
                    return true;
                }
            }
            return base.ProcessDialogKey(keyData);
        }
    }
}