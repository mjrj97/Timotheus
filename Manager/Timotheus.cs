using System;
using System.IO;
using System.Net;
using System.Text;
using System.Globalization;
using Timotheus.IO;

namespace Timotheus
{
    public static class Timotheus
    {
        /// <summary>
        /// A register containing all values found in the (Windows registry/macOS .plist/Linux etc folder) associated with Timotheus. Is loaded on start of program and saved on exit.
        /// </summary>
        public static Register Registry;
        /// <summary>
        /// Text encoding used by the program. Is essential to decode the text from Windows Registry.
        /// </summary>
        public readonly static Encoding Encoding = Encoding.BigEndianUnicode;
        /// <summary>
        /// Text encoding used by the program. Is essential to decode the text from Windows Registry.
        /// </summary>
        public readonly static CultureInfo Culture = CultureInfo.GetCultureInfo("da-DK");

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
            Registry.Save(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Programs/Timotheus/Registry.ini");
        }

        /// <summary>
        /// Loads the values stored in the (Windows registry/macOS .plist/Linux etc folder) associated with Timotheus.
        /// </summary>
        private static void LoadRegistry()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Programs/Timotheus";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            if (!File.Exists(path + "/Registry.ini"))
                File.Create(path + "/Registry.ini").Close();
            Registry = new Register(path + "/Registry.ini", ':');
        }
    }
}