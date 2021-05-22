using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections.Generic;
using Timotheus.Forms;
using Timotheus.IO;

namespace Timotheus
{
    /// <summary>
    /// Root class of the program which holds the main method.
    /// </summary>
    static class Program
    {
        /// <summary>
        /// The directory of the program. Used to load data from subfolders and files.
        /// </summary>
        public static string directory;
        /// <summary>
        /// Culture/language of the computer (e.g. en-GB). Used to load localization.
        /// </summary>
        public static CultureInfo culture;
        /// <summary>
        /// A register containing all the localizations for the program.
        /// </summary>
        public static Register Localization;
        /// <summary>
        /// A register containing all values found in the Windows registry associated with Timotheus. Is loaded on start of program and saved on exit.
        /// </summary>
        public static Register Registry = new Register();
        /// <summary>
        /// Text encoding used by the program. Is essential to decode the text from Windows Registry.
        /// </summary>
        public static Encoding encoding = Encoding.BigEndianUnicode;

        /// <summary>
        /// Starting point of the program, and loads the main window.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Define the localization directory.
            #if DEBUG
            directory = Application.StartupPath[0..^24] + "Localization\\";
            #else
            directory = Application.StartupPath + "locale\\";
            #endif
            culture = CultureInfo.CurrentUICulture;

            //Checks if localization file is in folder. If not, it defaults to en-US.
            string[] localizationFiles = Directory.GetFiles(directory);
            bool foundLocalization = false;
            int i = 0;
            while (i < localizationFiles.Length && !foundLocalization)
            {
                if (localizationFiles[i].Contains(culture.Name))
                    foundLocalization = true;
                i++;
            }
            if (!foundLocalization)
                culture = CultureInfo.GetCultureInfo("en-US");

            culture = CultureInfo.GetCultureInfo("da-DK"); //FOR DEBUG ONLY

            //Initializes the loading of localization.
            Localization = new Register(directory + culture.Name + ".txt");

            //Loads the values stored in Windows registry.
            LoadRegistry();

            //Defines the process exit event.
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);

            //Defines the security protocol
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            //Defines encoding 1252
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            //Open the MainWindow and makes sure there is only one instance.
            using Mutex mutex = new Mutex(true, "Timotheus", out bool createdNew);
            if (createdNew)
            {
                Application.SetHighDpiMode(HighDpiMode.SystemAware);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainWindow());
            }
            else
            {
                Process current = Process.GetCurrentProcess();
                foreach (Process process in Process.GetProcessesByName(current.ProcessName))
                {
                    if (process.Id != current.Id)
                    {
                        Error("Exception_Name", "Exception_AlreadyRunning");
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Saves the registry to the Windows registry.
        /// </summary>
        private static void SaveRegistry()
        {
            Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Timotheus", true);

            if (key == null)
                key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Timotheus");

            List<Key> keys = Registry.Keys();
            for (int i = 0; i < keys.Count; i++)
            {
                key.SetValue(keys[i].name, keys[i].value);
            }

            key.Close();
        }

        /// <summary>
        /// Loads the values stored in the Windows registry associated with Timotheus.
        /// </summary>
        private static void LoadRegistry()
        {
            Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Timotheus");

            if (key != null)
            {
                string[] names = key.GetValueNames();
                for (int i = 0; i < names.Length; i++)
                {
                    string value = Convert.ToString(key.GetValue(names[i]));
                    Registry.Add(names[i], value);
                }
            }

            key.Close();
        }

        /// <summary>
        /// Displays an error dialog to the user.
        /// </summary>
        /// <param name="name">Name of the exception to be found in Localization.</param>
        /// <param name="text">Specify the text of the error without localization.</param>
        public static void Error(string name, string text)
        {
            string errorName = Localization.Get(name, name);
            string errorText = Localization.Get(text, text);
            MessageBox.Show(errorText, errorName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Handles operations to do before the program closes.
        /// </summary>
        private static void OnProcessExit(object sender, EventArgs e)
        {
            SaveRegistry();
        }
    }
}