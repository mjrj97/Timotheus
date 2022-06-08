using Renci.SshNet;
using FluentFTP;
using System.IO;
using Renci.SshNet.Sftp;
using System.Collections.Generic;
using System.Linq;
using Renci.SshNet.Common;

namespace Timotheus.IO
{
    /// <summary>
    /// Wrapper class around a Ftp and Sftp client.
    /// </summary>
    public class DirectoryClient
    {
        private readonly SftpClient sftpClient;
        private readonly FtpClient ftpClient;

        /// <summary>
        /// The port used by the client. 22 = Sftp and 21 = Ftp.
        /// </summary>
        private readonly int port;

        /// <summary>
        /// Is the client connected to the remote directory.
        /// </summary>
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

        /// <summary>
        /// Creates an instace of a DirectoryClient. The port determines whether (22) SFTP or (21) FTP is used.
        /// </summary>
        /// <param name="host">Host/URL to the remote server.</param>
        /// <param name="port">Port of the server.</param>
        /// <param name="username">Username used to connect to the server.</param>
        /// <param name="password">Password of the user.</param>
        public DirectoryClient(string host, int port, string username, string password)
        {
            this.port = port;
            if (port == 22)
            {
                PasswordAuthenticationMethod auth = new(username, password);
                ConnectionInfo connectionInfo = new(host, port, username, auth)
                {
                    Encoding = System.Text.Encoding.UTF8
                };
                sftpClient = new SftpClient(connectionInfo);
            }
            else
                ftpClient = new FtpClient(host, port, username, password);
        }

        /// <summary>
        /// Returns a list of files in the remote directory.
        /// </summary>
        /// <param name="remote">Path to the remote directory.</param>
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

        /// <summary>
        /// Creates a directory on the remote directory at the given path.
        /// </summary>
        /// <param name="remote">Path of the new directory.</param>
        public void CreateDirectory(string remote)
        {
            if (port == 22)
                sftpClient.CreateDirectory(remote);
            else
                ftpClient.CreateDirectory(remote);
        }

        /// <summary>
        /// Checks whether a file/directory exists on the remote directory.
        /// </summary>
        /// <param name="remote">Path to the file or directory.</param>
        /// <param name="directory">Is this a directory?</param>
        public bool Exists(string remote, bool directory)
        {
            bool exists;

            bool isPreconnected = IsConnected;
            if (!isPreconnected)
            {
                Connect();
            }

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

            if (!isPreconnected)
            {
                Disconnect();
            }

            return exists;
        }

        /// <summary>
        /// Downloads a file from the remote directory to the local directory.
        /// </summary>
        /// <param name="remote">Path of the file on the remote directory.</param>
        /// <param name="local">Path of the file on the local machine.</param>
        public void DownloadFile(string remote, string local)
        {
            bool isPreconnected = IsConnected;
            if (!isPreconnected)
            {
                Connect();
            }

            if (port == 22)
            {
                using Stream fileStream = File.OpenWrite(local);
                sftpClient.DownloadFile(remote, fileStream);
            }
            else
                ftpClient.DownloadFile(local, remote);

            if (!isPreconnected)
            {
                Disconnect();
            }
        }

        /// <summary>
        /// Uploads a file from the local directory to the remote directory
        /// </summary>
        /// <param name="remote">Path of the file on the remote directory.</param>
        /// <param name="local">Path of the file on the local machine.</param>
        public void UploadFile(string remote, string local)
        {
            bool isPreconnected = IsConnected;
            if (!isPreconnected)
            {
                Connect();
            }

            try
            {
                using FileStream fs = File.Open(local, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                if (port == 22)
                    sftpClient.UploadFile(fs, remote, true);
                else
                    ftpClient.Upload(fs, remote);
            }
            catch (IOException ex) 
            {
                Timotheus.Log(ex);
                string tempFile = Path.GetTempFileName();
                File.Copy(local, tempFile, true);
                using (FileStream fs = File.Open(tempFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    if (port == 22)
                        sftpClient.UploadFile(fs, remote, true);
                    else
                        ftpClient.Upload(fs, remote);
                }
                File.Delete(tempFile);
            }

            if (!isPreconnected)
            {
                Disconnect();
            }
        }

        /// <summary>
        /// Deletes file on the remote directory.
        /// </summary>
        /// <param name="remote">Path of the file on the server.</param>
        public void DeleteFile(string remote)
        {
            bool isPreconnected = IsConnected;
            if (!isPreconnected)
            {
                Connect();
            }

            try
            {
                if (port == 22)
                    sftpClient.DeleteFile(remote);
                else
                    ftpClient.DeleteFile(remote);
            }
            catch (SftpPathNotFoundException ex)
            {
                Timotheus.Log(ex);
                //Do something when file is not found
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            if (!isPreconnected)
            {
                Disconnect();
            }
        }

        /// <summary>
        /// Deletes directory on the remote directory.
        /// </summary>
        /// <param name="remote">Path of the directory on the server.</param>
        public void DeleteDirectory(string remote)
        {
            bool isPreconnected = IsConnected;
            if (!isPreconnected)
            {
                Connect();
            }

            try
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
            catch (SftpPathNotFoundException ex)
            {
                Timotheus.Log(ex);
                //Do something when file is not found
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            if (!isPreconnected)
            {
                Disconnect();
            }
        }

        /// <summary>
        /// Sets the permissions of the file/directory with given path.
        /// </summary>
        public void SetPermissions(string path, short permissions)
        {
            bool isPreconnected = IsConnected;
            if (!isPreconnected)
            {
                Connect();
            }

            if (port == 22) //SFTP
            {
                SftpFileAttributes attributes = sftpClient.GetAttributes(path);
                attributes.SetPermissions(permissions);
                sftpClient.SetAttributes(path, attributes);
                System.Diagnostics.Debug.WriteLine(path + ": " + permissions);
            }
            else
            {
                ftpClient.SetFilePermissions(path, permissions);
            }

            if (!isPreconnected)
            {
                Disconnect();
            }
        }

        /// <summary>
        /// Establish connection to the remote directory.
        /// </summary>
        public void Connect()
        {
            if (port == 22)
                sftpClient.Connect();
            else
                ftpClient.Connect();
        }

        /// <summary>
        /// Disconnect from the remote directory.
        /// </summary>
        public void Disconnect()
        {
            if (port == 22)
                sftpClient.Disconnect();
            else
                ftpClient.Disconnect();
        }
    }
}