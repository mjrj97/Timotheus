﻿using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Timotheus.Views.Dialogs
{
    public partial class AddEvent : Dialog
    {
        private string _eventName = string.Empty;
        /// <summary>
        /// The title/name of the event. Should not be empty.
        /// </summary>
        public string EventName
        {
            get { return _eventName; }
            set
            {
                _eventName = value;
                NotifyPropertyChanged(nameof(EventName));
            }
        }

        private DateTime _start;
        public DateTime Start
        {
            get
            {
                return _start;
            }
            set
            {
                TimeSpan span = End - Start;
                _start = value;
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

        private string _startTime;
        /// <summary>
        /// Start time of the event. Has the format HH:mm.
        /// </summary>
        public string StartTime
        {
            get
            {
                return _startTime;
            }
            set
            {
                _startTime = value;
                NotifyPropertyChanged(nameof(StartTime));
            }
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
                    Start = new DateTime(Start.Year, Start.Month, Start.Day);
                }
                else
                {
                    try
                    {
                        Start = new DateTime(Start.Year, Start.Month, value + 1);
                    }
                    catch (Exception ex)
                    {
                        Program.Log(ex);
                        Start = new DateTime(Start.Year, Start.Month, 1);
                    }
                }
            }
        }

        public int StartMonth
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
                Start = new DateTime(Start.Year, value + 1, day);
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
                if (value > 0 && value <= 9999)
                    Start = new DateTime(value, Start.Month, Start.Day);
            }
        }

        private string _endTime;
        /// <summary>
        /// Start time of the event. Has the format HH:mm.
        /// </summary>
        public string EndTime
        {
            get
            {
                return _endTime;
            }
            set
            {
                _endTime = value;
                NotifyPropertyChanged(nameof(EndTime));
            }
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
                {
                    End = new DateTime(End.Year, End.Month, End.Day);
                }
                else
                {
                    try
                    {
                        End = new DateTime(End.Year, End.Month, value + 1);
                    }
                    catch (Exception ex)
                    {
                        Program.Log(ex);
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
                if (value > 0 && value <= 9999)
                    End = new DateTime(value, End.Month, End.Day);
            }
        }

        private bool _allDayEvent = false;
        /// <summary>
        /// Designates whether the Event is an all day event.
        /// </summary>
        public bool AllDayEvent
        {
            get => _allDayEvent;
            set
            {
                _allDayEvent = value;
                NotifyPropertyChanged(nameof(AllDayEvent));
            }
        }

        private string _location = string.Empty;
        /// <summary>
        /// The Event's location.
        /// </summary>
        public string Location
        {
            get { return _location; }
            set
            {
                _location = value;
                NotifyPropertyChanged(nameof(Location));
            }
        }

        private string _description = string.Empty;
        /// <summary>
        /// The Event's description.
        /// </summary>
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
                NotifyPropertyChanged(nameof(Description));
            }
        }

        private string _buttonName = Localization.AddEvent_AddButton;
        public string ButtonName
        {
            get
            {
                return _buttonName;
            }
            set
            {
                _buttonName = value;
                NotifyPropertyChanged(nameof(ButtonName));
            }
        }

        /// <summary>
        /// Loads the XAML and assigns the DataContext.
        /// </summary>
        public AddEvent()
        {
            Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 19, 00, 00);
            End = Start.AddMinutes(90);

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
        protected override void Ok_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (EventName.Trim() == string.Empty)
                    throw new Exception(Localization.Exception_EmptyName);
                if (End < Start)
                    throw new Exception(Localization.Exception_EndBeforeStart);

                DateTime EndSaved = End.Date;
                Start = Start.Date;
                End = EndSaved;

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
                Program.Error(Localization.Exception_Name, ex, this);
            }
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
            catch (ArgumentException ex) 
            {
                Program.Log(ex);
            }
        }
    }
}