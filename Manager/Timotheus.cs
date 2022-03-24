﻿using System;
using System.IO;
using System.Net;
using System.Text;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Collections.Generic;
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
        public const string Version = "1.2.1";
        /// <summary>
        /// Whether this is the first time the software runs on this computer.
        /// </summary>
        public static bool FirstTime { get; private set; }

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
            CultureInfo.CurrentCulture = Culture;

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
                Microsoft.Win32.Registry.CurrentUser.DeleteSubKey(@"SOFTWARE\Timotheus");
                Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Timotheus");

                List<Key> keys = Registry.RetrieveAll();
                for (int i = 0; i < keys.Count; i++)
                {
                    key.SetValue(keys[i].Name, keys[i].Value);
                }

                key.Close();
            }
            else
            {
                string directory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Programs/Timotheus";
                string fileName = directory + "/" + "Registry.ini";
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);
                if (!File.Exists(fileName))
                    File.Create(fileName).Close();
                Registry.Save(fileName);
            }
        }

        /// <summary>
        /// Loads the values stored in the (Windows registry/macOS .plist/Linux etc folder) associated with Timotheus.
        /// </summary>
        private static void LoadRegistry()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Registry = new();
                Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Timotheus");

                if (key != null)
                {
                    string[] names = key.GetValueNames();
                    for (int i = 0; i < names.Length; i++)
                    {
                        string value = Convert.ToString(key.GetValue(names[i]));
                        Registry.Create(names[i], value);
                    }

                    key.Close();
                }
                else
                    FirstTime = true;
            }
            else
            {
                string directory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Programs/Timotheus";
                string fileName = directory + "/" + "Registry.ini";
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);
                if (!File.Exists(fileName))
                {
                    FirstTime = true;
                    File.Create(fileName).Close();
                }
                Registry = new Register(fileName, ':');
            }
        }
    }
}