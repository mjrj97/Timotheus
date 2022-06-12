using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.VisualTree;
using System;
using System.Linq;
using System.Diagnostics;
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
                Directory.GoToDirectory(Directory.RemotePath);
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

        public FilesPage()
        {
            LoadingTitle = Localization.Localization.InsertKey_LoadFiles;
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
                try
                {
                    Directory = new DirectoryViewModel(MainViewModel.Instance.Keys.Retrieve("SSH-LocalDirectory"), MainViewModel.Instance.Keys.Retrieve("SSH-RemoteDirectory"), MainViewModel.Instance.Keys.Retrieve("SSH-URL"), int.Parse(MainViewModel.Instance.Keys.Retrieve("SSH-Port") == string.Empty ? "22" : MainViewModel.Instance.Keys.Retrieve("SSH-Port")), MainViewModel.Instance.Keys.Retrieve("SSH-Username"), MainViewModel.Instance.Keys.Retrieve("SSH-Password"));
                }
                catch (Exception ex)
                {
                    Timotheus.Log(ex);
                    Directory = new();
                }
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
                Timotheus.Log(ex);
                MainWindow.Instance.Error(Localization.Localization.Exception_Name, ex.Message);
            }
        }

        /// <summary>
        /// Synchronize the defined remote and local directories.
        /// </summary>
        private void UpdateDirectory_Click(object sender, RoutedEventArgs e)
        {
            Directory.GoToDirectory(Directory.CurrentDirectory);
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
                    Title = Localization.Localization.SFTP_SyncWorker
                };
                await dialog.ShowDialog(MainWindow.Instance, Directory.Sync);
                if (Directory.DirectoryExists(Directory.CurrentDirectory))
                    Directory.GoToDirectory(Directory.CurrentDirectory);
                else
                    Directory.GoToDirectory(Directory.RemotePath);
            }
            catch (Exception ex)
            {
                Timotheus.Log(ex);
                MainWindow.Instance.Error(Localization.Localization.Exception_Name, ex.Message);
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
                            DialogTitle = Localization.Localization.InsertKey_ChangeDetected,
                            DialogText = Localization.Localization.InsertKey_DoYouWantToSave
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
                    Timotheus.Log(ex);
                    MainWindow.Instance.Error(Localization.Localization.Exception_Name, ex.Message);
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
                    string first = file.IsPublic ? Localization.Localization.SFTP_Private : Localization.Localization.SFTP_Public;
                    string second = file.IsPublic ? Localization.Localization.SFTP_Public : Localization.Localization.SFTP_Private;

                    MessageBox msDialog = new()
                    {
                        DialogTitle = Localization.Localization.Exception_Warning,
                        DialogText = Localization.Localization.SFTP_ChangePermission.Replace("#1", file.Name).Replace("#2", first.ToLower()).Replace("#3", second.ToLower())
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
                    Timotheus.Log(ex);
                    MainWindow.Instance.Error(Localization.Localization.Exception_Name, ex.Message);
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

                    string first = button.Name == "ContextPublic" ? Localization.Localization.SFTP_Public : Localization.Localization.SFTP_Private;
                    string second = file.IsPublic ? Localization.Localization.SFTP_Public : Localization.Localization.SFTP_Private;

                    MessageBox msDialog = new()
                    {
                        DialogTitle = Localization.Localization.Exception_Warning,
                        DialogText = Localization.Localization.SFTP_ChangePermission.Replace("#1", file.Name).Replace("#2", first.ToLower()).Replace("#3", second.ToLower())
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
                    Timotheus.Log(ex);
                    MainWindow.Instance.Error(Localization.Localization.Exception_Name, ex.Message);
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
                Timotheus.Log(ex);
                MainWindow.Instance.Error(Localization.Localization.Exception_Name, ex.Message);
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
                Timotheus.Log(ex);
                MainWindow.Instance.Error(Localization.Localization.Exception_Name, ex.Message);
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
                    Title = Localization.Localization.SFTP_NewFolder
                };
                await dialog.ShowDialog(MainWindow.Instance);
                if (dialog.DialogResult == DialogResult.OK)
                    Directory.NewFolder(dialog.Text == string.Empty ? Localization.Localization.SFTP_NewFolder : dialog.Text);
            }
            catch (Exception ex)
            {
                Timotheus.Log(ex);
                MainWindow.Instance.Error(Localization.Localization.Exception_Name, ex.Message);
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
                    DialogTitle = Localization.Localization.Exception_Warning,
                    DialogText = Localization.Localization.SFTP_DeleteWarning.Replace("#", Directory.Selected.Name)
                };
                await msDialog.ShowDialog(MainWindow.Instance);
                if (msDialog.DialogResult == DialogResult.OK)
                {
                    Directory.Delete(Directory.Selected);
                }
            }
            catch (Exception ex)
            {
                Timotheus.Log(ex);
                MainWindow.Instance.Error(Localization.Localization.Exception_Name, ex.Message);
            }
        }

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