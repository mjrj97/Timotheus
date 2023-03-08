using Avalonia.Input;
using Avalonia.Controls;
using Avalonia.VisualTree;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using System;
using System.IO;
using System.Linq;
using Timotheus.IO;
using Timotheus.ViewModels;
using Timotheus.Views.Dialogs;

namespace Timotheus.Views.Tabs
{
    public partial class FilesPage : Tab
    {
        /// <summary>
        /// A SFTP object connecting a local and remote directory.
        /// </summary>
        public new DirectoryViewModel ViewModel
        {
            get
            {
                return (DirectoryViewModel)base.ViewModel;
            }
            set
            {
                base.ViewModel = value;
                value.GoToDirectory("/");
            }
        }

        /// <summary>
        /// Creates the tab.
        /// </summary>
        public FilesPage() : base()
        {
            AvaloniaXamlLoader.Load(this);
            Title = Localization.SFTP_Page;
            LoadingTitle = Localization.InsertKey_LoadFiles;
            Icon = "avares://Timotheus/Resources/Folder.png";
            ViewModel = new DirectoryViewModel();
        }

        /// <summary>
        /// Reloads the Directory with the current key in the MainViewModel.
        /// </summary>
        public override void Load()
        {
            if (Keys.Retrieve("SSH-LocalDirectory") != string.Empty)
            {
				try
				{
					ViewModel = new DirectoryViewModel(Keys.Retrieve("SSH-LocalDirectory"), Keys.Retrieve("SSH-RemoteDirectory"), Keys.Retrieve("SSH-URL"), int.Parse(Keys.Retrieve("SSH-Port") == string.Empty ? "22" : Keys.Retrieve("SSH-Port")), Keys.Retrieve("SSH-Username"), Keys.Retrieve("SSH-Password"), Keys.Retrieve("SSH-Sync") == "True", int.Parse(Keys.Retrieve("SSH-SyncInterval") == string.Empty ? "60" : Keys.Retrieve("SSH-SyncInterval")));
				}
				catch (Exception ex)
				{
					Program.Log(ex);
					ViewModel = new();
				}
			}
            else
                ViewModel = new();
        }

