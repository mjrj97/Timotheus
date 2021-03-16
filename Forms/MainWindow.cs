using Timotheus.Schedule;
using Timotheus.Utility;
using Timotheus.Persons;
using System;
using System.Text;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using Renci.SshNet.Sftp;
using Renci.SshNet;

namespace Timotheus.Forms
{
    /// <summary>
    /// Main window of the Timotheus program which holds all the primary data.
    /// </summary>
    public partial class MainWindow : Form
    {
        /// <summary>
        /// Holds the current instance of MainWindow.
        /// </summary>
        public static MainWindow window;
        /// <summary>
        /// List of events in the period a to b shown in the Calendar_View. Updated using UpdateTable().
        /// </summary>
        public SortableBindingList<Event> shownEvents = new SortableBindingList<Event>();
        /// <summary>
        /// List of files in the SFTP remote directory. Updated using ShowDirectory().
        /// </summary>
        public SortableBindingList<SftpFile> shownFiles = new SortableBindingList<SftpFile>();
        /// <summary>
        /// List of all consent forms loaded into the program.
        /// </summary>
        public SortableBindingList<ConsentForm> consentForms = new SortableBindingList<ConsentForm>();
        /// <summary>
        /// List of all persons.
        /// </summary>
        public SortableBindingList<Person> Persons = new SortableBindingList<Person>();
        /// <summary>
        /// List of all transactions.
        /// </summary>
        public SortableBindingList<Transaction> transactions = new SortableBindingList<Transaction>();

        /// <summary>
        /// Current calendar used by the program.
        /// </summary>
        public Calendar calendar = new Calendar();
        /// <summary>
        /// List of the names of each month.
        /// </summary>
        private readonly string[] month = new string[12];
        /// <summary>
        /// Name of the spring period.
        /// </summary>
        private string spring = "Spring";
        /// <summary>
        /// Name of the fall period.
        /// </summary>
        private string fall = "Fall";

        /// <summary>
        /// Start of the period which sorts the different lists.
        /// </summary>
        public DateTime a = new DateTime(DateTime.Now.Year, 1, 1);
        /// <summary>
        /// End of the period which sorts the different lists.
        /// </summary>
        public DateTime b = new DateTime(DateTime.Now.Year + 1, 1, 1);
        /// <summary>
        /// Type of period used by Calendar_View.
        /// </summary>
        private Period period = Period.Year;

