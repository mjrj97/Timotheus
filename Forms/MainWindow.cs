using Timotheus.Schedule;
using Timotheus.Utility;
using Timotheus.Persons;
using Timotheus.Accounting;
using System;
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
        /// List of events in the period from StartPeriod to EndPeriod shown in the Calendar_View. Updated using UpdateTable().
        /// </summary>
        public SortableBindingList<Event> events = new SortableBindingList<Event>();
        /// <summary>
        /// List of files in the SFTP remote directory. Updated using ShowDirectory().
        /// </summary>
        public SortableBindingList<SftpFile> shownFiles = new SortableBindingList<SftpFile>();
        /// <summary>
        /// List of Members in the defined period.
        /// </summary>
        public SortableBindingList<Person> members = new SortableBindingList<Person>();
        /// <summary>
        /// List of all consent forms loaded into the program.
        /// </summary>
        public SortableBindingList<Person> consentForms = new SortableBindingList<Person>();
        /// <summary>
        /// List of all transactions in the defined period.
        /// </summary>
        public SortableBindingList<Transaction> transactions = new SortableBindingList<Transaction>();
        /// <summary>
        /// List of all accounts.
        /// </summary>
        public SortableBindingList<Account> accounts = new SortableBindingList<Account>();

        /// <summary>
        /// Current calendar used by the program.
        /// </summary>
        public Calendar calendar = new Calendar();
        /// <summary>
        /// Name of the spring period.
        /// </summary>
        private string spring = "Spring";
        /// <summary>
        /// Name of the fall period.
        /// </summary>
        private string fall = "Fall";
        /// <summary>
        /// Name of the all period.
        /// </summary>
        private string all = "All";

        /// <summary>
        /// Start of the period in calanderwhich sorts the different lists.
        /// </summary>
        public DateTime StartPeriod = new DateTime(DateTime.Now.Year, 1, 1);
        /// <summary>
        /// End of the period in calander which sorts the different lists.
        /// </summary>
        public DateTime EndPeriod = new DateTime(DateTime.Now.Year + 1, 1, 1);
        /// <summary>
        /// Type of period used by Calendar_View.
        /// </summary>
        private Period period = Period.Year;

        /// <summary>
        /// To save the Members under 25 in the correct language
        /// </summary>
        private string MembersUnder25Text;

        /// <summary>
        /// Start of the period in Member sorts the different lists.
        /// </summary>
        private DateTime MemberlistYear = new DateTime(DateTime.Now.Year, 1, 1);

        /// <summary>
        /// Start of the period in acunting sorts the different lists.
        /// </summary>
        private DateTime AccountingYear = new DateTime(DateTime.Now.Year, 1, 1);

        /// <summary>
        /// Constructor. Loads initial data and localization.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            SetupUI();
            
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
            LocalizationLoader locale = new LocalizationLoader(Program.directory, Program.culture.Name);

            #region Calendar
            Calendar_View.AutoGenerateColumns = false;
            Calendar_View.DataSource = new BindingSource(events, null);
            
            Calendar_PeriodBox.Text = StartPeriod.Year.ToString();
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

            spring = locale.GetLocalization("Calendar_Spring", spring);
            fall = locale.GetLocalization("Calendar_Fall", fall);
            all = locale.GetLocalization("Calendar_All", all);
            #endregion

            #region SFTP
            SFTP_View.AutoGenerateColumns = false;
            SFTP_View.DataSource = new BindingSource(shownFiles, null);
            SFTP_PasswordBox.PasswordChar = '*';
            
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
            Members_View.AutoGenerateColumns = false;
            Members_View.DataSource = new BindingSource(members, null);

            Members_PeriodeBox.Text = MemberlistYear.Year.ToString();
            Members_Page.Text = locale.GetLocalization(Members_Page);
            Members_NameColumn.HeaderText = locale.GetLocalization(Members_NameColumn);
            Members_AddressColumn.HeaderText = locale.GetLocalization(Members_AddressColumn);
            Members_BirthdayColumn.HeaderText = locale.GetLocalization(Members_BirthdayColumn);
            Members_EntryColumn.HeaderText = locale.GetLocalization(Members_EntryColumn);
            Members_AddButton.Text = locale.GetLocalization(Members_AddButton);
            Members_RemoveButton.Text = locale.GetLocalization(Members_RemoveButton);
            MembersUnder25Text = locale.GetLocalization(Members_Under25Label);
            UpdateMemberTable();
            #endregion

            #region Consent Forms
            ConsentForms_View.AutoGenerateColumns = false;
            ConsentForms_View.DataSource = new BindingSource(consentForms, null);
            
            ConsentForms_Page.Text = locale.GetLocalization(ConsentForms_Page);
            ConsentForms_AddButton.Text = locale.GetLocalization(ConsentForms_AddButton);
            ConsentForms_RemoveButton.Text = locale.GetLocalization(ConsentForms_RemoveButton);
            ConsentForms_NameColumn.HeaderText = locale.GetLocalization(ConsentForms_NameColumn);
            ConsentForms_DateColumn.HeaderText = locale.GetLocalization(ConsentForms_DateColumn);
            ConsentForms_VersionColumn.HeaderText = locale.GetLocalization(ConsentForms_VersionColumn);
            ConsentForms_CommentColumn.HeaderText = locale.GetLocalization(ConsentForms_CommentColumn);
            #endregion

            #region Accounting
            Accounting_YearBox.Text = StartPeriod.Year.ToString();
            Accounting_TransactionsView.AutoGenerateColumns = false;
            Accounting_TransactionsView.DataSource = new BindingSource(transactions, null);
            Accounting_AccountsView.AutoGenerateColumns = false;
            Accounting_AccountsView.DataSource = new BindingSource(accounts, null);
            Accounting_TransactionsView.Columns[5].DefaultCellStyle.ForeColor = Color.Red;
            
            Accounting_Page.Text = locale.GetLocalization(Accounting_Page);
            Accounting_DateColumn.HeaderText = locale.GetLocalization(Accounting_DateColumn);
            Accounting_AppendixColumn.HeaderText = locale.GetLocalization(Accounting_AppendixColumn);
            Accounting_DescriptionColumn.HeaderText = locale.GetLocalization(Accounting_DescriptionColumn);
            Accounting_AccountNumberColumn.HeaderText = locale.GetLocalization(Accounting_AccountNumberColumn);
            Accounting_InColumn.HeaderText = locale.GetLocalization(Accounting_InColumn);
            Accounting_OutColumn.HeaderText = locale.GetLocalization(Accounting_OutColumn);
            Accounting_BalanceColumn.HeaderText = locale.GetLocalization(Accounting_BalanceColumn);

            //Only for testing
            accounts.Add(new Account(1, "Kontingent"));
            accounts.Add(new Account(2, "Materialer"));
            accounts.Add(new Account(3, "Gebyrer"));
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
            events.Clear();
            for (int i = 0; i < calendar.events.Count; i++)
            {
                if (calendar.events[i].IsInPeriod(StartPeriod,EndPeriod) && !calendar.events[i].Deleted)
                    events.Add(calendar.events[i]);
            }
            Calendar_View.Sort(Calendar_View.Columns[0], ListSortDirection.Ascending);
        }

        /// <summary>
        /// Changes the selected year and calls UpdateTable.
        /// </summary>
        private void UpdateCalenderPeriod(object sender, EventArgs e)
        {
            if (sender != null)
            {
                Button button = (Button)sender;
                if (button.Text == "+")
                {
                    if (period == Period.Year)
                    {
                        StartPeriod = StartPeriod.AddYears(1);
                        EndPeriod = EndPeriod.AddYears(1); 
                    }
                    else if (period == Period.Halfyear)
                    {
                        StartPeriod = StartPeriod.AddMonths(6);
                        EndPeriod = EndPeriod.AddMonths(6);  
                    }
                    else if (period == Period.Month)
                    {
                        StartPeriod = StartPeriod.AddMonths(1);
                        EndPeriod = EndPeriod.AddMonths(1);                        
                    }
                }
                else if (button.Text == "-")
                {
                    if (period == Period.Year)
                    {
                        StartPeriod = StartPeriod.AddYears(-1);
                        EndPeriod = EndPeriod.AddYears(-1);
                    }
                    else if (period == Period.Halfyear)
                    {
                        StartPeriod = StartPeriod.AddMonths(-6);
                        EndPeriod = EndPeriod.AddMonths(-6);
                    }
                    else if (period == Period.Month)
                    {
                        StartPeriod = StartPeriod.AddMonths(-1);
                        EndPeriod = EndPeriod.AddMonths(-1);
                        
                    }
                }
                UpdatePeriodBox();
            }
            
            UpdateCalendarTable();
        }

        /// <summary>
        /// Updates the year text according to the selected period.
        /// </summary>
        private void PeriodChangedButton(object sender, EventArgs e)
        {
            RadioButton button = (RadioButton)sender;
            if (button.Checked)
            {
                ChangePeriod();
            }
        }

        /// <summary>
        /// Changes the period
        /// </summary>
        private void ChangePeriod()
        {
            if (Calendar_AllButton.Checked)
            {
                Calendar_AddYearButton.Enabled = false;
                Calendar_SubtractYearButton.Enabled = false;

                StartPeriod = DateTime.MinValue;
                EndPeriod = DateTime.MaxValue;
                period = Period.All;
            }
            else
            {
                if (Calendar_YearButton.Checked)
                {
                    if (period == Period.All)
                    {
                        StartPeriod = new DateTime(DateTime.Now.Year, 1, 1);
                        EndPeriod = new DateTime(DateTime.Now.Year + 1, 1, 1);
                    }
                    else
                    {
                        StartPeriod = new DateTime(StartPeriod.Year, 1, 1);
                        EndPeriod = new DateTime(StartPeriod.Year + 1, 1, 1);
                    }

                    period = Period.Year;
                }
                else if (Calendar_HalfYearButton.Checked)
                {
                    if (period == Period.All)
                    {
                        if (DateTime.Now.Month > 6)
                        {
                            StartPeriod = new DateTime(DateTime.Now.Year, 7, 1);
                            EndPeriod = new DateTime(DateTime.Now.Year + 1, 1, 1);
                        }
                        else
                        {
                            StartPeriod = new DateTime(DateTime.Now.Year, 1, 1);
                            EndPeriod = new DateTime(DateTime.Now.Year, 7, 1);
                        }
                    }
                    else
                    {
                        if (StartPeriod.Month > 6)
                        {
                            StartPeriod = new DateTime(StartPeriod.Year, 7, 1);
                            EndPeriod = new DateTime(StartPeriod.Year + 1, 1, 1);
                        }
                        else
                        {
                            StartPeriod = new DateTime(StartPeriod.Year, 1, 1);
                            EndPeriod = new DateTime(StartPeriod.Year, 7, 1);
                        }
                    }

                    period = Period.Halfyear;
                }
                else if (Calendar_MonthButton.Checked)
                {
                    StartPeriod = new DateTime(StartPeriod.Year, StartPeriod.Month, 1);
                    EndPeriod = StartPeriod.AddMonths(1);

                    period = Period.Month;
                }

                Calendar_AddYearButton.Enabled = true;
                Calendar_SubtractYearButton.Enabled = true;
            }
            UpdatePeriodBox();
            UpdateCalendarTable();
        }

        /// <summary>
        /// Opens dialog where the user can define the attributes of the new event.
        /// </summary>
        private void AddEvent(object sender, EventArgs e)
        {
            AddEvent addEvent = new AddEvent(Settings_AddressBox.Text)
            {
                Owner = this
            };
            DialogResult result = addEvent.ShowDialog();
            if (result == DialogResult.OK)
            {
                Event ev = new Event(addEvent.Event_Start, addEvent.Event_End, addEvent.Event_Name, addEvent.Event_Description, addEvent.Event_Location, null);
                calendar.events.Add(ev);
                UpdateCalendarTable();
            }
        }

        /// <summary>
        /// Removes the selected event.
        /// </summary>
        private void RemoveEvent(object sender, EventArgs e)
        {
            if (events.Count > 0)
            {
                Event ev = events[Calendar_View.CurrentCell.OwningRow.Index];
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
                    byte[] data = System.Text.Encoding.UTF8.GetBytes(calendar.GetCalendarICS(Path.GetFileNameWithoutExtension(saveFileDialog.FileName)));
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
            DialogResult result = open.ShowDialog();
            if (result == DialogResult.OK)
            {
                if (open.Online)
                    calendar = new Calendar(open.Username, open.Password, open.CalDAV);
                else
                    calendar = new Calendar(open.ICS);

                UpdateCalendarTable();
            }
        }

        /// <summary>
        /// Opens a dialog where the user can save the current calendar as .pdf.
        /// </summary>
        private void ExportCalendar(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "PDF document (*.pdf)|*.pdf"
            };
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    FileInfo file = new FileInfo(saveFileDialog.FileName);
                    PDFCreater.ExportCalendar(events, file.DirectoryName, file.Name, Settings_NameBox.Text, Settings_AddressBox.Text, Settings_LogoBox.Text, Calendar_PeriodBox.Text);
                }
                catch (Exception ex)
                {
                    Program.Error(ex.Message, "Exception_Saving");
                }
            }
        }

        /// <summary>
        /// Opens dialog where the user can sync the current calendar with a remote calendar.
        /// </summary>
        private void SyncCalendar(object sender, EventArgs e)
        {
            SyncCalendar sync = new SyncCalendar(calendar.IsSetup(), Calendar_PeriodBox.Text)
            {
                Owner = this
            };
            DialogResult result = sync.ShowDialog();
            if (result == DialogResult.OK)
            {
                if (sync.Calendar_New)
                    calendar.SetupSync(sync.Username, sync.Password, sync.CalDAV);

                if (sync.SyncType == 0)
                    calendar.Sync();
                else if (sync.SyncType == 1)
                    calendar.Sync(StartPeriod, EndPeriod);
                else if (sync.SyncType == 2)
                    calendar.Sync(sync.a, sync.b);

                UpdateCalendarTable();
            }
        }

        /// <summary>
        /// Updates the period if the user manually changes the period in the text box.
        /// </summary>
        private void Calendar_PeriodBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string inputText = Calendar_PeriodBox.Text;
                string[] SpiltText = inputText.Split(" ");

                System.Globalization.DateTimeFormatInfo dtfi = System.Globalization.CultureInfo.GetCultureInfo(Program.culture.Name).DateTimeFormat;
                List<string> MonthNames = new List<string>(dtfi.MonthNames);
                MonthNames = MonthNames.ConvertAll(d => d.ToLower());

                string text;

                if (inputText.ToLower().Equals(all.ToLower()))
                {
                    Calendar_AllButton.Checked = true;
                }
                else if (SpiltText.Length == 1 && int.TryParse(inputText, out int NewYear) && NewYear > 0 && NewYear < 10000)
                {
                    Calendar_YearButton.Checked = true;

                    int change = NewYear - StartPeriod.Year;
                    StartPeriod = StartPeriod.AddYears(change);
                    EndPeriod = EndPeriod.AddYears(change);
                }
                else if (SpiltText.Length == 2)
                {
                    if (int.TryParse(SpiltText[0], out NewYear))
                    {
                        text = SpiltText[1].ToLower();
                    }
                    else if (int.TryParse(SpiltText[1], out NewYear))
                    {
                        text = SpiltText[0].ToLower();
                    }
                    else
                    {
                        text = null;
                    }

                    if (text != null)
                    {
                        //check if it is halfyear
                        if (NewYear > 0 && NewYear < 10000 && (text.Equals(fall.ToLower()) | text.Equals(spring.ToLower())))
                        {
                            Calendar_HalfYearButton.Checked = true;
                            
                            if (text.Equals(fall.ToLower()))
                            {
                                StartPeriod = new DateTime(NewYear, 7, 1);
                                EndPeriod = new DateTime(NewYear + 1, 1, 1);
                            }
                            else if (text.Equals(spring.ToLower()))
                            {
                                StartPeriod = new DateTime(NewYear, 1, 1);
                                EndPeriod = new DateTime(NewYear, 7, 1);
                            }
                        }
                        else if (MonthNames.Contains(text) && NewYear > 0 && NewYear < 10000)
                        {
                            Calendar_MonthButton.Checked = true;
                            
                            try
                            {
                                DateTime NewDateTime = DateTime.Parse(inputText, Program.culture);
                                StartPeriod = new DateTime(NewDateTime.Year, NewDateTime.Month, 1);
                                EndPeriod = StartPeriod.AddMonths(1);
                            }
                            catch (FormatException)
                            {
                                //Should there be mesage here?
                            }
                        }
                    }
                }

                ChangePeriod();
                UpdateCalendarTable();
                e.Handled = true;
            }
        }

        /// <summary>
        /// Updates the text in the period box.
        /// </summary>
        private void UpdatePeriodBox()
        {
            switch (period)
            {
                case Period.All:
                    Calendar_PeriodBox.Text = all;
                    break;
                case Period.Year:
                    Calendar_PeriodBox.Text = StartPeriod.Year.ToString();
                    break;
                case Period.Halfyear:
                    if (StartPeriod.Month > 6)
                        Calendar_PeriodBox.Text = StartPeriod.Year + " " + fall;
                    else
                        Calendar_PeriodBox.Text = StartPeriod.Year + " " + spring;
                    break;
                case Period.Month:
                        Calendar_PeriodBox.Text = StartPeriod.ToString("MMMM", Program.culture) + " " + StartPeriod.Year;
                    break;
            }
            
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
                Program.Error(ex.Message, "Exception_InvalidInput");
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
                Program.Error(ex.Message, "Exception_InvalidInput");
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
                Program.Error(ex.Message, "Exception_InvalidInput");
            }
        }
        #endregion

        #region Members
        /// <summary>
        /// Opens dialog where the user can add a new member
        /// </summary>
        private void AddMember(object sender, EventArgs e)
        {
            AddMember addMember = new AddMember(Person.list, MemberlistYear.Year)
            {
                Owner = this
            };
            DialogResult result = addMember.ShowDialog();
            if (result == DialogResult.OK)
            {
                if (addMember.Member_New)
                {
                    new Person(addMember.Member_Name, addMember.Member_Address, addMember.Member_Birthday, addMember.Member_Entry);
                }
                else
                {
                    Person person = Person.list[addMember.Member_Index];
                    person.Address = addMember.Member_Address;
                    person.Birthday = addMember.Member_Birthday;
                    person.Entry = addMember.Member_Entry;
                }

                UpdateMemberTable();
            }
        }

        /// <summary>
        /// Opens dialog where the user can remove a member
        /// </summary>
        private void RemoveMember(object sender, EventArgs e)
        {
            if (Person.list.Count > 0)
            {
                Person.list.Remove(members[Members_View.CurrentCell.OwningRow.Index]);
            }
            UpdateMemberTable();
        }

        /// <summary>
        /// Updates the text in the period text box.
        /// </summary>
        private void UpdateMemberPeriod(object sender, EventArgs e)
        {
            if (sender != null)
            {
                Button button = (Button)sender;
                if (button.Text == "+")
                {
                    MemberlistYear = MemberlistYear.AddYears(1);
                    Members_PeriodeBox.Text = MemberlistYear.Year.ToString();
                }
                else if (button.Text == "-")
                {

                    MemberlistYear = MemberlistYear.AddYears(-1);
                    Members_PeriodeBox.Text = MemberlistYear.Year.ToString();
                }
                UpdateMemberTable();
            }
        }

        /// <summary>
        /// Updates the contents of the Members_View
        /// </summary>
        public void UpdateMemberTable(object sender, DataGridViewCellEventArgs e)
        {
            int MembersUnder25 = 0;
            
            members.Clear();
            for (int i = 0; i < Person.list.Count; i++)
            {
                if (Person.list[i].Entry.Year == MemberlistYear.Year)
                {
                    members.Add(Person.list[i]);
                    if (Person.list[i].CalculateAge() < 25)
                        MembersUnder25++;
                }
            }
            Members_Under25Label.Text = MembersUnder25Text + " " + MembersUnder25;
        }
        public void UpdateMemberTable()
        {
            UpdateMemberTable(null, null);
        }

        /// <summary>
        /// Updates the year for the memberlist, if manually changed in the text box.
        /// </summary>
        private void Members_PeriodeBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (int.TryParse(Members_PeriodeBox.Text, out int NewYear) && NewYear > 0 && NewYear < 10000)
                {
                    int change = NewYear - MemberlistYear.Year;                   
                    MemberlistYear = MemberlistYear.AddYears(change);
                    Members_PeriodeBox.Text = MemberlistYear.Year.ToString();
                    UpdateMemberTable();
                }
                else
                {
                    Members_PeriodeBox.Text = MemberlistYear.Year.ToString();
                }
                e.Handled = true;
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
            DialogResult result = addConsentForm.ShowDialog();
            if (result == DialogResult.OK)
            {
                new Person(addConsentForm.ConsentForm_Name, addConsentForm.ConsentForm_Signed, addConsentForm.ConsentForm_Version, addConsentForm.ConsentForm_Comment);
                UpdateConsentFormsTable();
            }
        }

        /// <summary>
        /// Removes the selected consent form.
        /// </summary>
        private void RemoveConsentForm(object sender, EventArgs e)
        {
            if (consentForms.Count > 0)
            {
                Person.list.Remove(consentForms[ConsentForms_View.CurrentCell.OwningRow.Index]);
                UpdateConsentFormsTable();
            }
        }

        /// <summary>
        /// Updates the contents of the consent form table.
        /// </summary>
        public void UpdateConsentFormsTable()
        {
            consentForms.Clear();
            for (int i = 0; i < Person.list.Count; i++)
            {
                if (Person.list[i].Signed != DateTime.MinValue)
                    consentForms.Add(Person.list[i]);
            }
        }
        #endregion

        #region Accounting
        /// <summary>
        /// Changes the value in the year box to another year.
        /// </summary>
        private void UpdateAccountingYear(object sender, EventArgs e)
        {
            if (sender != null)
            {
                Button button = (Button)sender;
                if (button.Text == "+")
                {
                    AccountingYear = AccountingYear.AddYears(1);
                    Accounting_YearBox.Text = AccountingYear.Year.ToString();
                }
                else if (button.Text == "-")
                {
                    AccountingYear = AccountingYear.AddYears(-1);
                    Accounting_YearBox.Text = AccountingYear.Year.ToString();
                }
                UpdateTransactionsTable();
            }
        }

        /// <summary>
        /// Updates the contents of the Accounting_TransactionsView so only transactions in the given year are shown.
        /// </summary>
        public void UpdateTransactionsTable(object sender, DataGridViewCellEventArgs e)
        {
            int year = AccountingYear.Year;
            transactions.Clear();
            for (int i = 0; i < Transaction.list.Count; i++)
            {
                if (year == Transaction.list[i].Date.Year)
                    transactions.Add(Transaction.list[i]);
            }

            UpdateAccountsTable();
        }
        public void UpdateTransactionsTable()
        {
            UpdateTransactionsTable(null, null);
        }

        /// <summary>
        /// Updates the contents of the Accounting_AccountsView so the balance of each account only shows values relevant to that year.
        /// </summary>
        public void UpdateAccountsTable()
        {
            int year = AccountingYear.Year;
            double[] balances = new double[accounts.Count];
            for (int i = 0; i < transactions.Count; i++)
            {
                if (year == transactions[i].Date.Year)
                {
                    for (int j = 0; j < accounts.Count; j++)
                    {
                        if (accounts[j].ID == transactions[i].AccountNumber)
                            balances[j] += transactions[i].InValue - transactions[i].OutValue;
                    }
                }
            }
            for (int i = 0; i < balances.Length; i++)
            {
                accounts[i].SetBalance(balances[i]);
            }
            Accounting_AccountsView.Refresh();
        }

        /// <summary>
        /// Opens AddTransaction dialog.
        /// </summary>
        private void AddTransaction(object sender, EventArgs e)
        {
            AddTransaction addTransaction = new AddTransaction(accounts.ToArray())
            {
                Owner = this
            };
            DialogResult result = addTransaction.ShowDialog();
            if (result == DialogResult.OK)
            {
                new Transaction(addTransaction.Transaction_Date, addTransaction.Transaction_Appendix, addTransaction.Transaction_Description, addTransaction.Transaction_Account, addTransaction.Transaction_In, addTransaction.Transaction_Out);
                UpdateTransactionsTable();
            }
        }

        /// <summary>
        /// Removes the selected transaction in the Accounting_View and list in Transaction class.
        /// </summary>
        private void RemoveTransaction(object sender, EventArgs e)
        {
            if (transactions.Count > 0)
            {
                Transaction transaction = transactions[Accounting_TransactionsView.CurrentCell.OwningRow.Index];
                transactions.Remove(transaction);
                Transaction.list.Remove(transaction);
            }
        }

        /// <summary>
        /// Exports the accounts as a PDF only containing the transactions in the selected year.
        /// </summary>
        private void ExportAccounts(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Updates the accounting year if the user manually changes in the text box.
        /// </summary>
        private void Accounting_YearBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (int.TryParse(Accounting_YearBox.Text, out int NewYear) && NewYear > 0 && NewYear < 10000)
                {
                    AccountingYear = AccountingYear.AddYears(NewYear - AccountingYear.Year);
                    Accounting_YearBox.Text = AccountingYear.Year.ToString();
                    UpdateTransactionsTable();                    
                }
                else
                {
                    Accounting_YearBox.Text = AccountingYear.Year.ToString();
                }
                e.Handled = true;
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

        /// <summary>
        /// Automatically updates the logo in the picture box, if the user changes the text box.
        /// </summary>
        private void Settings_LogoBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    Settings_PictureBox.Image = Image.FromFile(Settings_LogoBox.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                e.Handled = true;
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
                    if (tabControl.SelectedIndex == tabControl.TabPages.IndexOf(Calendar_Page))
                    {
                        RemoveEvent(null, null);
                        return true;
                    }
                    //If in Consent Forms tab, it removes the selected consent form
                    else if (tabControl.SelectedIndex == tabControl.TabPages.IndexOf(ConsentForms_Page))
                    {
                        RemoveConsentForm(null, null);
                        return true;
                    }
                    //If in Member tab, it removes the selected event
                    if (tabControl.SelectedIndex == tabControl.TabPages.IndexOf(Members_Page))
                    {
                        RemoveMember(null, null);
                        return true;
                    }
                 
                }
            }
            return base.ProcessDialogKey(keyData);
        }

        /// <summary>
        /// Handles errors if inputed date is not formatted correctly.
        /// </summary>
        private void HandleDateError(object sender, DataGridViewDataErrorEventArgs e)
        {
            ((DataGridView)sender).Rows[e.RowIndex].Cells[e.ColumnIndex].Value = DateTime.Now.Date;
            e.Cancel = true;
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