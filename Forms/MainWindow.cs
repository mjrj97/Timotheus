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
using System.Drawing;
using Renci.SshNet.Sftp;
using Renci.SshNet;

namespace Timotheus.Forms
{
    public partial class MainWindow : Form
    {
        public static MainWindow window;
        public SortableBindingList<Event> shownEvents = new SortableBindingList<Event>();
        public SortableBindingList<SftpFile> shownFiles = new SortableBindingList<SftpFile>();
        public SortableBindingList<ConsentForm> consentForms = new SortableBindingList<ConsentForm>();

        private int year;
        public Calendar calendar = new Calendar();

        //Constructor
        public MainWindow()
        {
            window = this;
            year = DateTime.Now.Year;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            InitializeComponent();
            SetupUI();
            Year.Text = year.ToString();

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
                if (content.Length > 8)
                    NameBox.Text = content[8].Trim();
                if (content.Length > 9)
                    AddressBox.Text = content[9].Trim();
                if (content.Length > 10)
                {
                    LogoBox.Text = content[10].Trim();
                    LogoPictureBox.Image = Image.FromFile(content[10].Trim());
                }
            }
        }

        //Assigns the different lists to their appropriate DataGridViews and disables AutoGenerateColumns.
        private void SetupUI()
        {
            CalendarView.AutoGenerateColumns = false;
            FileView.AutoGenerateColumns = false;
            ConsentFormView.AutoGenerateColumns = false;
            CalendarView.DataSource = new BindingSource(shownEvents, null);
            FileView.DataSource = new BindingSource(shownFiles, null);
            ConsentFormView.DataSource = new BindingSource(consentForms, null);
            PasswordBox.PasswordChar = '*';
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

		//Opens a dialog where the user can save the current calendar as .pdf
        private void ExportPDF(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "PDF document (*.pdf)|*.pdf"
            };
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                FileInfo file = new FileInfo(saveFileDialog.FileName);

                calendar.ExportPDF(file.DirectoryName, file.Name);
            }
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
                IEnumerable<SftpFile> files = SFTP.GetListOfFiles(sftp, RemoteDirectoryBox.Text);
                sftp.Disconnect();
                shownFiles.Clear();
                foreach (SftpFile file in files)
                {
                    if (file.Name != "." && file.Name != "..")
                        shownFiles.Add(file);
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

        //Synchronizes the local and remote directory
        private void SyncDirectories(object sender, EventArgs e)
        {
            try
            {
                using SftpClient sftp = new SftpClient(HostBox.Text, UsernameBox.Text, PasswordBox.Text);
                sftp.Connect();
                SFTP.Synchronize(sftp, RemoteDirectoryBox.Text, LocalDirectoryBox.Text);
                sftp.Disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Consent forms

        private void AddConsentForm(object sender, EventArgs e)
        {
            consentForms.Add(new ConsentForm("Test person", DateTime.Now, DateTime.Now));
        }

        private void RemoveConsentForm(object sender, EventArgs e)
        {
            if (consentForms.Count > 0)
            {
                ConsentForm form = consentForms[ConsentFormView.CurrentCell.OwningRow.Index];
                consentForms.Remove(form);
            }
        }

        #endregion

        #region Settings

        private void BrowseLogo(object sender, EventArgs e)
        {
            // open file dialog   
            OpenFileDialog open = new OpenFileDialog
            {
                // image filters  
                Filter = "Image Files(*.png; *.jpg; *.jpeg; *.gif; *.bmp)|*.png; *.jpg; *.jpeg; *.gif; *.bmp"
            };
            if (open.ShowDialog() == DialogResult.OK)
            {
                // display image in picture box  
                LogoPictureBox.Image = Image.FromFile(open.FileName);
                // image file path  
                LogoBox.Text = open.FileName;
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
                if (keyData == Keys.Delete)
                {
                    //If in Calendar tab, it removes the selected event
                    if (tabControl.SelectedIndex == 0)
                    {
                        RemoveEvent(null, null);
                        return true;
                    }
                    //If in Consent Forms tab, it removes the selected consent form
                    else if (tabControl.SelectedIndex == 2)
                    {
                        RemoveConsentForm(null, null);
                        return true;
                    }
                }
            }
            return base.ProcessDialogKey(keyData);
        }
    }
}