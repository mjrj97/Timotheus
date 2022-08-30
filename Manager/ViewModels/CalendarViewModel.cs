using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Timotheus.Schedule;

namespace Timotheus.ViewModels
{
    public class CalendarViewModel : ViewModel
    {
        /// <summary>
        /// Type of period used by Calendar_View.
        /// </summary>
        private Period _calendarPeriod;
        public Period CalendarPeriod
        {
            get
            {
                return _calendarPeriod;
            }
            private set
            {
                _calendarPeriod = value;
            }
        }

        /// <summary>
        /// The index of the current period type.
        /// </summary>
        public int SelectedPeriod
        {
            get { return (int)CalendarPeriod.Type; }
            set
            {
                CalendarPeriod.SetType((PeriodType)value);
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

        public bool IsSetup
        {
            get
            {
                return Calendar.IsSetup();
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

        public bool HasBeenChanged
        {
            get
            {
                return Calendar.HasBeenChanged();
            }
        }

        /// <summary>
        /// Worker used for synchronizing the calendar in the ProgressDialog.
        /// </summary>
        public BackgroundWorker Sync 
        { 
            get
            {
                return Calendar.Sync;
            } 
        }

        public CalendarViewModel(string username, string password, string url)
        {
            Calendar = new Calendar(username, password, url);
            CalendarPeriod = new(DateTime.Now.Year + " " + (DateTime.Now.Month >= 7 ? Localization.Calendar_Fall : Localization.Calendar_Spring));
            PeriodText = CalendarPeriod.ToString();
        }
        public CalendarViewModel(string path)
        {
            Calendar = new(path);
            CalendarPeriod = new(DateTime.Now.Year + " " + (DateTime.Now.Month >= 7 ? Localization.Calendar_Fall : Localization.Calendar_Spring));
            PeriodText = CalendarPeriod.ToString();
        }
        public CalendarViewModel()
        {
            Calendar = new();
            CalendarPeriod = new(DateTime.Now.Year + " " + (DateTime.Now.Month >= 7 ? Localization.Calendar_Fall : Localization.Calendar_Spring));
            PeriodText = CalendarPeriod.ToString();
        }

        public void AddEvent(DateTime Start, DateTime End, string Name, string Description, string Location, string UID)
        {
            Calendar.Events.Add(new Event(Start, End, Name, Description, Location, UID));
        }

        public void Save(string path)
        {
            Calendar.Save(path);
        }

        public void RemoveEvent(EventViewModel evm)
        {
            evm.Deleted = true;
            UpdateCalendarTable();
        }

        public void SetupSync(string username, string password, string url)
        {
            Calendar.SetupSync(username, password, url);
        }

        /// <summary>
        /// Changes the selected year and calls UpdateTable.
        /// </summary>
        public void UpdatePeriod(bool add)
        {
            if (add)
                CalendarPeriod.Add();
            else
                CalendarPeriod.Subtract();
            UpdateCalendarTable();
        }
        /// <summary>
        /// Changes the selected year and calls UpdateTable.
        /// </summary>
        public void UpdatePeriod(string text)
        {
            CalendarPeriod.SetPeriod(text);
            NotifyPropertyChanged(nameof(SelectedPeriod));
        }

        /// <summary>
        /// Updates the contents of the event table.
        /// </summary>
        public void UpdateCalendarTable()
        {
            Events.Clear();
            for (int i = 0; i < Calendar.Events.Count; i++)
            {
                if (Calendar.Events[i].In(CalendarPeriod) && !Calendar.Events[i].Deleted)
                    Events.Add(new EventViewModel(Calendar.Events[i]));
            }
            Events = new ObservableCollection<EventViewModel>(Events.OrderBy(i => i.StartSort));
            PeriodText = CalendarPeriod.ToString();
        }
    }
}
