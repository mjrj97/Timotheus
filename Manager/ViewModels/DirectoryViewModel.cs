﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Threading;
using System.Net.Sockets;
using Timotheus.IO;
using Timotheus.Views;
using Avalonia.Threading;

namespace Timotheus.ViewModels
{
    /// <summary>
    /// Class that contains (S)FTP related methods.
    /// </summary>
    public class DirectoryViewModel : TabViewModel
    {
        /// <summary>
        /// The currently selected file. Used for the context menu.
        /// </summary>
        public FileViewModel Selected { get; set; }

        private bool _connected = false;
        /// <summary>
        /// Whether there is an internet connection.
        /// </summary>
        public bool Connected 
        { 
            get
            {
                return _connected;
            }
            set
            {
                _connected = value;
                NotifyPropertyChanged(nameof(Connected));
            }
        }

        /// <summary>
        /// Whether the tab is syncing files. It is NotSyncing because it has a binding to the Sync Button's IsEnabled. So True = Enabled Button.
        /// </summary>
        public bool NotSyncing
        {
            get
            {
                return !Sync.IsBusy;
            }
        }

        /// <summary>
        /// Client that is connected to the remote directory.
        /// </summary>
        private readonly DirectoryClient client;

        private string _currentDirectory = "/";
        /// <summary>
        /// The directory currently being shown.
        /// </summary>
        public string CurrentDirectory
        {
            get
            {
                return _currentDirectory;
            }
            set
            {
                _currentDirectory = value;
                NotifyPropertyChanged(nameof(CurrentDirectory));
				NotifyPropertyChanged(nameof(Refresh_ToolTip));
			}
        }

        /// <summary>
        /// Tool tip for Synchronize button
        /// </summary>
        public string SynchronizeToolTip
        {
            get
            {
                string log = Path + "/.tfilelog";
                string text = Localization.SFTP_Sync_ToolTip;
                if (File.Exists(log))
                {
                    FileInfo info = new(log);
                    text = text.Replace("#", info.LastAccessTime.ToString());
                }
                else
                {
					text = text.Replace("#", Localization.SFTP_NeverSynced);
				}
                return text;
            }
        }

        /// <summary>
        /// Tool tip for Refresh button
        /// </summary>
		public string Refresh_ToolTip
		{
			get
			{
				return Localization.SFTP_Refresh_ToolTip.Replace("#1", Path + CurrentDirectory);
			}
		}

		private ObservableCollection<FileViewModel> _Files = new();
        /// <summary>
        /// A list of files in the current directory.
        /// </summary>
        public ObservableCollection<FileViewModel> Files
        {
            get => _Files;
            set
            {
                _Files = value;
                NotifyPropertyChanged(nameof(Files));
            }
        }

        /// <summary>
        /// The path of the remote directory to be watched and synced with.
        /// </summary>
        public readonly string RemotePath = string.Empty;

        /// <summary>
        /// Worker that is used to track the progress of the synchronization.
        /// </summary>
        public BackgroundWorker Sync { get; private set; }

        public Timer BackgroundSync;

        /// <summary>
        /// Constructor. Is an object that is tasked with keeping a local and remote directory synced.
        /// </summary>
        /// <param name="localPath">Path to the local directory</param>
        /// <param name="remotePath">Path to the remote directory</param>
        /// <param name="host">Connection host/url</param>
        /// <param name="username">Username to the (S)FTP connection</param>
        /// <param name="password">Password to the (S)FTP connection</param>
        public DirectoryViewModel(string localPath, string remotePath, string host, int port, string username, string password, bool sync, int syncInterval) : this()
        {
            if (sync)
            {
                TimeSpan startTimeSpan = TimeSpan.FromMinutes(syncInterval < 10 ? syncInterval : 10);
                TimeSpan periodTimeSpan = TimeSpan.FromMinutes(syncInterval);

                BackgroundSync = new Timer((e) =>
                {
                    Sync.RunWorkerAsync();
                }, null, startTimeSpan, periodTimeSpan);
            }

            Path = System.IO.Path.TrimEndingDirectorySeparator(localPath);
            RemotePath = System.IO.Path.TrimEndingDirectorySeparator(remotePath);

            Path = Path.Replace('\\', '/');
            RemotePath = RemotePath.Replace('\\', '/');

            if (!Directory.Exists(Path))
                throw new Exception(Localization.Exception_CantFindFolder);
            if (port != 22 && port != 21)
                throw new Exception(Localization.Exception_PortNotSupported);

            client = new DirectoryClient(host, port, username, password);
        }
        public DirectoryViewModel()
        {
            Sync = new()
            {
                WorkerReportsProgress = true
            };
            Sync.DoWork += Synchronize;
            Sync.RunWorkerCompleted += SyncComplete;
        }

