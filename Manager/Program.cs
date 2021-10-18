using System;
using System.Globalization;
using System.Text;
using Avalonia;
using Timotheus.IO;

namespace Timotheus
{
    class Program
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

        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args) => BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace();
    }
}