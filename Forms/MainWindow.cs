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
        /// Holds the current instance of MainWindow.
        /// </summary>
        public static MainWindow window;
        /// <summary>
        /// List of events in the period from StartPeriod to EndPeriod shown in the Calendar_View. Updated using UpdateTable().
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
        /// List of Members in the period 
        /// </summary>
        public SortableBindingList<Person> shownPersons = new SortableBindingList<Person>();
        /// <summary>
        /// List of all transactions.
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
        /// To save the Members under 25 ind the correct language
        /// </summary>
        private string MembersUnder25Text;

        /// <summary>
        /// Start of the period in Member sorts the different lists.
        /// </summary>
        private DateTime MemberInYear = new DateTime(DateTime.Now.Year, 1, 1);

        /// <summary>
        /// Constructor. Loads initial data and localization.
        /// </summary>
        public MainWindow()
        {
            window = this;
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
            Calendar_View.DataSource = new BindingSource(shownEvents, null);
            
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
            Members_View.DataSource = new BindingSource(shownPersons, null);
            
            Members_PeriodeBox.Text = MemberInYear.Year.ToString();
            Update_Members_Under25Label();
            Members_Page.Text = locale.GetLocalization(Members_Page);
            Members_NameColumn.HeaderText = locale.GetLocalization(Members_NameColumn);
            Members_AddressColumn.HeaderText = locale.GetLocalization(Members_AddressColumn);
            Members_BirthdayColumn.HeaderText = locale.GetLocalization(Members_BirthdayColumn);
            Members_SinceColumn.HeaderText = locale.GetLocalization(Members_SinceColumn);
            Members_AddButton.Text = locale.GetLocalization(Members_AddButton);
            Members_RemoveButton.Text = locale.GetLocalization(Members_RemoveButton);
            MembersUnder25Text = locale.GetLocalization(Members_Under25Label);
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
            Accounting_YearBox.Text = a.Year.ToString();
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
                if (calendar.events[i].IsInPeriod(StartPeriod,EndPeriod) && !calendar.events[i].Deleted)
                    shownEvents.Add(calendar.events[i]);
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
                        Calendar_PeriodBox.Text = StartPeriod.Year.ToString();
                    }
                    else if (period == Period.Halfyear)
                    {
                        StartPeriod = StartPeriod.AddMonths(6);
                        EndPeriod = EndPeriod.AddMonths(6);

                        if (StartPeriod.Month > 6)
                            Calendar_PeriodBox.Text = StartPeriod.Year + " " + fall;
                        else
                            Calendar_PeriodBox.Text = StartPeriod.Year + " " + spring;
                    }
                    else if (period == Period.Month)
                    {
                        StartPeriod = StartPeriod.AddMonths(1);
                        EndPeriod = EndPeriod.AddMonths(1);
                        Calendar_PeriodBox.Text = StartPeriod.ToString("MMMM") + " " + StartPeriod.Year;
                    }
                }
                else if (button.Text == "-")
                {
                    if (period == Period.Year)
                    {
                        StartPeriod = StartPeriod.AddYears(-1);
                        EndPeriod = EndPeriod.AddYears(-1);
                        Calendar_PeriodBox.Text = StartPeriod.Year.ToString();
                    }
                    else if (period == Period.Halfyear)
                    {
                        StartPeriod = StartPeriod.AddMonths(-6);
                        EndPeriod = EndPeriod.AddMonths(-6);

                        if (StartPeriod.Month > 6)
                            Calendar_PeriodBox.Text = StartPeriod.Year + " " + fall;
                        else
                            Calendar_PeriodBox.Text = StartPeriod.Year + " " + spring;
                    }
                    else if (period == Period.Month)
                    {
                        StartPeriod = StartPeriod.AddMonths(-1);
                        EndPeriod = EndPeriod.AddMonths(-1);
                        Calendar_PeriodBox.Text = StartPeriod.ToString("MMMM") + " " + StartPeriod.Year + " ";
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

                        Calendar_PeriodBox.Text = StartPeriod.Year.ToString();
                        period = Period.Year;
                    }
                    else if (Calendar_HalfYearButton.Checked)
                    {
                        if (period == Period.All)
                        {
                            if (DateTime.Now.Month > 6)
                            {
                                StartPeriod = new DateTime(DateTime.Now.Year, 7, 1);
                                EndPeriod = new DateTime(DateTime.Now.Year+1, 1, 1);
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

                        if (StartPeriod.Month > 6)
                            Calendar_PeriodBox.Text = fall + " " + StartPeriod.Year;
                        else
                            Calendar_PeriodBox.Text = spring + " " + StartPeriod.Year;
                            
                        period = Period.Halfyear;
                    }
                    else if (Calendar_MonthButton.Checked)
                    {
                        StartPeriod = new DateTime(StartPeriod.Year, StartPeriod.Month, 1);
                        EndPeriod = StartPeriod.AddMonths(1);

                        Calendar_PeriodBox.Text = StartPeriod.ToString("MMMM") + " " + StartPeriod.Year;
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
            open.ShowDialog();
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
                FileInfo file = new FileInfo(saveFileDialog.FileName);

                calendar.ExportPDF(file.DirectoryName, file.Name, Settings_NameBox.Text, Settings_AddressBox.Text, Settings_LogoBox.Text, Calendar_PeriodBox.Text, a, b);
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
        /// <summary>
        /// Changes the value in the year box to another year.
        /// </summary>
        private void UpdateAccountingYear(object sender, EventArgs e)
        {
            if (sender != null)
            {
                Button button = (Button)sender;
                if (button.Text == "+")
                    Accounting_YearBox.Text = (int.Parse(Accounting_YearBox.Text) + 1).ToString();
                else if (button.Text == "-")
                    Accounting_YearBox.Text = (int.Parse(Accounting_YearBox.Text) - 1).ToString();
                UpdateAccountingTable();
            }
        }

        /// <summary>
        /// Updates the contents of the Accounting_View so only transactions in the given year are shown.
        /// </summary>
        public void UpdateAccountingTable()
        {
            int year = int.Parse(Accounting_YearBox.Text);
            transactions.Clear();
            for (int i = 0; i < Transaction.list.Count; i++)
            {
                if (year == Transaction.list[i].Date.Year)
                    transactions.Add(Transaction.list[i]);
            }
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

            addTransaction.ShowDialog();
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
        #endregion

        #region Members

        /// <summary>
        /// Opens dialog where the user can add a new member
        /// </summary>
        private void Members_AddButton_Click(object sender, EventArgs e)
        {
            
            AddMember addMember = new AddMember
            {
                Owner = this
            };

            addMember.ShowDialog();
            Update_Members_Under25Label();

        }

        /// <summary>
        /// Opens dialog where the user can remove a member
        /// </summary>
        private void RemoveMember(object sender, EventArgs e)
        {
            if (Persons.Count > 0)
            {
                Persons.Remove(shownPersons[Members_View.CurrentCell.OwningRow.Index]);
            }
            UpdateMemberTable();
            Update_Members_Under25Label();

        }

        /// <summary>
        /// Updates Members under 25 label
        /// </summary>
        private void Update_Members_Under25Label() 
        {
            int NumberUnder25 = 0;

            for (int i = 0; i < shownPersons.Count; i++)
            {
                if (shownPersons[i].Age < 25)
                    NumberUnder25++;
            }
            Members_Under25Label.Text = MembersUnder25Text + " " + NumberUnder25;
        }

        private void UpdateMemberPeriod(object sender, EventArgs e)
        {
            if (sender != null)
            {
                Button button = (Button)sender;
                if (button.Text == "+")
                {
                    MemberInYear = MemberInYear.AddYears(1);
                    Members_PeriodeBox.Text = MemberInYear.Year.ToString();


                }
                else if (button.Text == "-")
                {

                    MemberInYear = MemberInYear.AddYears(-1);
                    Members_PeriodeBox.Text = MemberInYear.Year.ToString();

                }
                UpdateMemberTable();
                Update_Members_Under25Label();
            }
        }

        public void UpdateMemberTable()
        {
            shownPersons.Clear();
            for (int i = 0; i < Persons.Count; i++)
            {
                if (Persons[i].memberSince.Year == MemberInYear.Year)
                    shownPersons.Add(Persons[i]);
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