using Renci.SshNet;
using Renci.SshNet.Sftp;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Timotheus.Utility
{
    public class SFTP
    {
        //Returns a list of files in the remote directory
        public static List<SftpFile> GetListOfFiles(SftpClient client, string remoteDirectory)
        {
            List<SftpFile> files = client.ListDirectory(remoteDirectory).ToList();
            return files;
        }

        //Downloads a file from remote directory to a local directory
        public static void DownloadFile(SftpClient client, SftpFile remoteFile, string localPath)
        {
            string path = Path.Combine(localPath, remoteFile.Name);
            using Stream fileStream = File.OpenWrite(path);
            client.DownloadFile(remoteFile.FullName, fileStream);
        }

        //Uploads a file from the local directory to the remote directory
        public static void UploadFile(SftpClient client, string remotePath, string localPath)
        {
            using Stream fileStream = File.OpenWrite(localPath);
            client.UploadFile(fileStream, remotePath);
        }

        //Deletes file on remote directory
        public static void DeleteFile(SftpClient client, SftpFile remoteFile)
        {
            client.DeleteFile(remoteFile.FullName);
        }

        //Downloads the entire directory and every file under each subdirectory
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
    }
}