        /// <summary>
        /// Goes one level up from the currently visible directory.
        /// </summary>
        private void UpDirectory_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewModel.GoUpDirectory();
            }
            catch (Exception ex)
            {
                Program.Error(Localization.Exception_Name, ex, MainWindow.Instance);
            }
        }

        /// <summary>
        /// Synchronize the defined remote and local directories.
        /// </summary>
        private void UpdateDirectory_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewModel.GoToDirectory(ViewModel.CurrentDirectory);
            }
            catch (Exception ex)
            {
                Program.Error(Localization.Exception_Name, ex, MainWindow.Instance);
            }
        }

        /// <summary>
        /// Synchronizes the files in the local and remote directory.
        /// </summary>
        private async void SyncFiles_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ProgressDialog dialog = new()
                {
                    Title = Localization.SFTP_SyncWorker
                };
                await dialog.ShowDialog(MainWindow.Instance, ViewModel.Sync);
                ViewModel.GoToDirectory(ViewModel.CurrentDirectory);
            }
            catch (Exception ex)
            {
                Program.Error(Localization.Exception_Name, ex, MainWindow.Instance);
            }
        }

        /// <summary>
        /// Opens a SetupSFTP dialog to define the SFTP parameters.
        /// </summary>
        private async void SetupFiles_Click(object sender, RoutedEventArgs e)
        {
            SetupSFTP dialog = new()
            {
                Local = Keys.Retrieve("SSH-LocalDirectory"),
                Remote = Keys.Retrieve("SSH-RemoteDirectory"),
                Host = Keys.Retrieve("SSH-URL"),
                Port = Keys.Retrieve("SSH-Port"),
                Username = Keys.Retrieve("SSH-Username"),
                Password = Keys.Retrieve("SSH-Password"),
                Sync = Keys.Retrieve("SSH-Sync") == "True",
                SyncInterval = Keys.Retrieve("SSH-SyncInterval") == string.Empty ? "60" : Keys.Retrieve("SSH-SyncInterval")
            };

            await dialog.ShowDialog(MainWindow.Instance);
            if (dialog.DialogResult == DialogResult.OK)
            {
                try
                {
                    if (dialog.Port == string.Empty)
                        dialog.Port = "22";
                    
                    bool changed = false;

                    changed |= Keys.Update("SSH-LocalDirectory", dialog.Local);
                    changed |= Keys.Update("SSH-RemoteDirectory", dialog.Remote);
                    changed |= Keys.Update("SSH-URL", dialog.Host);
                    changed |= Keys.Update("SSH-Port", dialog.Port);
                    changed |= Keys.Update("SSH-Username", dialog.Username);
                    changed |= Keys.Update("SSH-Password", dialog.Password);
                    changed |= Keys.Update("SSH-Sync", dialog.Sync.ToString());
                    changed |= Keys.Update("SSH-SyncInterval", dialog.SyncInterval);

                    if (changed)
                    {
                        MessageDialog messageBox = new()
                        {
                            DialogTitle = Localization.InsertKey_ChangeDetected,
                            DialogText = Localization.InsertKey_DoYouWantToSave
                        };
                        await messageBox.ShowDialog(MainWindow.Instance);
                        if (messageBox.DialogResult == DialogResult.OK)
                        {
                            MainWindow.Instance.SaveKey_Click(null, null);
                        }
                    }

                    ViewModel = new DirectoryViewModel(dialog.Local, dialog.Remote, dialog.Host, int.Parse(dialog.Port), dialog.Username, dialog.Password, dialog.Sync, int.Parse(dialog.SyncInterval));
                }
                catch (Exception ex)
                {
                    Program.Error(Localization.Exception_Name, ex, MainWindow.Instance);
                }
            }
        }

        /// <summary>
        /// Marks the selected event for deletion.
        /// </summary>
        private async void EditFilePermission_Click(object sender, RoutedEventArgs e)
        {
            FileViewModel file = (FileViewModel)((Button)e.Source).DataContext;
            if (sender != null)
            {
                try
                {
                    string first = file.IsPublic ? Localization.SFTP_Private : Localization.SFTP_Public;
                    string second = file.IsPublic ? Localization.SFTP_Public : Localization.SFTP_Private;

                    WarningDialog msDialog = new()
                    {
                        DialogTitle = Localization.Exception_Warning,
                        DialogText = Localization.SFTP_ChangePermission.Replace("#1", file.Name).Replace("#2", first.ToLower()).Replace("#3", second.ToLower())
                    };
                    await msDialog.ShowDialog(MainWindow.Instance);
                    if (msDialog.DialogResult == DialogResult.OK)
                    {
                        Button button = (Button)sender;
                        if (button.Name == "Public")
                        {
                            ViewModel.SetFilePermissions(file, 770);
                        }
                        else if (button.Name == "Private")
                        {
                            ViewModel.SetFilePermissions(file, 775);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Program.Error(Localization.Exception_Name, ex, MainWindow.Instance);
                }
            }
        }

        /// <summary>
        /// Marks the selected event for deletion.
        /// </summary>
        private async void EditFilePermission_ContextMenu_Click(object sender, RoutedEventArgs e)
        {
            FileViewModel file = ViewModel.Selected;
            if (sender != null)
            {
                try
                {
                    MenuItem button = (MenuItem)sender;

                    string first = button.Name == "ContextPublic" ? Localization.SFTP_Public : Localization.SFTP_Private;
                    string second = file.IsPublic ? Localization.SFTP_Public : Localization.SFTP_Private;

                    WarningDialog msDialog = new()
                    {
                        DialogTitle = Localization.Exception_Warning,
                        DialogText = Localization.SFTP_ChangePermission.Replace("#1", file.Name).Replace("#2", first.ToLower()).Replace("#3", second.ToLower())
                    };
                    await msDialog.ShowDialog(MainWindow.Instance);
                    if (msDialog.DialogResult == DialogResult.OK)
                    {
                        if (button.Name == "ContextPrivate")
                        {
                            ViewModel.SetFilePermissions(file, 770);
                        }
                        else if (button.Name == "ContextPublic")
                        {
                            ViewModel.SetFilePermissions(file, 775);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Program.Error(Localization.Exception_Name, ex, MainWindow.Instance);
                }
            }
        }

        /// <summary>
        /// Opens the file that is double clicked.
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
                    ViewModel.Open(file);
                }
            }
            catch (Exception ex)
            {
                Program.Error(Localization.Exception_Name, ex, MainWindow.Instance);
            }
        }

        /// <summary>
        /// Open the file from the context menu.
        /// </summary>
        private void Open_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewModel.Open(ViewModel.Selected);
            }
            catch (Exception ex)
            {
                Program.Error(Localization.Exception_Name, ex, MainWindow.Instance);
            }
        }

        /// <summary>
        /// Open the file from the context menu.
        /// </summary>
        private async void NewFolder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TextDialog dialog = new()
                {
                    Title = Localization.SFTP_NewFolder
                };
                await dialog.ShowDialog(MainWindow.Instance);
                if (dialog.DialogResult == DialogResult.OK)
                    ViewModel.NewFolder(dialog.Text == string.Empty ? Localization.SFTP_NewFolder : dialog.Text);
            }
            catch (Exception ex)
            {
                Program.Error(Localization.Exception_Name, ex, MainWindow.Instance);
            }
        }

        /// <summary>
        /// Synchronizes the file.
        /// </summary>
        private async void DeleteFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WarningDialog msDialog = new()
                {
                    DialogTitle = Localization.Exception_Warning,
                    DialogText = Localization.SFTP_DeleteWarning.Replace("#", ViewModel.Selected.Name)
                };
                await msDialog.ShowDialog(MainWindow.Instance);
                if (msDialog.DialogResult == DialogResult.OK)
                {
                    ViewModel.Delete(ViewModel.Selected);
                }
            }
            catch (Exception ex)
            {
                Program.Error(Localization.Exception_Name, ex, MainWindow.Instance);
            }
        }
        
        /// <summary>
        /// Synchronizes the file.
        /// </summary>
        private async void RenameFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TextDialog dialog = new()
                {
                    Title = Localization.SFTP_ContextMenu_Rename,
                    Text = ViewModel.Selected.Name
                };
                await dialog.ShowDialog(MainWindow.Instance);
                if (dialog.DialogResult == DialogResult.OK)
                {
                    if (ViewModel.Selected.IsDirectory)
                    {
                        if (System.IO.Directory.Exists(ViewModel.LocalPath + ViewModel.CurrentDirectory + dialog.Text))
                            throw new Exception(Localization.Exception_DirectoryAlreadyExists);
                    }
                    else
                    {
                        if (File.Exists(ViewModel.LocalPath + ViewModel.CurrentDirectory + dialog.Text))
                            throw new Exception(Localization.Exception_FileAlreadyExists);
                    }
                    ViewModel.RenameFile(ViewModel.Selected, dialog.Text);
                }
            }
            catch (Exception ex)
            {
                Program.Error(Localization.Exception_Name, ex, MainWindow.Instance);
            }
        }

        /// <summary>
        /// Fix the directory path textbox
        /// </summary>
        private void DirectoryText_KeyDown(object sender, KeyEventArgs e)
        {
            string text = ((TextBox)sender).Text;
            try
            {
                try
                {
                    string path = text.Trim();
                    path = path.Replace("\\", "/");
                    if (path.Length == 0)
                        path = "/";
                    else if (!path.StartsWith("/"))
                        path = "/" + path;

                    if (e.Key == Avalonia.Input.Key.Enter)
                    {
                        // GO TO DIRECTORY
                        ViewModel.GoToDirectory(path);
                        e.Handled = true;
                    }
                    else
                    {
                        ((TextBox)sender).Text = path;
                        NotifyPropertyChanged(nameof(ViewModel.CurrentDirectory));
                        e.Handled = true;
                    }
                }
                catch (ArgumentException ex)
                {
                    Program.Log(ex);
                }
            }
            catch (Exception ex)
            {
                Program.Error(Localization.Exception_Name, ex, MainWindow.Instance);
            }
        }

        /// <summary>
        /// Handles coloring of the rows.
        /// </summary>
        private void Files_RowLoading(object sender, DataGridRowEventArgs e)
        {
            if (e.Row.DataContext is FileViewModel file)
            {
                if (ViewModel.Connected)
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
        }

        /// <summary>
        /// Opens a file explorer at the selected file.
        /// </summary>
        private void OpenInFolder_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
            {
                FileName = System.IO.Path.GetDirectoryName(ViewModel.Selected.LocalFullName),
                UseShellExecute = true,
                Verb = "open"
            });
        }

        public override bool HasBeenChanged()
        {
            return false;
        }
    }
}