using Renci.SshNet;
using Renci.SshNet.Sftp;
using Renci.SshNet.Common;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;

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
        //private readonly FileSystemWatcher watcher;

        /// <summary>
        /// The path of the local directory to be watched and synced with.
        /// </summary>
        public readonly string LocalPath = string.Empty;
        /// <summary>
        /// The path of the remote directory to be watched and synced with.
        /// </summary>
        public readonly string RemotePath = string.Empty;

        private List<string> LastSync;

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
            /*watcher = new FileSystemWatcher(localPath)
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

            //ARE ALL MISSING LASTSYNC REFERENCES
            watcher.Changed += OnChanged;
            watcher.Created += OnCreated;
            watcher.Deleted += OnDeleted;
            watcher.Renamed += OnRenamed;
            watcher.Error += OnError;

            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = true;*/

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

            //LoadLastSync();
            PasswordAuthenticationMethod auth = new(username, password);
            ConnectionInfo connectionInfo = new(host, 22, username, auth);
            connectionInfo.Encoding = System.Text.Encoding.UTF8;

            client = new SftpClient(connectionInfo);
        }
        public DirectoryManager()
        {

        }

        private void LoadLastSync()
        {
            string path = LocalPath + "\\.LastSync.tsy";
            if (!File.Exists(path))
            {
                File.Create(path).Close();
            }
            else
            {
                LastSync = new List<string>();
                using StreamReader reader = new(path);
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    LastSync.Add(line);
                }
            }
        }

        private void SaveLastSync()
        {
            if (LastSync != null)
            {
                string path = LocalPath + "\\.LastSync.tsy";
                if (!File.Exists(path))
                    File.Create(path).Close();
                using StreamWriter writer = new(path);
                for (int i = 0; i < LastSync.Count; i++)
                {
                    writer.WriteLine(LastSync[i]);
                }
            }
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
        public List<SftpFile> ListDirectory(string remoteDirectory)
        {
            if (client != null)
            {
                bool isPreconnected = client.IsConnected;
                if (!isPreconnected)
                {
                    //watcher.EnableRaisingEvents = false;
                    client.Connect();
                }

                List<SftpFile> files = client.ListDirectory(remoteDirectory).ToList();
                files.RemoveAt(0); //Remove the '.' and '..' directories.
                files.RemoveAt(0);

                if (!isPreconnected)
                {
                    client.Disconnect();
                    //watcher.EnableRaisingEvents = true;
                }

                return files;
            }
            else return new List<SftpFile>();
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
                //watcher.EnableRaisingEvents = false;
                client.Connect();
            }

            string path = Path.Combine(localPath, remoteFile.Name);
            using Stream fileStream = File.OpenWrite(path);
            client.DownloadFile(remoteFile.FullName, fileStream);

            if (!isPreconnected)
            {
                client.Disconnect();
                //watcher.EnableRaisingEvents = true;
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
                //watcher.EnableRaisingEvents = false;
                client.Connect();
            }

            string path = remotePath + '/' + Path.GetFileName(localFile);
            FileStream fs = File.OpenRead(localFile);
            client.UploadFile(fs, path, true);
            fs.Close();

            if (!isPreconnected)
            {
                client.Disconnect();
                //watcher.EnableRaisingEvents = true;
            }
        }

        /// <summary>
        /// Deletes file on the remote directory.
        /// </summary>
        /// <param name="remoteFile">SFTP File that needs to be deleted.</param>
        private void DeleteFile(SftpFile remoteFile)
        {
            client.DeleteFile(remoteFile.FullName);
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
                //watcher.EnableRaisingEvents = false;
                client.Connect();
            }

            try
            {
                System.Diagnostics.Debug.WriteLine(path);
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
                //watcher.EnableRaisingEvents = true;
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
                //watcher.EnableRaisingEvents = false;
                client.Connect();
            }

            try
            {
                System.Diagnostics.Debug.WriteLine(path);
                foreach (SftpFile file in client.ListDirectory(path))
                {
                    if ((file.Name != ".") && (file.Name != ".."))
                    {
                        if (file.IsDirectory)
                        {
                            DeleteDirectory(file.FullName);
                        }
                        else
                        {
                            client.DeleteFile(file.FullName);
                        }
                    }
                }

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
                //watcher.EnableRaisingEvents = true;
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
                //watcher.EnableRaisingEvents = false;
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
                //watcher.EnableRaisingEvents = true;
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
                //watcher.EnableRaisingEvents = false;
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
                //watcher.EnableRaisingEvents = true;
            }
        }

        /// <summary>
        /// Synchronizes custom remote and local directories.
        /// </summary>
        /// <param name="remotePath">Path of the directory on the server.</param>
        /// <param name="localPath">Path of the directory on the local machine.</param>
        private void Synchronize(string remotePath, string localPath)
        {
            //Loop through prev sync files
            //The Last sync list needs to save the lastwritetime of the deleted file. Otherwise if I delete a file, and then someone else writes in it, their work will be lost.

            //If file can be found previously & locally & remotely => Find the one with the lastest changes (ADD TO LIST)
            //If file can be found previously & locally & !remotely => Delete local (REMOVE FROM LIST)
            //If file can be found previously & !locally & remotely => Delete remote (REMOVE FROM LIST)
            //If file can be found previously & !locally & !remotely => Nothing (REMOVE FROM LIST)
            //If file can be found !previously & locally & remotely => Find the one with the latest changes (ADD TO LIST)
            //If file can be found !previously & locally & !remotely => Upload (ADD TO LIST)
            //If file can be found !previously & !locally & remotely => Download (ADD TO LIST)
            //If file can be found !previously & !locally & !remotely => Nothing

            //Optimize so it only uploads/downloads if enough timespan has occured or the file size changed.
            //Ignore dot files (.gitignore)
            //Check if files have the same name

            #region CONNECTION
            bool isPreconnected = client.IsConnected;
            if (!isPreconnected)
            {
                //watcher.EnableRaisingEvents = false;
                client.Connect();
            }
            #endregion

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
                    //Only do it if file is available (ie. isn't open in another program)
                    //Optimize so it only uploads/downloads if enough timespan has occured or the file size changed.
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
                    UploadDirectory(ConvertPath(localDirectories[i].FullName), localDirectories[i].FullName);
            }
            #endregion

            #region DISCONNECTION
            if (!isPreconnected)
            {
                client.Disconnect();
                //watcher.EnableRaisingEvents = true;
                //SaveLastSync();
            }
            #endregion
        }

        /// <summary>
        /// Synchronize the defined remote and local directories.
        /// </summary>
        public void Synchronize()
        {
            if (client != null)
                NewSynchronize(RemotePath, LocalPath);
        }

        public void NewSynchronize(string remotePath, string localPath)
        {
            #region CONNECTION
            bool isPreconnected = client.IsConnected;
            if (!isPreconnected)
            {
                //watcher.EnableRaisingEvents = false;
                client.Connect();
            }
            #endregion

            List<DirectoryFile> files = GetFiles(remotePath, localPath);

            for (int i = 0; i < files.Count; i++)
            {
                DirectoryFile file = files[i];
                if (file.LocalFile != null && file.RemoteFile != null)
                {
                    //If file can be found (!)previously & locally & remotely => Find the one with the lastest changes
                    if (file.IsDirectory)
                        NewSynchronize(file.RemoteFile.FullName, file.LocalFile.FullName);
                    else
                    {
                        System.Diagnostics.Debug.WriteLine(file.Name + ": I need to sync!");
                    }
                }
                else if (file.LogItem.Equals(DirectoryLogItem.Empty))
                {
                    if (file.LocalFile != null && file.RemoteFile == null)
                    {
                        //If file can be found !previously & locally & !remotely => Upload
                        if (file.IsDirectory)
                            UploadDirectory(ConvertPath(file.LocalFile.FullName), file.LocalFile.FullName);
                        else
                            UploadFile(remotePath, file.LocalFile.FullName);
                    }
                    else if (file.LocalFile == null && file.RemoteFile != null)
                    {
                        //If file can be found !previously & !locally & remotely => Download
                        if (file.IsDirectory)
                            DownloadDirectory(file.RemoteFile.FullName, ConvertPath(file.RemoteFile.FullName));
                        else
                            DownloadFile(file.RemoteFile, localPath);
                    }
                }
                else
                {
                    if (file.LocalFile != null && file.RemoteFile == null)
                    {
                        //If file can be found previously & locally & !remotely => Delete local (If local & previously LastWriteTime is the same, otherwise upload)
                        if (!(file.LocalFile.LastWriteTimeUtc > file.LogItem.LastWriteTimeUtc))
                        {
                            if (file.IsDirectory)
                                Directory.Delete(file.LocalFile.FullName);
                            else
                                File.Delete(file.LocalFile.FullName);
                        }
                        else
                        {
                            if (file.IsDirectory)
                                UploadDirectory(ConvertPath(file.LocalFile.FullName), file.LocalFile.FullName);
                            else
                                UploadFile(remotePath, file.LocalFile.FullName);
                        }
                    }
                    else if (file.LocalFile == null && file.RemoteFile != null)
                    {
                        //If file can be found previously & !locally & remotely => Delete remote (If remote & previously LastWriteTime is the same, otherwise download)
                        if (!(file.RemoteFile.LastWriteTimeUtc > file.LogItem.LastWriteTimeUtc))
                        {
                            if (file.IsDirectory)
                                DeleteDirectory(file.RemoteFile.FullName);
                            else
                                DeleteFile(file.RemoteFile.FullName);
                        }
                        else
                        {
                            if (file.IsDirectory)
                                DownloadDirectory(file.RemoteFile.FullName, ConvertPath(file.RemoteFile.FullName));
                            else
                                DownloadFile(file.RemoteFile, localPath);
                        }
                    }
                }
            }

            DirectoryLog.Save(localPath);

            #region DISCONNECTION
            if (!isPreconnected)
            {
                client.Disconnect();
                //watcher.EnableRaisingEvents = true;
                //SaveLastSync();
            }
            #endregion
        }

        private List<DirectoryFile> GetFiles(string remotePath, string localPath)
        {
            DirectoryInfo dirInfo = new(localPath);
            FileSystemInfo[] localFiles = dirInfo.GetFileSystemInfos("*", SearchOption.TopDirectoryOnly);
            List<SftpFile> remoteFiles = ListDirectory(remotePath);

            List<DirectoryFile> files = new();
            List<DirectoryLogItem> logList = DirectoryLog.Load(localPath);

            //Find pairs
            for (int i = 0; i < localFiles.Length; i++)
            {
                if (localFiles[i].Name[0] == '.') // Also ignore tkeys
                    continue;

                DirectoryLogItem dli = DirectoryLogItem.Empty;

                bool found = false;
                int j = 0;
                while (!found && j < logList.Count)
                {
                    if (logList[j].Name == localFiles[i].Name)
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
                    if (localFiles[i].Name == remoteFiles[j].Name)
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
                DirectoryLogItem dli = DirectoryLogItem.Empty;

                bool found = false;
                int j = 0;
                while (!found && j < logList.Count)
                {
                    if (logList[j].Name == localFiles[i].Name)
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

        /* https://stackoverflow.com/questions/52392766/downloading-a-directory-using-ssh-net-sftp-in-c-sharp
        public static void DownloadDirectory(SftpClient sftpClient, string sourceRemotePath, string destLocalPath)
        {
            Directory.CreateDirectory(destLocalPath);
            IEnumerable<SftpFile> files = sftpClient.ListDirectory(sourceRemotePath);
            foreach (SftpFile file in files)
            {
                if ((file.Name != ".") && (file.Name != ".."))
                {
                    string sourceFilePath = sourceRemotePath + "/" + file.Name;
                    string destFilePath = Path.Combine(destLocalPath, file.Name);
                    if (file.IsDirectory)
                    {
                        DownloadDirectory(sftpClient, sourceFilePath, destFilePath);
                    }
                    else
                    {
                        using (Stream fileStream = File.Create(destFilePath))
                        {
                            sftpClient.DownloadFile(sourceFilePath, fileStream);
                        }
                    }
                }
            }
        }
         */

        /* https://stackoverflow.com/questions/39397746/ssh-net-upload-whole-folder
        void UploadDirectory(SftpClient client, string localPath, string remotePath)
        {
            Console.WriteLine("Uploading directory {0} to {1}", localPath, remotePath);

            IEnumerable<FileSystemInfo> infos =
                new DirectoryInfo(localPath).EnumerateFileSystemInfos();
            foreach (FileSystemInfo info in infos)
            {
                if (info.Attributes.HasFlag(FileAttributes.Directory))
                {
                    string subPath = remotePath + "/" + info.Name;
                    if (!client.Exists(subPath))
                    {
                        client.CreateDirectory(subPath);
                    }
                    UploadDirectory(client, info.FullName, remotePath + "/" + info.Name);
                }
                else
                {
                    using (var fileStream = new FileStream(info.FullName, FileMode.Open))
                    {
                        Console.WriteLine(
                            "Uploading {0} ({1:N0} bytes)",
                            info.FullName, ((FileInfo)info).Length);

                        client.UploadFile(fileStream, remotePath + "/" + info.Name);
                    }
                }
            }
        }
        */
    }
}