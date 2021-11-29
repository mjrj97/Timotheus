using System;

namespace Timotheus.IO
{
    public struct DirectoryLogItem
    {
        public bool IsDirectory;
        public string Name;
        public DateTime LastWriteTimeUtc;

        public readonly static DirectoryLogItem Empty = new();

        public DirectoryLogItem(string line)
        {
            string[] data = line.Split(';');
            IsDirectory = (data[0] == "D");
            Name = data[1];
            LastWriteTimeUtc = DateTime.Parse(data[2]);
        }

        public DirectoryLogItem(bool IsDirectory, string Name, DateTime LastWriteTimeUtc)
        {
            this.IsDirectory = IsDirectory;
            this.Name = Name;
            this.LastWriteTimeUtc = LastWriteTimeUtc;
        }

        public override string ToString()
        {
            string text = string.Empty;

            text += IsDirectory ? "D" : "F";
            text += ";" + Name + ";";
            text += LastWriteTimeUtc.ToString();

            return text;
        }
    }
}