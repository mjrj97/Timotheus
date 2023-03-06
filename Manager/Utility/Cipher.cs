using DeviceId;
using System.IO;
using System.Security.Cryptography;

namespace Timotheus.Utility
{
    /// <summary>
    /// Class that contains methods to encrypt and decrypt byte arrays [<see href="https://stackoverflow.com/questions/42834063/decrypting-byte-array-with-symmetricalgorithm-and-cryptostream">Source</see>].
    /// </summary>
    public static class Cipher
    {
        private static string _defkey = string.Empty;
		/// <summary>
		/// A default key that can be used to encrypt low-risk strings.
		/// </summary>
		private static string defkey
        {
            get
            {
                if (_defkey == null || _defkey == string.Empty)
                {
                    string deviceId = new DeviceIdBuilder().AddMachineName().AddMacAddress().ToString();

                    if (deviceId.Length > 32)
                    {
						deviceId = deviceId.Substring(0, 32);
					}
                    else if (deviceId.Length < 32)
                    {
                        deviceId.PadRight(32, '0');
                    }

					_defkey = deviceId;
				}

				return _defkey;
            }
        }

        /// <summary>
        /// Method that encrypts a byte array using a key.
        /// </summary>
        /// <param name="password">Password/key used to encrypt the data.</param>
        /// <param name="data">Data to be encrypted.</param>
        public static byte[] Encrypt(byte[] data, string password)
        {
            using SymmetricAlgorithm algorithm = GetAlgorithm(password);
            using ICryptoTransform encryptor = algorithm.CreateEncryptor();
            using MemoryStream ms = new();
            using CryptoStream cs = new(ms, encryptor, CryptoStreamMode.Write);
            cs.Write(data, 0, data.Length);
            cs.FlushFinalBlock();
            return ms.ToArray();
        }

        /// <summary>
        /// Method that decrypts a byte array to a string using a key.
        /// </summary>
        /// <param name="password">Password/key used to encrypt the data.</param>
        /// <param name="data">Encrypted data to be decrypted.</param>
        public static byte[] Decrypt(byte[] data, string password)
        {
            using SymmetricAlgorithm algorithm = GetAlgorithm(password);
            using ICryptoTransform decryptor = algorithm.CreateDecryptor();
            using MemoryStream ms = new();
            using CryptoStream cs = new(ms, decryptor, CryptoStreamMode.Write);
            cs.Write(data, 0, data.Length);
            cs.FlushFinalBlock();
            return ms.ToArray();
        }

        /// <summary>
        /// Encrypts a string using the standard encryption key.
        /// </summary>
        public static string Encrypt(string text)
        {
            byte[] decodedBytes = Timotheus.Encoding.GetBytes(text);
            byte[] encodedBytes = Encrypt(decodedBytes, defkey);
            return Timotheus.Encoding.GetString(encodedBytes);
        }

        /// <summary>
        /// Decrypts a string using the standard encryption key.
        /// </summary>
        public static string Decrypt(string text)
        {
            byte[] encodedBytes = Timotheus.Encoding.GetBytes(text);
            byte[] decodedBytes = Decrypt(encodedBytes, defkey);
            return Timotheus.Encoding.GetString(decodedBytes);
        }

        /// <summary>
        /// Returns the symmetric algorithm associated with the given password.
        /// </summary>
        private static SymmetricAlgorithm GetAlgorithm(string password)
        {
            Aes algorithm = Aes.Create();
            byte[] salt = { 0x53, 0x6f, 0x64, 0x69, 0x75, 0x6d, 0x20, 0x43, 0x68, 0x6c, 0x6f, 0x72, 0x69, 0x64, 0x65 };

            using (Rfc2898DeriveBytes rdb = new(password, salt))
            {
                algorithm.Padding = PaddingMode.PKCS7;
                algorithm.Key = rdb.GetBytes(32);
                algorithm.IV = rdb.GetBytes(16);
            }

            return algorithm;
        }
    }
}