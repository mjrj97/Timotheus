﻿using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
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
        private readonly List<Key> keys;
        /// <summary>
        /// Character that is used to separate a keys name and value in the file.
        /// </summary>
        private readonly char separator = ',';
        /// <summary>
        /// Text encoding used to encode/decode text to/from a file.
        /// </summary>
        private readonly Encoding encoding = Encoding.UTF8;

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
                throw new System.Exception("Exception_LoadFailed");

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
                throw new System.Exception("Exception_LoadFailed");

            byte[] data = Cipher.Decrypt(File.ReadAllBytes(path), password);
            string text = encoding.GetString(data);
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
        /// Gets all keys found in the given text.
        /// </summary>
        /// <param name="text">Unencrypted text with keys.</param>
        /// <returns></returns>
        private List<Key> Load(string text)
        {
            string[] lines = Regex.Split(text, "\r\n|\r|\n");
            List<Key> keys = new List<Key>();

            for (int i = 0; i < lines.Length; i++)
            {
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
            string text = ToString();
            byte[] data = encoding.GetBytes(text);
            File.WriteAllBytes(path, data);
        }
        /// <summary>
        /// Saves the register to the path as encrypted file.
        /// </summary>
        /// <param name="path">Path where the register should be saved. Must include filename and extension.</param>
        /// <param name="password">Password to encrypt the file.</param>
        public void Save(string path, string password)
        {
            string text = ToString();
            byte[] data = Cipher.Encrypt(encoding.GetBytes(text), password);
            File.WriteAllBytes(path, data);
        }
        
        /// <summary>
        /// Adds a key to the register with a name and value. Doesn't check if key already exists.
        /// </summary>
        /// <param name="name">Name of the key.</param>
        /// <param name="value">Value of the key.</param>
        public void Add(string name, string value)
        {
            keys.Add(new Key(name, value));
        }
        /// <summary>
        /// Adds a key to the register from a line. Uses the specified separator to get name and value. Doesn't check if key already exists.
        /// </summary>
        /// <param name="line"></param>
        public void Add(string line)
        {
            keys.Add(new Key(line, separator));
        }

        /// <summary>
        /// Finds the key with the given name and sets it value. If key is not in register, it creates a new key.
        /// </summary>
        /// <param name="name">Name of the key.</param>
        /// <param name="value">Value of the key.</param>
        public void Set(string name, string value)
        {
            int i = 0;
            bool found = false;
            while (!found && i < keys.Count)
            {
                if (keys[i].name == name)
                {
                    keys[i].value = value;
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
        /// Returns the value of the key with a given name.
        /// </summary>
        /// <param name="name">Name of the key.</param>
        public string Get(string name)
        {
            string value = string.Empty;
            int i = 0;
            bool found = false;

            while (i < keys.Count && !found)
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
        public string Get(System.Windows.Forms.Control control)
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
        public string Get(System.Windows.Forms.DataGridViewColumn column)
        {
            string value = Get(column.Name);
            if (value == string.Empty)
                return column.HeaderText;
            else
                return value;
        }

        /// <summary>
        /// Returns the keys in the register in a random sequence, with each line formatted as NAME,VALUE where ',' is the separator.
        /// </summary>
        public override string ToString()
        {
            string formatted = "";
            for (int i = 0; i < keys.Count; i++)
            {
                formatted += keys[i].name + separator + keys[i].value;
                if (i != keys.Count - 1)
                    formatted += "\n";
            }
            return formatted;
        }
    }
}