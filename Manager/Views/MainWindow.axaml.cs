using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;
using System.IO;
using System.Net.Http;
using System.ComponentModel;
using Timotheus.Utility;
using Timotheus.ViewModels;
using Timotheus.Views.Dialogs;

namespace Timotheus.Views
{
    /// <summary>
    /// MainWindow of the Timotheus app. Contains Calendar and Filesharing tabs.
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        /// <summary>
        /// Data Context of the MainWindow.
        /// </summary>
        private Project project 
        {
            get
            {
                return (Project)DataContext;
            }
            set
            {
                DataContext = value;
            }
        }

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
        /// Handles whether the program is being opened for the first time.
        /// </summary>
        private bool FirstOpen = true;
        /// <summary>
        /// Handles whether the program is being closed for the first time.
        /// </summary>
        private bool FirstClose = true;

        public bool IsDoneLoading { get; set; } = false;

        /// <summary>
        /// Constructor. Creates datacontext and loads XAML.
        /// </summary>
        public MainWindow()
        {
            Instance = this;
            AvaloniaXamlLoader.Load(this);
            project = new();
        }

        /// <summary>
        /// Is activated when program is closed (Systemtray, Titlebar etc.)
        /// </summary>
        protected async override void OnClosing(CancelEventArgs e)
        {
            bool closeFromTaskbar = !IsVisible;
            e.Cancel = FirstClose;
            if (FirstClose)
            {
                if (project.IsThereUnsavedProgress())
                {
                    WarningDialog msDialog = new()
                    {
                        DialogTitle = Localization.Exception_Warning,
                        DialogText = Localization.Exception_UnsavedProgress
                    };

                    if (!IsVisible)
                        Show();

                    await msDialog.ShowDialog(this);

                    if (msDialog.DialogResult == DialogResult.OK)
                    {
                        FirstClose = false;
                    }
                }

                if (closeFromTaskbar || !(Timotheus.Registry.Retrieve("HideToSystemTray") != "False"))
                {
                    FirstClose = false;
                    Close();
                }
                else
                {
                    FirstClose = true;
                    Hide();
                }
            }
        }

        /// <summary>
        /// Show the window and use the arguments given from start-up. Method is also called if the program is already running and user tries to open another instance.
        /// </summary>
        public async void Start(string[] args)
        {
            bool gui = true;

            // Check if args contains a file/key
            int i = 0;
            bool found = false;
            for (int j = 0; j < args.Length; j++)
            {
                if (File.Exists(args[j]))
                {
                    found = true;
                }
                else if (!found)
                    i++;
                if (args[j] == "nogui")
                    gui = false;
            }

            if (gui)
                Show();
            
            string path = string.Empty;
            string password = string.Empty;

            if (found)
            {
                DialogResult result = DialogResult.Cancel;
                if (!FirstOpen)
                {
                    if (args[i] != Timotheus.Registry.Retrieve("KeyPath"))
                    {
                        WarningDialog dialog = new()
                        {
                            DialogTitle = Localization.InsertKey_Args,
                            DialogText = Localization.InsertKey_ArgsMessage.Replace("#", Path.GetFileName(args[i]))
                        };
                        await dialog.ShowDialog(this);
                        result = dialog.DialogResult;
                    }
                }
                else
                    result = DialogResult.OK;
                
                if (result == DialogResult.OK)
                {
                    path = args[i];

                    if (path != Timotheus.Registry.Retrieve("KeyPath"))
                    {
                        PasswordDialog passDialog = new();
                        await passDialog.ShowDialog(this);
                        if (passDialog.DialogResult == DialogResult.OK)
                        {
                            password = passDialog.Password;
                            if (passDialog.Save)
                            {
                                string encodedPassword = Cipher.Encrypt(password);
                                Timotheus.Registry.Update("KeyPassword", encodedPassword);
                            }
                            else
                            {
                                Timotheus.Registry.Delete("KeyPassword");
                            }
                        }
                        else
                        {
                            path = string.Empty;
                            password = string.Empty;
                        }
                    }
                    else
                    {
                        string encodedPassword = Timotheus.Registry.Retrieve("KeyPassword");
                        if (encodedPassword != string.Empty)
                        {
                            password = Cipher.Decrypt(encodedPassword);
                        }
                    }
                }
            }

            bool lastUsedHasBeenMoved = false;

            if (FirstOpen)
            {
                FirstOpen = false;
                if (gui)
                    LookForUpdates();

                if (path == string.Empty)
                {
                    try
                    {
                        if (Timotheus.FirstTime && gui)
                        {
                            FirstTimeSetup fts = new();
                            await fts.ShowDialog(this);
                            if (fts.DialogResult == DialogResult.OK)
                                path = fts.Path;
                        }
                        else
                            path = Timotheus.Registry.Retrieve("KeyPath");

                        if (!File.Exists(path))
                        {
							WarningDialog dialog = new()
							{
                                DialogTitle = Localization.Exception_Warning,
								DialogText = Localization.Exception_CantFindKey.Replace("#1", path),
                                DialogShowCancel = false
							};
							await dialog.ShowDialog(this);
                            lastUsedHasBeenMoved = true;
							Timotheus.Registry.Delete("KeyPath");
						}
                        else
                        {
							if (path != string.Empty)
							{
								string encodedPassword = Timotheus.Registry.Retrieve("KeyPassword");
								if (encodedPassword != string.Empty)
								{
									password = Cipher.Decrypt(encodedPassword);
								}
								else if (gui)
								{
									PasswordDialog passDialog = new();
									await passDialog.ShowDialog(this);
									if (passDialog.DialogResult == DialogResult.OK)
									{
										password = passDialog.Password;
										if (passDialog.Save)
										{
											encodedPassword = Cipher.Encrypt(password);
											Timotheus.Registry.Update("KeyPassword", encodedPassword);
										}
										else
										{
											Timotheus.Registry.Delete("KeyPassword");
										}
									}
									else
									{
										path = string.Empty;
										password = string.Empty;
									}
								}
							}
						}
                    }
                    catch (Exception ex)
                    {
                        if (gui)
                            Program.Error(Localization.Exception_NoKeys, ex, this);
                        path = string.Empty;
                        password = string.Empty;
                    }
                }
            }

            if ((!(!FirstOpen && path == string.Empty) || (path == string.Empty && password == string.Empty)) && !lastUsedHasBeenMoved)
                InsertKey(path, password, gui);
        }

