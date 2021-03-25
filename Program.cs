using System;
using System.Globalization;
using System.Net;
using System.Text;
using System.Windows.Forms;
using Timotheus.Forms;

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
            #if DEBUG
            directory = Application.StartupPath[0..^24] + "Localization\\";
            #else
            directory = Application.StartupPath + "locale\\";
            #endif
            culture = CultureInfo.CurrentUICulture;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            //Defines encoding 1252
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow());
        }
    }
}