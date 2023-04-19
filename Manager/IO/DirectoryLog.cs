using System.IO;
using System.Collections.Generic;
using Timotheus.ViewModels;

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
			List<DirectoryLogItem> List = new();

			if (Directory.Exists(path))
			{
				path = Path.Combine(path, ".tfilelog");

				if (File.Exists(path))
				{
					using StreamReader reader = new(path);

					string line;
					while ((line = reader.ReadLine()) != null)
					{
						if (line.Contains(';'))
						{
							List.Add(new DirectoryLogItem(line));
						}
					}
				}
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

			if (!File.Exists(path))
			{
				FileStream stream = File.Create(path);
				File.SetAttributes(path, FileAttributes.Hidden);
				stream.Close();
			}
			else
				File.SetAttributes(path, FileAttributes.Hidden);

			using FileStream fileStream = new(path, FileMode.Open);
            using (TextWriter textWriter = new StreamWriter(fileStream, Timotheus.Encoding, -1, true))
            {
                textWriter.WriteLine(path);
                for (int i = 0; i < logItems.Count; i++)
                {
                    if (!DirectoryViewModel.Ignore(logItems[i].Name))
                        textWriter.WriteLine(logItems[i]);
                }
            }
            fileStream.SetLength(fileStream.Position);
        }
    }
}