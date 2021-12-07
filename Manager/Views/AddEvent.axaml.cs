using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using Timotheus.Utility;

namespace Timotheus.Views
{
    public partial class AddEvent : Dialog
    {
        private string _EventName = string.Empty;
        /// <summary>
        /// The title/name of the event. Should not be empty.
        /// </summary>
        public string EventName
        {
            get { return _EventName; }
            set { _EventName = value; }
        }

        private DateTime _Start;
        public DateTime Start 
        { 
            get
            {
                return _Start;
            }
            set
            {
                TimeSpan span = End - Start;
                _Start = value;
                End = Start + span;

                NotifyPropertyChanged(nameof(EndDay));
                NotifyPropertyChanged(nameof(EndMonth));
                NotifyPropertyChanged(nameof(EndYear));
            }
        }
        public DateTime End { get; set; }

        public List<string> Months { get; set; }
        public List<int> StartDays { get; set; }
        public List<int> EndDays { get; set; }

        /// <summary>
        /// Start time of the event. Has the format HH:mm.
        /// </summary>
        public string StartTime { get; set; }

        private int StartDay
        {
            get
            {
                return Start.Day - 1;
            }
            set
            {
                if (value == -1)
                {
                    Start = new DateTime(Start.Year, Start.Month, Start.Day);
                }
                else
                {
                    try
                    {
                        Start = new DateTime(Start.Year, Start.Month, value + 1);
                    }
                    catch (Exception)
                    {
                        Start = new DateTime(Start.Year, Start.Month, 1);
                    }
                }
            }
        }

        private int StartMonth
        {
            get
            {
                return Start.Month - 1;
            }
            set
            {
                StartDays = GetDays(value + 1, Start.Year);
                int day = Start.Day;
                if (day >= StartDays.Count)
                    day = StartDays.Count;
                Start = new DateTime(Start.Year, value+1, day);
                NotifyPropertyChanged(nameof(StartDays));
                NotifyPropertyChanged(nameof(StartDay));
            }
        }

        private int StartYear
        {
            get
            {
                return Start.Year;
            }
            set
            {
                if (value <= 9999)
                    Start = new DateTime(value, Start.Month, Start.Day);
            }
        }

        /// <summary>
        /// Start time of the event. Has the format HH:mm.
        /// </summary>
        public string EndTime { get; set; }

        private int EndDay
        {
            get
            {
                return End.Day - 1;
            }
            set
            {
                if (value == -1)
                {
                    End = new DateTime(End.Year, End.Month, End.Day);
                }
                else
                {
                    try
                    {
                        End = new DateTime(End.Year, End.Month, value + 1);
                    }
                    catch (Exception)
                    {
                        End = new DateTime(End.Year, End.Month, 1);
                    }
                }
            }
        }

        private int EndMonth
        {
            get
            {
                return End.Month - 1;
            }
            set
            {
                EndDays = GetDays(value + 1, End.Year); 
                int day = End.Day;
                if (day >= EndDays.Count)
                    day = EndDays.Count;
                End = new DateTime(End.Year, value + 1, day);
                NotifyPropertyChanged(nameof(EndDays));
                NotifyPropertyChanged(nameof(EndDay));
            }
        }

        private int EndYear
        {
            get
            {
                return End.Year;
            }
            set
            {
                if (value <= 9999)
                    End = new DateTime(value, End.Month, End.Day);
            }
        }

        private bool _AllDayEvent = false;
        /// <summary>
        /// Designates whether the Event is an all day event.
        /// </summary>
        private bool AllDayEvent
        {
            get => _AllDayEvent;
            set
            {
                _AllDayEvent = value;
                NotifyPropertyChanged(nameof(AllDayEvent));
            }
        }

        private string _Location = string.Empty;
        /// <summary>
        /// The Event's location.
        /// </summary>
        public string Location
        {
            get { return _Location; }
            set 
            { 
                _Location = value;
                NotifyPropertyChanged(nameof(Location));
            }
        }

        private string _Description = string.Empty;
        /// <summary>
        /// The Event's description.
        /// </summary>
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }

        private string _Error = string.Empty;
        /// <summary>
        /// Error message shown on the buttom.
        /// </summary>
        public string Error
        {
            get => _Error;
            set
            {
                _Error = value;
                NotifyPropertyChanged(nameof(Error));
            }
        }

        /// <summary>
        /// Loads the XAML and assigns the DataContext.
        /// </summary>
        public AddEvent()
        {
            Start = DateTime.Now;
            End = DateTime.Now.AddMinutes(90);

            StartTime = Start.ToString("t");
            EndTime = End.ToString("t");

            string[] months = DateTimeFormatInfo.CurrentInfo.MonthNames;
            Months = new List<string>();
            for (int i = 0; i < months.Length - 1; i++) { Months.Add(months[i]); }

            StartDays = GetDays(Start.Month, Start.Year);
            EndDays = GetDays(End.Month, End.Year);

            AvaloniaXamlLoader.Load(this);
            DataContext = this;
        }

        /// <summary>
        /// Returns the list of days in the month.
        /// </summary>
        private static List<int> GetDays(int month, int year)
        {
            int i = 1;
            List<int> numbers = new();
            DateTime time = new(year, month, i);
            while (time.Month == month)
            {
                time = time.AddDays(1);
                numbers.Add(i);
                i++;
            }
            return numbers;
        }

        /// <summary>
        /// Closes the dialog and sets the DialogResult to OK.
        /// </summary>
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (EventName.Trim() == string.Empty)
                    throw new Exception(Localization.Localization.Exception_EmptyName);
                if (End < Start)
                    throw new Exception(Localization.Localization.Exception_EndBeforeStart);

                Start = Start.Date;
                End = End.Date;

                if (!AllDayEvent)
                {
                    int hour, minute;

                    hour = int.Parse(StartTime[..(-3 + StartTime.Length)]);
                    minute = int.Parse(StartTime.Substring(-2 + StartTime.Length, 2));
                    Start = Start.Date.AddMinutes(minute + hour * 60);

                    hour = int.Parse(EndTime[..(-3 + EndTime.Length)]);
                    minute = int.Parse(EndTime.Substring(-2 + EndTime.Length, 2));
                    End = End.Date.AddMinutes(minute + hour * 60);
                }
                else
                    End = End.AddDays(1);

                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                Error = ex.Message;
            }
        }

        /// <summary>
        /// Closes the dialog and sets the DialogResult to Cancel.
        /// </summary>
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// Makes sure that the year fields only contain numbers.
        /// </summary>
        private void FixTime(object sender, KeyEventArgs e)
        {
            string text = ((TextBox)sender).Text;
            try
            {
                Regex regexObj = new(@"[^\d&&:&&.]");
                ((TextBox)sender).Text = regexObj.Replace(text, "");
                NotifyPropertyChanged(nameof(StartTime));
                NotifyPropertyChanged(nameof(EndTime));
            }
            catch (ArgumentException) { }
        }

        /// <summary>
        /// Makes sure that the year fields only contain numbers.
        /// </summary>
        private void FixYear(object sender, KeyEventArgs e)
        {
            string text = ((TextBox)sender).Text;
            try
            {
                Regex regexObj = new(@"[^\d]");
                ((TextBox)sender).Text = regexObj.Replace(text, "");
                NotifyPropertyChanged(nameof(StartYear));
                NotifyPropertyChanged(nameof(EndYear));
            }
            catch (ArgumentException) { }
        }
    }
}