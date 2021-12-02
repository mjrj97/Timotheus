using Renci.SshNet.Sftp;
using System.IO;

namespace Timotheus.IO
{
    /// <summary>
    /// Struct that symbolizes a file that is on a local and remote directory.
    /// </summary>
    public struct DirectoryFile
    {
        /// <summary>
        /// The local file
        /// </summary>
        public FileSystemInfo LocalFile;
        /// <summary>
        /// The remote file
        /// </summary>
        public SftpFile RemoteFile;
        /// <summary>
        /// Item that contains whether the file was in the last sync, and related info.
        /// </summary>
        public DirectoryLogItem LogItem;
        /// <summary>
        /// Whether the File is a Directory.
        /// </summary>
        public bool IsDirectory;
        /// <summary>
        /// The name of the file. Note it is not the fullName.
        /// </summary>
        public string Name;

        /// <summary>
        /// Connects the pairs.
        /// </summary>
        public DirectoryFile(FileSystemInfo LocalFile, SftpFile RemoteFile, DirectoryLogItem LogItem)
        {
            if (LocalFile == null)
            {
                Name = RemoteFile.Name;
                IsDirectory = RemoteFile.IsDirectory;
            }
            else
            {
                Name = LocalFile.Name;
                FileAttributes attr = File.GetAttributes(LocalFile.FullName);
                IsDirectory = attr.HasFlag(FileAttributes.Directory);
            }

            this.LogItem = LogItem;
            this.LocalFile = LocalFile;
            this.RemoteFile = RemoteFile;
        }
    }
}