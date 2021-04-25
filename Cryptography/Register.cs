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
        /// Gets all keys found in the given text.
        /// </summary>
        /// <param name="text">Unencrypted text with keys.</param>
        /// <returns></returns>
        private static List<Key> Load(string text)
        {
            //Needs code to decompile the text into a list of keys.
            return new List<Key>();
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