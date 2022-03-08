using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Timotheus.Utility;

namespace Timotheus.IO
{
    /// <summary>
    /// Class that can load a list of value with corresponding names. Can be loaded from encrypted (using a password) and unencrypted files. The file's line separator can be specified (ie. NAME,VALUE where ',' is used).
    /// </summary>
    public class Register : IRepository<Key>
    {
        /// <summary>
        /// Name of the register.
        /// </summary>
        public string Name = "Register";
        /// <summary>
        /// List of keys with a given name and value.
        /// </summary>
        private readonly List<Key> keys;
        /// <summary>
        /// Character that is used to separate a keys name and value in the file.
        /// </summary>
        private readonly char separator = ',';

        /// <summary>
        /// Creates an empty register with the char separator ','.
        /// </summary>
        public Register()
        {
            keys = new List<Key>();
        }
        /// <summary>
        /// Constructor. Loads an unencrypted file of keys.
        /// </summary>
        /// <param name="path">Path to the file.</param>
        public Register(string path)
        {
            if (!File.Exists(path))
                throw new System.Exception("Exception_NoKeys");
            Name = Path.GetFileName(path);

            string text = File.ReadAllText(path);
            keys = Load(text);
        }
        /// <summary>
        /// Constructor. Loads an unencrypted file of keys, with a given separator character.
        /// </summary>
        /// <param name="path">Path to the file.</param>
        /// <param name="separator">Define the character used to separate the name and value of a key.</param>
        public Register(string path, char separator)
        {
            this.separator = separator;
            if (!File.Exists(path))
                throw new System.Exception("Exception_NoKeys");
            Name = Path.GetFileName(path);

            string text = File.ReadAllText(path);
            keys = Load(text);
        }
        /// <summary>
        /// Constructor. Uses a string and a defined separator to load a register.
        /// </summary>
        /// <param name="text">Text to be converted into a register.</param>
        /// <param name="separator">Define the character used to separate the name and value of a key.</param>
        public Register(char separator, string text)
        {
            this.separator = separator;
            keys = Load(text);
        }
        /// <summary>
        /// Creates a new empty register and defines a separator.
        /// </summary>
        /// <param name="separator">Define the character used to separate the name and value of a key.</param>
        public Register(char separator)
        {
            this.separator = separator;
            keys = new List<Key>();
        }
        /// <summary>
        /// Constructor. Loads an encrypted file of keys using the given password.
        /// </summary>
        /// <param name="path">Path to the file.</param>
        /// <param name="password">Password to decrypt the file.</param>
        public Register(string path, string password)
        {
            if (!File.Exists(path))
                throw new System.Exception("Exception_NoKeys");
            Name = Path.GetFileName(path);

            byte[] data = Cipher.Decrypt(File.ReadAllBytes(path), password);
            string text = Timotheus.Encoding.GetString(data);
            keys = Load(text);
        }
        /// <summary>
        /// Constructor. Loads an encrypted file of keys using the given password, with a given separator character.
        /// </summary>
        /// <param name="path">Path to the file.</param>
        /// <param name="password">Password to decrypt the file.</param>
        /// <param name="separator">Define the character used to separate the name and value of a key.</param>
        public Register(string path, string password, char separator)
        {
            this.separator = separator;
            if (!File.Exists(path))
                throw new System.Exception("Exception_NoKeys");
            Name = Path.GetFileName(path);

            byte[] data = Cipher.Decrypt(File.ReadAllBytes(path), password);
            string text = Timotheus.Encoding.GetString(data);
            keys = Load(text);
        }

        /// <summary>
        /// Gets all keys found in the given text.
        /// </summary>
        /// <param name="text">Unencrypted text with keys.</param>
        /// <returns></returns>
        private List<Key> Load(string text)
        {
            string[] lines = Regex.Split(text, "\r\n|\r|\n");
            List<Key> keys = new();

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains(separator))
                    keys.Add(new Key(lines[i], separator));
            }

