using System;
using System.Collections.ObjectModel;
using Timotheus.Schedule;
using ReactiveUI;

namespace Timotheus
{
    public class Data : ReactiveObject
    {
        private string _Caption = "";
        public string Caption
        {
            get => _Caption;
            set => this.RaiseAndSetIfChanged(ref _Caption, value);
        }

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

        private string _PeriodText = string.Empty;
        public string PeriodText
        {
            get => _PeriodText;
            set => this.RaiseAndSetIfChanged(ref _PeriodText, value);
        }

        private ObservableCollection<Event> _Events = new ObservableCollection<Event>();
        public ObservableCollection<Event> Events
        {
            get => _Events;
            set => this.RaiseAndSetIfChanged(ref _Events, value);
        }

        public Data() {
            PeriodText = calendarPeriod.ToString();
            Caption = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
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

        public void Remove(Event ev) {
            Events.Remove(ev);
        }
    }
}