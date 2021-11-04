using System;
using System.Collections.ObjectModel;
using System.IO;
using Timotheus.IO;
using Timotheus.Utility;
using Timotheus.Schedule;
using ReactiveUI;
using Renci.SshNet.Sftp;

namespace Timotheus
{
    public class MainController : ReactiveObject
    {
        /// <summary>
        /// Register containing all the keys loaded at startup or manually from a key file (.tkey or .txt)
        /// </summary>
        public Register keys;

        /// <summary>
        /// Current calendar used by the program.
        /// </summary>
        public Calendar _Calendar = new();
        public Calendar Calendar
        {
            get
            {
                return _Calendar;
            }
            set
            {
                _Calendar = value;
                UpdateCalendarTable();
            }
        }

        /// <summary>
        /// Type of period used by Calendar_View.
        /// </summary>
        public Period calendarPeriod = new(new DateTime(DateTime.Now.Year, 1, 1), PeriodType.Year);

        public int SelectedPeriod
        {
            get { return (int)calendarPeriod.Type; }
            set
            {
                calendarPeriod.SetType((PeriodType)value);
                UpdateCalendarTable();
            }
        }

        private string _PeriodText = string.Empty;
        public string PeriodText
        {
            get => _PeriodText;
            set => this.RaiseAndSetIfChanged(ref _PeriodText, value);
        }

        private ObservableCollection<Event> _Events = new();
        public ObservableCollection<Event> Events
        {
            get => _Events;
            set => this.RaiseAndSetIfChanged(ref _Events, value);
        }

        private ObservableCollection<SftpFile> _Files = new();
        public ObservableCollection<SftpFile> Files
        {
            get => _Files;
            set => this.RaiseAndSetIfChanged(ref _Files, value);
        }

        private DirectoryManager _Directory;
        public DirectoryManager Directory
        {
            get
            {
                return _Directory;
            }
            set
            {
                _Directory = value;
                GoToDirectory(_Directory.RemotePath);
            }
        }

        private string currentDirectory = string.Empty;

        public MainController() {
            string KeyPath = Timotheus.Registry.Get("KeyPath");
            keys = LoadKey(KeyPath, false);

            try
            {
                Directory = new DirectoryManager(keys.Get("SSH-LocalDirectory"), keys.Get("SSH-RemoteDirectory"), keys.Get("SSH-URL"), keys.Get("SSH-Username"), keys.Get("SSH-Password"));
            }
            catch (Exception)
            {

            }

            try
            {
                Calendar = new(keys.Get("Calendar-Email"), keys.Get("Calendar-Password"), keys.Get("Calendar-URL"));
            }
            catch (Exception)
            {
                
            }

            PeriodText = calendarPeriod.ToString();
        }

        /// <summary>
        /// Updates the contents of the event table.
        /// </summary>
        public void UpdateCalendarTable()
        {
            Events.Clear();
            for (int i = 0; i < Calendar.events.Count; i++)
            {
                if (Calendar.events[i].In(calendarPeriod) && !Calendar.events[i].Deleted)
                    Events.Add(Calendar.events[i]);
            }
            PeriodText = calendarPeriod.ToString();
        }

        /// <summary>
        /// Changes the selected year and calls UpdateTable.
        /// </summary>
        public void UpdatePeriod(bool add)
        {
            if (add)
                calendarPeriod.Add();
            else
                calendarPeriod.Subtract();
            UpdateCalendarTable();
        }

        /// <summary>
        /// Removes the event from list.
        /// </summary>
        public void Remove(Event ev) {
            ev.Deleted = true;
            UpdateCalendarTable();
        }

        public void GoUpDirectory()
        {
            GoToDirectory(Path.GetDirectoryName(currentDirectory) + "/");
        }

        public void GoToDirectory(int i)
        {
            if (Files[i].IsDirectory)
            {
                GoToDirectory(Files[i].FullName);
            }
        }
        public void GoToDirectory(string path)
        {
            currentDirectory = Path.TrimEndingDirectorySeparator(path.Replace('\\', '/'));
            Files = Directory.GetFilesList(currentDirectory);
        }

