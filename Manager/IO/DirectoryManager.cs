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
        /// The path of the local directory to be watched and synced with.
        /// </summary>
        public readonly string LocalPath = string.Empty;
        /// <summary>
        /// The path of the remote directory to be watched and synced with.
        /// </summary>
        public readonly string RemotePath = string.Empty;

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

        /// <summary>
        /// Returns a list of files in the remote directory
        /// </summary>
        /// <param name="remoteDirectory">Path of the directory on the server.</param>
        private List<SftpFile> ListDirectory(string remoteDirectory)
        {
            if (client != null)
            {
                bool isPreconnected = client.IsConnected;
                if (!isPreconnected)
                {
                    client.Connect();
                }

                List<SftpFile> files = client.ListDirectory(remoteDirectory).ToList();
                files.RemoveAt(0); //Remove the '.' and '..' directories.
                files.RemoveAt(0);

                if (!isPreconnected)
                {
                    client.Disconnect();
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
                client.Connect();
            }

            string path = Path.Combine(localPath, remoteFile.Name);
            using Stream fileStream = File.OpenWrite(path);
            client.DownloadFile(remoteFile.FullName, fileStream);

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

            string path = remotePath + '/' + Path.GetFileName(localFile);
            FileStream fs = File.OpenRead(localFile);
            client.UploadFile(fs, path, true);
            fs.Close();

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
            }
        }

        /// <summary>
        /// Synchronize the defined remote and local directories.
        /// </summary>
        public void Synchronize()
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
                DirectoryFile file = files[i];
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
                            DownloadFile(file.RemoteFile, localPath);
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
                            Directory.Delete(file.LocalFile.FullName, true);
                        else
                            File.Delete(file.LocalFile.FullName);
                        break;
                    case FileHandle.DeleteRemote:
                        if (file.IsDirectory)
                            DeleteDirectory(file.RemoteFile.FullName);
                        else
                            DeleteFile(file.RemoteFile.FullName);
                        break;
                }

                /*
                DirectoryFile file = files[i];
                if (file.LocalFile != null && file.RemoteFile != null)
                {
                    //If file can be found (!)previously & locally & remotely => Find the one with the lastest changes
                    if (file.IsDirectory)
                        Synchronize(file.RemoteFile.FullName, file.LocalFile.FullName);
                    else
                    {
                        //Synchronize
                        if (file.LocalFile.LastWriteTimeUtc.Ticks == file.LogItem.LocalTicks && file.RemoteFile.LastWriteTimeUtc.Ticks != file.LogItem.RemoteTicks)
                        {
                            //Download
                            DownloadFile(file.RemoteFile, localPath);
                        }
                        else if (file.LocalFile.LastWriteTimeUtc.Ticks != file.LogItem.LocalTicks && file.RemoteFile.LastWriteTimeUtc.Ticks == file.LogItem.RemoteTicks)
                        {
                            //Upload
                            UploadFile(remotePath, file.LocalFile.FullName);
                        }
                        else if (file.LocalFile.LastWriteTimeUtc.Ticks != file.LogItem.LocalTicks && file.RemoteFile.LastWriteTimeUtc.Ticks != file.LogItem.RemoteTicks)
                        {
                            if (file.LocalFile.LastWriteTimeUtc.Ticks < file.RemoteFile.LastWriteTimeUtc.Ticks)
                            {
                                //Download
                                DownloadFile(file.RemoteFile, localPath);
                            }
                            else if (file.LocalFile.LastWriteTimeUtc.Ticks > file.RemoteFile.LastWriteTimeUtc.Ticks)
                            {
                                //Upload
                                UploadFile(remotePath, file.LocalFile.FullName);
                            }
                        }
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
                        if (file.LocalFile.LastWriteTimeUtc.Ticks == file.LogItem.LocalTicks)
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
                        if (file.RemoteFile.LastWriteTimeUtc.Ticks == file.LogItem.RemoteTicks)
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
                */
            }

            DirectoryLog.Save(localPath, ListDirectory(remotePath));

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
            DirectoryInfo dirInfo = new(localPath);
            FileSystemInfo[] localFiles = dirInfo.GetFileSystemInfos("*", SearchOption.TopDirectoryOnly);
            List<SftpFile> remoteFiles = ListDirectory(remotePath);

            List<DirectoryFile> files = new();
            List<DirectoryLogItem> logList = DirectoryLog.Load(localPath);

            //Find pairs
            for (int i = 0; i < localFiles.Length; i++)
            {
                if (localFiles[i].Name[0] == '.' || Path.GetExtension(localFiles[i].Name) == ".tkey") // Also ignore tkeys
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
            if (path[^1] != '/')
                path += '/';

            if (path == string.Empty)
                return new List<DirectoryFile>();
            else
                return GetFiles(path, ConvertPath(path));
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