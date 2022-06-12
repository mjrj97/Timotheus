using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.ComponentModel;
using Timotheus.IO;
using Timotheus.Utility;
using System.Collections.ObjectModel;
using System.Threading;

namespace Timotheus.ViewModels
{
    /// <summary>
    /// Class that contains (S)FTP related methods.
    /// </summary>
    public class DirectoryViewModel : ViewModel
    {
        /// <summary>
        /// The currently selected file. Used for the context menu.
        /// </summary>
        public FileViewModel Selected { get; set; }

        /// <summary>
        /// Client that is connected to the remote directory.
        /// </summary>
        private readonly DirectoryClient client;

        private string _currentDirectory = string.Empty;
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
        /// The path of the local directory to be watched and synced with.
        /// </summary>
        public readonly string LocalPath = string.Empty;
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
        public DirectoryViewModel(string localPath, string remotePath, string host, int port, string username, string password) : this()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                LocalPath = localPath.Replace('/', '\\');
                if (LocalPath[^1] != '\\')
                    LocalPath += '\\';
            }
            else
            {
                LocalPath = localPath;
                if (LocalPath[^1] != '/')
                    LocalPath += '/';
            }
            RemotePath = remotePath.Replace('\\', '/');

            if (!Directory.Exists(LocalPath))
                throw new Exception();
            if (port != 22 && port != 21)
                throw new Exception("Port is not supported.");

            client = new DirectoryClient(host, port, username, password);

            GoToDirectory(RemotePath);
        }
        public DirectoryViewModel(string localPath, string remotePath, string host, string username, string password) : this(localPath, remotePath, host, 22, username, password) { }
        public DirectoryViewModel()
        {
            Sync = new();
            Sync.DoWork += Synchronize;

            /*var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromMinutes(1);

            BackgroundSync = new Timer((e) =>
            {
                File.AppendAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/Test.txt", DateTime.Now.ToString() + "\n");
            }, null, startTimeSpan, periodTimeSpan);*/
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
                        DirectoryInfo dir = Directory.CreateDirectory(Path.Combine(localPath, file.Name));
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
                string name = Path.GetFileName(localFilePaths[i]);
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
            if (client != null)
                Synchronize(RemotePath, LocalPath);
        }

        /// <summary>
        /// Synchronizes custom remote and local directories.
        /// </summary>
        /// <param name="remotePath">Path of the directory on the server.</param>
        /// <param name="localPath">Path of the directory on the local machine.</param>
        private void Synchronize(string remotePath, string localPath)
        {
            #region CONNECTION
            bool isPreconnected = client.IsConnected;
            if (!isPreconnected)
            {
                client.Connect();
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
                    //File.Handle is determined by the Directory file on initialization.
                    switch (file.Handle)
                    {
                        case SyncHandle.Synchronize:
                            if (file.IsDirectory)
                                Synchronize(file.RemoteFile.FullName, file.LocalFile.FullName);
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

                    //Report the progress to the ProgressDialog
                    Sync.ReportProgress((i*100) / files.Count, file.Name);
                }
            }

            DirectoryLog.Save(localPath, client.ListDirectory(remotePath));

            #region DISCONNECTION
            if (!isPreconnected)
            {
                client.Disconnect();
            }
            #endregion
        }

        /// <summary>
        /// Go up a level in the directory.
        /// </summary>
        public void GoUpDirectory()
        {
            string path = Path.GetDirectoryName(CurrentDirectory) + "/";
            if (path.Length >= RemotePath.Length)
                GoToDirectory(path);
            else
                throw new Exception(Localization.Localization.Exception_CantGoUpDirectory);
        }

        /// <summary>
        /// Changes the current directory to the given path.
        /// </summary>
        public void GoToDirectory(string path)
        {
            CurrentDirectory = Path.TrimEndingDirectorySeparator(path.Replace('\\', '/'));
            List<DirectoryFile> files = GetFiles(CurrentDirectory);
            List<FileViewModel> viewFiles = new();
            for (int i = 0; i < files.Count; i++)
            {
                viewFiles.Add(new FileViewModel(files[i]));
            }
            viewFiles.Sort((x, y) => x.SortName.CompareTo(y.SortName));

            Files = new ObservableCollection<FileViewModel>(viewFiles);
        }

        /// <summary>
        /// Sets the permissions of the file.
        /// </summary>
        public void SetFilePermissions(FileViewModel file, short permissions)
        {
            client.SetPermissions(file.RemoteFullName, permissions);
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
            List<RemoteFile> remoteFiles = client.ListDirectory(remotePath);

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
            if (path[^1] != '/')
                path += '/';
            if (RemotePath.StartsWith(path))
                path = RemotePath;
            if (!path.StartsWith(RemotePath))
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    path = path.Replace('/', '\\');
                if (path.StartsWith(LocalPath))
                    path = ConvertPath(path);
            }

            return GetFiles(path, ConvertPath(path));
        }

        /// <summary>
        /// Checks whether the file should be ignored in the sync.
        /// </summary>
        private static bool Ignore(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return fileName[0] == '.' || Path.GetExtension(fileName) == ".tkey" || fileName.StartsWith("~$");
        }

        /// <summary>
        /// Converts the given path to the corresponding path in the opposite directory.
        /// </summary>
        /// <param name="path">Path to be converted.</param>
        private string ConvertPath(string path)
        {
            string newPath;
            if (path.StartsWith(LocalPath))
            {
                newPath = RemotePath + path[LocalPath.Length..];
                newPath = newPath.Replace('\\', '/');
            }
            else if (path.StartsWith(RemotePath))
            {
                newPath = LocalPath + path[RemotePath.Length..];
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    newPath = newPath.Replace('/', '\\');
            }
            else
                throw new Exception("Exception_SFTPInvalidPath");
            return newPath;
        }
    }
}