using Renci.SshNet.Sftp;
using FluentFTP;
using System;

namespace Timotheus.IO
{
    /// <summary>
    /// Wrapper class around a SftpFile or FtpListItem.
    /// </summary>
    public class RemoteFile
    {
        private readonly SftpFile sftpFile;
        private readonly FtpListItem ftpFile;

        /// <summary>
        /// Whether the file is a directory.
        /// </summary>
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

        /// <summary>
        /// Whether the file is a link.
        /// </summary>
        public bool IsSymbolicLink
        {
            get
            {
                if (sftpFile != null)
                    return sftpFile.IsSymbolicLink;
                else
                    return ftpFile.Type == FtpFileSystemObjectType.Link;
            }
        }

        /// <summary>
        /// Full name of the file. Includes the path and extension to the file.
        /// </summary>
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

        /// <summary>
        /// Name of the files without path. Inclues the extension.
        /// </summary>
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

        /// <summary>
        /// Size of the file on the remote server.
        /// </summary>
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

        /// <summary>
        /// Last time the file was written to on the remote server.
        /// </summary>
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

        /// <summary>
        /// Creates a instance of RemoteFile using a SftpFile.
        /// </summary>
        public RemoteFile(SftpFile file)
        {
            sftpFile = file;
        }

        /// <summary>
        /// Creates a instance of RemoteFile using a FtpListItem.
        /// </summary>
        public RemoteFile(FtpListItem file)
        {
            ftpFile = file;
        }
    }
}