using Renci.SshNet.Sftp;
using System.IO;

namespace Timotheus.IO
{
    public struct DirectoryFile
    {
        public readonly FileSystemInfo localFile;
        public readonly SftpFile remoteFile;
        public readonly bool WasInLastSync;
        public readonly bool IsDirectory;
        public readonly string Name;

        public DirectoryFile(FileSystemInfo localFile, SftpFile remoteFile, bool WasInLastSync = false)
        {
            if (localFile == null)
            {
                Name = remoteFile.Name;
                IsDirectory = remoteFile.IsDirectory;
            }
            else
            {
                Name = localFile.Name;
                FileAttributes attr = File.GetAttributes(localFile.FullName);
                IsDirectory = attr.HasFlag(FileAttributes.Directory);
            }

            this.WasInLastSync = WasInLastSync;
            this.localFile = localFile;
            this.remoteFile = remoteFile;
        }
    }
}