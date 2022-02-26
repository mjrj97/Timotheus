﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Timotheus.IO;
using Timotheus.Schedule;
using Timotheus.Persons;

namespace Timotheus.ViewModels
{
    public class MainViewModel : ViewModel
    {
        private Register _Keys = new();
        /// <summary>
        /// Register containing all the keys loaded at startup or manually from a key file (.tkey or .txt)
        /// </summary>
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

        private Calendar _Calendar = new();
        /// <summary>
        /// Current calendar used by the program.
        /// </summary>
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
        public List<Person> Persons = new();

        /// <summary>
        /// Type of period used by Calendar_View.
        /// </summary>
        private readonly Period calendarPeriod;

        /// <summary>
        /// The index of the current period type.
        /// </summary>
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
        /// <summary>
        /// The text showing the current period.
        /// </summary>
        public string PeriodText
        {
            get => _PeriodText;
            set
            {
                _PeriodText = value;
                NotifyPropertyChanged(nameof(PeriodText));
            }
        }

        private ObservableCollection<EventViewModel> _Events = new();
        /// <summary>
        /// A list of the events in the current period.
        /// </summary>
        public ObservableCollection<EventViewModel> Events
        {
            get => _Events;
            set
            {
                _Events = value;
                NotifyPropertyChanged(nameof(Events));
            }
        }

        private ObservableCollection<FileViewModel> _Files = new();
        /// <summary>
        /// A list of files in the current directory.
        /// </summary>
        public ObservableCollection<FileViewModel> Files
        {
            get => _Files;
            set
            {
                _Files = value;
                NotifyPropertyChanged(nameof(Files));
            }
        }

        private ObservableCollection<PersonViewModel> _People = new();
        public ObservableCollection<PersonViewModel> People
        {
            get => _People;
            set
            {
                _People = value;
                NotifyPropertyChanged(nameof(People));
            }
        }

        private DirectoryViewModel _Directory = new();
        /// <summary>
        /// A SFTP object connecting a local and remote directory.
        /// </summary>
        public DirectoryViewModel Directory
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

        private string _currentDirectory = string.Empty;
        /// <summary>
        /// The directory currently being shown.
        /// </summary>
        public string CurrentDirectory
        {
            get
            {
                return _currentDirectory;
            }
            set
            {
                _currentDirectory = value;
                NotifyPropertyChanged(nameof(CurrentDirectory));
            }
        }

        public MainViewModel() {
            calendarPeriod = new(DateTime.Now.Year + " " + (DateTime.Now.Month >= 7 ? Localization.Localization.Calendar_Fall : Localization.Localization.Calendar_Spring));
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
                    Events.Add(new EventViewModel(Calendar.Events[i]));
            }
            PeriodText = calendarPeriod.ToString();
        }

        public void UpdatePeopleTable()
        {
            People.Clear();
            for (int i = 0; i < Persons.Count; i++)
            {
                if (!Persons[i].Deleted)
                    People.Add(new PersonViewModel(Persons[i]));
            }

            NotifyPropertyChanged(nameof(People));
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
        /// Changes the selected year and calls UpdateTable.
        /// </summary>
        public void UpdatePeriod(string text)
        {
            calendarPeriod.SetPeriod(text);
            NotifyPropertyChanged(nameof(SelectedPeriod));
        }

        /// <summary>
        /// Exports the current Calendar in the selected period as a PDF.
        /// </summary>
        /// <param name="name">File name</param>
        /// <param name="path">Path to save</param>
        public void ExportCalendar(string name, string path)
        {
            Calendar.Export(name, path, Keys.Get("Settings-Name"), Keys.Get("Settings-Address"), Keys.Get("Settings-Image"), calendarPeriod);
        }

        /// <summary>
        /// Adds a person to the list.
        /// </summary>
        public void AddPerson(string Name, DateTime ConsentDate, string ConsentVersion, string ConsentComment)
        {
            Persons.Add(new Person(Name, ConsentDate, ConsentVersion, ConsentComment, true));
            UpdatePeopleTable();
        }

        /// <summary>
        /// Removes the event from list.
        /// </summary>
        public void Remove(EventViewModel ev)
        {
            ev.Deleted = true;
            UpdateCalendarTable();
        }

        /// <summary>
        /// Removes the person from list.
        /// </summary>
        public void Remove(PersonViewModel person)
        {
            person.Deleted = true;
            UpdatePeopleTable();
        }

        /// <summary>
        /// Go up a level in the directory.
        /// </summary>
        public void GoUpDirectory()
        {
            string path = Path.GetDirectoryName(CurrentDirectory) + "/";
            if (path.Length >= Directory.RemotePath.Length)
                GoToDirectory(path);
        }

        /// <summary>
        /// Changes the current directory to the given path.
        /// </summary>
        public void GoToDirectory(string path)
        {
            CurrentDirectory = Path.TrimEndingDirectorySeparator(path.Replace('\\', '/'));
            List<DirectoryFile> files = Directory.GetFiles(CurrentDirectory);
            List<FileViewModel> viewFiles = new();
            for (int i = 0; i < files.Count; i++)
            {
                viewFiles.Add(new FileViewModel(files[i]));
            }
            viewFiles.Sort((x, y) => x.SortName.CompareTo(y.SortName));

            Files = new ObservableCollection<FileViewModel>(viewFiles);
        }

        /// <summary>
        /// "Inserts" the current key, and tries to open the Calendar and Filsharing system.
        /// </summary>
        private void InsertKey()
        {
            try
            {
                Directory = new DirectoryViewModel(Keys.Get("SSH-LocalDirectory"), Keys.Get("SSH-RemoteDirectory"), Keys.Get("SSH-URL"), int.Parse(Keys.Get("SSH-Port")), Keys.Get("SSH-Username"), Keys.Get("SSH-Password"));
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