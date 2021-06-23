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
        public static IEnumerable<SftpFile> GetListOfFiles(SftpClient client, string remoteDirectory)
        {
            IEnumerable<SftpFile> files = client.ListDirectory(remoteDirectory);
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
            //List all files in the remote directory
            List<SftpFile> remoteFiles = client.ListDirectory(remotePath).ToList();

            //Remove the '.' and '..' directories.
            remoteFiles.RemoveAt(0);
            remoteFiles.RemoveAt(0);

            //Go from the top and add files from subdirectories
            int i = 0;
            while (i < remoteFiles.Count && i < 50)
            {
                if (remoteFiles[i].IsDirectory)
                {
                    List<SftpFile> next = client.ListDirectory(remoteFiles[i].FullName).ToList();
                    for (int j = 0; j < next.Count; j++)
                    {
                        if (next[j].Name != "." && next[j].Name != "..")
                            remoteFiles.Add(next[j]);
                    }
                }
                i++;
            }

            //List all files in the local directory
            string[] localFilePaths = Directory.GetFiles(localPath, "*.*", SearchOption.AllDirectories);
            string[] localDirectoryPaths = Directory.GetDirectories(localPath, "*.*", SearchOption.AllDirectories);

            List<FileInfo> localFiles = new List<FileInfo>();
            List<DirectoryInfo> localDirectories = new List<DirectoryInfo>();

            //Get file info for all local files
            for (i = 0; i < localFilePaths.Length; i++)
            {
                localFiles.Add(new FileInfo(localFilePaths[i]));
            }

            //Get directory info for all directories
            for (i = 0; i < localDirectoryPaths.Length; i++)
            {
                localDirectories.Add(new DirectoryInfo(localDirectoryPaths[i]));
            }
        }
    }
}