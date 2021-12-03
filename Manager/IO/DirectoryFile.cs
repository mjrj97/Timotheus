﻿using Renci.SshNet.Sftp;
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
        /// The size of the file in bytes
        /// </summary>
        public long Size;
        /// <summary>
        /// Variable that tells the software how to handle this file on sync.
        /// </summary>
        public FileHandle Handle;

        /// <summary>
        /// Connects the pairs.
        /// </summary>
        public DirectoryFile(FileSystemInfo LocalFile, SftpFile RemoteFile, DirectoryLogItem LogItem)
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

            Handle = FileHandle.Nothing;
            this.LogItem = LogItem;
            this.LocalFile = LocalFile;
            this.RemoteFile = RemoteFile;

            if (LocalFile != null && RemoteFile != null)
            {
                //If file can be found (!)previously & locally & remotely => Find the one with the lastest changes
                if (IsDirectory)
                    Handle = FileHandle.Synchronize;
                else
                {
                    //Synchronize
                    if (LocalFile.LastWriteTimeUtc.Ticks == LogItem.LocalTicks && RemoteFile.LastWriteTimeUtc.Ticks != LogItem.RemoteTicks)
                    {
                        Handle = FileHandle.Download;
                    }
                    else if (LocalFile.LastWriteTimeUtc.Ticks != LogItem.LocalTicks && RemoteFile.LastWriteTimeUtc.Ticks == LogItem.RemoteTicks)
                    {
                        Handle = FileHandle.Upload;
                    }
                    else if (LocalFile.LastWriteTimeUtc.Ticks != LogItem.LocalTicks && RemoteFile.LastWriteTimeUtc.Ticks != LogItem.RemoteTicks)
                    {
                        if (LocalFile.LastWriteTimeUtc.Ticks < RemoteFile.LastWriteTimeUtc.Ticks)
                        {
                            //Download
                            Handle = FileHandle.Download;
                        }
                        else if (LocalFile.LastWriteTimeUtc.Ticks > RemoteFile.LastWriteTimeUtc.Ticks)
                        {
                            //Upload
                            Handle = FileHandle.Upload;
                        }
                    }
                }
            }
            else if (LogItem.Equals(DirectoryLogItem.Empty))
            {
                if (LocalFile != null && RemoteFile == null)
                {
                    //If file can be found !previously & locally & !remotely => Upload
                    Handle = FileHandle.NewUpload;
                }
                else if (LocalFile == null && RemoteFile != null)
                {
                    //If file can be found !previously & !locally & remotely => Download
                    Handle = FileHandle.NewDownload;
                }
            }
            else
            {
                if (LocalFile != null && RemoteFile == null)
                {
                    //If file can be found previously & locally & !remotely => Delete local (If local & previously LastWriteTime is the same, otherwise upload)
                    if (LocalFile.LastWriteTimeUtc.Ticks == LogItem.LocalTicks)
                    {
                        Handle = FileHandle.DeleteLocal;
                    }
                    else
                    {
                        Handle = FileHandle.NewUpload;
                    }
                }
                else if (LocalFile == null && RemoteFile != null)
                {
                    //If file can be found previously & !locally & remotely => Delete remote (If remote & previously LastWriteTime is the same, otherwise download)
                    if (RemoteFile.LastWriteTimeUtc.Ticks == LogItem.RemoteTicks)
                    {
                        Handle = FileHandle.DeleteRemote;
                    }
                    else
                    {
                        Handle = FileHandle.NewDownload;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Enum that tells the software how to handle a file on sync.
    /// </summary>
    public enum FileHandle
    {
        Nothing,
        Synchronize,
        NewDownload,
        Download,
        NewUpload,
        Upload,
        DeleteLocal,
        DeleteRemote
    }
}