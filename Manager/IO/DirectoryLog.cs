﻿using System.IO;
using System.Collections.Generic;
using Renci.SshNet.Sftp;

namespace Timotheus.IO
{
    public class DirectoryLog
    {
        public static List<DirectoryLogItem> Load(string path)
        {
            path = Path.Combine(path, ".tfilelog");
            Secure(path);
            using StreamReader reader = new(path);
            List<DirectoryLogItem> List = new();

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                List.Add(new DirectoryLogItem(line));
            }

            return List;
        }

        public static void Save(string path, List<SftpFile> remoteFiles)
        {
            List<DirectoryLogItem> logItems = new();
            DirectoryInfo dirInfo = new(path);
            FileSystemInfo[] localFiles = dirInfo.GetFileSystemInfos("*", SearchOption.TopDirectoryOnly);
            for (int i = 0; i < localFiles.Length; i++)
            {
                bool IsDirectory = File.GetAttributes(localFiles[i].FullName).HasFlag(FileAttributes.Directory);

                int j = 0;
                bool found = false;
                while (!found && j < remoteFiles.Count)
                {
                    if (remoteFiles[j].Name == localFiles[i].Name && remoteFiles[j].IsDirectory == IsDirectory)
                    {
                        found = true;
                    }
                    else
                        j++;
                }

                logItems.Add(new DirectoryLogItem(IsDirectory, localFiles[i].Name, localFiles[i].LastWriteTimeUtc.Ticks, found ? remoteFiles[j].LastWriteTimeUtc.Ticks : 0));
            }

            path = Path.Combine(path, ".tfilelog");
            Secure(path);
            using FileStream fs = new(path, FileMode.Open);
            using (TextWriter tw = new StreamWriter(fs, Timotheus.Encoding, -1, true))
            {
                for (int i = 0; i < logItems.Count; i++)
                {
                    if (logItems[i].Name[0] != '.' && Path.GetExtension(localFiles[i].Name) != ".tkey")
                        tw.WriteLine(logItems[i]);
                }
            }
            fs.SetLength(fs.Position);
        }

        private static void Secure(string path)
        {
            if (!File.Exists(path))
            {
                FileStream stream = File.Create(path);
                File.SetAttributes(path, FileAttributes.Hidden);
                stream.Close();
            }
        }
    }
}