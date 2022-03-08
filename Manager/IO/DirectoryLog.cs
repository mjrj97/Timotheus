using System.IO;
using System.Collections.Generic;

namespace Timotheus.IO
{
    /// <summary>
    /// Class that handles the loading of directory log files.
    /// </summary>
    public static class DirectoryLog
    {
        /// <summary>
        /// Loads a log file from a directory. Creates the log if not present.
        /// </summary>
        /// <param name="path">Directory path</param>
        public static List<DirectoryLogItem> Load(string path)
        {
            if (!Directory.Exists(path))
                return new List<DirectoryLogItem>();
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

        /// <summary>
        /// Saves a log file to a directory. Creates the log if not present.
        /// </summary>
        /// <param name="path">Directory path</param>
        /// <param name="remoteFiles">The files on the remote</param>
        public static void Save(string path, List<RemoteFile> remoteFiles)
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

        /// <summary>
        /// Checks if the log file exists and makes it hidden.
        /// </summary>
        /// <param name="path"></param>
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