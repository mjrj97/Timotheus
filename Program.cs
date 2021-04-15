using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;
using Timotheus.Forms;
using Timotheus.Utility;

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

            //Initializes the loading of localization.
            Localization.Initialize(directory, culture.Name);

            //Defines the security protocol
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            //Defines encoding 1252
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            //Open the MainWindow
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow());
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
    }
}