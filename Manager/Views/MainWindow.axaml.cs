using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.VisualTree;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using Timotheus.Schedule;
using Timotheus.Utility;
using Timotheus.ViewModels;

namespace Timotheus.Views
{
    /// <summary>
    /// MainWindow of the Timotheus app. Contains Calendar and Filesharing tabs.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Data Context of the MainWindow.
        /// </summary>
        private readonly MainViewModel mvm;

        /// <summary>
        /// Constructor. Creates datacontext and loads XAML.
        /// </summary>
        public MainWindow()
        {
            mvm = new();
            AvaloniaXamlLoader.Load(this);
            DataContext = mvm;
        }

        /// <summary>
        /// Loads the key last used.
        /// </summary>
        private async void StartUpKey()
        {
            try
            {
                string keyPath = string.Empty;

                if (Timotheus.FirstTime)
                {
                    FirstTimeSetup fts = new();
                    await fts.ShowDialog(this);
                    if (fts.DialogResult == DialogResult.OK)
                    {
                        keyPath = fts.Path;
                    }
                }
                else
                    keyPath = Timotheus.Registry.Retrieve("KeyPath").Value;

                if (!File.Exists(keyPath))
                {
                    Timotheus.Registry.Delete("KeyPath");
                }
                switch (Path.GetExtension(keyPath))
                {
                    case ".tkey":
                        string encodedPassword = Timotheus.Registry.Retrieve("KeyPassword").Value;
                        string password = string.Empty;
                        if (encodedPassword != string.Empty)
                        {
                            password = Cipher.Decrypt(encodedPassword);
                            mvm.LoadKey(keyPath, password);
                            InsertKey();
                        }
                        else
                        {
                            PasswordDialog passDialog = new();
                            await passDialog.ShowDialog(this);
                            if (passDialog.DialogResult == DialogResult.OK)
                            {
                                password = passDialog.Password;
                                mvm.LoadKey(keyPath, password);
                                InsertKey();

                                if (passDialog.Save)
                                {
                                    encodedPassword = Cipher.Encrypt(password);
                                    Timotheus.Registry.Update("KeyPassword", encodedPassword);
                                }
                            }
                        }
                        break;
                    case ".txt":
                        mvm.LoadKey(keyPath);
                        InsertKey();
                        break;
                }
            }
            catch (Exception ex)
            {
                Error(Localization.Localization.Exception_NoKeys, ex.Message);
            }
        }

        /// <summary>
        /// Looks for updates on the website.
        /// </summary>
        private async void LookForUpdates()
        {
            string foundVersion = Timotheus.Version;

            // Fetch version from website
            WebRequest request = WebRequest.Create("https://mjrj.dk/software/timotheus/index.html");
            WebResponse response = request.GetResponse();
            StreamReader reader = new(response.GetResponseStream());
            string[] text = reader.ReadToEnd().Split(Environment.NewLine);
            response.Close();

            for (int i = 0; i < text.Length; i++)
            {
                string line = text[i].Trim();
                if (line.StartsWith("v."))
                    foundVersion = line[3..];
            }

            // Show update dialog if user hasn't disabled it
            if (foundVersion != Timotheus.Version && Timotheus.Registry.Retrieve("ShowUpdateDialog").Value != "false")
            {
                UpdateWindow dialog = new();
                dialog.DialogTitle = Localization.Localization.UpdateDialog_Title;
                dialog.DialogText = Localization.Localization.UpdateDialog_Text.Replace("#", foundVersion);
                await dialog.ShowDialog(this);
                if (dialog.DontShowAgain)
                    Timotheus.Registry.Update("ShowUpdateDialog", "false");
            }
        }

        /// <summary>
        /// Opens the window and retrieves last used key.
        /// </summary>
        public override void Show()
        {
            base.Show();
            if (!isShown)
            {
                StartUpKey();
                LookForUpdates();
                isShown = true;
            }
        }
        private static bool isShown = false;

        private async void InsertKey()
        {
            ProgressDialog dialog = new();
            try
            {
                dialog.Title = Localization.Localization.InsertKey_Dialog;
                await dialog.ShowDialog(this, mvm.InsertingKey);
                mvm.UpdateCalendarTable();
                mvm.UpdatePeopleTable();
            }
            catch (Exception ex)
            {
                Error(Localization.Localization.Exception_Name, ex.Message);
            }
        }

