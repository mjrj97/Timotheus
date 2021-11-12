using Renci.SshNet;
using Renci.SshNet.Sftp;
using Renci.SshNet.Common;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;

namespace Timotheus.IO
{
    /// <summary>
    /// Class that contains SFTP related methods. Uses SSH.NET.
    /// </summary>
    public class DirectoryManager
    {
        /// <summary>
        /// Client that is connected to the remote directory.
        /// </summary>
        private readonly SftpClient client;
        /// <summary>
        /// Throws events if changes happen to the local directory.
        /// </summary>
        private readonly FileSystemWatcher watcher;

        /// <summary>
        /// The path of the local directory to be watched and synced with.
        /// </summary>
        public readonly string LocalPath;
        /// <summary>
        /// The path of the remote directory to be watched and synced with.
        /// </summary>
        public readonly string RemotePath;

        /// <summary>
        /// Constructor. Is an object that is tasked with keeping a local and remote directory synced.
        /// </summary>
        /// <param name="localPath">Path to the local directory</param>
        /// <param name="remotePath">Path to the remote directory</param>
        /// <param name="host">Connection host/url</param>
        /// <param name="username">Username to the SFTP connection</param>
        /// <param name="password">Password to the SFTP connection</param>
        public DirectoryManager(string localPath, string remotePath, string host, string username, string password)
        {
            watcher = new FileSystemWatcher(localPath)
            {
                NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Security
                                 | NotifyFilters.Size
            };

            watcher.Changed += OnChanged;
            watcher.Created += OnCreated;
            watcher.Deleted += OnDeleted;
            watcher.Renamed += OnRenamed;
            watcher.Error += OnError;

            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = true;

            LocalPath = localPath.Replace('/', '\\');
            RemotePath = remotePath.Replace('\\', '/');
            client = new SftpClient(host, username, password);
        }

        /// <summary>
        /// Handles changes in the local directory and its subdirectories.
        /// </summary>
        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (Path.GetExtension(e.Name) != ".tmp" && File.GetAttributes(e.FullPath) != FileAttributes.Directory)
            {
                if (e.ChangeType != WatcherChangeTypes.Changed)
                {
                    return;
                }
                System.Diagnostics.Debug.WriteLine($"Changed: {e.FullPath}");
                UploadFile(ConvertPath(Path.GetDirectoryName(e.FullPath)), e.FullPath);
            }
        }

