using Renci.SshNet;
using FluentFTP;
using System.IO;
using Renci.SshNet.Sftp;
using System.Collections.Generic;
using System.Linq;

namespace Timotheus.IO
{
    public class DirectoryClient
    {
        private readonly SftpClient sftpClient;
        private readonly FtpClient ftpClient;

        private readonly int port;

        public bool IsConnected
        {
            get
            {
                if (port == 22)
                    return sftpClient.IsConnected;
                else
                    return ftpClient.IsConnected;
            }
        }

        public DirectoryClient(string host, int port, string username, string password)
        {
            this.port = port;
            if (port == 22)
            {
                PasswordAuthenticationMethod auth = new(username, password);
                ConnectionInfo connectionInfo = new(host, port, username, auth);
                connectionInfo.Encoding = System.Text.Encoding.UTF8;
                sftpClient = new SftpClient(connectionInfo);
            }
            else
                ftpClient = new FtpClient(host, port, username, password);
        }

        public List<RemoteFile> ListDirectory(string remote)
        {
            List<RemoteFile> files = new();
            bool isPreconnected = IsConnected;
            if (!isPreconnected)
            {
                Connect();
            }

            if (port == 22)
            {
                if (sftpClient.Exists(remote))
                {
                    List<SftpFile> f = sftpClient.ListDirectory(remote).ToList();
                    f.RemoveAt(0); //Remove the '.' and '..' directories.
                    f.RemoveAt(0);

                    foreach (SftpFile file in f)
                    {
                        files.Add(new RemoteFile(file));
                    }
                }
            }
            else
            {
                if (ftpClient.DirectoryExists(remote))
                {
                    FtpListItem[] f = ftpClient.GetListing(remote);

                    foreach (FtpListItem file in f)
                    {
                        if (file.Name != "." && file.Name != "..")
                            files.Add(new RemoteFile(file));
                    }
                }
            }

            if (!isPreconnected)
            {
                Disconnect();
            }

            return files;
        }

        public void CreateDirectory(string remote)
        {
            if (port == 22)
                sftpClient.CreateDirectory(remote);
            else
                ftpClient.CreateDirectory(remote);
        }

        public bool Exists(string remote, bool directory)
        {
            bool exists;
            if (port == 22)
            {
                exists = sftpClient.Exists(remote);
            }
            else
            {
                if (directory)
                    exists = ftpClient.DirectoryExists(remote);
                else
                    exists = ftpClient.FileExists(remote);
            }
            return exists;
        }

        public void DownloadFile(string remote, string local)
        {
            if (port == 22)
            {
                string path = Path.Combine(local, remote);
                using Stream fileStream = File.OpenWrite(path);
                sftpClient.DownloadFile(remote, fileStream);
            }
            else
                ftpClient.DownloadFile(local, remote);
        }

        public void DownloadDirectory(string remote, string local)
        {
            if (port == 22)
            {
                foreach (SftpFile file in sftpClient.ListDirectory(remote))
                {
                    if ((file.Name != ".") && (file.Name != ".."))
                    {
                        if (!file.IsDirectory && !file.IsSymbolicLink)
                            DownloadFile(file.FullName, local);
                        else if (file.IsSymbolicLink)
                            System.Diagnostics.Debug.WriteLine("Symbolic link ignore: " + file.FullName);
                        else
                        {
                            DirectoryInfo dir = Directory.CreateDirectory(Path.Combine(local, file.Name));
                            DownloadDirectory(file.FullName, dir.FullName);
                        }
                    }
                }
            }
            else
            {
                foreach (FtpListItem file in ftpClient.GetListing(remote))
                {
                    if ((file.Name != ".") && (file.Name != ".."))
                    {
                        if (file.Type != FtpFileSystemObjectType.Directory && file.Type != FtpFileSystemObjectType.Link)
                            DownloadFile(file.FullName, local);
                        else if (file.Type == FtpFileSystemObjectType.Link)
                            System.Diagnostics.Debug.WriteLine("Symbolic link ignore: " + file.FullName);
                        else
                        {
                            DirectoryInfo dir = Directory.CreateDirectory(Path.Combine(local, file.Name));
                            DownloadDirectory(file.FullName, dir.FullName);
                        }
                    }
                }
            }
        }

        public void UploadFile(string remote, string local)
        {
            string path = remote + '/' + Path.GetFileName(local);
            if (port == 22)
            {
                using FileStream fs = File.Open(local, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                sftpClient.UploadFile(fs, path, true);
            }
            else
                ftpClient.UploadFile(local, path);
        }

        public void DeleteFile(string remote)
        {
            if (port == 22)
                sftpClient.DeleteFile(remote);
            else
                ftpClient.DeleteFile(remote);
        }

        public void DeleteDirectory(string remote)
        {
            if (port == 22)
            {
                foreach (SftpFile file in sftpClient.ListDirectory(remote))
                {
                    if ((file.Name != ".") && (file.Name != ".."))
                    {
                        if (file.IsDirectory)
                        {
                            DeleteDirectory(file.FullName);
                        }
                        else
                        {
                            sftpClient.DeleteFile(file.FullName);
                        }
                    }
                }

                sftpClient.DeleteDirectory(remote);
            }
            else
            {
                foreach (FtpListItem file in ftpClient.GetListing(remote))
                {
                    if ((file.Name != ".") && (file.Name != ".."))
                    {
                        if (file.Type == FtpFileSystemObjectType.Directory)
                        {
                            DeleteDirectory(file.FullName);
                        }
                        else
                        {
                            ftpClient.DeleteFile(file.FullName);
                        }
                    }
                }

                ftpClient.DeleteDirectory(remote);
            }
        }

        public void Connect()
        {
            if (port == 22)
                sftpClient.Connect();
            else
                ftpClient.Connect();
        }

        public void Disconnect()
        {
            if (port == 22)
                sftpClient.Disconnect();
            else
                ftpClient.Disconnect();
        }
    }
}