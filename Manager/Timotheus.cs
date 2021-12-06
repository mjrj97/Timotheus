using System;
using System.IO;
using System.Net;
using System.Text;
using System.Reflection;
using System.Globalization;
using Timotheus.IO;

namespace Timotheus
{
    public static class Timotheus
    {
        private static Register _Registry;
        /// <summary>
        /// A register containing all values found in the (Windows registry/macOS .plist/Linux etc folder) associated with Timotheus. Is loaded on start of program and saved on exit.
        /// </summary>
        public static Register Registry
        {
            get { return _Registry; }
            set { _Registry = value; }
        }
        /// <summary>
        /// Text encoding used by the program. Is essential to decode the text from Windows Registry.
        /// </summary>
        public readonly static Encoding Encoding = Encoding.BigEndianUnicode;
        /// <summary>
        /// Text encoding used by the program. Is essential to decode the text from Windows Registry.
        /// </summary>
        public readonly static CultureInfo Culture = CultureInfo.GetCultureInfo("da-DK");
        /// <summary>
        /// Version of the software.
        /// </summary>
        public static string Version = "1.0.0";
        /// <summary>
        /// Whether this is the first time the software runs on this computer.
        /// </summary>
        public static bool FirstTime = false;

        /// <summary>
        /// Initializes the static variables and loads the Registry.
        /// </summary>
        public static void Initalize()
        {
            LoadRegistry();

            //Defines the security protocol
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            //Defines encoding 1252 for PDF
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            CultureInfo.CurrentUICulture = Culture;

            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);

            string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            Version = version[0..^2];
        }

        /// <summary>
        /// Handles operations to do before the program closes.
        /// </summary>
        private static void OnProcessExit(object sender, EventArgs e)
        {
            SaveRegistry();
        }

        /// <summary>
        /// Saves the registry to the (Windows registry/macOS .plist/Linux etc folder).
        /// </summary>
        private static void SaveRegistry()
        {
            string directory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Programs/Timotheus";
            string fileName = "Registry.ini";
            SecureFile(directory, fileName);
            Registry.Save(directory + "/" + fileName);
        }

        /// <summary>
        /// Checks if the directory and file exists. If not both are created.
        /// </summary>
        /// <param name="directory">Path to directory</param>
        /// <param name="fileName">File name (without path)</param>
        private static void SecureFile(string directory, string fileName)
        {
            if (!Directory.Exists(directory))
            {
                FirstTime = true;
                Directory.CreateDirectory(directory);
            }
            if (!File.Exists(directory + "/" + fileName))
                File.Create(directory + "/" + fileName).Close();
        }

        /// <summary>
        /// Loads the values stored in the (Windows registry/macOS .plist/Linux etc folder) associated with Timotheus.
        /// </summary>
        private static void LoadRegistry()
        {
            string directory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Programs/Timotheus";
            string fileName = "Registry.ini";
            SecureFile(directory, fileName);
            Registry = new Register(directory + "/" + fileName, ':');
        }
    }
}