        /// <summary>
        /// Returns whether the directory exists.
        /// </summary>
        public bool DirectoryExists(string path)
        {
            return client.Exists(path, true);
        }

        /// <summary>
        /// Downloads the entire remote directory and every file under each subfolder to the local directory.
        /// </summary>
        /// <param name="remotePath">Path of the directory on the server.</param>
        /// <param name="localPath">Path of the directory on the local machine.</param>
        private void DownloadDirectory(string remotePath, string localPath)
        {
            if (!Directory.Exists(localPath))
                Directory.CreateDirectory(localPath);
            bool isPreconnected = client.IsConnected;
            if (!isPreconnected)
            {
                client.Connect();
            }

            foreach (RemoteFile file in client.ListDirectory(remotePath))
            {
                if ((file.Name != ".") && (file.Name != ".."))
                {
                    if (!file.IsDirectory && !file.IsSymbolicLink)
                        client.DownloadFile(file.FullName, ConvertPath(file.FullName));
                    else if (file.IsSymbolicLink)
                        System.Diagnostics.Debug.WriteLine("Symbolic link ignore: " + file.FullName);
                    else
                    {
                        DirectoryInfo dir = Directory.CreateDirectory(System.IO.Path.Combine(localPath, file.Name));
                        DownloadDirectory(file.FullName, dir.FullName);
                    }
                }
            }

            DirectoryLog.Save(localPath, client.ListDirectory(remotePath));

            if (!isPreconnected)
            {
                client.Disconnect();
            }
        }

        /// <summary>
        /// Uploads the entire remote directory and every file under each subfolder to the remote directory.
        /// </summary>
        /// <param name="remotePath">Path of the directory on the server.</param>
        /// <param name="localPath">Path of the directory on the local machine.</param>
        private void UploadDirectory(string remotePath, string localPath)
        {
            bool isPreconnected = client.IsConnected;
            if (!isPreconnected)
            {
                client.Connect();
            }

            string convertedPath = ConvertPath(localPath);
            if (!client.Exists(convertedPath, true))
                client.CreateDirectory(convertedPath);

            //Files in local directory
            string[] localFilePaths = Directory.GetFiles(localPath);
            string[] localDirectoryPaths = Directory.GetDirectories(localPath);

            for (int i = 0; i < localFilePaths.Length; i++)
            {
                string name = System.IO.Path.GetFileName(localFilePaths[i]);
                if (Ignore(name))
                    continue;
                client.UploadFile(ConvertPath(localFilePaths[i]), localFilePaths[i]);
            }

            for (int i = 0; i < localDirectoryPaths.Length; i++)
            {
                string path = ConvertPath(localDirectoryPaths[i]);
                client.CreateDirectory(path);
                UploadDirectory(path, localDirectoryPaths[i]);
            }

            DirectoryLog.Save(localPath, client.ListDirectory(remotePath));

            if (!isPreconnected)
            {
                client.Disconnect();
            }
        }

        /// <summary>
        /// Synchronize the defined remote and local directories.
        /// </summary>
        private void Synchronize(object sender, DoWorkEventArgs e)
        {
            try
            {
                NotifyPropertyChanged(nameof(NotSyncing));
                if (client != null)
					SynchronizeFolder(RemotePath, Path);
            }
            catch (Exception ex)
            {
                e.Result = ex;
            }
        }

        /// <summary>
        /// Synchronizes custom remote and local directories.
        /// </summary>
        /// <param name="remotePath">Path of the directory on the server.</param>
        /// <param name="localPath">Path of the directory on the local machine.</param>
        private void SynchronizeFolder(string remotePath, string localPath)
        {
            #region CONNECTION
            bool isPreconnected = client.IsConnected;
            if (!isPreconnected)
            {
                try
                {
                    client.Connect();
                }
                catch (SocketException)
                {
                    throw new Exception(Localization.Exception_CantConnectToServer);
                }
            }
            #endregion

            List<DirectoryFile> files = GetFiles(remotePath, localPath);

            for (int i = 0; i < files.Count; i++)
            {
                //Stop synchronization if user presses 'Cancel'
                if (Sync.CancellationPending == true)
                    break;
                else
                {
                    DirectoryFile file = files[i];
                    SynchronizeFile(file);
                    Sync.ReportProgress((i*100) / files.Count, file.Name);
                }
            }

			if (Sync.CancellationPending != true)
				DirectoryLog.Save(localPath, client.ListDirectory(remotePath));

            #region DISCONNECTION
            if (!isPreconnected)
            {
                client.Disconnect();
            }
            #endregion
        }

