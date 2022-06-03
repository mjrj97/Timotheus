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
                catch (Exception) { Directory = new(); }
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
                MainWindow.Instance.Error(Localization.Localization.Exception_Name, ex.Message);
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
                    Title = Localization.Localization.SFTP_SyncWorker
                };
                await dialog.ShowDialog(MainWindow.Instance, Directory.Sync);
                Directory.GoToDirectory(Directory.RemotePath);
            }
            catch (Exception ex)
            {
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
                    //Error(Localization.Localization.Exception_Name, ex.Message);
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
                            Directory.GoToDirectory(file.RemoteFullName);
                        else
                            Directory.GoToDirectory(file.LocalFullName);
                    }
                    else
                    {
                        if (file.LocalFullName != string.Empty)
                        {
                            Process p = new()
                            {
                                StartInfo = new ProcessStartInfo(file.LocalFullName)
                                {
                                    UseShellExecute = true
                                }
                            };
                            p.Start();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
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

        public override void Update()
        {
            
        }
    }
}