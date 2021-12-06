namespace Timotheus.IO
{
    /// <summary>
    /// A struct that logs a given file after a sync.
    /// </summary>
    public struct DirectoryLogItem
    {
        /// <summary>
        /// Whether the file is a directory.
        /// </summary>
        public bool IsDirectory;
        /// <summary>
        /// The name of the file. Note it is NOT the fullName.
        /// </summary>
        public string Name;
        /// <summary>
        /// The LastWriteTimeUtc ticks on last sync for the local file.
        /// </summary>
        public long LocalTicks;
        /// <summary>
        /// The LastWriteTimeUtc ticks on last sync for the remote file.
        /// </summary>
        public long RemoteTicks;

        /// <summary>
        /// Analogous to string.Empty and a struct equivalent to null.
        /// </summary>
        public readonly static DirectoryLogItem Empty = new();

        /// <summary>
        /// Create a DLI from a line in a csv file.
        /// </summary>
        public DirectoryLogItem(string line)
        {
            string[] data = line.Split(';');
            IsDirectory = data[0] == "D";
            Name = data[1];
            LocalTicks = long.Parse(data[2]);
            RemoteTicks = long.Parse(data[3]);
        }

        /// <summary>
        /// Create a DLI with given values.
        /// </summary>
        public DirectoryLogItem(bool IsDirectory, string Name, long LocalTicks, long RemoteTicks)
        {
            this.IsDirectory = IsDirectory;
            this.Name = Name;
            this.LocalTicks = LocalTicks;
            this.RemoteTicks = RemoteTicks;
        }

        /// <summary>
        /// Returns the DLI as a string compatible with the constructor (Used for csv files).
        /// </summary>
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