        #region Calendar
        /// <summary>
        /// Changes the selected year and calls UpdateTable.
        /// </summary>
        private void Period_Click(object sender, RoutedEventArgs e)
        {
            if (sender != null)
            {
                Button button = (Button)sender;
                if (button.Name == "+")
                    mvm.UpdatePeriod(true);
                else if (button.Name == "-")
                    mvm.UpdatePeriod(false);
            }
        }

        /// <summary>
        /// Updates the period according to the textbox.
        /// </summary>
        private void Period_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Avalonia.Input.Key.Enter)
            {
                mvm.UpdatePeriod(((TextBox)sender).Text);
                mvm.UpdateCalendarTable();
            }
        }

        /// <summary>
        /// Opens a SyncCalendar dialog to sync the current calendar.
        /// </summary>
        private async void SyncCalendar_Click(object sender, RoutedEventArgs e)
        {
            SyncCalendar dialog = new();
            dialog.Period = mvm.PeriodText;
            dialog.UseCurrent = mvm.Calendar.IsSetup();
            dialog.CanUseCurrent = mvm.Calendar.IsSetup();

            await dialog.ShowDialog(this);

            if (dialog.DialogResult == DialogResult.OK)
            {
                try
                {
                    if (!dialog.UseCurrent)
                    {
                        mvm.Calendar.SetupSync(dialog.Username, dialog.Password, dialog.URL);
                        mvm.Keys.Update("Calendar-Email", dialog.Username);
                        mvm.Keys.Update("Calendar-Password", dialog.Password);
                        mvm.Keys.Update("Calendar-URL", dialog.URL);
                    }

                    ProgressDialog pDialog = new();
                    pDialog.Title = Localization.Localization.SyncCalendar_Worker;
                    Period syncPeriod;
                    if (dialog.SyncAll)
                        syncPeriod = new Period(DateTime.MinValue, DateTime.MaxValue);
                    else if (dialog.SyncPeriod)
                        syncPeriod = new Period(mvm.PeriodText);
                    else
                        syncPeriod = new Period(dialog.Start, dialog.End.AddDays(1));

                    await pDialog.ShowDialog(this, mvm.Calendar.Sync, syncPeriod);

                    mvm.UpdateCalendarTable();
                }
                catch (Exception ex)
                {
                    Error(Localization.Localization.Exception_Sync, ex.Message);
                }
            }
        }

        /// <summary>
        /// Opens a AddEvent dialog, and adds the result to the current calendar.
        /// </summary>
        private async void AddEvent_Click(object sender, RoutedEventArgs e)
        {
            AddEvent dialog = new();

            string text;
            if ((text = mvm.Keys.Retrieve("Settings-Address").Value) != string.Empty)
                dialog.Location = text;
            if ((text = mvm.Keys.Retrieve("Settings-EventDescription").Value) != string.Empty)
                dialog.Description = text;
            if ((text = mvm.Keys.Retrieve("Settings-EventStart").Value) != string.Empty)
                dialog.StartTime = text;
            if ((text = mvm.Keys.Retrieve("Settings-EventEnd").Value) != string.Empty)
                dialog.EndTime = text;

            await dialog.ShowDialog(this);

            if (dialog.DialogResult == DialogResult.OK)
            {
                try
                {
                    mvm.Calendar.Events.Add(new Event(dialog.Start, dialog.End, dialog.EventName, dialog.Description, dialog.Location, string.Empty));
                    mvm.UpdateCalendarTable();
                }
                catch (Exception ex)
                {
                    Error(Localization.Localization.Exception_InvalidEvent, ex.Message);
                }
            }
        }

        /// <summary>
        /// Marks the selected event for deletion.
        /// </summary>
        private void RemoveEvent_Click(object sender, RoutedEventArgs e)
        {
            EventViewModel ev = (EventViewModel)((Button)e.Source).DataContext;
            if (ev != null)
            {
                mvm.Remove(ev);
            }
        }

        /// <summary>
        /// Opens a SaveFileDialog to export the current Calendar (in the given period) as a PDF.
        /// </summary>
        private async void ExportPDF_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new();
            FileDialogFilter filter = new();
            filter.Extensions.Add("pdf");
            filter.Name = "PDF Files (.pdf)";

            saveFileDialog.Filters = new();
            saveFileDialog.Filters.Add(filter);

            string result = await saveFileDialog.ShowAsync(this);

            if (result != null)
            {
                try
                {
                    FileInfo file = new(result);
                    mvm.ExportCalendar(file.Name, file.DirectoryName);
                }
                catch (Exception ex)
                {
                    Error(Localization.Localization.Exception_Saving, ex.Message);
                }
            }
        }
        #endregion

        #region Files
        /// <summary>
        /// Goes one level up from the currently visible directory.
        /// </summary>
        private void UpDirectory_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                mvm.GoUpDirectory();
            }
            catch (Exception ex)
            {
                Error(Localization.Localization.Exception_Name, ex.Message);
            }
        }

        /// <summary>
        /// Synchronizes the files in the local and remote directory.
        /// </summary>
        private async void SyncFiles_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ProgressDialog dialog = new();
                dialog.Title = Localization.Localization.SFTP_SyncWorker;
                await dialog.ShowDialog(this, mvm.Directory.Sync);
                mvm.GoToDirectory(mvm.Directory.RemotePath);
            }
            catch (Exception ex)
            {
                Error(Localization.Localization.Exception_Name, ex.Message);
            }
        }

        /// <summary>
        /// Opens a SetupSFTP dialog to define the SFTP parameters.
        /// </summary>
        private async void SetupFiles_Click(object sender, RoutedEventArgs e)
        {
            SetupSFTP dialog = new();
            dialog.Local = mvm.Keys.Retrieve("SSH-LocalDirectory").Value;
            dialog.Remote = mvm.Keys.Retrieve("SSH-RemoteDirectory").Value;
            dialog.Host = mvm.Keys.Retrieve("SSH-URL").Value;
            dialog.Port = mvm.Keys.Retrieve("SSH-Port").Value;
            dialog.Username = mvm.Keys.Retrieve("SSH-Username").Value;
            dialog.Password = mvm.Keys.Retrieve("SSH-Password").Value;

            await dialog.ShowDialog(this);
            if (dialog.DialogResult == DialogResult.OK)
            {
                try
                {
                    mvm.Directory = new DirectoryViewModel(dialog.Local, dialog.Remote, dialog.Host, int.Parse(dialog.Port), dialog.Username, dialog.Password);
                    mvm.Keys.Update("SSH-LocalDirectory", dialog.Local);
                    mvm.Keys.Update("SSH-RemoteDirectory", dialog.Remote);
                    mvm.Keys.Update("SSH-URL", dialog.Host);
                    mvm.Keys.Update("SSH-Port", dialog.Port);
                    mvm.Keys.Update("SSH-Username", dialog.Username);
                    mvm.Keys.Update("SSH-Password", dialog.Password);
                }
                catch (Exception ex)
                {
                    Error(Localization.Localization.Exception_Name, ex.Message);
                }
            }
        }

        /// <summary>
        /// Goes one level down into the selected directory.
        /// </summary>
        private void File_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var row = ((IControl)e.Source).GetSelfAndVisualAncestors()
                                .OfType<DataGridRow>()
                                .FirstOrDefault();

                if (row != null)
                {
                    FileViewModel file = row.DataContext as FileViewModel;
                    if (file.IsDirectory)
                    {
                        if (file.RemoteFullName != string.Empty)
                            mvm.GoToDirectory(file.RemoteFullName);
                        else
                            mvm.GoToDirectory(file.LocalFullName);
                    }
                    else
                    {
                        if (file.LocalFullName != string.Empty)
                        {
                            Process p = new();
                            p.StartInfo = new ProcessStartInfo(file.LocalFullName)
                            {
                                UseShellExecute = true
                            };
                            p.Start();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Error(Localization.Localization.Exception_Name, ex.Message);
            }
        }

        #region Colors
        readonly IBrush NewLight = new SolidColorBrush(Color.FromRgb(230, 255, 230));
        readonly IBrush NewDark = new SolidColorBrush(Color.FromRgb(210, 255, 210));

        readonly IBrush UpdateLight = new SolidColorBrush(Color.FromRgb(255, 255, 230));
        readonly IBrush UpdateDark = new SolidColorBrush(Color.FromRgb(255, 255, 200));

        readonly IBrush DeleteLight = new SolidColorBrush(Color.FromRgb(255, 230, 230));
        readonly IBrush DeleteDark = new SolidColorBrush(Color.FromRgb(255, 210, 210));

        readonly IBrush StdLight = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        readonly IBrush StdDark = new SolidColorBrush(Color.FromRgb(230, 230, 230));
        #endregion

        private void Files_RowLoading(object sender, DataGridRowEventArgs e)
        {
            if (e.Row.DataContext is FileViewModel file)
            {
                if (e.Row.GetIndex() % 2 == 1)
                {
                    e.Row.Background = file.Handle switch
                    {
                        SyncHandle.NewDownload or SyncHandle.NewUpload => NewDark,
                        SyncHandle.Download or SyncHandle.Upload => UpdateDark,
                        SyncHandle.DeleteLocal or SyncHandle.DeleteRemote => DeleteDark,
                        _ => StdDark,
                    };
                }
                else
                {
                    e.Row.Background = file.Handle switch
                    {
                        SyncHandle.NewDownload or SyncHandle.NewUpload => NewLight,
                        SyncHandle.Download or SyncHandle.Upload => UpdateLight,
                        SyncHandle.DeleteLocal or SyncHandle.DeleteRemote => DeleteLight,
                        _ => StdLight,
                    };
                }
            }
        }
        #endregion

        #region Consent Forms
        private async void AddPerson_Click(object sender, RoutedEventArgs e)
        {
            AddConsentForm dialog = new();
            await dialog.ShowDialog(this);
            if (dialog.DialogResult == DialogResult.OK)
            {
                mvm.AddPerson(dialog.ConsentName, dialog.ConsentDate, dialog.ConsentVersion, dialog.ConsentComment);
            }
        }

        private void ToggleActivePerson_Click(object sender, RoutedEventArgs e)
        {
            PersonViewModel person = (PersonViewModel)((Button)e.Source).DataContext;
            person.Active = !person.Active;
            mvm.UpdatePeopleTable();
        }

        private void People_RowLoading(object sender, DataGridRowEventArgs e)
        {
            if (e.Row.DataContext is PersonViewModel person)
            {
                if (person.Active)
                    e.Row.Background = StdLight;
                else
                    e.Row.Background = StdDark;
            }
        }

        private void RemovePerson_Click(object sender, RoutedEventArgs e)
        {
            PersonViewModel person = (PersonViewModel)((Button)e.Source).DataContext;
            if (person != null)
            {
                mvm.Remove(person);
            }
        }

        private void ToggleInactive_Click(object sender, RoutedEventArgs e)
        {
            mvm.ShowInactive = !mvm.ShowInactive;
            mvm.UpdatePeopleTable();
        }

        private void SearchPeople(object sender, KeyEventArgs e)
        {
            mvm.UpdatePeopleTable();
        }
        #endregion

        #region Toolstrip
        /// <summary>
        /// Clears the Calendar and Directory.
        /// </summary>
        private async void NewProject_Click(object sender, RoutedEventArgs e)
        {
            MessageBox msDialog = new();
            msDialog.DialogTitle = Localization.Localization.ToolStrip_NewFile;
            msDialog.DialogText = Localization.Localization.ToolStrip_NewSecure;
            await msDialog.ShowDialog(this);
            if (msDialog.DialogResult == DialogResult.OK)
            {
                Timotheus.Registry.Delete("KeyPath");
                Timotheus.Registry.Delete("KeyPassword");
                mvm.NewProject();
            }
        }

        /// <summary>
        /// Opens a OpenCalendar dialog
        /// </summary>
        private async void Open_Click(object sender, RoutedEventArgs e)
        {
            switch (mvm.CurrentTab) {
                case 0:
                    OpenCalendar dialog = new();
                    dialog.Username = mvm.Keys.Retrieve("Calendar-Email").Value;
                    dialog.Password = mvm.Keys.Retrieve("Calendar-Password").Value;
                    dialog.URL = mvm.Keys.Retrieve("Calendar-URL").Value;
                    dialog.Path = mvm.Keys.Retrieve("Calendar-Path").Value;

                    await dialog.ShowDialog(this);

                    if (dialog.DialogResult == DialogResult.OK)
                    {
                        try
                        {
                            if (dialog.IsRemote)
                            {
                                mvm.Calendar = new(dialog.Username, dialog.Password, dialog.URL);
                                mvm.Keys.Update("Calendar-Email", dialog.Username);
                                mvm.Keys.Update("Calendar-Password", dialog.Password);
                                mvm.Keys.Update("Calendar-URL", dialog.URL);
                                mvm.UpdateCalendarTable();
                            }
                            else
                            {
                                mvm.Calendar = new(dialog.Path);
                                mvm.Keys.Update("Calendar-Path", dialog.Path);
                                mvm.UpdateCalendarTable();
                            }
                        }
                        catch (Exception ex)
                        {
                            Error(Localization.Localization.Exception_InvalidCalendar, ex.Message);
                        }
                    }
                    break;
                case 2:
                    OpenFileDialog openFileDialog = new();

                    FileDialogFilter txtFilter = new();
                    txtFilter.Extensions.Add("csv");
                    txtFilter.Name = "CSV (.csv)";

                    openFileDialog.Filters = new();
                    openFileDialog.Filters.Add(txtFilter);

                    string[] result = await openFileDialog.ShowAsync(this);
                    if (result != null && result.Length > 0)
                    {
                        mvm.PersonRepo = new(result[0]);
                        mvm.UpdatePeopleTable();
                        mvm.Keys.Update("Person-File", result[0]);
                    }
                    break;
            }
        }

        /// <summary>
        /// Opens a SaveFileDialog to save the current Calendar.
        /// </summary>
        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new();
            FileDialogFilter filter = new();
            string result;
            switch (mvm.CurrentTab)
            {
                case 0:
                    filter.Extensions.Add("ics");
                    filter.Name = "Calendar (.ics)";

                    saveFileDialog.Filters = new();
                    saveFileDialog.Filters.Add(filter);

                    result = await saveFileDialog.ShowAsync(this);
                    if (result != null)
                    {
                        try
                        {
                            mvm.Calendar.Save(result);
                        }
                        catch (Exception ex)
                        {
                            Error(Localization.Localization.Exception_Saving, ex.Message);
                        }
                    }
                    break;
                case 2:
                    filter.Extensions.Add("csv");
                    filter.Name = "CSV (.csv)";

                    saveFileDialog.Filters = new();
                    saveFileDialog.Filters.Add(filter);

                    result = await saveFileDialog.ShowAsync(this);
                    if (result != null)
                    {
                        try
                        {
                            mvm.PersonRepo.Save(result);
                        }
                        catch (Exception ex)
                        {
                            Error(Localization.Localization.Exception_Saving, ex.Message);
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Opens a SaveFileDialog so the user can save the current key as a file.
        /// </summary>
        private async void SaveKey_Click(object sender, RoutedEventArgs e)
        {
            string keyPath = Timotheus.Registry.Retrieve("KeyPath").Value;
            if (!File.Exists(keyPath))
                SaveAsKey_Click(sender, e);
            else
            {
                switch (Path.GetExtension(keyPath))
                {
                    case ".tkey":
                        string encodedPassword = Timotheus.Registry.Retrieve("KeyPassword").Value;
                        string password;
                        if (encodedPassword != string.Empty)
                        {
                            password = Cipher.Decrypt(encodedPassword);
                            try
                            {
                                mvm.SaveKey(keyPath, password);
                            }
                            catch (Exception ex)
                            {
                                Error(Localization.Localization.Exception_Saving, ex.Message);
                            }
                        }
                        else
                        {
                            PasswordDialog dialog = new();
                            await dialog.ShowDialog(this);
                            if (dialog.DialogResult == DialogResult.OK)
                            {
                                password = dialog.Password;
                                try
                                {
                                    mvm.SaveKey(keyPath, password);
                                }
                                catch (Exception ex)
                                {
                                    Error(Localization.Localization.Exception_Saving, ex.Message);
                                }

                                if (dialog.Save)
                                {
                                    encodedPassword = Cipher.Encrypt(password);
                                    Timotheus.Registry.Update("KeyPassword", encodedPassword);
                                }
                            }
                        }
                        break;
                    case ".txt":
                        try
                        {
                            mvm.SaveKey(keyPath);
                        }
                        catch (Exception ex)
                        {
                            Error(Localization.Localization.Exception_Saving, ex.Message);
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Opens a SaveFileDialog so the user can save the current key as a file.
        /// </summary>
        private async void SaveAsKey_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new();
            saveFileDialog.Filters = new();

            FileDialogFilter tkeyFilter = new();
            tkeyFilter.Extensions.Add("tkey");
            tkeyFilter.Name = "Encrypted key (.tkey)";
            saveFileDialog.Filters.Add(tkeyFilter);

            FileDialogFilter txtFilter = new();
            txtFilter.Extensions.Add("txt");
            txtFilter.Name = "Text files (.txt)";
            saveFileDialog.Filters.Add(txtFilter);

            string result = await saveFileDialog.ShowAsync(this);
            if (result != null)
            {
                switch (Path.GetExtension(result))
                {
                    case ".tkey":
                        PasswordDialog dialog = new();
                        await dialog.ShowDialog(this);
                        if (dialog.DialogResult == DialogResult.OK)
                        {
                            string password = dialog.Password;
                            try
                            {
                                mvm.SaveKey(result, password);
                            }
                            catch (Exception ex)
                            {
                                Error(Localization.Localization.Exception_Saving, ex.Message);
                            }
                        }
                        break;
                    case ".txt":
                        try
                        {
                            mvm.SaveKey(result);
                        }
                        catch (Exception ex)
                        {
                            Error(Localization.Localization.Exception_Saving, ex.Message);
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Opens an OpenFileDialog so the user can open a key file.
        /// </summary>
        private async void OpenKey_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new();

            FileDialogFilter txtFilter = new();
            txtFilter.Extensions.Add("txt");
            txtFilter.Extensions.Add("tkey");
            txtFilter.Name = "Key files (.txt, .tkey)";

            openFileDialog.Filters = new();
            openFileDialog.Filters.Add(txtFilter);

            string[] result = await openFileDialog.ShowAsync(this);
            if (result != null && result.Length > 0)
            {
                switch (Path.GetExtension(result[0]))
                {
                    case ".tkey":
                        PasswordDialog passDialog = new();
                        await passDialog.ShowDialog(this);
                        if (passDialog.DialogResult == DialogResult.OK)
                        {
                            string password = passDialog.Password;
                            if (passDialog.Save)
                            {
                                string encodedPassword = Cipher.Encrypt(password);
                                Timotheus.Registry.Update("KeyPassword", encodedPassword);
                            }
                            else
                                Timotheus.Registry.Delete("KeyPassword");
                            try
                            {
                                mvm.LoadKey(result[0], password);
                                InsertKey();
                            }
                            catch (Exception ex)
                            {
                                Error(Localization.Localization.Exception_LoadFailed, ex.Message);
                            }
                        }
                        break;
                    case ".txt":
                        try
                        {
                            mvm.LoadKey(result[0]);
                            InsertKey();
                        }
                        catch (Exception ex)
                        {
                            Error(Localization.Localization.Exception_LoadFailed, ex.Message);
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Opens an EditKey dialog where the user can change the values of the key.
        /// </summary>
        private async void EditKey_Click(object sender, RoutedEventArgs e)
        {
            EditKey dialog = new();
            dialog.Text = mvm.Keys.ToString();
            await dialog.ShowDialog(this);
            if (dialog.DialogResult == DialogResult.OK)
            {
                try
                {
                    mvm.NewProject(dialog.Text);
                }
                catch (Exception ex)
                {
                    Error(Localization.Localization.Exception_Name, ex.Message);
                }
            }
        }

        /// <summary>
        /// Opens a Help dialog.
        /// </summary>
        private void Help_Click(object sender, RoutedEventArgs e)
        {
            Help dialog = new();
            dialog.Show(this);
        }

        /// <summary>
        /// Opens a Settings dialog.
        /// </summary>
        private async void Settings_Click(object sender, RoutedEventArgs e)
        {
            Settings dialog = new();

            dialog.AssociationName = mvm.Keys.Retrieve("Settings-Name").Value;
            dialog.AssociationAddress = mvm.Keys.Retrieve("Settings-Address").Value;
            dialog.ImagePath = mvm.Keys.Retrieve("Settings-Image").Value;
            dialog.Description = mvm.Keys.Retrieve("Settings-EventDescription").Value;
            dialog.StartTime = mvm.Keys.Retrieve("Settings-EventStart").Value;
            dialog.EndTime = mvm.Keys.Retrieve("Settings-EventEnd").Value;

            await dialog.ShowDialog(this);
            if (dialog.DialogResult == DialogResult.OK)
            {
                if (dialog.AssociationName != string.Empty)
                    mvm.Keys.Update("Settings-Name", dialog.AssociationName);
                if (dialog.AssociationAddress != string.Empty)
                    mvm.Keys.Update("Settings-Address", dialog.AssociationAddress);
                if (dialog.ImagePath != string.Empty)
                    mvm.Keys.Update("Settings-Image", dialog.ImagePath);
                if (dialog.Description != string.Empty)
                    mvm.Keys.Update("Settings-EventDescription", dialog.Description);
                if (dialog.StartTime != string.Empty)
                    mvm.Keys.Update("Settings-EventStart", dialog.StartTime);
                if (dialog.EndTime != string.Empty)
                    mvm.Keys.Update("Settings-EventEnd", dialog.EndTime);
            }
        }
        #endregion

        private async void Error(string title, string Message)
        {
            MessageBox msDialog = new();
            msDialog.DialogTitle = title;
            msDialog.DialogText = Message;
            await msDialog.ShowDialog(this);
        }
    }
}