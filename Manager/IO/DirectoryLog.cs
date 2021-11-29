using System.IO;
using System.Collections.Generic;

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

        public static void Save(string path)
        {
            List<DirectoryLogItem> logItems = new();
            DirectoryInfo dirInfo = new(path);
            FileSystemInfo[] localFiles = dirInfo.GetFileSystemInfos("*", SearchOption.TopDirectoryOnly);
            for (int i = 0; i < localFiles.Length; i++)
            {
                bool IsDirectory = File.GetAttributes(localFiles[i].FullName).HasFlag(FileAttributes.Directory);
                logItems.Add(new DirectoryLogItem(IsDirectory, localFiles[i].Name, localFiles[i].LastWriteTimeUtc));
            }

            path = Path.Combine(path, ".tfilelog");
            Secure(path);
            using FileStream fs = new(path, FileMode.Open);
            using (TextWriter tw = new StreamWriter(fs, Timotheus.Encoding, -1, true))
            {
                for (int i = 0; i < logItems.Count; i++)
                {
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