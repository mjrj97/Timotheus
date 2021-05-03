using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using Timotheus.Cryptography;

namespace Timotheus.Utility
{
    /// <summary>
    /// Class that can load a list of value with corresponding names. Can be loaded from encrypted (using a password) and unencrypted files. The file's line separator can be specified (ie. NAME,VALUE where ',' is used).
    /// </summary>
    public class Register
    {
        /// <summary>
        /// List of keys with a given name and value.
        /// </summary>
        private readonly Key[] keys;

        /// <summary>
        /// Character that is used to separate a keys name and value in the file.
        /// </summary>
        private readonly char separator = ',';

        /// <summary>
        /// Constructor. Loads an encrypted file of keys using the given password.
        /// </summary>
        /// <param name="path">Path to the file.</param>
        /// <param name="password">Password to decrypt the file.</param>
        public Register(string path, string password)
        {
            if (!File.Exists(path))
                throw new System.Exception("Exception_LoadFailed");

            byte[] data = Cipher.Decrypt(File.ReadAllBytes(path), password);
            string text = System.Text.Encoding.UTF8.GetString(data);
            keys = Load(text);
        }

        /// <summary>
        /// Constructor. Loads an encrypted file of keys using the given password, with a given separator character.
        /// </summary>
        /// <param name="path">Path to the file.</param>
        /// <param name="password">Password to decrypt the file.</param>
        /// <param name="separator">Define the character used to separate the name and value of a key.</param>
        public Register(string path, string password, char separator) : this(path, password)
        {
            this.separator = separator;
        }

        /// <summary>
        /// Constructor. Loads an unencrypted file of keys.
        /// </summary>
        /// <param name="path">Path to the file.</param>
        public Register(string path)
        {
            if (!File.Exists(path))
                throw new System.Exception("Exception_LoadFailed");

            string text = File.ReadAllText(path);
            keys = Load(text);
            System.Diagnostics.Debug.WriteLine(ToString());
        }

        /// <summary>
        /// Constructor. Loads an unencrypted file of keys, with a given separator character.
        /// </summary>
        /// <param name="path">Path to the file.</param>
        /// <param name="separator">Define the character used to separate the name and value of a key.</param>
        public Register(string path, char separator) : this(path)
        {
            this.separator = separator;
        }

        /// <summary>
        /// Gets all keys found in the given text.
        /// </summary>
        /// <param name="text">Unencrypted text with keys.</param>
        /// <returns></returns>
        private Key[] Load(string text)
        {
            string[] lines = text.Split('\n');
            Key[] keys = new Key[lines.Length];

            string name;
            string value; 

            for (int i = 0; i < lines.Length; i++)
            {
                int j = 0;
                while (lines[i][j] != separator && j < lines[i].Length)
                {
                    j++;
                }
                name = lines[i].Substring(0, j);
                value = lines[i].Substring(j + 1, lines[i].Length - j - 1).Trim();

                keys[i] = new Key(name, value);
            }

            return keys;
        }

        /// <summary>
        /// Returns the keys in the register in a random sequence, with each line formatted as NAME,VALUE where ',' is the separator.
        /// </summary>
        public override string ToString()
        {
            List<Key> list = new List<Key>();
            for (int i = 0; i < keys.Length; i++)
            {
                list.Add(keys[i]);
            }
            Shuffle(list);
            string formatted = "";
            for (int i = 0; i < list.Count; i++)
            {
                formatted += list[i].name + separator + list[i].value;
                if (i != list.Count - 1)
                    formatted += "\n";
            }
            return formatted;
        }

        /// <summary>
        /// Shuffles a list. Note it changes the lists sequence.
        /// </summary>
        /// <param name="list">The list to be shuffled.</param>
        private static void Shuffle<T>(List<T> list)
        {
            System.Random rng = new System.Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        /// <summary>
        /// Returns the value of the key with a given name.
        /// </summary>
        /// <param name="name">Name of the key.</param>
        public string Get(string name)
        {
            string value = string.Empty;
            int i = 0;
            bool found = false;

            while (i < keys.Length && !found)
            {
                if (keys[i].name == name)
                {
                    value = keys[i].value;
                    found = true;
                }
                i++;
            }
            return value;
        }
        /// <summary>
        /// Returns the value withc corresponding the name, but a standard value can be provided in case the variable wasn't found.
        /// </summary>
        /// <param name="name">The name of the variable (e.g. Calendar_Page).</param>
        /// <param name="standard">The standard value in case the variable wasn't found.</param>
        public string Get(string name, string standard)
        {
            string value = Get(name);
            if (value == string.Empty)
                return standard;
            else
                return value;
        }
        /// <summary>
        /// Finds the localization using control.Name.
        /// </summary>
        /// <param name="control">The control object that needs a language specific text.</param>
        public string Get(Control control)
        {
            string value = Get(control.Name);
            if (value == string.Empty)
                return control.Text;
            else
                return value;
        }
        /// <summary>
        /// Finds the localization using column.Name.
        /// </summary>
        /// <param name="column">The column that needs a language specific header text.</param>
        public string Get(DataGridViewColumn column)
        {
            string value = Get(column.Name);
            if (value == string.Empty)
                return column.HeaderText;
            else
                return value;
        }
    }
}