            return keys;
        }

        /// <summary>
        /// Saves the register to the path as unencrypted text.
        /// </summary>
        /// <param name="path">Path where the register should be saved. Must include filename and extension.</param>
        public void Save(string path)
        {
            File.WriteAllText(path, ToString(), Timotheus.Encoding);
        }
        /// <summary>
        /// Saves the register to the path as encrypted file.
        /// </summary>
        /// <param name="path">Path where the register should be saved. Must include filename and extension.</param>
        /// <param name="password">Password to encrypt the file.</param>
        public void Save(string path, string password)
        {
            string text = ToString();
            byte[] data = Cipher.Encrypt(Timotheus.Encoding.GetBytes(text), password);
            File.WriteAllBytes(path, data);
        }
        
        /// <summary>
        /// Adds a key to the register with a name and value. Doesn't check if key already exists.
        /// </summary>
        /// <param name="name">Name of the key.</param>
        /// <param name="value">Value of the key.</param>
        public void Create(string name, string value)
        {
            Create(new Key(name, value));
        }
        /// <summary>
        /// Adds a key to the register. Doesn't check if key already exists.
        /// </summary>
        /// <param name="key">The key to be added.</param>
        public void Create(Key key)
        {
            keys.Add(key);
        }
        /// <summary>
        /// Adds a key to the register from a line. Uses the specified separator to get name and value. Doesn't check if key already exists.
        /// </summary>
        /// <param name="line"></param>
        public void Create(string line)
        {
            Create(new Key(line, separator));
        }

        /// <summary>
        /// Finds the key with the given name and sets it value. If key is not in register, it creates a new key.
        /// </summary>
        /// <param name="name">Name of the key.</param>
        /// <param name="value">Value of the key.</param>
        public void Update(string name, string value)
        {
            int i = 0;
            bool found = false;
            while (!found && i < keys.Count)
            {
                if (keys[i].Name == name)
                {
                    keys[i].Value = value;
                    found = true;
                }
                i++;
            }
            if (!found)
            {
                keys.Add(new Key(name, value));
            }
        }
        /// <summary>
        /// Updates the given key.
        /// </summary>
        /// <param name="key">Key to be updated.</param>
        public void Update(Key key)
        {
            Update(key.Name, key.Value);
        }

        /// <summary>
        /// Removes a key with the given name.
        /// </summary>
        /// <param name="name">Name of the key</param>
        public void Delete(string name)
        {
            int i = 0;
            bool found = false;
            while (!found && i < keys.Count)
            {
                if (keys[i].Name == name)
                {
                    keys.Remove(keys[i]);
                    found = true;
                }
                i++;
            }
        }
        /// <summary>
        /// Removes a key from the Register.
        /// </summary>
        /// <param name="key"></param>
        public void Delete(Key key)
        {
            keys.Remove(key);
        }

        /// <summary>
        /// Returns the value of the key with a given name.
        /// </summary>
        /// <param name="name">Name of the key.</param>
        public Key Retrieve(string name)
        {
            int i = 0;
            bool found = false;

            while (i < keys.Count && !found)
            {
                if (keys[i].Name == name)
                {
                    found = true;
                }
                else
                    i++;
            }
            if (found)
                return keys[i];
            else
                return new Key(string.Empty, string.Empty);
        }
        /// <summary>
        /// Returns the value with corresponding the name, but a standard value can be provided in case the variable wasn't found.
        /// </summary>
        /// <param name="name">The name of the variable (e.g. Calendar_Page).</param>
        /// <param name="standard">The standard value in case the variable wasn't found.</param>
        public Key Retrieve(string name, string standard)
        {
            Key value = Retrieve(name);
            if (value == null)
                return new Key(string.Empty, standard);
            else
                return value;
        }

        /// <summary>
        /// Returns a list of the keys in the register.
        /// </summary>
        public List<Key> RetrieveAll()
        {
            return keys;
        }

        /// <summary>
        /// Returns the keys in the register in a random sequence, with each line formatted as NAME,VALUE where ',' is the separator.
        /// </summary>
        public override string ToString()
        {
            StringBuilder builder = new();
            for (int i = 0; i < keys.Count; i++)
            {
                builder.Append(keys[i].ToString());
                if (i != keys.Count - 1)
                    builder.Append('\n');
            }
            return builder.ToString();
        }
    }
}