using System;
using Timotheus.IO;

namespace Timotheus.ViewModels
{
    public class FileViewModel : ViewModel
    {
        /// <summary>
        /// File which the view model uses.
        /// </summary>
        private readonly DirectoryFile file;

        /// <summary>
        /// Whether the file is an directory.
        /// </summary>
        public bool IsDirectory
        {
            get
            {
                return file.IsDirectory;
            }
        }

        /// <summary>
        /// Takes the Name and adds a D or F in front to sort by directories and files respectively.
        /// </summary>
        public string SortName
        {
            get
            {
                return (IsDirectory ? "D" : "F") + Name;
            }
        }

        /// <summary>
        /// Name of the file without directories.
        /// </summary>
        public string Name
        {
            get
            {
                return file.Name;
            }
        }

        /// <summary>
        /// The full name/address of the local file.
        /// </summary>
        public string LocalFullName
        {
            get
            {
                if (file.LocalFile != null)
                    return file.LocalFile.FullName;
                else
                    return string.Empty;
            }
        }

        /// <summary>
        /// The full name/address of the remote file.
        /// </summary>
        public string RemoteFullName
        {
            get
            {
                if (file.RemoteFile != null)
                    return file.RemoteFile.FullName;
                else
                    return string.Empty;
            }
        }

        /// <summary>
        /// The size of the file in bytes (ie. 12,2 KB)
        /// </summary>
        public string Size
        {
            get
            {
                if (!IsDirectory)
                    return FormatSize(file.Size);
                else
                    return string.Empty;
            }
        }

        /// <summary>
        /// Variable that tells the software how to handle this file on sync.
        /// </summary>
        public FileHandle Handle
        {
            get
            {
                return file.Handle;
            }
        }

        /// <summary>
        /// Used in the UI to tell the user what the program will do with the file.
        /// </summary>
        public string HandleText
        {
            get
            {
                return file.Handle switch
                {
                    FileHandle.NewDownload => Localization.Localization.SFTP_NewRemote,
                    FileHandle.Download => Localization.Localization.SFTP_ChangedRemote,
                    FileHandle.NewUpload => Localization.Localization.SFTP_NewLocal,
                    FileHandle.Upload => Localization.Localization.SFTP_ChangedLocal,
                    FileHandle.DeleteLocal => Localization.Localization.SFTP_DeleteRemote,
                    FileHandle.DeleteRemote => Localization.Localization.SFTP_DeleteLocal,
                    _ => string.Empty,
                };
            }
        }

        /// <summary>
        /// Suffixes in file sizing.
        /// </summary>
        static readonly string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

        /// <summary>
        /// Converts a SftpFile into the FileViewModel.
        /// </summary>
        public FileViewModel(DirectoryFile file)
        {
            this.file = file;
        }

        // At https://stackoverflow.com/questions/14488796/does-net-provide-an-easy-way-convert-bytes-to-kb-mb-gb-etc
        private static string FormatSize(long value, int decimalPlaces = 1)
        {
            if (value < 0) { return "-" + FormatSize(-value, decimalPlaces); }

            int i = 0;
            decimal dValue = value;
            while (Math.Round(dValue, decimalPlaces) >= 1000)
            {
                dValue /= 1024;
                i++;
            }

            return string.Format("{0:n" + decimalPlaces + "} {1}", dValue, SizeSuffixes[i]);
        }
    }
}