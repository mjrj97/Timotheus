using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Timotheus.Views.Controls
{
    public partial class DatePicker : UserControl, INotifyPropertyChanged
    {
        public static readonly DirectProperty<DatePicker, DateTime> DateProperty =
            AvaloniaProperty.RegisterDirect<DatePicker, DateTime>(
                nameof(Date),
                o => o.Date,
                (o, v) => o.Date = v, DateTime.Now, BindingMode.TwoWay);

        private DateTime _date = DateTime.Now.Date;
        public DateTime Date
        { 
            get
            {
                return _date;
            }
            set
            {
                DateTime old = _date;

                if (value == old)
                {
                    return;
                }

                _date = value;
                RaisePropertyChanged(DateProperty, old, value);

                Days = GetDays(Date.Month, Date.Year);

                NotifyPropertyChanged(nameof(Day));
                NotifyPropertyChanged(nameof(Month));
                NotifyPropertyChanged(nameof(Year));
            }
        }

        protected int Day
        {
            get
            {
                return Date.Day - 1;
            }
            set
            {
                if (value == -1)
                {
                    Date = new DateTime(Date.Year, Date.Month, Date.Day);
                }
                else
                {
                    try
                    {
                        Date = new DateTime(Date.Year, Date.Month, value + 1);
                    }
                    catch (Exception ex)
                    {
                        Timotheus.Log(ex);
                        Date = new DateTime(Date.Year, Date.Month, 1);
                    }
                }
            }
        }

        protected int Month
        {
            get
            {
                return Date.Month - 1;
            }
            set
            {
                Days = GetDays(value + 1, Date.Year);
                int day = Date.Day;
                if (day >= Days.Count)
                    day = Days.Count;
                Date = new DateTime(Date.Year, value + 1, day);
                NotifyPropertyChanged(nameof(Days));
                NotifyPropertyChanged(nameof(Day));
            }
        }

        protected int Year
        {
            get
            {
                return Date.Year;
            }
            set
            {
                if (value <= 9999)
                    Date = new DateTime(value, Date.Month, Date.Day);
            }
        }

        private ObservableCollection<int> Days { get; set; }
        private ObservableCollection<string> Months { get; set; }

        public DatePicker()
        {
            string[] months = DateTimeFormatInfo.CurrentInfo.MonthNames;
            Months = new ObservableCollection<string>();
            for (int i = 0; i < months.Length - 1; i++) { Months.Add(months[i]); }

            Days = GetDays(Date.Month, Date.Year);

            AvaloniaXamlLoader.Load(this);
            DataContext = this;
        }

        /// <summary>
        /// Returns the list of days in the month.
        /// </summary>
        private static ObservableCollection<int> GetDays(int month, int year)
        {
            int i = 1;
            ObservableCollection<int> numbers = new();
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
        /// Makes sure that the year fields only contain numbers.
        /// </summary>
        private void FixYear(object sender, KeyEventArgs e)
        {
            string text = ((TextBox)sender).Text;
            try
            {
                Regex regexObj = new(@"[^\d]");
                ((TextBox)sender).Text = regexObj.Replace(text, "");
                NotifyPropertyChanged(nameof(Year));
            }
            catch (ArgumentException ex)
            {
                Timotheus.Log(ex);
            }
        }

        public new event PropertyChangedEventHandler PropertyChanged;
        internal void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
