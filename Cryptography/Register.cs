using System.IO;
using System.Collections.Generic;
using System;

namespace Timotheus.Cryptography
{
    public class Register
    {
        /// <summary>
        /// List of keys with a given name and value.
        /// </summary>
        private readonly List<Key> keys;

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
                throw new Exception("Exception_LoadFailed");

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
                throw new Exception("Exception_LoadFailed");

            string text = File.ReadAllText(path);
            keys = Load(text);
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
        private List<Key> Load(string text)
        {
            List<Key> keys = new List<Key>();
            string[] lines = text.Split('\n');

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

                keys.Add(new Key(name, value));
            }

            return keys;
        }

        /// <summary>
        /// Returns the value of the key with a given name.
        /// </summary>
        /// <param name="name">Name of the key.</param>
        public string Get(string name)
        {
            string value = string.Empty;
            for (int i = 0; i < keys.Count; i++)
            {
                if (keys[i].name == name)
                    value = keys[i].value;
            }
            return value;
        }
    }
}