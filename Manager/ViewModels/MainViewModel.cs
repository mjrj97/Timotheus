using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Timotheus.IO;
using Timotheus.Schedule;
using Timotheus.Persons;
using System.Linq;
using System.ComponentModel;

namespace Timotheus.ViewModels
{
    public class MainViewModel : ViewModel
    {
        /// <summary>
        /// Index of the currently open tab.
        /// </summary>
        public int CurrentTab { get; set; }

        private Register _keys = new();
        /// <summary>
        /// Register containing all the keys loaded at startup or manually from a key file (.tkey or .txt)
        /// </summary>
        public Register Keys
        {
            get
            {
                return _keys;
            }
            private set
            {
                _keys = value;
            }
        }

        private Calendar _calendar = new();
        /// <summary>
        /// Current calendar used by the program.
        /// </summary>
        public Calendar Calendar
        {
            get
            {
                return _calendar;
            }
            set
            {
                _calendar = value;
            }
        }

        private PersonRepository _personRepo = new();
        /// <summary>
        /// The repository containing all the people.
        /// </summary>
        public PersonRepository PersonRepo
        {
            get
            {
                return _personRepo;
            }
            set
            {
                _personRepo = value;
            }
        }

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
        
        private ObservableCollection<PersonViewModel> _People = new();
        /// <summary>
        /// A list of people that has made consent.
        /// </summary>
        public ObservableCollection<PersonViewModel> People
        {
            get => _People;
            set
            {
                _People = value;
                NotifyPropertyChanged(nameof(People));
            }
        }

        private string _searchField = string.Empty;
        /// <summary>
        /// The search field in the people tab.
        /// </summary>
        public string SearchField
        {
            get
            {
                return _searchField;
            }
            set
            {
                _searchField = value;
                NotifyPropertyChanged(nameof(SearchField));
            }
        }

        private bool _showInactive = true;
        /// <summary>
        /// Whether inactive people should be shown in the people table.
        /// </summary>
        public bool ShowInactive 
        {
            get
            {
                return _showInactive;
            }
            set
            {
                _showInactive = value;
                NotifyPropertyChanged(nameof(ShowInactive));
                NotifyPropertyChanged(nameof(HideInactive));
            }
        }

        /// <summary>
        /// The inverse of ShowInactive. Used for UI:
        /// </summary>
        public bool HideInactive
        {
            get
            {
                return !_showInactive;
            }
        }

        private static MainViewModel s_instance;
        public static MainViewModel Instance
        {
            get
            {
                return s_instance;
            }
            private set
            {
                s_instance = value;
            }
        }

        /// <summary>
        /// Creates an instance of the MainViewModel
        /// </summary>
        public MainViewModel() {
            Instance = this;
            calendarPeriod = new(DateTime.Now.Year + " " + (DateTime.Now.Month >= 7 ? Localization.Localization.Calendar_Fall : Localization.Localization.Calendar_Spring));
            PeriodText = calendarPeriod.ToString();
        }

        public void NewProject(Register register)
        {
            Keys = register;
            UpdateCalendarTable();
            UpdatePeopleTable();
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

        /// <summary>
        /// Updates the contents of the persons/people table.
        /// </summary>
        public void UpdatePeopleTable()
        {
            People.Clear();
            List<Person> people = PersonRepo.RetrieveAll().OrderBy(o => o.Name).ToList();
            for (int i = 0; i < people.Count; i++)
            {
                if (!people[i].Deleted && (people[i].Active || (!people[i].Active && ShowInactive))  && (people[i].Name.ToLower().Contains(SearchField.ToLower()) || people[i].Comment.ToLower().Contains(SearchField.ToLower())))
                    People.Add(new PersonViewModel(people[i]));
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
            Calendar.Export(name, path, Keys.Retrieve("Settings-Name"), Keys.Retrieve("Settings-Address"), Keys.Retrieve("Settings-Image"), calendarPeriod);
        }

        /// <summary>
        /// Adds a person to the list.
        /// </summary>
        public void AddPerson(string Name, DateTime ConsentDate, string ConsentVersion, string ConsentComment)
        {
            PersonRepo.Create(new Person(Name, ConsentDate, ConsentVersion, ConsentComment, true));
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
        /// Loads the key from the path. Saves the path and password to the registry.
        /// </summary>
        /// <param name="path">Path to the key file.</param>
        /// <param name="password">The password used to decrypt the key.</param>
        public void LoadKey(string path, string password)
        {
            Keys = new Register(path, password, ':');
            Timotheus.Registry.Update("KeyPath", path);
        }
        /// <summary>
        /// Loads the key from the path.
        /// </summary>
        /// <param name="path">Path to the key file.</param>
        public void LoadKey(string path)
        {
            Keys = new Register(path, ':');
            Timotheus.Registry.Update("KeyPath", path);
            Timotheus.Registry.Delete("KeyPassword");
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

        /// <summary>
        /// Returns whether the user has made progress that hasn't been saved.
        /// </summary>
        /// <returns></returns>
        public bool IsThereUnsavedProgress()
        {
            return false;
        }
    }
}