        /// <summary>
        /// Synchronizes a specific file.
        /// </summary>
        public void SynchronizeFile(DirectoryFile file)
        {
            //File.Handle is determined by the Directory file on initialization.
            switch (file.Handle)
            {
                case SyncHandle.Synchronize:
                    if (file.IsDirectory)
						SynchronizeFolder(file.RemoteFile.FullName, file.LocalFile.FullName);
                    break;
                case SyncHandle.NewDownload:
                case SyncHandle.Download:
                    if (file.IsDirectory)
                        DownloadDirectory(file.RemoteFile.FullName, ConvertPath(file.RemoteFile.FullName));
                    else
                        client.DownloadFile(file.RemoteFile.FullName, ConvertPath(file.RemoteFile.FullName));
                    break;
                case SyncHandle.NewUpload:
                case SyncHandle.Upload:
                    if (file.IsDirectory)
                        UploadDirectory(ConvertPath(file.LocalFile.FullName), file.LocalFile.FullName);
                    else
                        client.UploadFile(ConvertPath(file.LocalFile.FullName), file.LocalFile.FullName);
                    break;
                case SyncHandle.DeleteLocal:
                    if (file.IsDirectory)
                    {
                        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                            Microsoft.VisualBasic.FileIO.FileSystem.DeleteDirectory(file.LocalFile.FullName, Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs, Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin);
                        else
                            Directory.Delete(file.LocalFile.FullName, true);
                    }
                    else
                    {
                        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                            Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(file.LocalFile.FullName, Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs, Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin);
                        else
                            File.Delete(file.LocalFile.FullName);
                    }
                    break;
                case SyncHandle.DeleteRemote:
                    if (file.IsDirectory)
                        client.DeleteDirectory(file.RemoteFile.FullName);
                    else
                        client.DeleteFile(file.RemoteFile.FullName);
                    break;
            }
        }

		/// <summary>
		/// Event after the synchronization is complete.
		/// </summary>
		private void SyncComplete(object sender, RunWorkerCompletedEventArgs e)
		{
			if (e.Result is Exception ex)
			{
				Dispatcher.UIThread.InvokeAsync(delegate
				{
					Program.Error(Localization.Exception_Name, ex, MainWindow.Instance);
				});
			}
			NotifyPropertyChanged(nameof(NotSyncing));
			NotifyPropertyChanged(nameof(SynchronizeToolTip));
		}

		/// <summary>
		/// Deletes the file in the local and remote directory.
		/// </summary>
		/// <param name="file"></param>
		public void Delete(FileViewModel file)
        {
            if (file.IsDirectory)
            {
                try
                {
                    if (file.RemoteFullName != string.Empty && file.Handle != SyncHandle.NewUpload && client.Exists(file.RemoteFullName, true))
                        client.DeleteDirectory(file.RemoteFullName);
                }
                catch (SocketException)
                {
                    GoToDirectory(CurrentDirectory);
                    throw new Exception(Localization.Exception_NoInternet);
                }
                if (file.LocalFullName != string.Empty && Directory.Exists(file.LocalFullName))
                    Directory.Delete(file.LocalFullName + (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? '\\' : '/'), true);
            }
            else
            {
                try
                {
                    if (file.RemoteFullName != string.Empty && file.Handle != SyncHandle.NewUpload && client.Exists(file.RemoteFullName, false))
                        client.DeleteFile(file.RemoteFullName);
                }
                catch (SocketException)
                {
                    GoToDirectory(CurrentDirectory);
                    throw new Exception(Localization.Exception_NoInternet);
                }
                if (file.LocalFullName != string.Empty && File.Exists(file.LocalFullName))
                    File.Delete(file.LocalFullName);
            }

            GoToDirectory(CurrentDirectory);
            if (Connected)
                DirectoryLog.Save(Path + CurrentDirectory, client.ListDirectory(RemotePath + CurrentDirectory));
        }

