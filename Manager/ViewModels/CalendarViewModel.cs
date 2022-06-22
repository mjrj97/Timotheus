using System;
using System.IO;
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
            calendarPeriod = new(DateTime.Now.Year + " " + (DateTime.Now.Month >= 7 ? Localization.Localization.Calendar_Fall : Localization.Localization.Calendar_Spring));
            PeriodText = calendarPeriod.ToString();
        }
        public CalendarViewModel(string path)
        {
            Calendar = new(path);
            calendarPeriod = new(DateTime.Now.Year + " " + (DateTime.Now.Month >= 7 ? Localization.Localization.Calendar_Fall : Localization.Localization.Calendar_Spring));
            PeriodText = calendarPeriod.ToString();
        }
        public CalendarViewModel()
        {
            Calendar = new();
            calendarPeriod = new(DateTime.Now.Year + " " + (DateTime.Now.Month >= 7 ? Localization.Localization.Calendar_Fall : Localization.Localization.Calendar_Spring));
            PeriodText = calendarPeriod.ToString();
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
        public void ExportCalendar(string path, string archive, string title, string subtitle, string footer, string img)
        {
            Calendar.Export(path, title, subtitle, footer, img, calendarPeriod);
            if (archive != string.Empty)
            {
                if (!Directory.Exists(archive))
                    throw new Exception(Localization.Localization.Exception_PDFArchiveNotFound);
                File.Copy(path, Path.Combine(archive, calendarPeriod.ToFileName() + ".pdf"));
            }
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
            Events = new ObservableCollection<EventViewModel>(Events.OrderBy(i => i.StartSort));
            PeriodText = calendarPeriod.ToString();
        }
    }
}
