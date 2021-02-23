using Renci.SshNet;
using Renci.SshNet.Sftp;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Timotheus.Utility
{
    public class SFTP
    {
        private readonly SftpClient sftp;

        public SFTP(string host, string username, string password)
        {
            sftp = new SftpClient(host, username, password);
        }

        public List<SftpFile> GetListOfFiles(string remoteDirectory)
        {
            List<SftpFile> files = new List<SftpFile>();
            
            try
            {
                sftp.Connect();
                files = sftp.ListDirectory(remoteDirectory).ToList();
                sftp.Disconnect();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            for (int i = 0; i < files.Count; i++)
            {
                System.Diagnostics.Debug.WriteLine(files[i].FullName);
            }

            return files;
        }

        public void DownloadFile(string remotePath, string localPath)
        {
            try
            {
                sftp.Connect();

                using (Stream fileStream = File.OpenWrite(localPath))
                {
                    sftp.DownloadFile(remotePath, fileStream);
                }

                sftp.Disconnect();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        public void UploadFile(string remotePath, string localPath)
        {
            try
            {
                sftp.Connect();

                using (Stream fileStream = File.OpenWrite(localPath))
                {
                    sftp.UploadFile(fileStream, remotePath);
                }

                sftp.Disconnect();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        public void DeleteFile(string remotePath)
        {
            try
            {
                sftp.Connect();

                sftp.DeleteFile(remotePath);

                sftp.Disconnect();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
    }
}
