namespace Timotheus.IO
{
    public struct DirectoryLogItem
    {
        public bool IsDirectory;
        public string Name;
        public long LocalTicks;
        public long RemoteTicks;

        public readonly static DirectoryLogItem Empty = new();

        public DirectoryLogItem(string line)
        {
            string[] data = line.Split(';');
            IsDirectory = data[0] == "D";
            Name = data[1];
            LocalTicks = long.Parse(data[2]);
            RemoteTicks = long.Parse(data[3]);
        }

        public DirectoryLogItem(bool IsDirectory, string Name, long LocalTicks, long RemoteTicks)
        {
            this.IsDirectory = IsDirectory;
            this.Name = Name;
            this.LocalTicks = LocalTicks;
            this.RemoteTicks = RemoteTicks;
        }

        public override string ToString()
        {
            string text = string.Empty;

            text += IsDirectory ? "D" : "F";
            text += ";" + Name + ";";
            text += LocalTicks.ToString();
            text += ";" + RemoteTicks.ToString();

            return text;
        }
    }
}