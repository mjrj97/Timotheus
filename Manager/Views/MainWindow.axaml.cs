using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using Timotheus.IO;
using Timotheus.Utility;
using Timotheus.ViewModels;
using Timotheus.Views.Dialogs;
using Timotheus.Views.Tabs;

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

        private static MainWindow s_instance;
        public static MainWindow Instance
        {
            get
            {
                return s_instance;
            }
            private set
            {
                s_instance = value;
            }
        }

        /// <summary>
        /// Worker that is used to track the progress of the inserting a key.
        /// </summary>
        public BackgroundWorker InsertingKey { get; private set; }

        public List<Tab> Tabs { get; set; }

        /// <summary>
        /// Constructor. Creates datacontext and loads XAML.
        /// </summary>
        public MainWindow()
        {
            Instance = this;
            mvm = new();
            InsertingKey = new();
            InsertingKey.DoWork += InsertKey;
            AvaloniaXamlLoader.Load(this);

            Tabs = new List<Tab>
            {
                this.FindControl<Tab>("FilPage")
            };

            Closing += OnWindowClose;
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
                    keyPath = Timotheus.Registry.Retrieve("KeyPath");

                if (!File.Exists(keyPath))
                {
                    Timotheus.Registry.Delete("KeyPath");
                }
                switch (Path.GetExtension(keyPath))
                {
                    case ".tkey":
                        string encodedPassword = Timotheus.Registry.Retrieve("KeyPassword");
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
            try
            {
                string foundVersion = Timotheus.Version;

                // Fetch version from website
                WebRequest request = WebRequest.Create("https://www.mjrj.dk/software/timotheus/index.html");
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
                if (foundVersion != Timotheus.Version && Timotheus.Registry.Retrieve("LookForUpdates") != "False")
                {
                    UpdateWindow dialog = new();
                    dialog.DialogTitle = Localization.Localization.UpdateDialog_Title;
                    dialog.DialogText = Localization.Localization.UpdateDialog_Text.Replace("#", foundVersion);
                    await dialog.ShowDialog(this);
                    if (dialog.DontShowAgain)
                        Timotheus.Registry.Update("ShowUpdateDialog", "false");
                }
            }
            catch (Exception) { }
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
                await dialog.ShowDialog(this, InsertingKey);

                // WORK AROUND - Cannot set data context in InsertingKey because it is on another thread.
                FilesPage page = (FilesPage)Tabs[0];
                page.DataContext = page.Directory;

                mvm.UpdateCalendarTable();
                mvm.UpdatePeopleTable();
            }
            catch (Exception ex)
            {
                Error(Localization.Localization.Exception_Name, ex.Message);
            }

            if (!Directory.Exists(mvm.Keys.Retrieve("SSH-LocalDirectory")))
            {
                MessageBox messageBox = new();
                messageBox.DialogTitle = Localization.Localization.Exception_Name;
                messageBox.DialogText = Localization.Localization.Exception_FolderNotFound;
                await messageBox.ShowDialog(this);
                if (messageBox.DialogResult == DialogResult.OK)
                {
                    OpenFolderDialog openFolder = new();
                    string path = await openFolder.ShowAsync(this);
                    if (path != string.Empty && path != null)
                    {
                        mvm.Keys.Update("SSH-LocalDirectory", path);
                        InsertKey();
                        messageBox = new();
                        messageBox.DialogTitle = Localization.Localization.InsertKey_ChangeDetected;
                        messageBox.DialogText = Localization.Localization.InsertKey_DoYouWantToSave;
                        await messageBox.ShowDialog(this);
                        if (messageBox.DialogResult == DialogResult.OK)
                            SaveKey_Click(null, null);
                    }
                }
            }
        }

        /// <summary>
        /// "Inserts" the current key, and tries to open the Calendar and File sharing system.
        /// </summary>
        private void InsertKey(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i < Tabs.Count; i++)
            {
                if (sender != null && e != null)
                {
                    InsertingKey.ReportProgress(100/(Tabs.Count)*i, Tabs[i].LoadingTitle);
                    if (InsertingKey.CancellationPending == true)
                        return;
                }

                Tabs[i].Load();
            }

            if (sender != null && e != null)
            {
                InsertingKey.ReportProgress(33, Localization.Localization.InsertKey_LoadCalendar);
                if (InsertingKey.CancellationPending == true)
                    return;
            }

            if (mvm.Keys.Retrieve("Calendar-Email") != string.Empty)
            {
                try
                {
                    mvm.Calendar = new(mvm.Keys.Retrieve("Calendar-Email"), mvm.Keys.Retrieve("Calendar-Password"), mvm.Keys.Retrieve("Calendar-URL"));
                }
                catch (Exception) { mvm.Calendar = new(); }
            }
            else
                mvm.Calendar = new();

            if (sender != null && e != null)
            {
                InsertingKey.ReportProgress(66, Localization.Localization.InsertKey_LoadPeople);
                if (InsertingKey.CancellationPending == true)
                    return;
            }

            if (mvm.Keys.Retrieve("Person-File") != string.Empty)
            {
                try
                {
                    mvm.PersonRepo = new(mvm.Keys.Retrieve("Person-File"));
                }
                catch (Exception) { mvm.PersonRepo = new(); }
            }
            else
                mvm.PersonRepo = new();
        }

        /// <summary>
        /// Clears the Calendar and Directory.
        /// </summary>
        private async void NewProject_Click(object sender, RoutedEventArgs e)
        {
            MessageBox msDialog = new()
            {
                DialogTitle = Localization.Localization.ToolStrip_NewFile,
                DialogText = Localization.Localization.ToolStrip_NewSecure
            };
            await msDialog.ShowDialog(this);
            if (msDialog.DialogResult == DialogResult.OK)
            {
                Timotheus.Registry.Delete("KeyPath");
                Timotheus.Registry.Delete("KeyPassword");

                mvm.NewProject(new Register(':'));
                InsertKey(null, null);
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
                    dialog.Username = mvm.Keys.Retrieve("Calendar-Email");
                    dialog.Password = mvm.Keys.Retrieve("Calendar-Password");
                    dialog.URL = mvm.Keys.Retrieve("Calendar-URL");
                    dialog.Path = mvm.Keys.Retrieve("Calendar-Path");

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
        public async void SaveKey_Click(object sender, RoutedEventArgs e)
        {
            string keyPath = Timotheus.Registry.Retrieve("KeyPath");
            if (!File.Exists(keyPath))
                SaveAsKey_Click(sender, e);
            else
            {
                switch (Path.GetExtension(keyPath))
                {
                    case ".tkey":
                        string encodedPassword = Timotheus.Registry.Retrieve("KeyPassword");
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
        public async void SaveAsKey_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new();
            saveFileDialog.Filters = new();

            FileDialogFilter tkeyFilter = new();
            tkeyFilter.Extensions.Add("tkey");
            tkeyFilter.Name = "Encrypted key (.tkey)";
            saveFileDialog.Filters.Add(tkeyFilter);

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
            txtFilter.Extensions.Add("tkey");
            txtFilter.Name = "Key files (.tkey)";

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
            dialog.Title = dialog.Title + " (" + Timotheus.Registry.Retrieve("KeyPath") + ")";
            dialog.Text = mvm.Keys.ToString();
            await dialog.ShowDialog(this);
            if (dialog.DialogResult == DialogResult.OK)
            {
                try
                {
                    mvm.NewProject(new Register(':', dialog.Text));
                    InsertKey(null, null);
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

            dialog.AssociationName = mvm.Keys.Retrieve("Settings-Name");
            dialog.AssociationAddress = mvm.Keys.Retrieve("Settings-Address");
            dialog.ImagePath = mvm.Keys.Retrieve("Settings-Image");
            dialog.Description = mvm.Keys.Retrieve("Settings-EventDescription");
            dialog.StartTime = mvm.Keys.Retrieve("Settings-EventStart");
            dialog.EndTime = mvm.Keys.Retrieve("Settings-EventEnd");
            dialog.LookForUpdates = Timotheus.Registry.Retrieve("LookForUpdates") != "False";
            dialog.SelectedLanguage = Timotheus.Registry.Retrieve("Language") == "da-DK" ? 1 : 0;
            int initialSelected = dialog.SelectedLanguage;

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
                if (initialSelected != dialog.SelectedLanguage)
                {
                    Timotheus.Registry.Update("Language", dialog.SelectedLanguage == 0 ? "en-US" : "da-DK");

                    MessageBox messageBox = new();
                    messageBox.DialogTitle = Localization.Localization.Settings;
                    messageBox.DialogText = Localization.Localization.Settings_LanguageChanged;
                    await messageBox.ShowDialog(this);
                    if (messageBox.DialogResult == DialogResult.OK)
                    {

                    }
                }

                Timotheus.Registry.Update("LookForUpdates", dialog.LookForUpdates.ToString());
            }
        }
        
        private bool firstClose = true;
        private async void OnWindowClose(object sender, CancelEventArgs e)
        {
            if (firstClose)
            {
                if (mvm.IsThereUnsavedProgress())
                {
                    e.Cancel = true;

                    MessageBox msDialog = new();
                    msDialog.DialogTitle = Localization.Localization.Exception_Warning;
                    msDialog.DialogText = Localization.Localization.Exception_UnsavedProgress;
                    await msDialog.ShowDialog(this);

                    if (msDialog.DialogResult == DialogResult.OK)
                    {
                        firstClose = false;
                        Close();
                    }
                }
            }
        }

        public async void Error(string title, string message)
        {
            MessageBox msDialog = new();
            msDialog.DialogTitle = title;
            msDialog.DialogText = message;
            await msDialog.ShowDialog(this);
        }
    }
}