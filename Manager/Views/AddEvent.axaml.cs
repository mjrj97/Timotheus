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

        private DateTime _Start = DateTime.Now;
        public DateTime Start
        {
            get
            {
                return _Start;
            }
            set
            {
                _Start = value;
            }
        }

        private DateTime _End = DateTime.Now.AddMinutes(90);
        public DateTime End
        {
            get
            {
                return _End;
            }
            set
            {
                _End = value;
            }
        }

        public List<string> Months { get; set; }
        public List<int> StartDays { get; set; }
        public List<int> EndDays { get; set; }

        private string _StartTime;
        /// <summary>
        /// Start time of the event. Has the format HH:mm.
        /// </summary>
        public string StartTime
        {
            get { return _StartTime; }
            set { _StartTime = value; }
        }

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
                    Start = new DateTime(Start.Year, Start.Month, Start.Day, Start.Hour, Start.Minute, Start.Second);
                }
                else
                {
                    try
                    {
                        Start = new DateTime(Start.Year, Start.Month, value + 1, Start.Hour, Start.Minute, Start.Second);
                    }
                    catch (Exception)
                    {
                        Start = new DateTime(Start.Year, Start.Month, 1, Start.Hour, Start.Minute, Start.Second);
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
                Start = new DateTime(Start.Year, value+1, Start.Day, Start.Hour, Start.Minute, Start.Second);
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
                    Start = new DateTime(value, Start.Month, Start.Day, Start.Hour, Start.Minute, Start.Second);
            }
        }

        private string _EndTime;
        /// <summary>
        /// Start time of the event. Has the format HH:mm.
        /// </summary>
        public string EndTime
        {
            get { return _EndTime; }
            set { _EndTime = value; }
        }

        private int EndDay
        {
            get
            {
                return End.Day - 1;
            }
            set
            {
                if (value == -1)
                    value = Start.Day;
                End = new DateTime(End.Year, End.Month, value + 1, End.Hour, End.Minute, End.Second);
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
                End = new DateTime(End.Year, value + 1, End.Day, End.Hour, End.Minute, End.Second);
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
                    End = new DateTime(value, End.Month, End.Day, End.Hour, End.Minute, End.Second);
            }
        }

        private bool _AllDayEvent = false;
        /// <summary>
        /// Designates whether the Event is an all day event.
        /// </summary>
        public bool AllDayEvent
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

        private void FixStartYear(object sender, KeyEventArgs e)
        {
            string text = ((TextBox)sender).Text;
            try
            {
                Regex regexObj = new(@"[^\d]");
                ((TextBox)sender).Text = regexObj.Replace(text, "");
                NotifyPropertyChanged(nameof(StartYear));
            }
            catch (ArgumentException) { }
        }

        private void FixEndYear(object sender, KeyEventArgs e)
        {
            string text = ((TextBox)sender).Text;
            try
            {
                Regex regexObj = new(@"[^\d]");
                ((TextBox)sender).Text = regexObj.Replace(text, "");
                NotifyPropertyChanged(nameof(EndYear));
            }
            catch (ArgumentException){}
        }
    }
}