using Renci.SshNet.Sftp;
using System.IO;

namespace Timotheus.IO
{
    public struct DirectoryFile
    {
        public FileSystemInfo LocalFile;
        public SftpFile RemoteFile;
        public DirectoryLogItem LogItem;
        public bool IsDirectory;
        public string Name;

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