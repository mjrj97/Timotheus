using System.IO;
using System.Collections.Generic;

namespace Timotheus.IO
{
    public class DirectoryLog
    {
        public List<DirectoryLogItem> List { get; protected set; }
        private readonly string path;

        public DirectoryLog(string path)
        {
            this.path = Path.Combine(path, ".tfilelog");
            Load();
        }

        public void Add(DirectoryLogItem dli)
        {
            List.Add(dli);
        }

        public void Remove(DirectoryLogItem dli)
        {
            List.Remove(dli);
        }

        private void Load()
        {
            Secure();
            using StreamReader reader = new(path);
            List = new();

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                List.Add(new DirectoryLogItem(line));
            }
        }

        private void Secure()
        {
            if (!File.Exists(path))
            {
                FileStream stream = File.Create(path);
                File.SetAttributes(path, FileAttributes.Hidden);
                stream.Close();
            }
        }

        public void Save()
        {
            Secure();
            using (FileStream fs = new(path, FileMode.Open))
            {
                using (TextWriter tw = new StreamWriter(fs, Timotheus.Encoding, -1, true))
                {
                    for (int i = 0; i < List.Count; i++)
                    {
                        tw.WriteLine(List[i]);
                    }
                }
                fs.SetLength(fs.Position);
            }
        }
    }
}