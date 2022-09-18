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
                    return ftpFile.Type == FtpObjectType.Directory;
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
                    return ftpFile.Type == FtpObjectType.Link;
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

        /// <summary>
        /// Returns the permissions of the file.
        /// </summary>
        public short Permissions
        {
            get
            {
                short sum = 0;

                if (sftpFile != null)
                {
                    sum += (short)(sftpFile.OthersCanExecute ? 1 : 0);
                    sum += (short)(sftpFile.OthersCanWrite ? 2 : 0);
                    sum += (short)(sftpFile.OthersCanRead ? 4 : 0);

                    sum += (short)(sftpFile.GroupCanExecute ? 10 : 0);
                    sum += (short)(sftpFile.GroupCanWrite ? 20 : 0);
                    sum += (short)(sftpFile.GroupCanRead ? 40 : 0);

                    sum += (short)(sftpFile.OwnerCanExecute ? 100 : 0);
                    sum += (short)(sftpFile.OwnerCanWrite ? 200 : 0);
                    sum += (short)(sftpFile.OwnerCanRead ? 400 : 0);
                }
                else
                {
                    sum += (short)((int)ftpFile.OthersPermissions * 1);
                    sum += (short)((int)ftpFile.GroupPermissions * 10);
                    sum += (short)((int)ftpFile.OwnerPermissions * 100);
                }
                
                return sum;
            }
        }
    }
}