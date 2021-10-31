using System;
using System.Net;
using System.Text;
using Avalonia;
using Timotheus.IO;

namespace Timotheus
{
    class Program
    {
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
        public static void Main(string[] args)
        {
            Registry.Set("KeyPath", System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Key.txt"));

            //Defines the security protocol
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            //Defines encoding 1252 for PDF
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace();
    }
}