        /// <summary>
        /// Loads and returns the key from the path.
        /// </summary>
        /// <param name="path">Path to the key file.</param>
        /// <param name="requirePasswordDialog">Whether a password dialog should be required. If false it tries to get the password stored in the Progarm.Registry.</param>
        private static Register LoadKey(string path, bool requirePasswordDialog)
        {
            Register keys = null;
            if (path != string.Empty)
            {
                string extension = Path.GetExtension(path);
                switch (extension)
                {
                    case ".tkey":
                        string encodedPassword = requirePasswordDialog ? string.Empty : Timotheus.Registry.Get("KeyPassword");
                        byte[] encodedBytes = Timotheus.Encoding.GetBytes(encodedPassword);
                        if (requirePasswordDialog)
                            Timotheus.Registry.Remove("KeyPassword");

                        if (encodedPassword != string.Empty)
                        {
                            try
                            {
                                byte[] decodedBytes = Cipher.Decrypt(encodedBytes, Cipher.defkey);
                                string password = Timotheus.Encoding.GetString(decodedBytes);

                                keys = new Register(path, password, ':');
                                Timotheus.Registry.Set("KeyPath", path);
                            }
                            catch (System.Security.Cryptography.CryptographicException e)
                            {
                                //Program.Error("Exception_WrongPassword", e.Message);
                                keys = new Register(':');
                            }
                        }
                        else
                        {
                            try
                            {
                                /*PasswordDialog passwordDialog = new PasswordDialog()
                                {
                                    Owner = this
                                };
                                if (passwordDialog.ShowDialog() == DialogResult.OK)
                                {
                                    keys = new Register(path, passwordDialog.Password, ':');
                                    if (passwordDialog.Check)
                                    {
                                        byte[] decodedBytes = Program.encoding.GetBytes(passwordDialog.Password);
                                        encodedBytes = Cipher.Encrypt(decodedBytes, Cipher.defkey);
                                        encodedPassword = Program.encoding.GetString(encodedBytes);
                                        Program.Registry.Set("KeyPassword", encodedPassword);
                                    }
                                    Program.Registry.Set("KeyPath", path);
                                }
                                else*/
                                {
                                    keys = new Register(':');
                                }
                            }
                            catch (System.Security.Cryptography.CryptographicException e)
                            {
                                //Program.Error("Exception_WrongPassword", e.Message);
                                keys = new Register(':');
                            }
                        }
                        break;
                    case ".txt":
                        keys = new Register(path, ':');
                        Timotheus.Registry.Set("KeyPath", path);
                        break;
                }

                //InsertKeys();
            }
            else
                keys = new Register(':');

            return keys;
        }

        /// <summary>
        /// Opens a dialog so the user can save the current loaded keys to a file.
        /// </summary>
        public void SaveKey(string path)
        {
            string extension = Path.GetExtension(path);
            switch (extension)
            {
                case ".txt":
                    keys.Save(path);
                    break;
                /*case ".tkey":
                    PasswordDialog dialog = new PasswordDialog
                    {
                        Owner = this
                    };

                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        keys.Save(path, dialog.Password);
                        if (dialog.Check)
                        {
                            byte[] decodedBytes = Timotheus.Encoding.GetBytes(dialog.Password);
                            byte[] encodedBytes = Cipher.Encrypt(decodedBytes, Cipher.defkey);
                            string encodedPassword = Timotheus.Encoding.GetString(encodedBytes);
                            Timotheus.Registry.Set("KeyPassword", encodedPassword);
                        }
                    }
                    break;*/
            }
        }

        /// <summary>
        /// Opens a dialog so the user can select a file that has all the keys.
        /// </summary>
        public void OpenKey(string path)
        {
            keys = LoadKey(path, true);
        }
    }
}