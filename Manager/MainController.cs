﻿using System;
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
        private Register _Keys = new();
        public Register Keys
        {
            get
            {
                return _Keys;
            }
            set
            {
                _Keys = value;
                InsertKey();
            }
        }

        /// <summary>
        /// Current calendar used by the program.
        /// </summary>
        private Calendar _Calendar;
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

        private DirectoryManager _Directory = new();
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
            PeriodText = calendarPeriod.ToString();
        }

        /// <summary>
        /// Updates the contents of the event table.
        /// </summary>
        public void UpdateCalendarTable()
        {
            Events.Clear();
            for (int i = 0; i < Calendar.Events.Count; i++)
            {
                if (Calendar.Events[i].In(calendarPeriod) && !Calendar.Events[i].Deleted)
                    Events.Add(Calendar.Events[i]);
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
                GoToDirectory(Files[i].FullName);
        }
        public void GoToDirectory(string path)
        {
            currentDirectory = Path.TrimEndingDirectorySeparator(path.Replace('\\', '/'));
            Files = Directory.GetFilesList(currentDirectory);
        }

        /// <summary>
        /// "Inserts" the current key, and tries to open the Calendar and Filsharing system.
        /// </summary>
        private void InsertKey()
        {
            try
            {
                Directory = new DirectoryManager(Keys.Get("SSH-LocalDirectory"), Keys.Get("SSH-RemoteDirectory"), Keys.Get("SSH-URL"), Keys.Get("SSH-Username"), Keys.Get("SSH-Password"));
            }
            catch (Exception) { Directory = new(); }

            try
            {
                Calendar = new(Keys.Get("Calendar-Email"), Keys.Get("Calendar-Password"), Keys.Get("Calendar-URL"));
            }
            catch (Exception) { Calendar = new(); }
        }

        /// <summary>
        /// Loads the key from the path. Saves the path and password to the registry.
        /// </summary>
        /// <param name="path">Path to the key file.</param>
        /// <param name="password">The password used to decrypt the key.</param>
        public void LoadKey(string path, string password)
        {
            Keys = new Register(path, password, ':');
            Timotheus.Registry.Set("KeyPath", path);
        }
        /// <summary>
        /// Loads the key from the path.
        /// </summary>
        /// <param name="path">Path to the key file.</param>
        public void LoadKey(string path)
        {
            Keys = new Register(path, ':');
            Timotheus.Registry.Set("KeyPath", path);
            Timotheus.Registry.Remove("KeyPassword");
        }

        /// <summary>
        /// Save the key to the path.
        /// </summary>
        public void SaveKey(string path)
        {
            Keys.Save(path);
        }
        /// <summary>
        /// Save the encrypted key to the path.
        /// </summary>
        public void SaveKey(string path, string password)
        {
            Keys.Save(path, password);
        }
    }
}