        /// <summary>
        /// Go up a level in the directory.
        /// </summary>
        public void GoUpDirectory()
        {
            string path;
            if (CurrentDirectory.Length > 1)
                path = System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(CurrentDirectory)).Replace("\\", "/");
            else
                throw new Exception(Localization.Exception_CantGoUpDirectory);
            if (!path.EndsWith('/'))
                path += '/';
            GoToDirectory(path);
        }

        /// <summary>
        /// Changes the current directory to the given path.
        /// </summary>
        public void GoToDirectory(string path)
        {
            path = path.Trim().Replace("\\", "/");
            if (path.Length == 0)
                path = "/";
            else
            {
                if (!path.StartsWith("/"))
                    path = "/" + path;
                if (!path.EndsWith("/"))
                {
                    string filePath = Path + path;
                    if (File.Exists(filePath))
                    {
                        Open(filePath);
                        CurrentDirectory = System.IO.Path.GetDirectoryName(path).Replace('\\', '/');
                        GoToDirectory(CurrentDirectory);
                        return;
                    }

                    path += "/";
                }
            }

            if (client != null)
            {
                bool ExistsLocally = Directory.Exists(Path + path);
                bool ExistsRemotely = false;

                try
                {
                    ExistsRemotely = client.Exists(RemotePath + path, true);
                    Connected = true;
                }
                catch (Exception)
                {
                    Connected = false;
                }

                if (!ExistsLocally && !ExistsRemotely)
                {
                    GoToDirectory("/");
                    throw new Exception(Localization.Exception_SFTPInvalidPath);
                }

                CurrentDirectory = path;
                List<DirectoryFile> files = GetFiles(CurrentDirectory);
                List<FileViewModel> viewFiles = new();
                for (int i = 0; i < files.Count; i++)
                {
                    viewFiles.Add(new FileViewModel(files[i]));
                }
                viewFiles.Sort((x, y) => x.SortName.CompareTo(y.SortName));

                Files = new ObservableCollection<FileViewModel>(viewFiles);
            }
        }

        /// <summary>
        /// Sets the permissions of the file.
        /// </summary>
        public void SetFilePermissions(FileViewModel file, short permissions)
        {
            try
            {
                client.SetPermissions(file.RemoteFullName, permissions);
            }
            catch (SocketException)
            {
                throw new Exception(Localization.Exception_NoInternet);
            }
            finally
            {
                GoToDirectory(CurrentDirectory);
            }
        }

