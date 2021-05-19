using Timotheus.Schedule;
using Timotheus.Utility;
using Timotheus.Persons;
using Timotheus.Accounting;
using Timotheus.IO;
using System;
using System.ComponentModel;
using System.Collections.Generic;
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
        /// Register containing all the keys loaded at startup.
        /// </summary>
        private Register keys;
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
        /// Type of period used by Calendar_View.
        /// </summary>
        private readonly Period calendarPeriod = new Period(new DateTime(DateTime.Now.Year, 1, 1), new DateTime(DateTime.Now.Year + 1, 1, 1));

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
            try
            {
                string KeyPath = Program.Registry.Get("KeyPath");
                if (KeyPath != string.Empty)
                {
                    keys = new Register(KeyPath, ':');
                    InsertKeys();
                }
                else
                    keys = new Register(':');
            }
            catch (Exception e)
            {
                Program.Error("Exception_LoadFailed", e.Message);
                keys = new Register(':');
            }
        }

        /// <summary>
        /// Assigns the different lists to their appropriate DataGridViews, disables AutoGenerateColumns, and loads localization.
        /// </summary>
        private void SetupUI()
        {
            #region Calendar
            Calendar_View.AutoGenerateColumns = false;
            Calendar_View.DataSource = new BindingSource(events, null);
            
            Calendar_PeriodBox.Text = calendarPeriod.ToString();
            Calendar_Page.Text = Program.Localization.Get(Calendar_Page);
            Calendar_StartColumn.HeaderText = Program.Localization.Get(Calendar_StartColumn);
            Calendar_EndColumn.HeaderText = Program.Localization.Get(Calendar_EndColumn);
            Calendar_NameColumn.HeaderText = Program.Localization.Get(Calendar_NameColumn);
            Calendar_DescriptionColumn.HeaderText = Program.Localization.Get(Calendar_DescriptionColumn);
            Calendar_LocationColumn.HeaderText = Program.Localization.Get(Calendar_LocationColumn);
            Calendar_MonthButton.Text = Program.Localization.Get(Calendar_MonthButton);
            Calendar_HalfYearButton.Text = Program.Localization.Get(Calendar_HalfYearButton);
            Calendar_YearButton.Text = Program.Localization.Get(Calendar_YearButton);
            Calendar_AllButton.Text = Program.Localization.Get(Calendar_AllButton);
            Calendar_SaveButton.Text = Program.Localization.Get(Calendar_SaveButton);
            Calendar_OpenButton.Text = Program.Localization.Get(Calendar_OpenButton);
            Calendar_SyncButton.Text = Program.Localization.Get(Calendar_SyncButton);
            Calendar_ExportButton.Text = Program.Localization.Get(Calendar_ExportButton);
            Calendar_RemoveButton.Text = Program.Localization.Get(Calendar_RemoveButton);
            Calendar_AddButton.Text = Program.Localization.Get(Calendar_AddButton);
            #endregion

            #region SFTP
            SFTP_View.AutoGenerateColumns = false;
            SFTP_View.DataSource = new BindingSource(shownFiles, null);
            SFTP_PasswordBox.PasswordChar = '*';
            
            SFTP_Page.Text = Program.Localization.Get(SFTP_Page);
            SFTP_HostLabel.Text = Program.Localization.Get(SFTP_HostLabel);
            SFTP_UsernameLabel.Text = Program.Localization.Get(SFTP_UsernameLabel);
            SFTP_PasswordLabel.Text = Program.Localization.Get(SFTP_PasswordLabel);
            SFTP_RemoteDirectoryLabel.Text = Program.Localization.Get(SFTP_RemoteDirectoryLabel);
            SFTP_LocalDirectoryLabel.Text = Program.Localization.Get(SFTP_LocalDirectoryLabel);
            SFTP_BrowseButton.Text = Program.Localization.Get(SFTP_BrowseButton);
            SFTP_ShowDirectoryButton.Text = Program.Localization.Get(SFTP_ShowDirectoryButton);
            SFTP_DownloadButton.Text = Program.Localization.Get(SFTP_DownloadButton);
            SFTP_SyncButton.Text = Program.Localization.Get(SFTP_SyncButton);
            SFTP_NameColumn.HeaderText = Program.Localization.Get(SFTP_NameColumn);
            SFTP_SizeColumn.HeaderText = Program.Localization.Get(SFTP_SizeColumn);
            #endregion

            #region Members
            Members_View.AutoGenerateColumns = false;
            Members_View.DataSource = new BindingSource(members, null);

            Members_PeriodeBox.Text = MemberlistYear.Year.ToString();
            Members_Page.Text = Program.Localization.Get(Members_Page);
            Members_NameColumn.HeaderText = Program.Localization.Get(Members_NameColumn);
            Members_AddressColumn.HeaderText = Program.Localization.Get(Members_AddressColumn);
            Members_BirthdayColumn.HeaderText = Program.Localization.Get(Members_BirthdayColumn);
            Members_EntryColumn.HeaderText = Program.Localization.Get(Members_EntryColumn);
            Members_AddButton.Text = Program.Localization.Get(Members_AddButton);
            Members_RemoveButton.Text = Program.Localization.Get(Members_RemoveButton);
            MembersUnder25Text = Program.Localization.Get(Members_Under25Label);
            UpdateMemberTable();
            #endregion

            #region Consent Forms
            ConsentForms_View.AutoGenerateColumns = false;
            ConsentForms_View.DataSource = new BindingSource(consentForms, null);
            
            ConsentForms_Page.Text = Program.Localization.Get(ConsentForms_Page);
            ConsentForms_AddButton.Text = Program.Localization.Get(ConsentForms_AddButton);
            ConsentForms_RemoveButton.Text = Program.Localization.Get(ConsentForms_RemoveButton);
            ConsentForms_NameColumn.HeaderText = Program.Localization.Get(ConsentForms_NameColumn);
            ConsentForms_DateColumn.HeaderText = Program.Localization.Get(ConsentForms_DateColumn);
            ConsentForms_VersionColumn.HeaderText = Program.Localization.Get(ConsentForms_VersionColumn);
            ConsentForms_CommentColumn.HeaderText = Program.Localization.Get(ConsentForms_CommentColumn);
            #endregion

            #region Accounting
            Accounting_YearBox.Text = calendarPeriod.ToString();
            Accounting_TransactionsView.AutoGenerateColumns = false;
            Accounting_TransactionsView.DataSource = new BindingSource(transactions, null);
            Accounting_AccountsView.AutoGenerateColumns = false;
            Accounting_AccountsView.DataSource = new BindingSource(accounts, null);
            Accounting_TransactionsView.Columns[5].DefaultCellStyle.ForeColor = Color.Red;
            
            Accounting_Page.Text = Program.Localization.Get(Accounting_Page);
            Accounting_DateColumn.HeaderText = Program.Localization.Get(Accounting_DateColumn);
            Accounting_AppendixColumn.HeaderText = Program.Localization.Get(Accounting_AppendixColumn);
            Accounting_DescriptionColumn.HeaderText = Program.Localization.Get(Accounting_DescriptionColumn);
            Accounting_AccountNumberColumn.HeaderText = Program.Localization.Get(Accounting_AccountNumberColumn);
            Accounting_InColumn.HeaderText = Program.Localization.Get(Accounting_InColumn);
            Accounting_OutColumn.HeaderText = Program.Localization.Get(Accounting_OutColumn);
            Accounting_BalanceColumn.HeaderText = Program.Localization.Get(Accounting_BalanceColumn);

            //Only for testing
            accounts.Add(new Account(1, "Kontingent"));
            accounts.Add(new Account(2, "Materialer"));
            accounts.Add(new Account(3, "Gebyrer"));
            #endregion

            #region Settings
            Settings_Page.Text = Program.Localization.Get(Settings_Page);
            Settings_InfoBox.Text = Program.Localization.Get(Settings_InfoBox);
            Settings_NameLabel.Text = Program.Localization.Get(Settings_NameLabel);
            Settings_AddressLabel.Text = Program.Localization.Get(Settings_AddressLabel);
            Settings_LogoLabel.Text = Program.Localization.Get(Settings_LogoLabel);
            Settings_BrowseButton.Text = Program.Localization.Get(Settings_BrowseButton);
            #endregion
        }

        /// <summary>
        /// Inserts the keys into their respective fields.
        /// </summary>
        private void InsertKeys()
        {
            SFTP_UsernameBox.Text = keys.Get("SSH-Username");
            SFTP_PasswordBox.Text = keys.Get("SSH-Password");
            SFTP_HostBox.Text = keys.Get("SSH-URL");
            SFTP_RemoteDirectoryBox.Text = keys.Get("SSH-RemoteDirectory");
            SFTP_LocalDirectoryBox.Text = keys.Get("SSH-LocalDirectory");
            Settings_NameBox.Text = keys.Get("Settings-Name");
            Settings_AddressBox.Text = keys.Get("Settings-Address");
            Settings_LogoBox.Text = keys.Get("Settings-Image");
            Settings_PictureBox.Image = Image.FromFile(keys.Get("Settings-Image"));
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
                if (calendarPeriod.In(calendar.events[i]) && !calendar.events[i].Deleted)
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
                    calendarPeriod.Add();
                }
                else if (button.Text == "-")
                {
                    calendarPeriod.Subtract();
                }
                UpdatePeriodBox();
                UpdateCalendarTable();
            }
        }

        /// <summary>
        /// Updates the year text according to the selected period.
        /// </summary>
        private void PeriodChangedButton(object sender, EventArgs e)
        {
            Calendar_AddYearButton.Enabled = !Calendar_AllButton.Checked;
            Calendar_SubtractYearButton.Enabled = !Calendar_AllButton.Checked;

            if (Calendar_AllButton.Checked)
                calendarPeriod.SetType(PeriodType.All);
            if (Calendar_YearButton.Checked)
                calendarPeriod.SetType(PeriodType.Year);
            else if (Calendar_HalfYearButton.Checked)
                calendarPeriod.SetType(PeriodType.Halfyear);
            else if (Calendar_MonthButton.Checked)
                calendarPeriod.SetType(PeriodType.Month);

            UpdatePeriodBox();
            UpdateCalendarTable();
        }

        /// <summary>
        /// Opens dialog where the user can define the attributes of the new event.
        /// </summary>
        private void AddEvent(object sender, EventArgs e)
        {
            AddEvent addEvent = new AddEvent(calendar, Settings_AddressBox.Text)
            {
                Owner = this
            };
            DialogResult result = addEvent.ShowDialog();
            if (result == DialogResult.OK)
                UpdateCalendarTable();
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
                    byte[] data = System.Text.Encoding.UTF8.GetBytes(calendar.ToString());
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
            OpenCalendar open = new OpenCalendar(keys)
            {
                Owner = this
            };
            DialogResult result = open.ShowDialog();
            if (result == DialogResult.OK)
            {
                calendar = open.calendar;
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
                    Program.Error("Exception_Saving", ex.Message);
                }
            }
        }

        /// <summary>
        /// Opens dialog where the user can sync the current calendar with a remote calendar.
        /// </summary>
        private void SyncCalendar(object sender, EventArgs e)
        {
            SyncCalendar sync = new SyncCalendar(calendar, calendarPeriod, keys)
            {
                Owner = this
            };
            DialogResult result = sync.ShowDialog();
            if (result == DialogResult.OK)
                UpdateCalendarTable();
        }

        /// <summary>
        /// Updates the period if the user manually changes the period in the text box.
        /// </summary>
        private void Calendar_PeriodBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                calendarPeriod.SetPeriod(Calendar_PeriodBox.Text);

                switch (calendarPeriod.Type)
                {
                    case PeriodType.All:
                        Calendar_AllButton.Checked = true;
                        break;
                    case PeriodType.Year:
                        Calendar_YearButton.Checked = true;
                        break;
                    case PeriodType.Halfyear:
                        Calendar_HalfYearButton.Checked = true;
                        break;
                    case PeriodType.Month:
                        Calendar_MonthButton.Checked = true;
                        break;
                }

                UpdateCalendarTable();
                e.Handled = true;
            }
        }

        /// <summary>
        /// Updates the text in the period box.
        /// </summary>
        private void UpdatePeriodBox()
        {
            Calendar_PeriodBox.Text = calendarPeriod.ToString();
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
                Program.Error("Exception_InvalidInput", ex.Message);
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
                Program.Error("Exception_InvalidInput", ex.Message);
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
                Program.Error("Exception_InvalidInput", ex.Message);
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
                UpdateMemberTable();
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
            AddConsentForm addConsentForm = new AddConsentForm(Person.list)
            {
                Owner = this
            };
            DialogResult result = addConsentForm.ShowDialog();
            if (result == DialogResult.OK)
                UpdateConsentFormsTable();
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
                UpdateTransactionsTable();
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

        #region Toolstrip
        /// <summary>
        /// Opens a dialog so the user can select a file that has all the keys.
        /// </summary>
        private void LoadKey(object sender, EventArgs e)
        {
            // open file dialog   
            OpenFileDialog open = new OpenFileDialog
            {
                // image filters  
                Filter = "Key files (*.txt; *.tkey)|*.txt; *.tkey;"
            };

            if (open.ShowDialog() == DialogResult.OK)
            {
                keys = new Register(open.FileName, ':');
                Program.Registry.Set("KeyPath", open.FileName);
                InsertKeys();
            }
        }

        /// <summary>
        /// Opens the help dialog where information about the software and its authors is located.
        /// </summary>
        private void Help(object sender, EventArgs e)
        {
            Help help = new Help
            {
                Owner = this
            };
            help.ShowDialog();
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
        /// Handles errors if inputed date (DataGridView) is not formatted correctly.
        /// </summary>
        private void HandleDateError(object sender, DataGridViewDataErrorEventArgs e)
        {
            ((DataGridView)sender).Rows[e.RowIndex].Cells[e.ColumnIndex].Value = DateTime.Now.Date;
            e.Cancel = true;
        }
    }
}