using System;
using System.Net;
using System.Text;
using System.Globalization;
using System.Runtime.InteropServices;
using Timotheus.IO;
using System.Collections.Generic;

namespace Timotheus
{
    public static class Timotheus
    {
        /// <summary>
        /// A register containing all values found in the (Windows registry/macOS .plist/Linux etc folder) associated with Timotheus. Is loaded on start of program and saved on exit.
        /// </summary>
        public static Register Registry = new();
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
            Registry.Set("KeyPath", System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Key.txt"));

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
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
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
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {

            }
        }

        /// <summary>
        /// Loads the values stored in the (Windows registry/macOS .plist/Linux etc folder) associated with Timotheus.
        /// </summary>
        private static void LoadRegistry()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
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

                    key.Close();
                }
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                
            }
        }
    }
}