        /// <summary>
        /// Renames the given file to the new name.
        /// </summary>
        public void RenameFile(FileViewModel file, string newName)
        {
            if (newName != file.Name)
            {
                if (file.LocalFullName != null && file.LocalFullName != string.Empty)
                {
                    string newlocal = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(file.LocalFullName), newName);
                    if (file.IsDirectory)
                        Directory.Move(file.LocalFullName, newlocal);
                    else
                        File.Move(file.LocalFullName, newlocal);
                }
                if (Connected && file.RemoteFullName != null && file.RemoteFullName != string.Empty)
                {
                    string newremote = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(file.RemoteFullName), newName).Replace('\\', '/');
                    try
                    {
                        client.RenameFile(file.RemoteFullName, newremote);
                    }
                    catch (SocketException)
                    {
                        GoToDirectory(CurrentDirectory);
                        throw new Exception(Localization.Exception_NoInternet);
                    }
                }
            }
            GoToDirectory(CurrentDirectory);
        }

        /// <summary>
        /// Opens the given file. If it is a directory, the list of files is opdated. If it is a file, it opens the file like the OS would.
        /// </summary>
        /// <param name="file"></param>
        public void Open(FileViewModel file)
        {
            if (file.IsDirectory)
            {
                GoToDirectory(CurrentDirectory + file.Name);
            }
            else
            {
                if (file.LocalFullName != string.Empty)
                {
                    Open(file.LocalFullName);
                }
                else
                    throw new Exception(Localization.Exception_OnlineFile);
            }
        }

        public static void Open(string file)
        {
			if (File.Exists(file))
			{
				System.Diagnostics.Process p = new()
				{
					StartInfo = new System.Diagnostics.ProcessStartInfo(file)
					{
						UseShellExecute = true
					}
				};
				p.Start();
			}
		}

        /// <summary>
        /// Creates a new folder with the given name in the current directory.
        /// </summary>
        public void NewFolder(string name)
        {
            string localpath = Path + CurrentDirectory + name;
            if (!Directory.Exists(localpath))
                Directory.CreateDirectory(localpath);
            else
                throw new Exception(Localization.SFTP_FolderExists);
            GoToDirectory(CurrentDirectory);
        }

        /// <summary>
        /// Make a list of all the files in the directories. Trying to match the files according to their name.
        /// </summary>
        /// <param name="remotePath"></param>
        /// <param name="localPath"></param>
        /// <returns></returns>
        public List<DirectoryFile> GetFiles(string remotePath, string localPath)
        {
            DirectoryInfo dirInfo;
            FileSystemInfo[] localFiles = Array.Empty<FileSystemInfo>();

            if (Directory.Exists(localPath))
            {
                dirInfo = new(localPath);
                localFiles = dirInfo.GetFileSystemInfos("*", SearchOption.TopDirectoryOnly);
            }
            List<RemoteFile> remoteFiles;
            if (Connected)
                remoteFiles = client.ListDirectory(remotePath);
            else
                remoteFiles = new List<RemoteFile>();

            List<DirectoryFile> files = new();
            List<DirectoryLogItem> logList = DirectoryLog.Load(localPath);

            //Find pairs
            for (int i = 0; i < localFiles.Length; i++)
            {
                if (Ignore(localFiles[i].Name)) // Also ignore tkeys
                    continue;

                DirectoryLogItem dli = DirectoryLogItem.Empty;

                FileAttributes attr = File.GetAttributes(localFiles[i].FullName);
                bool IsDirectory = attr.HasFlag(FileAttributes.Directory);

                bool found = false;
                int j = 0;
                while (!found && j < logList.Count)
                {
                    if (logList[j].Name == localFiles[i].Name && IsDirectory == logList[j].IsDirectory)
                    {
                        dli = logList[j];
                        logList.RemoveAt(j);
                        found = true;
                    }
                    else
                        j++;
                }

                found = false;
                j = 0;
                while (!found && j < remoteFiles.Count)
                {
                    if (localFiles[i].Name == remoteFiles[j].Name && IsDirectory == remoteFiles[j].IsDirectory)
                    {
                        files.Add(new DirectoryFile(localFiles[i], remoteFiles[j], dli));
                        remoteFiles.RemoveAt(j); //Remove so the remaining remoteFiles are only remote
                        found = true;
                    }
                    else
                        j++;
                }
                if (!found)
                    files.Add(new DirectoryFile(localFiles[i], null, dli));
            }

            //Add only remote files
            for (int i = 0; i < remoteFiles.Count; i++)
            {
                if (Ignore(remoteFiles[i].Name)) // Also ignore tkeys
                    continue;

                DirectoryLogItem dli = DirectoryLogItem.Empty;

                bool found = false;
                int j = 0;
                while (!found && j < logList.Count)
                {
                    if (logList[j].Name == remoteFiles[i].Name && logList[j].IsDirectory == remoteFiles[i].IsDirectory)
                    {
                        dli = logList[j];
                        logList.RemoveAt(j);
                        found = true;
                    }
                    else
                        j++;
                }

                files.Add(new DirectoryFile(null, remoteFiles[i], dli));
            }

            return files;
        }
        public List<DirectoryFile> GetFiles(string path)
        {
            if (path == string.Empty)
                return new List<DirectoryFile>();
            return GetFiles(RemotePath + path, Path + path);
        }

        /// <summary>
        /// Checks whether the file should be ignored in the sync.
        /// </summary>
        public static bool Ignore(string fileName)
        {
            fileName = System.IO.Path.GetFileName(fileName);
            return fileName[0] == '.' || System.IO.Path.GetExtension(fileName) == ".tkey" || fileName.StartsWith("~$");
        }

        /// <summary>
        /// Converts the given path to the corresponding path in the opposite directory.
        /// </summary>
        /// <param name="path">Path to be converted.</param>
        private string ConvertPath(string path)
        {
            path = path.Replace('\\', '/');
            string newPath;
            if (path.StartsWith(Path))
            {
                newPath = RemotePath + path[Path.Length..];
            }
            else if (path.StartsWith(RemotePath))
            {
                newPath = Path + path[RemotePath.Length..];
            }
            else
                throw new Exception(Localization.Exception_ConversionError);
            return newPath.Replace('\\', '/');
        }
    }
}