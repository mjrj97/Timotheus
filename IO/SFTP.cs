using Renci.SshNet;
using Renci.SshNet.Sftp;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Timotheus.IO
{
    /// <summary>
    /// Class that contains SFTP related methods. Uses SSH.NET.
    /// </summary>
    public static class SFTP
    {
        /// <summary>
        /// Returns a list of files in the remote directory
        /// </summary>
        /// <param name="client">The Sftp client used to connect to the remote directory.</param>
        /// <param name="remoteDirectory">Path of the directory on the server.</param>
        public static List<SftpFile> GetListOfFiles(SftpClient client, string remoteDirectory)
        {
            List<SftpFile> files = client.ListDirectory(remoteDirectory).ToList();
            files.RemoveAt(0); //Remove the '.' and '..' directories.
            files.RemoveAt(0);
            return files;
        }

        /// <summary>
        /// Downloads a file from remote directory to a local directory
        /// </summary>
        /// <param name="client">The Sftp client used to connect to the remote directory.</param>
        /// <param name="remoteFile">Path of the file on the server.</param>
        /// <param name="localPath">Path of the directory on the local machine.</param>
        public static void DownloadFile(SftpClient client, SftpFile remoteFile, string localPath)
        {
            string path = Path.Combine(localPath, remoteFile.Name);
            using Stream fileStream = File.OpenWrite(path);
            client.DownloadFile(remoteFile.FullName, fileStream);
        }

        /// <summary>
        /// Uploads a file from the local directory to the remote directory
        /// </summary>
        /// <param name="client">The Sftp client used to connect to the remote directory.</param>
        /// <param name="remotePath">Path of the directory on the server.</param>
        /// <param name="localFile">Path of the file on the local machine.</param>
        public static void UploadFile(SftpClient client, string remotePath, string localFile)
        {
            using Stream fileStream = File.OpenWrite(localFile);
            client.UploadFile(fileStream, remotePath);
        }

        /// <summary>
        /// Deletes file on the remote directory.
        /// </summary>
        /// <param name="client">The Sftp client used to connect to the remote directory.</param>
        /// <param name="remoteFile">Path of the file on the server.</param>
        public static void DeleteFile(SftpClient client, SftpFile remoteFile)
        {
            client.DeleteFile(remoteFile.FullName);
        }

        /// <summary>
        /// Downloads the entire remote directory and every file under each subfolder to the local directory.
        /// </summary>
        /// <param name="client">The Sftp client used to connect to the remote directory.</param>
        /// <param name="remotePath">Path of the directory on the server.</param>
        /// <param name="localPath">Path of the directory on the local machine.</param>
        public static void DownloadDirectory(SftpClient client, string remotePath, string localPath)
        {
            IEnumerable<SftpFile> files = client.ListDirectory(remotePath);

            foreach (SftpFile file in files)
            {
                if (!file.IsDirectory && !file.IsSymbolicLink)
                {
                    DownloadFile(client, file, localPath);
                }
                else if (file.IsSymbolicLink)
                {
                    System.Diagnostics.Debug.WriteLine("Symbolic link ignore: " + file.FullName);
                }
                else if (file.Name != "." && file.Name != "..")
                {
                    DirectoryInfo dir = Directory.CreateDirectory(Path.Combine(localPath, file.Name));
                    DownloadDirectory(client, file.FullName, dir.FullName);
                }
            }
        }

        /// <summary>
        /// Synchronizes the remote and local directories.
        /// </summary>
        /// <param name="client">The Sftp client used to connect to the remote directory.</param>
        /// <param name="remotePath">Path of the directory on the server.</param>
        /// <param name="localPath">Path of the directory on the local machine.</param>
        public static void Synchronize(SftpClient client, string remotePath, string localPath)
        {
            #region List all files in local and remote directory
            //Files in remote directory
            List<SftpFile> remote = GetListOfFiles(client, remotePath);

            List<SftpFile> remoteFiles = new List<SftpFile>();
            List<SftpFile> remoteDirectories = new List<SftpFile>();
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

            List<FileInfo> localFiles = new List<FileInfo>();
            List<DirectoryInfo> localDirectories = new List<DirectoryInfo>();

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
                    System.Diagnostics.Debug.WriteLine("Remote: " + remoteFiles[i].Name);
                    System.Diagnostics.Debug.WriteLine("Local: " + localFiles[j].Name);
                    if (!localFound[j] && remoteFiles[i].Name == localFiles[j].Name)
                    {
                        System.Diagnostics.Debug.WriteLine("Found!");
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
                {
                    //Download file!
                }
                else
                {
                    if (remoteFiles[i].LastWriteTimeUtc > localFiles[indices[i]].LastWriteTimeUtc)
                    {
                        //Download file!
                    }
                    else if(remoteFiles[i].LastWriteTimeUtc < localFiles[indices[i]].LastWriteTimeUtc)
                    {
                        //Upload file!
                    }
                    else
                    {
                        //They should be the same, so don't do anything
                    }
                }
            }

            for (int i = 0; i < localFiles.Count; i++)
            {
                if (!localFound[i])
                {
                    //Upload file!
                }
            }

            #endregion

            #region Sync the folders

            #endregion
        }
    }
}