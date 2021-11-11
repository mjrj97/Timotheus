namespace Timotheus.IO
{
    /// <summary>
    /// A key is named and has a corresponding value. Can be used as holding a password ie. Password,12345 or localization etc.
    /// </summary>
    public class Key
    {
        /// <summary>
        /// Name of the key, could be "Password"
        /// </summary>
        public readonly string name;
        
        /// <summary>
        /// Value of the key, could be a password.
        /// </summary>
        public string value;

        /// <summary>
        /// Constructor to create a key with a name and value.
        /// </summary>
        /// <param name="name">Designates the name of the key. Is used when trying to get the value.</param>
        /// <param name="value">The value assigned to this key.</param>
        public Key(string name, string value)
        {
            this.name = name;
            this.value = value;
        }

        /// <summary>
        /// Constructor that creates a key from a string with a separator char (ie. NAME,VALUE)
        /// </summary>
        /// <param name="line">Line that contains of a name, separator and value.</param>
        /// <param name="separator">The character used to separate the name and value.</param>
        public Key(string line, char separator)
        {
            int i = 0;
            while (line[i] != separator && i < line.Length)
            {
                i++;
            }
            name = line.Substring(0, i);
            value = line.Substring(i + 1, line.Length - i - 1);
        }

        /// <summary>
        /// Returns the name from a line with format NAME,VALUE where ',' is the separator.
        /// </summary>
        /// <param name="line">Line that contains of a name, separator and value.</param>
        /// <param name="separator">The character used to separate the name and value.</param>
        public static string Name(string line, char separator)
        {
            int i = 0;
            while (line[i] != separator && i < line.Length)
            {
                i++;
            }
            return line.Substring(0, i);
        }

        /// <summary>
        /// Returns the value from a line with format NAME,VALUE where ',' is the separator.
        /// </summary>
        /// <param name="line">Line that contains of a name, separator and value.</param>
        /// <param name="separator">The character used to separate the name and value.</param>
        public static string Value(string line, char separator)
        {
            int i = 0;
            while (line[i] != separator && i < line.Length)
            {
                i++;
            }
            return line.Substring(i + 1, line.Length - i - 1);
        }

        /// <summary>
        /// Returns the key in the format name:value.
        /// </summary>
        public override string ToString()
        {
            return name + ":" + value;
        }
    }
}