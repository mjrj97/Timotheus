using System.IO;
using System.Security.Cryptography;

namespace Timotheus.Utility
{
    /// <summary>
    /// Class that contains methods to encrypt and decrypt byte arrays [<see href="https://stackoverflow.com/questions/42834063/decrypting-byte-array-with-symmetricalgorithm-and-cryptostream">Source</see>].
    /// </summary>
    public static class Cipher
    {
        /// <summary>
        /// Method that encrypts a byte array using a key.
        /// </summary>
        /// <param name="password">Password/key used to encrypt the data.</param>
        /// <param name="data">Data to be encrypted.</param>
        public static byte[] Encrypt(byte[] data, string password)
        {
            using (SymmetricAlgorithm algorithm = GetAlgorithm(password))
            using (ICryptoTransform encryptor = algorithm.CreateEncryptor())
            using (MemoryStream ms = new MemoryStream())
            using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            {
                cs.Write(data, 0, data.Length);
                cs.FlushFinalBlock();
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Method that decrypts a byte array to a string using a key.
        /// </summary>
        /// <param name="password">Password/key used to encrypt the data.</param>
        /// <param name="data">Encrypted data to be decrypted.</param>
        public static byte[] Decrypt(byte[] data, string password)
        {
            using (SymmetricAlgorithm algorithm = GetAlgorithm(password))
            using (ICryptoTransform decryptor = algorithm.CreateDecryptor())
            using (MemoryStream ms = new MemoryStream())
            using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write))
            {
                cs.Write(data, 0, data.Length);
                cs.FlushFinalBlock();
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Returns the symmetric algorithm associated with the given password.
        /// </summary>
        private static SymmetricAlgorithm GetAlgorithm(string password)
        {
            Rijndael algorithm = Rijndael.Create();
            byte[] salt = { 0x53, 0x6f, 0x64, 0x69, 0x75, 0x6d, 0x20, 0x43, 0x68, 0x6c, 0x6f, 0x72, 0x69, 0x64, 0x65 };

            using (Rfc2898DeriveBytes rdb = new Rfc2898DeriveBytes(password, salt))
            {
                algorithm.Padding = PaddingMode.ISO10126;
                algorithm.Key = rdb.GetBytes(32);
                algorithm.IV = rdb.GetBytes(16);
            }

            return algorithm;
        }
    }
}
