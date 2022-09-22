using Microsoft.Win32;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Timotheus.Utility
{
    public static class FileAssociations
    {
        private class FileAssociation
        {
            public string Extension { get; set; }
            public string ProgId { get; set; }
            public string FileTypeDescription { get; set; }
            public string IconFilePath { get; set; }
            public string ExecutableFilePath { get; set; }
        }

        // needed so that Explorer windows get refreshed after the registry is updated
        [DllImport("Shell32.dll")]
        private static extern int SHChangeNotify(int eventId, int flags, IntPtr item1, IntPtr item2);

        private const int SHCNE_ASSOCCHANGED = 0x8000000;
        private const int SHCNF_FLUSH = 0x1000;

        public static void EnsureAssociations()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                throw new Exception("Cannot be used on other platforms than Windows.");

            var filePath = Environment.ProcessPath;
            EnsureAssociationsSet(
                new FileAssociation
                {
                    Extension = ".tkey",
                    ProgId = "Timotheus",
                    FileTypeDescription = "Timotheus key",
                    IconFilePath = Path.GetDirectoryName(filePath) + "\\Resources\\ProjectFileIcon.ico",
                    ExecutableFilePath = filePath
                });
        }

        private static void EnsureAssociationsSet(params FileAssociation[] associations)
        {
            bool madeChanges = false;
            foreach (var association in associations)
            {
                madeChanges |= SetAssociation(
                    association.Extension,
                    association.ProgId,
                    association.FileTypeDescription,
                    association.IconFilePath,
                    association.ExecutableFilePath);
            }

            if (madeChanges)
            {
                _ = SHChangeNotify(SHCNE_ASSOCCHANGED, SHCNF_FLUSH, IntPtr.Zero, IntPtr.Zero);
            }
        }

        private static bool SetAssociation(string extension, string progId, string fileTypeDescription, string iconFilePath, string applicationFilePath)
        {
            bool madeChanges = false;
            madeChanges |= SetKeyDefaultValue(@"Software\Classes\" + extension, progId);
            madeChanges |= SetKeyDefaultValue(@"Software\Classes\" + progId, fileTypeDescription);
            madeChanges |= SetKeyDefaultValue($@"Software\Classes\{progId}\DefaultIcon", iconFilePath);
            madeChanges |= SetKeyDefaultValue($@"Software\Classes\{progId}\shell\open\command", "\"" + applicationFilePath + "\" \"%1\"");
            return madeChanges;
        }

        private static bool SetKeyDefaultValue(string keyPath, string value)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                using var key = Registry.CurrentUser.CreateSubKey(keyPath);
                if (key.GetValue(null) as string != value)
                {
                    key.SetValue(null, value);
                    return true;
                }
            }

            return false;
        }
    }
}