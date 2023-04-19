using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using System;
using System.IO;
using System.Net.Http;
using System.ComponentModel;
using System.Collections.Generic;
using System.Security.Cryptography;
using Timotheus.Utility;
using Timotheus.ViewModels;
using Timotheus.Views.Dialogs;
using Avalonia.Controls.Shapes;
using System.Threading.Tasks;

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
        private Project CurrentProject 
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
        /// <summary>
        /// The current instance of the main window.
        /// </summary>
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
        /// <summary>
        /// Whether the language has been changed in this session.
        /// </summary>
        private bool LanguageChanged = false;

        /// <summary>
        /// Constructor. Creates datacontext and loads XAML.
        /// </summary>
        public MainWindow()
        {
            Instance = this;
            AvaloniaXamlLoader.Load(this);
            CurrentProject = new();
        }

        /// <summary>
        /// Checks for unsaved progress
        /// </summary>
		protected async Task<bool> UnsavedProgress()
		{
			List<string> UnsavedProgress = CurrentProject.IsThereUnsavedProgress();
			bool close = true;

			if (UnsavedProgress.Count > 0)
			{
				string warning = Localization.Exception_UnsavedProgress;
				for (int i = 0; i < UnsavedProgress.Count; i++)
				{
					warning += "\n - " + UnsavedProgress[i];
				}

				WarningDialog msDialog = new()
				{
					DialogTitle = Localization.Exception_Warning,
					DialogText = warning
				};

				if (!IsVisible)
					Show();

				await msDialog.ShowDialog(this);

				if (msDialog.DialogResult != DialogResult.OK)
				{
					close = false;
				}
			}

			return !close;
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
                bool unsaved = await UnsavedProgress();

                if (!unsaved)
                {
                    if (closeFromTaskbar || !(Timotheus.Registry.Retrieve("HideToSystemTray") != "False") || LanguageChanged)
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
		}

		/// <summary>
		/// Used for calling close from the tray
		/// </summary>
		public async void CloseTray()
		{
			FirstClose = false;
			bool unsaved = await UnsavedProgress();

			if (!unsaved)
				Close();
		}

		/// <summary>
		/// Show the window and use the arguments given from start-up. Method is also called if the program is already running and user tries to open another instance.
		/// </summary>
		public async void Start(string[] args)
        {
            try
            {
                List<string> arguments = new(args);

                string path = Timotheus.Registry.Retrieve("KeyPath");
                string password = Cipher.Decrypt(Timotheus.Registry.Retrieve("KeyPassword"));

                bool gui = !arguments.Contains("nogui");
                bool pathFoundInArgs = false;
                bool overrideKey = false;
                bool firstTimeOpening = FirstOpen;

                // Check if args contains a file/key
                for (int i = 0; i < arguments.Count; i++)
                {
                    if (File.Exists(arguments[i]))
                    {
                        pathFoundInArgs = true;
                        path = arguments[i];
                    }
                }

                if (gui)
                {
                    Show();
                    LookForUpdates();
                }

                if (pathFoundInArgs)
                {
                    if (!FirstOpen)
                    {
                        WarningDialog dialog = new()
                        {
                            DialogTitle = Localization.InsertKey_Args,
                            DialogText = Localization.InsertKey_ArgsMessage.Replace("#", System.IO.Path.GetFileName(path))
                        };
                        if (!gui)
                            Show();
                        await dialog.ShowDialog(this);
                        if (dialog.DialogResult == DialogResult.OK)
                        {
                            overrideKey = true;
                            password = string.Empty;
                        }
                        else
                        {
                            path = Timotheus.Registry.Retrieve("KeyPath");
                        }
                    }
                    else if (path != Timotheus.Registry.Retrieve("KeyPath"))
                    {
                        overrideKey = true;
                        FirstOpen = false;
                        password = string.Empty;
                    }
                }
                else
                {
                    if (FirstOpen)
                    {
                        FirstOpen = false;

                        try
                        {
                            if (Timotheus.FirstTime && gui)
                            {
                                FirstTimeSetup firstTimeSetup = new();
                                await firstTimeSetup.ShowDialog(this);
                                if (firstTimeSetup.DialogResult == DialogResult.OK)
                                {
                                    path = firstTimeSetup.Path;
                                    password = string.Empty;
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

                if (!File.Exists(path) && path != string.Empty)
                {
                    WarningDialog dialog = new()
                    {
                        DialogTitle = Localization.Exception_Warning,
                        DialogText = Localization.Exception_CantFindKey.Replace("#1", path),
                        DialogShowCancel = false
                    };
                    if (!gui)
                        Show();
                    await dialog.ShowDialog(this);
                    Timotheus.Registry.Delete("KeyPath");
                    path = string.Empty;
                    password = string.Empty;
                }

                if (overrideKey || (password == string.Empty && path != string.Empty))
                {
                    PasswordDialog passwordDialog = new()
                    {
                        Title = Localization.PasswordDialog_Key
                    };
                    await passwordDialog.ShowDialog(this);
                    if (passwordDialog.DialogResult == DialogResult.OK)
                    {
                        password = passwordDialog.Password;
                        if (passwordDialog.Save)
                        {
                            string encodedPassword = Cipher.Encrypt(password);
                            Timotheus.Registry.Update("KeyPassword", encodedPassword);
                        }
                        else
                        {
                            Timotheus.Registry.Delete("KeyPassword");
                        }
                    }
                }

                if (firstTimeOpening || overrideKey)
                    InsertKey(path, password, gui);
            }
            catch (Exception ex)
            {
                Exception exception = ex;

                if (ex is CryptographicException)
                    exception = new Exception(Localization.Exception_InvalidSavedPassword);

                Program.Error(Localization.Exception_Name, exception, this);

                CurrentProject = new Project();
                Timotheus.Registry.Delete("KeyPath");
                Timotheus.Registry.Delete("KeyPassword");
            }
		}

        /// <summary>
        /// Inserts the key with the given path and password
        /// </summary>
        private async void InsertKey(string path, string password, bool gui = true)
        {
            if (path != string.Empty && System.IO.Path.GetExtension(path) == ".tkey")
            {
                try
                {
                    bool objections = false;

					if (!Project.IsSafeProjectPath(System.IO.Path.GetDirectoryName(path)))
					{
						WarningDialog dialog = new()
						{
							DialogTitle = Localization.Exception_Warning,
							DialogText = Localization.Exception_BadKeyPath.Replace("#1", System.IO.Path.GetDirectoryName(path)),
							DialogShowCancel = true
						};

						await dialog.ShowDialog(this);

                        if (dialog.DialogResult == DialogResult.Cancel)
                            objections = true;
					}

                    if (!objections)
                    {
						try
						{
							CurrentProject = new Project(path, password);
						}
						catch (Exception)
						{
							throw new Exception(Localization.Exception_WrongPassword);
						}
					}
                    else
                    {
                        CurrentProject = new Project();
						Timotheus.Registry.Delete("KeyPath");
						Timotheus.Registry.Delete("KeyPassword");

                        MessageDialog dialog = new()
                        {
							DialogTitle = Localization.Exception_Message,
							DialogText = Localization.Exception_CancelledKeyOpening,
							DialogShowCancel = false
						};

                        await dialog.ShowDialog(this);
					}

					if (gui)
                    {
                        try
                        {
							ProgressDialog dialog = new()
							{
								Title = Localization.InsertKey_Dialog,
								Message = Localization.InsertKey_Dialog
							};
							await dialog.ShowDialog(this, CurrentProject.Loader);
                        }
                        catch (Exception ex)
                        {
                            Program.Error(Localization.Exception_Name, ex, this);
                        }
                    }
                    else
                        CurrentProject.Loader.RunWorkerAsync();

					

                    Timotheus.SaveRegistry();
                }
                catch (Exception ex)
                {
                    Program.Error(Localization.Exception_Name, ex, this);
                    CurrentProject = new Project();
                    Timotheus.Registry.Delete("KeyPath");
                    Timotheus.Registry.Delete("KeyPassword");
                }
            }
            else
            {
				CurrentProject = new Project();
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
            try
            {
                bool newProject = true;

                if (CurrentProject.IsThereUnsavedProgress().Count > 0)
                {
					WarningDialog warningDialog = new()
					{
						DialogTitle = Localization.ToolStrip_NewFile,
						DialogText = Localization.ToolStrip_NewSecure
					};
					await warningDialog.ShowDialog(this);

					newProject = warningDialog.DialogResult == DialogResult.OK;
				}

				if (newProject)
                {
					CurrentProject = new Project();
					Timotheus.Registry.Delete("KeyPath");
					Timotheus.Registry.Delete("KeyPassword");
				}
            }
            catch (Exception exception)
            {
                Program.Error(Localization.Exception_Name, exception, this);
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
                        CurrentProject.Save(keyPath, password);
                    }
                    catch (Exception ex)
                    {
                        Program.Error(Localization.Exception_Saving, ex, this);
                    }
                }
                else
                {
                    PasswordDialog dialog = new()
                    {
                        Title = Localization.PasswordDialog_SaveKey
                    };
                    await dialog.ShowDialog(this);
                    if (dialog.DialogResult == DialogResult.OK)
                    {
                        password = dialog.Password;
                        try
                        {
                            CurrentProject.Save(keyPath, password);
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
                bool show = true;

				if (!Project.IsSafeProjectPath(System.IO.Path.GetDirectoryName(result)))
				{
					WarningDialog warning = new()
					{
						DialogTitle = Localization.Exception_Warning,
						DialogText = Localization.Exception_BadKeyPath.Replace("#1", result)
					};

					await warning.ShowDialog(this);

                    show = warning.DialogResult == DialogResult.OK;
				}

                if (show)
                {
					PasswordDialog dialog = new()
					{
						Title = Localization.PasswordDialog_SaveKey
					};
					await dialog.ShowDialog(this);
					if (dialog.DialogResult == DialogResult.OK)
					{
						string password = dialog.Password;
						string encodedPassword = Cipher.Encrypt(password);
						Timotheus.Registry.Update("KeyPassword", encodedPassword);

						try
						{
							CurrentProject.Save(result, password);
							Timotheus.Registry.Update("KeyPath", result);

							MessageDialog confirmation = new()
							{
								DialogTitle = Localization.Confirmation,
								DialogText = Localization.Confirmation_SaveKey,
								DialogShowCancel = false
							};
							await confirmation.ShowDialog(Instance);
						}
						catch (Exception exception)
						{
							Program.Error(Localization.Exception_Saving, exception, this);
						}
					}
				}
            }
        }

        /// <summary>
        /// Opens an OpenFileDialog so the user can open a key file.
        /// </summary>
        private async void OpenKey_Click(object sender, RoutedEventArgs e)
        {
            try
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
					PasswordDialog passwordDialog = new()
					{
						Title = Localization.PasswordDialog_Key
					};

					await passwordDialog.ShowDialog(this);
					if (passwordDialog.DialogResult == DialogResult.OK)
					{
						string password = passwordDialog.Password;
						if (passwordDialog.Save)
						{
							string encodedPassword = Cipher.Encrypt(password);
							Timotheus.Registry.Update("KeyPassword", encodedPassword);
						}
						else
							Timotheus.Registry.Delete("KeyPassword");
						try
						{
							InsertKey(result[0], password);
							Timotheus.SaveRegistry();
						}
						catch (Exception ex)
						{
							Program.Error(Localization.Exception_LoadFailed, ex, this);
						}
					}
				}
            }
            catch (Exception exception)
            {
                Program.Error(Localization.Exception_Name, exception, this);
            }
        }

        /// <summary>
        /// Opens a Help dialog.
        /// </summary>
        private void Help_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Help dialog = new();
                dialog.Show(this);
            }
            catch (Exception exception)
            {
                Program.Error(Localization.Exception_Name, exception, this);
            }
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
                    HideToSystemTray = Timotheus.Registry.Retrieve("HideToSystemTray") != "False",
                    LookForUpdates = Timotheus.Registry.Retrieve("LookForUpdates") != "False",
                    SelectedLanguage = Timotheus.Registry.Retrieve("Language") == "da-DK" ? 1 : 0,
                    OpenOnStartUp = Timotheus.OpenOnStartUp
                };
                int initialSelected = dialog.SelectedLanguage;

                await dialog.ShowDialog(this);
                if (dialog.DialogResult == DialogResult.OK)
                {
                    LanguageChanged = initialSelected == dialog.SelectedLanguage;
                    if (initialSelected != dialog.SelectedLanguage)
                    {
                        Timotheus.Registry.Update("Language", dialog.SelectedLanguage == 0 ? "en-US" : "da-DK");
                        LanguageChanged = true;

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