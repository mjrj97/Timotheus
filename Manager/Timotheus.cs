using System;
using System.Net;
using System.Text;
using System.Globalization;
using System.Runtime.InteropServices;
using Timotheus.IO;

namespace Timotheus
{
    public static class Timotheus
    {
        /// <summary>
        /// A register containing all values found in the Windows registry associated with Timotheus. Is loaded on start of program and saved on exit.
        /// </summary>
        public static Register Registry = new Register();
        /// <summary>
        /// Text encoding used by the program. Is essential to decode the text from Windows Registry.
        /// </summary>
        public readonly static Encoding encoding = Encoding.BigEndianUnicode;
        /// <summary>
        /// Text encoding used by the program. Is essential to decode the text from Windows Registry.
        /// </summary>
        public readonly static CultureInfo Culture = CultureInfo.GetCultureInfo("da-DK");

        /// <summary>
        /// Initializes the static variables and loads the Registry.
        /// </summary>
        public static void Initalize()
        {
            Registry = LoadRegistry();

            //Defines the security protocol
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            //Defines encoding 1252 for PDF
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            CultureInfo.CurrentUICulture = Culture;
            Registry.Set("KeyPath", System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Key.txt"));
        }

        private static Register LoadRegistry()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                
            }

            return new Register();
        }
    }
}