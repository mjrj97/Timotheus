using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Http;
using Timotheus.IO;
using Timotheus.Utility;
using Timotheus.ViewModels;
using Timotheus.Views.Dialogs;

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
                this.FindControl<Tab>("CalPage"),
                this.FindControl<Tab>("FilPage"),
                this.FindControl<Tab>("PeoPage")
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
                if (!CheckForInternetConnection())
                    throw new Exception(Localization.Localization.Exception_NoInternet);

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
                    default:
                        InsertKey(null, null);
                        UpdateTabs();
                        break;
                }
            }
            catch (Exception ex)
            {
                Timotheus.Log(ex);
                Error(Localization.Localization.Exception_NoKeys, ex.Message);

                mvm.NewProject(new Register(':'));
                InsertKey(null, null);
                UpdateTabs();
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
                HttpClient client = new();
                HttpResponseMessage response = await client.GetAsync("http://www.mjrj.dk/software/timotheus/index.html");
                response.EnsureSuccessStatusCode();
                string[] text = (await response.Content.ReadAsStringAsync()).Split(Environment.NewLine);

                for (int i = 0; i < text.Length; i++)
                {
                    string line = text[i].Trim();
                    if (line.StartsWith("v."))
                        foundVersion = line[3..];
                }

                // Show update dialog if user hasn't disabled it
                if (foundVersion != Timotheus.Version && Timotheus.Registry.Retrieve("LookForUpdates") != "False")
                {
                    UpdateWindow dialog = new()
                    {
                        DialogTitle = Localization.Localization.UpdateDialog_Title,
                        DialogText = Localization.Localization.UpdateDialog_Text.Replace("#", foundVersion)
                    };
                    await dialog.ShowDialog(this);
                    if (dialog.DontShowAgain)
                        Timotheus.Registry.Update("ShowUpdateDialog", "false");
                }
            }
            catch (Exception) 
            {
                //Timotheus.Log(ex); FIX
            }
        }

        /// <summary>
        /// Calls the Update method of all tabs.
        /// </summary>
        private void UpdateTabs()
        {
            foreach (Tab tab in Tabs)
            {
                tab.DataContext = tab.ViewModel;
                tab.Update();
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
                await dialog.ShowDialog(this, InsertingKey);
                UpdateTabs();
            }
            catch (Exception ex)
            {
                Timotheus.Log(ex);
                Error(Localization.Localization.Exception_Name, ex.Message);
            }

            if (mvm.Keys.Retrieve("SSH-URL") != string.Empty)
            {
                if (!Directory.Exists(mvm.Keys.Retrieve("SSH-LocalDirectory")))
                {
                    MessageBox messageBox = new()
                    {
                        DialogTitle = Localization.Localization.Exception_Name,
                        DialogText = Localization.Localization.Exception_FolderNotFound
                    };
                    await messageBox.ShowDialog(this);
                    if (messageBox.DialogResult == DialogResult.OK)
                    {
                        OpenFolderDialog openFolder = new();
                        string path = await openFolder.ShowAsync(this);
                        if (path != string.Empty && path != null)
                        {
                            mvm.Keys.Update("SSH-LocalDirectory", path);
                            InsertKey();
                            messageBox = new()
                            {
                                DialogTitle = Localization.Localization.InsertKey_ChangeDetected,
                                DialogText = Localization.Localization.InsertKey_DoYouWantToSave
                            };
                            await messageBox.ShowDialog(this);
                            if (messageBox.DialogResult == DialogResult.OK)
                                SaveKey_Click(null, null);
                        }
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
                UpdateTabs();
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
                                Timotheus.Log(ex);
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
                                    Timotheus.Log(ex);
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
                            Timotheus.Log(ex);
                            Error(Localization.Localization.Exception_Saving, ex.Message);
                        }
                        break;
                }

                if (sender != null)
                {
                    MessageBox msDialog = new()
                    {
                        DialogTitle = Localization.Localization.Exception_Message,
                        DialogText = Localization.Localization.Exception_SaveSuccessful
                    };
                    await msDialog.ShowDialog(this);
                }
            }
        }

        /// <summary>
        /// Opens a SaveFileDialog so the user can save the current key as a file.
        /// </summary>
        public async void SaveAsKey_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new()
            {
                Filters = new()
            };

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
                                Timotheus.Log(ex);
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
                            Timotheus.Log(ex);
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

            openFileDialog.Filters = new()
            {
                txtFilter
            };

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
                                Timotheus.Log(ex);
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
                            Timotheus.Log(ex);
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
                    Timotheus.Log(ex);
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
            Settings dialog = new()
            {
                AssociationName = mvm.Keys.Retrieve("Settings-Name"),
                AssociationAddress = mvm.Keys.Retrieve("Settings-Address"),
                ImagePath = mvm.Keys.Retrieve("Settings-Image"),
                Description = mvm.Keys.Retrieve("Settings-EventDescription"),
                StartTime = mvm.Keys.Retrieve("Settings-EventStart"),
                EndTime = mvm.Keys.Retrieve("Settings-EventEnd"),
                LookForUpdates = Timotheus.Registry.Retrieve("LookForUpdates") != "False",
                SelectedLanguage = Timotheus.Registry.Retrieve("Language") == "da-DK" ? 1 : 0
            };
            int initialSelected = dialog.SelectedLanguage;

            await dialog.ShowDialog(this);
            if (dialog.DialogResult == DialogResult.OK)
            {
                bool changed = false;

                if (dialog.AssociationName != string.Empty)
                    changed |= mvm.Keys.Update("Settings-Name", dialog.AssociationName);
                if (dialog.AssociationAddress != string.Empty)
                    changed |= mvm.Keys.Update("Settings-Address", dialog.AssociationAddress);
                if (dialog.ImagePath != string.Empty)
                    changed |= mvm.Keys.Update("Settings-Image", dialog.ImagePath);
                if (dialog.Description != string.Empty)
                    changed |= mvm.Keys.Update("Settings-EventDescription", dialog.Description);
                if (dialog.StartTime != string.Empty)
                    changed |= mvm.Keys.Update("Settings-EventStart", dialog.StartTime);
                if (dialog.EndTime != string.Empty)
                    changed |= mvm.Keys.Update("Settings-EventEnd", dialog.EndTime);

                if (changed)
                {
                    MessageBox messageBox = new()
                    {
                        DialogTitle = Localization.Localization.InsertKey_ChangeDetected,
                        DialogText = Localization.Localization.InsertKey_DoYouWantToSave
                    };
                    await messageBox.ShowDialog(this);
                    if (messageBox.DialogResult == DialogResult.OK)
                    {
                        SaveKey_Click(null, null);
                    }
                }

                if (initialSelected != dialog.SelectedLanguage)
                {
                    Timotheus.Registry.Update("Language", dialog.SelectedLanguage == 0 ? "en-US" : "da-DK");

                    MessageBox messageBox = new()
                    {
                        DialogTitle = Localization.Localization.Settings,
                        DialogText = Localization.Localization.Settings_LanguageChanged
                    };
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
                if (IsThereUnsavedProgress())
                {
                    e.Cancel = true;

                    MessageBox msDialog = new()
                    {
                        DialogTitle = Localization.Localization.Exception_Warning,
                        DialogText = Localization.Localization.Exception_UnsavedProgress
                    };
                    await msDialog.ShowDialog(this);

                    if (msDialog.DialogResult == DialogResult.OK)
                    {
                        firstClose = false;
                        Close();
                    }
                }
            }
        }

        /// <summary>
        /// Returns whether the user has made progress that hasn't been saved.
        /// </summary>
        public bool IsThereUnsavedProgress()
        {
            bool isThereUnsavedProgress = false;

            for (int i = 0; i < Tabs.Count; i++)
            {
                isThereUnsavedProgress |= Tabs[i].HasBeenChanged();
            }

            return isThereUnsavedProgress;
        }

        public static bool CheckForInternetConnection(int timeoutMs = 10000, string url = null)
        {
            try
            {
                url ??= System.Globalization.CultureInfo.InstalledUICulture switch
                {
                    { Name: var n } when n.StartsWith("fa") => // Iran
                        "http://www.aparat.com",
                    { Name: var n } when n.StartsWith("zh") => // China
                        "http://www.baidu.com",
                    _ =>
                        "http://www.gstatic.com/generate_204",
                };

                var request = (HttpWebRequest)WebRequest.Create(url);
                request.KeepAlive = false;
                request.Timeout = timeoutMs;
                using var response = (HttpWebResponse)request.GetResponse();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async void Error(string title, string message)
        {
            MessageBox msDialog = new()
            {
                DialogTitle = title,
                DialogText = message
            };
            await msDialog.ShowDialog(this);
        }
    }
}