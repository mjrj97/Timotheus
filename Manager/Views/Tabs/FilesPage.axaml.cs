using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;
using System;
using System.IO;
using System.Linq;
using Timotheus.Utility;
using Timotheus.ViewModels;
using Timotheus.Views.Dialogs;

namespace Timotheus.Views.Tabs
{
    public partial class FilesPage : Tab
    {
        /// <summary>
        /// A SFTP object connecting a local and remote directory.
        /// </summary>
        public DirectoryViewModel Directory
        {
            get
            {
                return (DirectoryViewModel)ViewModel;
            }
            set
            {
                ViewModel = value;
                Directory.GoToDirectory("/");
            }
        }

        /// <summary>
        /// Creates the tab.
        /// </summary>
        public FilesPage()
        {
            LoadingTitle = Localization.InsertKey_LoadFiles;
            AvaloniaXamlLoader.Load(this);
            DataContext = Directory;
        }

        /// <summary>
        /// Reloads the Directory with the current key in the MainViewModel.
        /// </summary>
        public override void Load()
        {
            if (MainViewModel.Instance.Keys.Retrieve("SSH-LocalDirectory") != string.Empty)
            {
                Directory = new DirectoryViewModel(MainViewModel.Instance.Keys.Retrieve("SSH-LocalDirectory"), MainViewModel.Instance.Keys.Retrieve("SSH-RemoteDirectory"), MainViewModel.Instance.Keys.Retrieve("SSH-URL"), int.Parse(MainViewModel.Instance.Keys.Retrieve("SSH-Port") == string.Empty ? "22" : MainViewModel.Instance.Keys.Retrieve("SSH-Port")), MainViewModel.Instance.Keys.Retrieve("SSH-Username"), MainViewModel.Instance.Keys.Retrieve("SSH-Password"));
            }
            else
                Directory = new();
        }

        /// <summary>
        /// Goes one level up from the currently visible directory.
        /// </summary>
        private void UpDirectory_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Directory.GoUpDirectory();
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
                Directory.GoToDirectory(Directory.CurrentDirectory);
            }
            catch (Exception ex)
            {
                MainWindow.Instance.Error(ex);
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
                await dialog.ShowDialog(MainWindow.Instance, Directory.Sync);
                Directory.GoToDirectory(Directory.CurrentDirectory);
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
                Local = MainViewModel.Instance.Keys.Retrieve("SSH-LocalDirectory"),
                Remote = MainViewModel.Instance.Keys.Retrieve("SSH-RemoteDirectory"),
                Host = MainViewModel.Instance.Keys.Retrieve("SSH-URL"),
                Port = MainViewModel.Instance.Keys.Retrieve("SSH-Port"),
                Username = MainViewModel.Instance.Keys.Retrieve("SSH-Username"),
                Password = MainViewModel.Instance.Keys.Retrieve("SSH-Password")
            };

            await dialog.ShowDialog(MainWindow.Instance);
            if (dialog.DialogResult == DialogResult.OK)
            {
                try
                {
                    if (dialog.Port == string.Empty)
                        dialog.Port = "22";
                    Directory = new DirectoryViewModel(dialog.Local, dialog.Remote, dialog.Host, int.Parse(dialog.Port), dialog.Username, dialog.Password);
                    DataContext = ViewModel;

                    bool changed = false;

                    changed |= MainViewModel.Instance.Keys.Update("SSH-LocalDirectory", dialog.Local);
                    changed |= MainViewModel.Instance.Keys.Update("SSH-RemoteDirectory", dialog.Remote);
                    changed |= MainViewModel.Instance.Keys.Update("SSH-URL", dialog.Host);
                    changed |= MainViewModel.Instance.Keys.Update("SSH-Port", dialog.Port);
                    changed |= MainViewModel.Instance.Keys.Update("SSH-Username", dialog.Username);
                    changed |= MainViewModel.Instance.Keys.Update("SSH-Password", dialog.Password);

                    if (changed)
                    {
                        MessageBox messageBox = new()
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

                    MessageBox msDialog = new()
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
                            Directory.SetFilePermissions(file, 770);
                        }
                        else if (button.Name == "Private")
                        {
                            Directory.SetFilePermissions(file, 775);
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
            FileViewModel file = Directory.Selected;
            if (sender != null)
            {
                try
                {
                    MenuItem button = (MenuItem)sender;

                    string first = button.Name == "ContextPublic" ? Localization.SFTP_Public : Localization.SFTP_Private;
                    string second = file.IsPublic ? Localization.SFTP_Public : Localization.SFTP_Private;

                    MessageBox msDialog = new()
                    {
                        DialogTitle = Localization.Exception_Warning,
                        DialogText = Localization.SFTP_ChangePermission.Replace("#1", file.Name).Replace("#2", first.ToLower()).Replace("#3", second.ToLower())
                    };
                    await msDialog.ShowDialog(MainWindow.Instance);
                    if (msDialog.DialogResult == DialogResult.OK)
                    {
                        if (button.Name == "ContextPrivate")
                        {
                            Directory.SetFilePermissions(file, 770);
                        }
                        else if (button.Name == "ContextPublic")
                        {
                            Directory.SetFilePermissions(file, 775);
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
                    Directory.Open(file);
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
                Directory.Open(Directory.Selected);
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
                    Directory.NewFolder(dialog.Text == string.Empty ? Localization.SFTP_NewFolder : dialog.Text);
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
                MessageBox msDialog = new()
                {
                    DialogTitle = Localization.Exception_Warning,
                    DialogText = Localization.SFTP_DeleteWarning.Replace("#", Directory.Selected.Name)
                };
                await msDialog.ShowDialog(MainWindow.Instance);
                if (msDialog.DialogResult == DialogResult.OK)
                {
                    Directory.Delete(Directory.Selected);
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
                    Text = Directory.Selected.Name
                };
                await dialog.ShowDialog(MainWindow.Instance);
                if (dialog.DialogResult == DialogResult.OK)
                {
                    Directory.RenameFile(Directory.Selected, dialog.Text);
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

                    if (e.Key == Key.Enter)
                    {
                        // GO TO DIRECTORY
                        Directory.GoToDirectory(path);
                        e.Handled = true;
                    }
                    else
                    {
                        ((TextBox)sender).Text = path;
                        NotifyPropertyChanged(nameof(Directory.CurrentDirectory));
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
                if (Directory.Connected)
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
                FileName = System.IO.Path.GetDirectoryName(Directory.Selected.LocalFullName),
                UseShellExecute = true,
                Verb = "open"
            });
        }

        public override void Update()
        {
            
        }

        public override bool HasBeenChanged()
        {
            return false;
        }
    }
}