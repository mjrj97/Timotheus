using Renci.SshNet.Sftp;
using FluentFTP;
using System;

namespace Timotheus.IO
{
    public class RemoteFile
    {
        private readonly SftpFile sftpFile;
        private readonly FtpListItem ftpFile;

        public bool IsDirectory
        {
            get
            {
                if (sftpFile != null)
                    return sftpFile.IsDirectory;
                else
                    return ftpFile.Type == FtpFileSystemObjectType.Directory;
            }
        }

        public string FullName
        {
            get
            {
                if (sftpFile != null)
                    return sftpFile.FullName;
                else
                    return ftpFile.FullName;
            }
        }

        public string Name
        {
            get
            {
                if (sftpFile != null)
                    return sftpFile.Name;
                else
                    return ftpFile.Name;
            }
        }

        public long Length
        {
            get
            {
                if (sftpFile != null)
                    return sftpFile.Length;
                else
                    return ftpFile.Size;
            }
        }

        public DateTime LastWriteTimeUtc
        {
            get
            {
                if (sftpFile != null)
                    return sftpFile.LastWriteTimeUtc;
                else
                    return ftpFile.RawModified;
            }
        }

        public RemoteFile(SftpFile file)
        {
            sftpFile = file;
        }

        public RemoteFile(FtpListItem file)
        {
            ftpFile = file;
        }
    }
}