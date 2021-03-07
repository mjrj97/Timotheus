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

        public Calendar calendar = new Calendar();
        
        public DateTime a = new DateTime(DateTime.Now.Year, 1, 1);
        public DateTime b = new DateTime(DateTime.Now.Year + 1, 1, 1);
        private Period period = Period.Year;

        //Constructor
        public MainWindow()
        {
            window = this;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            InitializeComponent();
            SetupUI();
            Calendar_PeriodBox.Text = a.Year.ToString();

            string fullName = Path.Combine(Application.StartupPath, "Data.txt");
            if (File.Exists(fullName))
            {
                StreamReader steamReader = new StreamReader(fullName);
                string[] content = steamReader.ReadToEnd().Split("\n");
                steamReader.Close();

                if (content.Length > 4)
                    SFTP_UsernameBox.Text = content[4].Trim();
                if (content.Length > 5)
                    SFTP_PasswordBox.Text = content[5].Trim();
                if (content.Length > 3)
                    SFTP_HostBox.Text = content[3].Trim();
                if (content.Length > 6)
                    SFTP_RemoteDirectoryBox.Text = content[6].Trim();
                if (content.Length > 7)
                    SFTP_LocalDirectoryBox.Text = content[7].Trim();
                if (content.Length > 8)
                    Settings_NameBox.Text = content[8].Trim();
                if (content.Length > 9)
                    Settings_AddressBox.Text = content[9].Trim();
                if (content.Length > 10)
                {
                    Settings_LogoBox.Text = content[10].Trim();
                    if (File.Exists(content[10].Trim()))
                        Settings_PictureBox.Image = Image.FromFile(content[10].Trim());
                }
            }
        }

        //Assigns the different lists to their appropriate DataGridViews, disables AutoGenerateColumns, and loads localization.
        private void SetupUI()
        {
            Calendar_View.AutoGenerateColumns = false;
            SFTP_View.AutoGenerateColumns = false;
            ConsentForms_View.AutoGenerateColumns = false;
            Calendar_View.DataSource = new BindingSource(shownEvents, null);
            SFTP_View.DataSource = new BindingSource(shownFiles, null);
            ConsentForms_View.DataSource = new BindingSource(consentForms, null);
            SFTP_PasswordBox.PasswordChar = '*';

            LocalizationLoader locale = new LocalizationLoader(System.Globalization.CultureInfo.CurrentCulture.Name);

            #region Calendar
            Calendar_StartColumn.HeaderText = locale.GetLocalization(Calendar_StartColumn.Name);
            Calendar_EndColumn.HeaderText = locale.GetLocalization(Calendar_EndColumn.Name);
            Calendar_NameColumn.HeaderText = locale.GetLocalization(Calendar_NameColumn.Name);
            Calendar_DescriptionColumn.HeaderText = locale.GetLocalization(Calendar_DescriptionColumn.Name);
            Calendar_LocationColumn.HeaderText = locale.GetLocalization(Calendar_LocationColumn.Name);
            Calendar_Page.Text = locale.GetLocalization(Calendar_Page.Name);
            Calendar_MonthButton.Text = locale.GetLocalization(Calendar_MonthButton.Name);
            Calendar_HalfYearButton.Text = locale.GetLocalization(Calendar_HalfYearButton.Name);
            Calendar_YearButton.Text = locale.GetLocalization(Calendar_YearButton.Name);
            Calendar_AllButton.Text = locale.GetLocalization(Calendar_AllButton.Name);
            Calendar_SaveButton.Text = locale.GetLocalization(Calendar_SaveButton.Name);
            Calendar_OpenButton.Text = locale.GetLocalization(Calendar_OpenButton.Name);
            Calendar_SyncButton.Text = locale.GetLocalization(Calendar_SyncButton.Name);
            Calendar_ExportButton.Text = locale.GetLocalization(Calendar_ExportButton.Name);
            Calendar_RemoveButton.Text = locale.GetLocalization(Calendar_RemoveButton.Name);
            Calendar_AddButton.Text = locale.GetLocalization(Calendar_AddButton.Name);
            #endregion

            #region SFTP
            SFTP_Page.Text = locale.GetLocalization(SFTP_Page.Name);
            SFTP_HostLabel.Text = locale.GetLocalization(SFTP_HostLabel.Name);
            SFTP_UsernameLabel.Text = locale.GetLocalization(SFTP_UsernameLabel.Name);
            SFTP_PasswordLabel.Text = locale.GetLocalization(SFTP_PasswordLabel.Name);
            SFTP_RemoteDirectoryLabel.Text = locale.GetLocalization(SFTP_RemoteDirectoryLabel.Name);
            SFTP_LocalDirectoryLabel.Text = locale.GetLocalization(SFTP_LocalDirectoryLabel.Name);
            SFTP_BrowseButton.Text = locale.GetLocalization(SFTP_BrowseButton.Name);
            SFTP_ShowDirectoryButton.Text = locale.GetLocalization(SFTP_ShowDirectoryButton.Name);
            SFTP_DownloadButton.Text = locale.GetLocalization(SFTP_DownloadButton.Name);
            SFTP_SyncButton.Text = locale.GetLocalization(SFTP_SyncButton.Name);
            SFTP_NameColumn.HeaderText = locale.GetLocalization(SFTP_NameColumn.Name);
            SFTP_SizeColumn.HeaderText = locale.GetLocalization(SFTP_SizeColumn.Name);
            #endregion

            #region Consent Forms
            ConsentForms_Page.Text = locale.GetLocalization(ConsentForms_Page.Name);
            ConsentForms_AddButton.Text = locale.GetLocalization(ConsentForms_AddButton.Name);
            ConsentForms_RemoveButton.Text = locale.GetLocalization(ConsentForms_RemoveButton.Name);
            ConsentForms_NameColumn.HeaderText = locale.GetLocalization(ConsentForms_NameColumn.Name);
            ConsentForms_DateColumn.HeaderText = locale.GetLocalization(ConsentForms_DateColumn.Name);
            ConsentForms_VersionColumn.HeaderText = locale.GetLocalization(ConsentForms_VersionColumn.Name);
            ConsentForms_CommentColumn.HeaderText = locale.GetLocalization(ConsentForms_CommentColumn.Name);
            #endregion

            #region Settings
            Settings_Page.Text = locale.GetLocalization(Settings_Page.Name);
            Settings_NameLabel.Text = locale.GetLocalization(Settings_NameLabel.Name);
            Settings_AddressLabel.Text = locale.GetLocalization(Settings_AddressLabel.Name);
            Settings_LogoLabel.Text = locale.GetLocalization(Settings_LogoLabel.Name);
            Settings_BrowseButton.Text = locale.GetLocalization(Settings_BrowseButton.Name);
            #endregion
        }

        #region Calendar

        //Updates the contents of the event table
        public void UpdateTable()
        {
            shownEvents.Clear();
            for (int i = 0; i < calendar.events.Count; i++)
            {
                if (calendar.events[i].IsInPeriod(a,b) && !calendar.events[i].Deleted)
                    shownEvents.Add(calendar.events[i]);
            }
            Calendar_View.Sort(Calendar_View.Columns[0], ListSortDirection.Ascending);
        }

        //Changes the selected year and updates calls UpdateTable
        private void UpdatePeriod(object sender, EventArgs e)
        {
            if (sender != null)
            {
                Button button = (Button)sender;
                if (button.Text == "+")
                {
                    if (period == Period.Year)
                    {
                        a = a.AddYears(1);
                        b = b.AddYears(1);
                        Calendar_PeriodBox.Text = a.Year.ToString();
                    }
                    else if (period == Period.Halfyear)
                    {
                        a = a.AddMonths(6);
                        b = b.AddMonths(6);

                        if (a.Month > 6)
                            Calendar_PeriodBox.Text = a.Year + " Fall";
                        else
                            Calendar_PeriodBox.Text = a.Year + " Spring";
                    }
                    else if (period == Period.Month)
                    {
                        a = a.AddMonths(1);
                        b = b.AddMonths(1);
                        Calendar_PeriodBox.Text = a.Year + " " + a.Month;
                    }
                }
                else if (button.Text == "-")
                {
                    if (period == Period.Year)
                    {
                        a = a.AddYears(-1);
                        b = b.AddYears(-1);
                        Calendar_PeriodBox.Text = a.Year.ToString();
                    }
                    else if (period == Period.Halfyear)
                    {
                        a = a.AddMonths(-6);
                        b = b.AddMonths(-6);

                        if (a.Month > 6)
                            Calendar_PeriodBox.Text = a.Year + " Fall";
                        else
                            Calendar_PeriodBox.Text = a.Year + " Spring";
                    }
                    else if (period == Period.Month)
                    {
                        a = a.AddMonths(-1);
                        b = b.AddMonths(-1);
                        Calendar_PeriodBox.Text = a.Year + " " + a.Month;
                    }
                }
            }

            UpdateTable();
        }

        //Updates the year text according to the selected period
        private void PeriodChanged(object sender, EventArgs e)
        {
            RadioButton button = (RadioButton)sender;
            if (button.Checked)
            {
                if (Calendar_AllButton.Checked)
                {
                    Calendar_PeriodBox.Text = "All";
                    Calendar_AddYearButton.Enabled = false;
                    Calendar_SubtractYearButton.Enabled = false;

                    a = DateTime.MinValue;
                    b = DateTime.MaxValue;
                    period = Period.All;
                }
                else
                {
                    if (Calendar_YearButton.Checked)
                    {
                        if (period == Period.All)
                        {
                            a = new DateTime(DateTime.Now.Year, 1, 1);
                            b = new DateTime(DateTime.Now.Year + 1, 1, 1);
                        }
                        else
                        {
                            a = new DateTime(a.Year, 1, 1);
                            b = new DateTime(a.Year + 1, 1, 1);
                        }

                        Calendar_PeriodBox.Text = a.Year.ToString();
                        period = Period.Year;
                    }
                    else if (Calendar_HalfYearButton.Checked)
                    {
                        if (period == Period.All)
                        {
                            if (DateTime.Now.Month > 6)
                            {
                                a = new DateTime(DateTime.Now.Year, 7, 1);
                                b = new DateTime(DateTime.Now.Year+1, 1, 1);
                            }
                            else
                            {
                                a = new DateTime(DateTime.Now.Year, 1, 1);
                                b = new DateTime(DateTime.Now.Year, 7, 1);
                            }
                        }
                        else
                        {
                            if (a.Month > 6)
                            {
                                a = new DateTime(a.Year, 7, 1);
                                b = new DateTime(a.Year + 1, 1, 1);
                            }
                            else
                            {
                                a = new DateTime(a.Year, 1, 1);
                                b = new DateTime(a.Year, 7, 1);
                            }
                        }

                        if (a.Month > 6)
                            Calendar_PeriodBox.Text = a.Year + " Fall";
                        else
                            Calendar_PeriodBox.Text = a.Year + " Spring";
                        period = Period.Halfyear;
                    }
                    else if (Calendar_MonthButton.Checked)
                    {
                        a = new DateTime(a.Year, a.Month, 1);
                        b = a.AddMonths(1);

                        Calendar_PeriodBox.Text = a.Year + " " + a.Month;
                        period = Period.Month;
                    }

                    Calendar_AddYearButton.Enabled = true;
                    Calendar_SubtractYearButton.Enabled = true;
                }

                UpdateTable();
            }
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
                Event ev = shownEvents[Calendar_View.CurrentCell.OwningRow.Index];
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
                SFTP_LocalDirectoryBox.Text = openFolderDialog.SelectedPath;
            }
        }

        //Gets the list of files from the remote directory and displays them in FileView
        private void ShowDirectory(object sender, EventArgs e)
        {
            try
            {
                using SftpClient sftp = new SftpClient(SFTP_HostBox.Text, SFTP_UsernameBox.Text, SFTP_PasswordBox.Text);
                sftp.Connect();
                IEnumerable<SftpFile> files = SFTP.GetListOfFiles(sftp, SFTP_RemoteDirectoryBox.Text);
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
                using SftpClient sftp = new SftpClient(SFTP_HostBox.Text, SFTP_UsernameBox.Text, SFTP_PasswordBox.Text);
                sftp.Connect();
                SFTP.DownloadDirectory(sftp, SFTP_RemoteDirectoryBox.Text, SFTP_LocalDirectoryBox.Text);
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
                using SftpClient sftp = new SftpClient(SFTP_HostBox.Text, SFTP_UsernameBox.Text, SFTP_PasswordBox.Text);
                sftp.Connect();
                SFTP.Synchronize(sftp, SFTP_RemoteDirectoryBox.Text, SFTP_LocalDirectoryBox.Text);
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
                ConsentForm form = consentForms[ConsentForms_View.CurrentCell.OwningRow.Index];
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
                Settings_PictureBox.Image = Image.FromFile(open.FileName);
                // image file path  
                Settings_LogoBox.Text = open.FileName;
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

    enum Period
    {
        All,
        Year,
        Halfyear,
        Month
    }
}