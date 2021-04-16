using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Timotheus.Utility
{
    // Inspired by: https://www.c-sharpcorner.com/article/encryption-and-decryption-using-a-symmetric-key-in-c-sharp/
    public static class Cipher
    {
        /// <summary>
        /// Method that encrypts a string to a byte array using a key.
        /// </summary>
        /// <param name="key">Password/key used to encrypt the data.</param>
        /// <param name="text">Text to be encrypted.</param>
        public static byte[] Encrypt(string key, string text)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(text);
                        }
                        array = memoryStream.ToArray();
                    }
                }
            }
            return array;
        }

        /// <summary>
        /// Method that decrypts a byte array to a string using a key.
        /// </summary>
        /// <param name="key">Password/key used to encrypt the data.</param>
        /// <param name="data">Encrypted data to be decrypted.</param>
        public static string Decrypt(string key, byte[] data)
        {
            byte[] iv = new byte[16];

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(data))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader(cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