        /// <summary>
        /// Constructor. Loads initial data and localization.
        /// </summary>
        public MainWindow()
        {
            window = this;
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

        /// <summary>
        /// Assigns the different lists to their appropriate DataGridViews, disables AutoGenerateColumns, and loads localization.
        /// </summary>
        private void SetupUI()
        {
            Calendar_View.AutoGenerateColumns = false;
            SFTP_View.AutoGenerateColumns = false;
            ConsentForms_View.AutoGenerateColumns = false;
            Members_View.AutoGenerateColumns = false;
            Accounting_View.AutoGenerateColumns = false;
            Calendar_View.DataSource = new BindingSource(shownEvents, null);
            SFTP_View.DataSource = new BindingSource(shownFiles, null);
            ConsentForms_View.DataSource = new BindingSource(consentForms, null);
            Members_View.DataSource = new BindingSource(Persons, null);
            Accounting_View.DataSource = new BindingSource(transactions, null);
            Accounting_View.Columns[5].DefaultCellStyle.ForeColor = Color.Red;
            SFTP_PasswordBox.PasswordChar = '*';

            LocalizationLoader locale = new LocalizationLoader(Program.directory, Program.culture);

            #region Calendar
            Calendar_Page.Text = locale.GetLocalization(Calendar_Page);
            Calendar_StartColumn.HeaderText = locale.GetLocalization(Calendar_StartColumn);
            Calendar_EndColumn.HeaderText = locale.GetLocalization(Calendar_EndColumn);
            Calendar_NameColumn.HeaderText = locale.GetLocalization(Calendar_NameColumn);
            Calendar_DescriptionColumn.HeaderText = locale.GetLocalization(Calendar_DescriptionColumn);
            Calendar_LocationColumn.HeaderText = locale.GetLocalization(Calendar_LocationColumn);
            Calendar_MonthButton.Text = locale.GetLocalization(Calendar_MonthButton);
            Calendar_HalfYearButton.Text = locale.GetLocalization(Calendar_HalfYearButton);
            Calendar_YearButton.Text = locale.GetLocalization(Calendar_YearButton);
            Calendar_AllButton.Text = locale.GetLocalization(Calendar_AllButton);
            Calendar_SaveButton.Text = locale.GetLocalization(Calendar_SaveButton);
            Calendar_OpenButton.Text = locale.GetLocalization(Calendar_OpenButton);
            Calendar_SyncButton.Text = locale.GetLocalization(Calendar_SyncButton);
            Calendar_ExportButton.Text = locale.GetLocalization(Calendar_ExportButton);
            Calendar_RemoveButton.Text = locale.GetLocalization(Calendar_RemoveButton);
            Calendar_AddButton.Text = locale.GetLocalization(Calendar_AddButton);

            month[0] = locale.GetLocalization("Calendar_January", "January");
            month[1] = locale.GetLocalization("Calendar_February", "February");
            month[2] = locale.GetLocalization("Calendar_March", "March");
            month[3] = locale.GetLocalization("Calendar_April", "April");
            month[4] = locale.GetLocalization("Calendar_May", "May");
            month[5] = locale.GetLocalization("Calendar_June", "June");
            month[6] = locale.GetLocalization("Calendar_July", "July");
            month[7] = locale.GetLocalization("Calendar_August", "August");
            month[8] = locale.GetLocalization("Calendar_September", "September");
            month[9] = locale.GetLocalization("Calendar_October", "October");
            month[10] = locale.GetLocalization("Calendar_November", "November");
            month[11] = locale.GetLocalization("Calendar_December", "December");

            spring = locale.GetLocalization("Calendar_Spring", spring);
            fall = locale.GetLocalization("Calendar_Fall", fall);
            #endregion

            #region SFTP
            SFTP_Page.Text = locale.GetLocalization(SFTP_Page);
            SFTP_HostLabel.Text = locale.GetLocalization(SFTP_HostLabel);
            SFTP_UsernameLabel.Text = locale.GetLocalization(SFTP_UsernameLabel);
            SFTP_PasswordLabel.Text = locale.GetLocalization(SFTP_PasswordLabel);
            SFTP_RemoteDirectoryLabel.Text = locale.GetLocalization(SFTP_RemoteDirectoryLabel);
            SFTP_LocalDirectoryLabel.Text = locale.GetLocalization(SFTP_LocalDirectoryLabel);
            SFTP_BrowseButton.Text = locale.GetLocalization(SFTP_BrowseButton);
            SFTP_ShowDirectoryButton.Text = locale.GetLocalization(SFTP_ShowDirectoryButton);
            SFTP_DownloadButton.Text = locale.GetLocalization(SFTP_DownloadButton);
            SFTP_SyncButton.Text = locale.GetLocalization(SFTP_SyncButton);
            SFTP_NameColumn.HeaderText = locale.GetLocalization(SFTP_NameColumn);
            SFTP_SizeColumn.HeaderText = locale.GetLocalization(SFTP_SizeColumn);
            #endregion

            #region Members
            Members_Page.Text = locale.GetLocalization(Members_Page);
            Members_NameColumn.HeaderText = locale.GetLocalization(Members_NameColumn);
            Members_AddressColumn.HeaderText = locale.GetLocalization(Members_AddressColumn);
            Members_BirthdayColumn.HeaderText = locale.GetLocalization(Members_BirthdayColumn);
            Members_SinceColumn.HeaderText = locale.GetLocalization(Members_SinceColumn);
            #endregion

            #region Consent Forms
            ConsentForms_Page.Text = locale.GetLocalization(ConsentForms_Page);
            ConsentForms_AddButton.Text = locale.GetLocalization(ConsentForms_AddButton);
            ConsentForms_RemoveButton.Text = locale.GetLocalization(ConsentForms_RemoveButton);
            ConsentForms_NameColumn.HeaderText = locale.GetLocalization(ConsentForms_NameColumn);
            ConsentForms_DateColumn.HeaderText = locale.GetLocalization(ConsentForms_DateColumn);
            ConsentForms_VersionColumn.HeaderText = locale.GetLocalization(ConsentForms_VersionColumn);
            ConsentForms_CommentColumn.HeaderText = locale.GetLocalization(ConsentForms_CommentColumn);
            #endregion

            #region Accounting
            Accounting_Page.Text = locale.GetLocalization(Accounting_Page);
            Accounting_DateColumn.HeaderText = locale.GetLocalization(Accounting_DateColumn);
            Accounting_AppendixColumn.HeaderText = locale.GetLocalization(Accounting_AppendixColumn);
            Accounting_DescriptionColumn.HeaderText = locale.GetLocalization(Accounting_DescriptionColumn);
            Accounting_AccountNumberColumn.HeaderText = locale.GetLocalization(Accounting_AccountNumberColumn);
            Accounting_InColumn.HeaderText = locale.GetLocalization(Accounting_InColumn);
            Accounting_OutColumn.HeaderText = locale.GetLocalization(Accounting_OutColumn);
            Accounting_BalanceColumn.HeaderText = locale.GetLocalization(Accounting_BalanceColumn);
            #endregion

            #region Settings
            Settings_Page.Text = locale.GetLocalization(Settings_Page);
            Settings_InfoBox.Text = locale.GetLocalization(Settings_InfoBox);
            Settings_NameLabel.Text = locale.GetLocalization(Settings_NameLabel);
            Settings_AddressLabel.Text = locale.GetLocalization(Settings_AddressLabel);
            Settings_LogoLabel.Text = locale.GetLocalization(Settings_LogoLabel);
            Settings_BrowseButton.Text = locale.GetLocalization(Settings_BrowseButton);
            #endregion

            #region Help
            Help_Page.Text = locale.GetLocalization(Help_Page);
            Help_AuthorLabel.Text = locale.GetLocalization(Help_AuthorLabel) + ": Martin J. R. Jensen";
            Help_VersionLabel.Text = locale.GetLocalization(Help_VersionLabel) + " v. 0.1.0";
            Help_LicenseLabel.Text = locale.GetLocalization(Help_LicenseLabel) + ": Apache-2.0";
            Help_EmailLabel.Text = locale.GetLocalization(Help_EmailLabel);
            Help_SourceLabel.Text = locale.GetLocalization(Help_SourceLabel);
            #endregion
        }

        #region Calendar
        /// <summary>
        /// Updates the contents of the event table.
        /// </summary>
        public void UpdateCalendarTable()
        {
            shownEvents.Clear();
            for (int i = 0; i < calendar.events.Count; i++)
            {
                if (calendar.events[i].IsInPeriod(a,b) && !calendar.events[i].Deleted)
                    shownEvents.Add(calendar.events[i]);
            }
            Calendar_View.Sort(Calendar_View.Columns[0], ListSortDirection.Ascending);
        }

        /// <summary>
        /// Changes the selected year and calls UpdateTable.
        /// </summary>
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
                            Calendar_PeriodBox.Text = a.Year + " " + fall;
                        else
                            Calendar_PeriodBox.Text = a.Year + " " + spring;
                    }
                    else if (period == Period.Month)
                    {
                        a = a.AddMonths(1);
                        b = b.AddMonths(1);
                        Calendar_PeriodBox.Text = a.Year + " " + month[a.Month-1];
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
                            Calendar_PeriodBox.Text = a.Year + " " + fall;
                        else
                            Calendar_PeriodBox.Text = a.Year + " " + spring;
                    }
                    else if (period == Period.Month)
                    {
                        a = a.AddMonths(-1);
                        b = b.AddMonths(-1);
                        Calendar_PeriodBox.Text = a.Year + " " + month[a.Month - 1];
                    }
                }
            }

            UpdateCalendarTable();
        }

        /// <summary>
        /// Updates the year text according to the selected period.
        /// </summary>
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
                            Calendar_PeriodBox.Text = a.Year + " " + fall;
                        else
                            Calendar_PeriodBox.Text = a.Year + " " + spring;
                        period = Period.Halfyear;
                    }
                    else if (Calendar_MonthButton.Checked)
                    {
                        a = new DateTime(a.Year, a.Month, 1);
                        b = a.AddMonths(1);

                        Calendar_PeriodBox.Text = a.Year + " " + month[a.Month - 1];
                        period = Period.Month;
                    }

                    Calendar_AddYearButton.Enabled = true;
                    Calendar_SubtractYearButton.Enabled = true;
                }

                UpdateCalendarTable();
            }
        }

        /// <summary>
        /// Opens dialog where the user can define the attributes of the new event.
        /// </summary>
        private void AddEvent(object sender, EventArgs e)
        {
            AddEvent addEvent = new AddEvent
            {
                Owner = this
            };

            addEvent.ShowDialog();
        }

        /// <summary>
        /// Removes the selected event.
        /// </summary>
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
                UpdateCalendarTable();
            }
        }

        /// <summary>
        /// Opens dialog and saves the current calendar as .ics.
        /// </summary>
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

        /// <summary>
        /// Opens dialog where the user can open the calendar from a .ics file or CalDAV link.
        /// </summary>
        private void OpenCalendar(object sender, EventArgs e)
        {
              OpenCalendar open = new OpenCalendar
            {
                Owner = this
            };
            open.ShowDialog();
        }

        /// <summary>
        /// Opens a dialog where the user can save the current calendar as .pdf.
        /// </summary>
        private void ExportPDF(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "PDF document (*.pdf)|*.pdf"
            };
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                FileInfo file = new FileInfo(saveFileDialog.FileName);

                calendar.ExportPDF(file.DirectoryName, file.Name, Settings_NameBox.Text, Settings_AddressBox.Text, Settings_PictureBox.Image);
            }
        }

        /// <summary>
        /// Opens dialog where the user can sync the current calendar with a remote calendar.
        /// </summary>
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
        /// <summary>
        /// Opens dialog so the user can chose the local directory SFTP should use.
        /// </summary>
        private void BrowseLocalDirectory(object sender, EventArgs e)
        {
            using FolderBrowserDialog openFolderDialog = new FolderBrowserDialog();
            if (openFolderDialog.ShowDialog() == DialogResult.OK)
            {
                SFTP_LocalDirectoryBox.Text = openFolderDialog.SelectedPath;
            }
        }

        /// <summary>
        /// Gets the list of files from the remote directory and displays them in FileView.
        /// </summary>
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

        /// <summary>
        /// Downloads all files in remote directory and subfolders to local directory.
        /// </summary>
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

        /// <summary>
        /// Synchronizes the local and remote directory.
        /// </summary>
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
        /// <summary>
        /// Add a simple consent form.
        /// </summary>
        private void AddConsentForm(object sender, EventArgs e)
        {
            AddConsentForm addConsentForm = new AddConsentForm
            {
                Owner = this
            };

            addConsentForm.ShowDialog();
        }

        /// <summary>
        /// Removes the selected consent form.
        /// </summary>
        private void RemoveConsentForm(object sender, EventArgs e)
        {
            if (consentForms.Count > 0)
            {
                ConsentForm form = consentForms[ConsentForms_View.CurrentCell.OwningRow.Index];
                consentForms.Remove(form);
            }
        }
        #endregion

        #region Accounting
        public void UpdateAccountingTable()
        {
            transactions.Clear();
            for (int i = 0; i < Transaction.list.Count; i++)
            {
                transactions.Add(Transaction.list[i]);
            }
        }

        private void AddTransaction(object sender, EventArgs e)
        {
            new Transaction(DateTime.Now.Date, 0, "Test transaction", 0, 100.0, 50.0);
            UpdateAccountingTable();
        }

        private void RemoveTransaction(object sender, EventArgs e)
        {
            if (transactions.Count > 0)
            {
                Transaction transaction = transactions[Accounting_View.CurrentCell.OwningRow.Index];
                transactions.Remove(transaction);
            }
        }
        #endregion

        #region Settings
        /// <summary>
        /// Opens a dialog where the user can find the logo file.
        /// </summary>
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
        /// <summary>
        /// Opens link to the GitHub repository.
        /// </summary>
        private void SourceLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Help_SourceLink.LinkVisited = true;
            Process p = new Process();
            p.StartInfo.FileName = "cmd";
            p.StartInfo.Arguments = "/c start https://www.github.com/mjrj97/Manager";
            p.StartInfo.CreateNoWindow = true;
            p.Start();
        }

        /// <summary>
        /// Send email to the author.
        /// </summary>
        private void EmailLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Help_EmailLink.LinkVisited = true;
            Process p = new Process();
            p.StartInfo.FileName = "cmd";
            p.StartInfo.Arguments = "/c start mailto:martin.jensen.1997@hotmail.com";
            p.StartInfo.CreateNoWindow = true;
            p.Start();
        }
        #endregion

        #region Tray icon
        /// <summary>
        /// Reopens the Timotheus window.
        /// </summary>
        private void Open(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
            TrayIcon.Visible = false;
        }

        /// <summary>
        /// Closes the application.
        /// </summary>
        private void Exit(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// If the user minimizes the application, minimize it to the tray.
        /// </summary>
        private void Manager_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Hide();
                TrayIcon.Visible = true;
            }
        }
        
        #endregion

        /// <summary>
        /// Processes the hotkeys. Delete removes the selected item in a DataGridView.
        /// </summary>
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

    /// <summary>
    /// Used to define the type of period used by a DataGridView.
    /// </summary>
    enum Period
    {
        All,
        Year,
        Halfyear,
        Month
    }
}