        /// <summary>
        /// Handles file/folder creation in the local directory and its subdirectories.
        /// </summary>
        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            if (Path.GetExtension(e.Name) != ".tmp")
            {
                string value = $"Created: {e.FullPath}";
                System.Diagnostics.Debug.WriteLine(value);
                if (File.GetAttributes(e.FullPath) == FileAttributes.Directory)
                    UploadDirectory(ConvertPath(Path.GetDirectoryName(e.FullPath)), e.FullPath);
                else
                    UploadFile(ConvertPath(Path.GetDirectoryName(e.FullPath)), e.FullPath);
            }
        }

        /// <summary>
        /// Handles deletions in the local directory and its subdirectories.
        /// </summary>
        private void OnDeleted(object sender, FileSystemEventArgs e)
        {
            if (Path.GetExtension(e.Name) != ".tmp")
            {
                System.Diagnostics.Debug.WriteLine($"Deleted: {e.FullPath}");
                DeleteFile(ConvertPath(e.FullPath));
            }
        }

        /// <summary>
        /// Handles renaming in the local directory and its subdirectories.
        /// </summary>
        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            if (Path.GetExtension(e.Name) != ".tmp" && Path.GetExtension(e.OldFullPath) != ".tmp")
            {
                System.Diagnostics.Debug.WriteLine($"Renamed:");
                System.Diagnostics.Debug.WriteLine($"    Old: {e.OldFullPath}");
                System.Diagnostics.Debug.WriteLine($"    New: {e.FullPath}");

                client.Connect();
                client.RenameFile(ConvertPath(e.OldFullPath), ConvertPath(e.FullPath));
                client.Disconnect();
            }
        }

        /// <summary>
        /// Handles errors with the FileSystemWatcher.
        /// </summary>
        private void OnError(object sender, ErrorEventArgs e)
        {
            Exception ex = e.GetException();
            if (ex != null)
                System.Diagnostics.Debug.WriteLine($"Message: {ex.Message}");
        }

        /// <summary>
        /// Returns a list of files in the remote directory
        /// </summary>
        /// <param name="remoteDirectory">Path of the directory on the server.</param>
        private List<SftpFile> ListDirectory(string remoteDirectory)
        {
            bool isPreconnected = client.IsConnected;
            if (!isPreconnected)
            {
                watcher.EnableRaisingEvents = false;
                client.Connect();
            }

            List<SftpFile> files = client.ListDirectory(remoteDirectory).ToList();
            files.RemoveAt(0); //Remove the '.' and '..' directories.
            files.RemoveAt(0);

            if (!isPreconnected)
            {
                client.Disconnect();
                watcher.EnableRaisingEvents = true;
            }

            return files;
        }

        /// <summary>
        /// Returns a list of files in the remote directory
        /// </summary>
        /// <param name="remoteDirectory">Path of the directory on the server.</param>
        public ObservableCollection<SftpFile> GetFilesList(string remoteDirectory)
        {
            List<SftpFile> files = ListDirectory(remoteDirectory);
            ObservableCollection<SftpFile> listOfFiles = new();
            foreach (SftpFile file in files)
            {
                listOfFiles.Add(file);
            }
            return listOfFiles;
        }

        /// <summary>
        /// Downloads a file from remote directory to a local directory
        /// </summary>
        /// <param name="remoteFile">Path of the file on the server.</param>
        /// <param name="localPath">Path of the directory on the local machine.</param>
        private void DownloadFile(SftpFile remoteFile, string localPath)
        {
            bool isPreconnected = client.IsConnected;
            if (!isPreconnected)
            {
                watcher.EnableRaisingEvents = false;
                client.Connect();
            }

            string path = Path.Combine(localPath, remoteFile.Name);
            using Stream fileStream = File.OpenWrite(path);
            client.DownloadFile(remoteFile.FullName, fileStream);

            if (!isPreconnected)
            {
                client.Disconnect();
                watcher.EnableRaisingEvents = true;
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
                watcher.EnableRaisingEvents = false;
                client.Connect();
            }

            string path = remotePath + '/' + Path.GetFileName(localFile);
            FileStream fs = File.OpenRead(localFile);
            client.UploadFile(fs, path, true);
            fs.Close();

            if (!isPreconnected)
            {
                client.Disconnect();
                watcher.EnableRaisingEvents = true;
            }
        }

        /// <summary>
        /// Deletes file on the remote directory.
        /// </summary>
        /// <param name="remoteFile">SFTP File that needs to be deleted.</param>
        private void DeleteFile(SftpFile remoteFile)
        {
            bool isPreconnected = client.IsConnected;
            if (!isPreconnected)
            {
                watcher.EnableRaisingEvents = false;
                client.Connect();
            }

            client.DeleteFile(remoteFile.FullName);

            if (!isPreconnected)
            {
                client.Disconnect();
                watcher.EnableRaisingEvents = true;
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
                watcher.EnableRaisingEvents = false;
                client.Connect();
            }

            try
            {
                System.Diagnostics.Debug.WriteLine(path);
                client.Delete(path);
            }
            catch (SftpPathNotFoundException e)
            {
                //Do something when file is not found
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            if (!isPreconnected)
            {
                client.Disconnect();
                watcher.EnableRaisingEvents = true;
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
                watcher.EnableRaisingEvents = false;
                client.Connect();
            }

            IList<SftpFile> files = ListDirectory(remotePath);

            foreach (SftpFile file in files)
            {
                if (!file.IsDirectory && !file.IsSymbolicLink)
                    DownloadFile(file, localPath);
                else if (file.IsSymbolicLink)
                    System.Diagnostics.Debug.WriteLine("Symbolic link ignore: " + file.FullName);
                else
                {
                    DirectoryInfo dir = Directory.CreateDirectory(Path.Combine(localPath, file.Name));
                    DownloadDirectory(file.FullName, dir.FullName);
                }
            }

            if (!isPreconnected)
            {
                client.Disconnect();
                watcher.EnableRaisingEvents = true;
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
                watcher.EnableRaisingEvents = false;
                client.Connect();
            }

            string convertedPath = ConvertPath(localPath);
            if (!client.Exists(convertedPath))
                client.CreateDirectory(convertedPath);

            //Files in local directory
            string[] localFilePaths = Directory.GetFiles(localPath);
            string[] localDirectoryPaths = Directory.GetDirectories(localPath);

            for (int i = 0; i < localFilePaths.Length; i++)
            {
                System.Diagnostics.Debug.WriteLine("Upload: " + localFilePaths[i]);
                UploadFile(remotePath, localFilePaths[i]);
            }

            for (int i = 0; i < localDirectoryPaths.Length; i++)
            {
                string path = ConvertPath(localDirectoryPaths[i]);
                System.Diagnostics.Debug.WriteLine("Create: " + path);
                client.CreateDirectory(path);
                UploadDirectory(path, localDirectoryPaths[i]);
            }

            if (!isPreconnected)
            {
                client.Disconnect();
                watcher.EnableRaisingEvents = true;
            }
        }

        /// <summary>
        /// Synchronizes custom remote and local directories.
        /// </summary>
        /// <param name="remotePath">Path of the directory on the server.</param>
        /// <param name="localPath">Path of the directory on the local machine.</param>
        private void Synchronize(string remotePath, string localPath)
        {
            bool isPreconnected = client.IsConnected;
            if (!isPreconnected)
            {
                watcher.EnableRaisingEvents = false;
                client.Connect();
            }

            #region List all files in local and remote directory
            //Files in remote directory
            IList<SftpFile> remote = ListDirectory(remotePath);

            List<SftpFile> remoteFiles = new();
            List<SftpFile> remoteDirectories = new();
            for (int i = 0; i < remote.Count; i++)
            {
                if (remote[i].IsDirectory)
                    remoteDirectories.Add(remote[i]);
                else
                    remoteFiles.Add(remote[i]);
            }

            //Files in local directory
            string[] localFilePaths = Directory.GetFiles(localPath);
            string[] localDirectoryPaths = Directory.GetDirectories(localPath);

            List<FileInfo> localFiles = new();
            List<DirectoryInfo> localDirectories = new();

            for (int i = 0; i < localFilePaths.Length; i++)
            {
                //Get file info for all local files
                localFiles.Add(new FileInfo(localFilePaths[i]));
            }

            for (int i = 0; i < localDirectoryPaths.Length; i++)
            {
                //Get directory info for all directories
                localDirectories.Add(new DirectoryInfo(localDirectoryPaths[i]));
            }
            #endregion

            #region Sync the files
            int[] indices = new int[remoteFiles.Count];
            bool[] localFound = new bool[localFiles.Count];
            for (int i = 0; i < remoteFiles.Count; i++)
            {
                bool found = false;
                int j = 0;
                while (!found && j < localFiles.Count)
                {
                    if (!localFound[j] && remoteFiles[i].Name == localFiles[j].Name)
                    {
                        found = true;
                        localFound[j] = true;
                        indices[i] = j;
                    }
                    j++;
                }

                if (!found)
                    indices[i] = -1; //Set index to -1 if not found locally
            }

            for (int i = 0; i < remoteFiles.Count; i++)
            {
                if (indices[i] == -1)
                    DownloadFile(remoteFiles[i], localPath);
                else
                {
                    if (remoteFiles[i].LastWriteTimeUtc > localFiles[indices[i]].LastWriteTimeUtc)
                        DownloadFile(remoteFiles[i], localPath);
                    else if (remoteFiles[i].LastWriteTimeUtc < localFiles[indices[i]].LastWriteTimeUtc)
                        UploadFile(remotePath, localFiles[indices[i]].FullName);
                }
            }

            for (int i = 0; i < localFiles.Count; i++)
            {
                if (!localFound[i])
                    UploadFile(remotePath, localFiles[i].FullName);
            }

            #endregion

            #region Sync the folders
            indices = new int[remoteDirectories.Count];
            localFound = new bool[localDirectories.Count];
            for (int i = 0; i < remoteDirectories.Count; i++)
            {
                bool found = false;
                int j = 0;
                while (!found && j < localDirectories.Count)
                {
                    if (!localFound[j] && remoteDirectories[i].Name == localDirectories[j].Name)
                    {
                        found = true;
                        localFound[j] = true;
                        indices[i] = j;
                    }
                    j++;
                }

                if (!found)
                    indices[i] = -1; //Set index to -1 if not found locally
            }

            for (int i = 0; i < remoteDirectories.Count; i++)
            {
                if (indices[i] == -1)
                    DownloadDirectory(remoteDirectories[i].FullName, ConvertPath(remoteDirectories[i].FullName));
                else
                    Synchronize(remoteDirectories[i].FullName, localDirectories[indices[i]].FullName);
            }

            for (int i = 0; i < localDirectories.Count; i++)
            {
                if (!localFound[i])
                    UploadDirectory(remotePath, localDirectories[i].FullName);
            }
            #endregion

            if (!isPreconnected)
            {
                client.Disconnect();
                watcher.EnableRaisingEvents = true;
            }
        }

        /// <summary>
        /// Synchronize the defined remote and local directories.
        /// </summary>
        public void Synchronize()
        {
            Synchronize(RemotePath, LocalPath);
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
                newPath = newPath.Replace('/', '\\');
            }
            else
                throw new Exception("Exception_SFTPInvalidPath");
            return newPath;
        }
    }
}