using System;

namespace Timotheus.IO
{
    public struct DirectoryLogItem
    {
        public bool IsDirectory;
        public string Name;
        public DateTime LastWriteTime;
        public long Length;

        public DirectoryLogItem(string line)
        {
            string[] data = line.Split(';');
            IsDirectory = (data[0] == "D");
            Name = data[1];
            LastWriteTime = DateTime.Parse(data[2]);
            Length = long.Parse(data[3]);
        }

        public DirectoryLogItem(bool IsDirectory, string Name, DateTime LastWriteTime, long Length)
        {
            this.IsDirectory = IsDirectory;
            this.Name = Name;
            this.LastWriteTime = LastWriteTime;
            this.Length = Length;
        }

        public override string ToString()
        {
            string text = string.Empty;

            text += IsDirectory ? "D" : "F";
            text += ";" + Name + ";";
            text += LastWriteTime.ToString();
            text += ";" + Length;

            return text;
        }
    }
}