        /// <summary>
        /// Inserts the key with the given path and password
        /// </summary>
        private async void InsertKey(string path, string password, bool gui = true)
        {
            if (path != string.Empty)
            {
                try
                {
                    try
                    {
                        project = new Project(path, password);
                    }
                    catch (Exception)
                    {
                        throw new Exception(Localization.Exception_InvalidPassword);
                    }

                    ProgressDialog dialog = new();
                    if (gui)
                    {
                        try
                        {
                            dialog.Title = Localization.InsertKey_Dialog;
                            dialog.Message = Localization.InsertKey_Dialog;
                            await dialog.ShowDialog(this, project.Loader);
                        }
                        catch (Exception ex)
                        {
                            Program.Error(Localization.Exception_Name, ex, this);
                        }
                    }
                    else
                        project.Loader.RunWorkerAsync();

                    if (project.Keys.Retrieve("SSH-URL") != string.Empty && gui)
                    {
                        if (!Directory.Exists(project.Keys.Retrieve("SSH-LocalDirectory")))
                        {
                            WarningDialog warningBox = new()
                            {
                                DialogTitle = Localization.Exception_Name,
                                DialogText = Localization.Exception_FolderNotFound
                            };
                            await warningBox.ShowDialog(this);
                            if (warningBox.DialogResult == DialogResult.OK)
                            {
                                OpenFolderDialog openFolder = new();
                                string newPath = await openFolder.ShowAsync(this);
                                if (newPath != string.Empty && newPath != null)
                                {
                                    project.Keys.Update("SSH-LocalDirectory", newPath);

                                    try
                                    {
                                        dialog.Title = Localization.InsertKey_Dialog;
                                        await dialog.ShowDialog(this, project.Loader);

                                        MessageDialog messageBox = new()
                                        {
                                            DialogTitle = Localization.InsertKey_ChangeDetected,
                                            DialogText = Localization.InsertKey_DoYouWantToSave
                                        };
                                        await messageBox.ShowDialog(this);
                                        if (messageBox.DialogResult == DialogResult.OK)
                                            SaveKey_Click(null, null);
                                    }
                                    catch (Exception ex)
                                    {
                                        Program.Error(Localization.Exception_Name, ex, this);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Program.Error(Localization.Exception_Name, ex, this);
                    project = new Project();
                    Timotheus.Registry.Delete("KeyPath");
                    Timotheus.Registry.Delete("KeyPassword");
                }
            }
            else
            {
                project = new Project();
                Timotheus.Registry.Delete("KeyPath");
                Timotheus.Registry.Delete("KeyPassword");
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
                HttpResponseMessage response = await client.GetAsync("http://www.mjrj.dk/api/Version?name=Timotheus");
                response.EnsureSuccessStatusCode();
                string[] text = (await response.Content.ReadAsStringAsync()).Split(Environment.NewLine);
                foundVersion = text[0];

                // Show update dialog if user hasn't disabled it
                if (foundVersion != Timotheus.Version && Timotheus.Registry.Retrieve("LookForUpdates") != "False")
                {
                    UpdateWindow dialog = new()
                    {
                        DialogTitle = Localization.UpdateDialog_Title,
                        DialogText = Localization.UpdateDialog_Text.Replace("#", foundVersion)
                    };
                    await dialog.ShowDialog(this);
                    if (dialog.DontShowAgain)
                        Timotheus.Registry.Update("LookForUpdates", "False");
                }
            }
            catch (Exception ex) 
            {
                Program.Log(ex);
            }
        }

        /// <summary>
        /// Clears the Calendar and Directory.
        /// </summary>
        private async void NewProject_Click(object sender, RoutedEventArgs e)
        {
            WarningDialog msDialog = new()
            {
                DialogTitle = Localization.ToolStrip_NewFile,
                DialogText = Localization.ToolStrip_NewSecure
            };
            await msDialog.ShowDialog(this);
            if (msDialog.DialogResult == DialogResult.OK)
            {
                project = new Project();
                Timotheus.Registry.Delete("KeyPath");
                Timotheus.Registry.Delete("KeyPassword");
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
                string encodedPassword = Timotheus.Registry.Retrieve("KeyPassword");
                string password;
                if (encodedPassword != string.Empty)
                {
                    password = Cipher.Decrypt(encodedPassword);
                    try
                    {
                        project.Save(keyPath, password);
                    }
                    catch (Exception ex)
                    {
                        Program.Error(Localization.Exception_Saving, ex, this);
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
                            project.Save(keyPath, password);
						}
                        catch (Exception ex)
                        {
                            Program.Error(Localization.Exception_Saving, ex, this);
                        }

                        if (dialog.Save)
                        {
                            encodedPassword = Cipher.Encrypt(password);
                            Timotheus.Registry.Update("KeyPassword", encodedPassword);
                        }
                    }
                }

                if (sender != null)
                {
					MessageDialog confirmation = new()
					{
						DialogTitle = Localization.Confirmation,
						DialogText = Localization.Confirmation_SaveKey,
						DialogShowCancel = false
					};
					await confirmation.ShowDialog(Instance);
				}
            }
        }

        /// <summary>
        /// Opens a SaveFileDialog so the user can save the current key as a file, but it doesn't update the current key path.
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
                PasswordDialog dialog = new();
                await dialog.ShowDialog(this);
                if (dialog.DialogResult == DialogResult.OK)
                {
                    string password = dialog.Password;
                    try
                    {
                        project.Save(result, password);

						MessageDialog confirmation = new()
						{
							DialogTitle = Localization.Confirmation,
							DialogText = Localization.Confirmation_SaveKey,
							DialogShowCancel = false
						};
						await confirmation.ShowDialog(Instance);
					}
                    catch (Exception ex)
                    {
                        Program.Error(Localization.Exception_Saving, ex, this);
                    }
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
                        InsertKey(result[0], password);
                    }
                    catch (Exception ex)
                    {
                        Program.Error(Localization.Exception_LoadFailed, ex, this);
                    }
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
            try
            {
                Settings dialog = new()
                {
                    Description = project.Keys.Retrieve("Settings-EventDescription"),
                    StartTime = project.Keys.Retrieve("Settings-EventStart"),
                    EndTime = project.Keys.Retrieve("Settings-EventEnd"),
                    HideToSystemTray = Timotheus.Registry.Retrieve("HideToSystemTray") != "False",
                    LookForUpdates = Timotheus.Registry.Retrieve("LookForUpdates") != "False",
                    SelectedLanguage = Timotheus.Registry.Retrieve("Language") == "da-DK" ? 1 : 0,
                    OpenOnStartUp = Timotheus.OpenOnStartUp
                };
                int initialSelected = dialog.SelectedLanguage;

                await dialog.ShowDialog(this);
                if (dialog.DialogResult == DialogResult.OK)
                {
                    bool changed = false;

                    if (dialog.Description != string.Empty)
                        changed |= project.Keys.Update("Settings-EventDescription", dialog.Description);
                    if (dialog.StartTime != string.Empty)
                        changed |= project.Keys.Update("Settings-EventStart", dialog.StartTime);
                    if (dialog.EndTime != string.Empty)
                        changed |= project.Keys.Update("Settings-EventEnd", dialog.EndTime);

                    if (changed)
                    {
                        MessageDialog messageBox = new()
                        {
                            DialogTitle = Localization.InsertKey_ChangeDetected,
                            DialogText = Localization.InsertKey_DoYouWantToSave
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

                        MessageDialog messageBox = new()
                        {
                            DialogTitle = Localization.Settings,
                            DialogText = Localization.Settings_LanguageChanged
                        };
                        await messageBox.ShowDialog(this);
                    }

                    Timotheus.OpenOnStartUp = dialog.OpenOnStartUp;
                    Timotheus.Registry.Update("HideToSystemTray", dialog.HideToSystemTray.ToString());
                    Timotheus.Registry.Update("LookForUpdates", dialog.LookForUpdates.ToString());
                }
            }
            catch (Exception ex)
            {
                Program.Error(Localization.Exception_Name, ex, this);
            }
        }
    }
}