using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.ComponentModel;
using Renci.SshNet.Common;

namespace Timotheus.IO
{
    /// <summary>
    /// Class that contains (S)FTP related methods.
    /// </summary>
    public class DirectoryManager
    {
        /// <summary>
        /// Client that is connected to the remote directory.
        /// </summary>
        private readonly DirectoryClient client;

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

        /// <summary>
        /// Constructor. Is an object that is tasked with keeping a local and remote directory synced.
        /// </summary>
        /// <param name="localPath">Path to the local directory</param>
        /// <param name="remotePath">Path to the remote directory</param>
        /// <param name="host">Connection host/url</param>
        /// <param name="username">Username to the (S)FTP connection</param>
        /// <param name="password">Password to the (S)FTP connection</param>
        public DirectoryManager(string localPath, string remotePath, string host, int port, string username, string password) : this()
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
        }
        public DirectoryManager(string localPath, string remotePath, string host, string username, string password) : this(localPath, remotePath, host, 22, username, password) { }
        public DirectoryManager()
        {
            Sync = new();
            Sync.DoWork += Synchronize;
        }

        /// <summary>
        /// Downloads a file from remote directory to a local directory
        /// </summary>
        /// <param name="remotePath">Path of the file on the server.</param>
        /// <param name="localPath">Path of the directory on the local machine.</param>
        private void DownloadFile(string remotePath, string localPath)
        {
            bool isPreconnected = client.IsConnected;
            if (!isPreconnected)
            {
                client.Connect();
            }

            client.DownloadFile(remotePath, localPath);

            if (!isPreconnected)
            {
                client.Disconnect();
            }
        }

        /// <summary>
        /// Uploads a file from the local directory to the remote directory
        /// </summary>
        /// <param name="remotePath">Path of the directory on the server.</param>
        /// <param name="localFile">Path of the file on the local machine.</param>
        private void UploadFile(string remotePath, string localFile)
        {
            bool isPreconnected = client.IsConnected;
            if (!isPreconnected)
            {
                client.Connect();
            }

            try
            {
                client.UploadFile(remotePath, localFile);
            }
            catch (IOException) { }

            if (!isPreconnected)
            {
                client.Disconnect();
            }
        }

        /// <summary>
        /// Deletes file on the remote directory.
        /// </summary>
        /// <param name="path">Path of the file on the server.</param>
        private void DeleteFile(string path)
        {
            bool isPreconnected = client.IsConnected;
            if (!isPreconnected)
            {
                client.Connect();
            }

            try
            {
                client.DeleteFile(path);
            }
            catch (SftpPathNotFoundException e)
            {
                //Do something when file is not found
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            if (!isPreconnected)
            {
                client.Disconnect();
            }
        }

        /// <summary>
        /// Deletes directory on the remote directory.
        /// </summary>
        /// <param name="path">Path of the directory on the server.</param>
        private void DeleteDirectory(string path)
        {
            bool isPreconnected = client.IsConnected;
            if (!isPreconnected)
            {
                client.Connect();
            }

            try
            {
                client.DeleteDirectory(path);
            }
            catch (SftpPathNotFoundException e)
            {
                //Do something when file is not found
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            if (!isPreconnected)
            {
                client.Disconnect();
            }
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

            client.DownloadDirectory(remotePath, localPath);

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
                UploadFile(remotePath, localFilePaths[i]);
            }

            for (int i = 0; i < localDirectoryPaths.Length; i++)
            {
                string path = ConvertPath(localDirectoryPaths[i]);
                client.CreateDirectory(path);
                UploadDirectory(path, localDirectoryPaths[i]);
            }

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
                if (Sync.CancellationPending == true)
                {
                    //Top synchronization if user presses 'Cancel'
                    break;
                }
                else
                {
                    DirectoryFile file = files[i];
                    //File.Handle is determined by the Directory file on initialization.
                    switch (file.Handle)
                    {
                        case FileHandle.Synchronize:
                            if (file.IsDirectory)
                                Synchronize(file.RemoteFile.FullName, file.LocalFile.FullName);
                            break;
                        case FileHandle.NewDownload:
                        case FileHandle.Download:
                            if (file.IsDirectory)
                                DownloadDirectory(file.RemoteFile.FullName, ConvertPath(file.RemoteFile.FullName));
                            else
                                DownloadFile(file.RemoteFile.FullName, localPath);
                            break;
                        case FileHandle.NewUpload:
                        case FileHandle.Upload:
                            if (file.IsDirectory)
                                UploadDirectory(ConvertPath(file.LocalFile.FullName), file.LocalFile.FullName);
                            else
                                UploadFile(remotePath, file.LocalFile.FullName);
                            break;
                        case FileHandle.DeleteLocal:
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
                        case FileHandle.DeleteRemote:
                            if (file.IsDirectory)
                                DeleteDirectory(file.RemoteFile.FullName);
                            else
                                DeleteFile(file.RemoteFile.FullName);
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
            return fileName[0] == '.' || Path.GetExtension(fileName) == ".tkey";
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