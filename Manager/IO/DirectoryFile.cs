using System.IO;
using Timotheus.Utility;

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
        public RemoteFile RemoteFile;
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
        /// The size of the file in bytes
        /// </summary>
        public long Size;
        /// <summary>
        /// Variable that tells the software how to handle this file on sync.
        /// </summary>
        public SyncHandle Handle;
        /// <summary>
        /// Returns the value of the files permission.
        /// </summary>
        public short Permissions;

        /// <summary>
        /// Connects the pairs.
        /// </summary>
        public DirectoryFile(FileSystemInfo LocalFile, RemoteFile RemoteFile, DirectoryLogItem LogItem)
        {
            Size = 0;
            if (LocalFile == null)
            {
                Name = RemoteFile.Name;
                Size = RemoteFile.Length;
                IsDirectory = RemoteFile.IsDirectory;
            }
            else
            {
                Name = LocalFile.Name;
                FileAttributes attr = File.GetAttributes(LocalFile.FullName);
                IsDirectory = attr.HasFlag(FileAttributes.Directory);
                if (!IsDirectory)
                    Size = new FileInfo(LocalFile.FullName).Length;
            }

            Handle = SyncHandle.Nothing;
            this.LogItem = LogItem;
            this.LocalFile = LocalFile;
            this.RemoteFile = RemoteFile;

            if (LocalFile != null && RemoteFile != null)
            {
                //If file can be found (!)previously & locally & remotely => Find the one with the lastest changes
                if (IsDirectory)
                    Handle = SyncHandle.Synchronize;
                else
                {
                    //Synchronize
                    if (LocalFile.LastWriteTimeUtc.Ticks == LogItem.LocalTicks && RemoteFile.LastWriteTimeUtc.Ticks != LogItem.RemoteTicks)
                    {
                        Handle = SyncHandle.Download;
                    }
                    else if (LocalFile.LastWriteTimeUtc.Ticks != LogItem.LocalTicks && RemoteFile.LastWriteTimeUtc.Ticks == LogItem.RemoteTicks)
                    {
                        Handle = SyncHandle.Upload;
                    }
                    else if (LocalFile.LastWriteTimeUtc.Ticks != LogItem.LocalTicks && RemoteFile.LastWriteTimeUtc.Ticks != LogItem.RemoteTicks)
                    {
                        if (LocalFile.LastWriteTimeUtc.Ticks < RemoteFile.LastWriteTimeUtc.Ticks)
                        {
                            //Download
                            Handle = SyncHandle.Download;
                        }
                        else if (LocalFile.LastWriteTimeUtc.Ticks > RemoteFile.LastWriteTimeUtc.Ticks)
                        {
                            //Upload
                            Handle = SyncHandle.Upload;
                        }
                    }
                }
            }
            else if (LogItem.Equals(DirectoryLogItem.Empty))
            {
                if (LocalFile != null && RemoteFile == null)
                {
                    //If file can be found !previously & locally & !remotely => Upload
                    Handle = SyncHandle.NewUpload;
                }
                else if (LocalFile == null && RemoteFile != null)
                {
                    //If file can be found !previously & !locally & remotely => Download
                    Handle = SyncHandle.NewDownload;
                }
            }
            else
            {
                if (LocalFile != null && RemoteFile == null)
                {
                    //If file can be found previously & locally & !remotely => Delete local (If local & previously LastWriteTime is the same, otherwise upload)
                    if (LocalFile.LastWriteTimeUtc.Ticks == LogItem.LocalTicks && LogItem.RemoteTicks != 0)
                    {
                        Handle = SyncHandle.DeleteLocal;
                    }
                    else
                    {
                        Handle = SyncHandle.NewUpload;
                    }
                }
                else if (LocalFile == null && RemoteFile != null)
                {
                    //If file can be found previously & !locally & remotely => Delete remote (If remote & previously LastWriteTime is the same, otherwise download)
                    if (RemoteFile.LastWriteTimeUtc.Ticks == LogItem.RemoteTicks)
                    {
                        Handle = SyncHandle.DeleteRemote;
                    }
                    else
                    {
                        Handle = SyncHandle.NewDownload;
                    }
                }
            }

            if (RemoteFile != null)
                Permissions = RemoteFile.Permissions;
            else
                Permissions = 0;
        }
    }
}