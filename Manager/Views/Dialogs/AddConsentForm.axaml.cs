using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Timotheus.Views.Dialogs
{
    public partial class AddConsentForm : Dialog
    {
        private string _buttonName = Localization.AddConsentForm_AddButton;
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

        private string _consentName = string.Empty;
        /// <summary>
        /// The name of the person giving consent.
        /// </summary>
        public string ConsentName
        {
            get
            {
                return _consentName;
            }
            set
            {
                _consentName = value;
                NotifyPropertyChanged(nameof(ConsentName));
            }
        }

        private DateTime _consentDate;
        /// <summary>
        /// The date of where consent was given.
        /// </summary>
        public DateTime ConsentDate
        {
            get
            {
                return _consentDate;
            }
            set
            {
                _consentDate = value;
                NotifyPropertyChanged(nameof(ConsentDate));
            }
        }

        public List<string> Months { get; set; }
        public List<int> Days { get; set; }

        private int Day
        {
            get
            {
                return ConsentDate.Day - 1;
            }
            set
            {
                if (value == -1)
                {
                    ConsentDate = new DateTime(ConsentDate.Year, ConsentDate.Month, ConsentDate.Day);
                }
                else
                {
                    try
                    {
                        ConsentDate = new DateTime(ConsentDate.Year, ConsentDate.Month, value + 1);
                    }
                    catch (Exception ex)
                    {
                        Program.Log(ex);
                        ConsentDate = new DateTime(ConsentDate.Year, ConsentDate.Month, 1);
                    }
                }
            }
        }

        public int Month
        {
            get
            {
                return ConsentDate.Month - 1;
            }
            set
            {
                Days = GetDays(value + 1, ConsentDate.Year);
                int day = ConsentDate.Day;
                if (day >= Days.Count)
                    day = Days.Count;
                ConsentDate = new DateTime(ConsentDate.Year, value + 1, day);
                NotifyPropertyChanged(nameof(Days));
                NotifyPropertyChanged(nameof(Day));
            }
        }

        private int Year
        {
            get
            {
                return ConsentDate.Year;
            }
            set
            {
                if (value > 0 && value <= 9999)
                    ConsentDate = new DateTime(value, ConsentDate.Month, ConsentDate.Day);
            }
        }

        private string _consentVersion = string.Empty;
        /// <summary>
        /// The version of the consent form.
        /// </summary>
        public string ConsentVersion
        {
            get
            {
                return _consentVersion;
            }
            set
            {
                _consentVersion = value;
                NotifyPropertyChanged(nameof(ConsentVersion));
            }
        }

        private string _consentComment = string.Empty;
        /// <summary>
        /// Any comment given to the consent.
        /// </summary>
        public string ConsentComment
        {
            get
            {
                return _consentComment;
            }
            set
            {
                _consentComment = value;
                NotifyPropertyChanged(nameof(ConsentComment));
            }
        }

        /// <summary>
        /// Constructor creating the dialog.
        /// </summary>
        public AddConsentForm()
        {
            ConsentDate = DateTime.Now.Date;

            string[] months = DateTimeFormatInfo.CurrentInfo.MonthNames;
            Months = new List<string>();
            for (int i = 0; i < months.Length - 1; i++) { Months.Add(months[i]); }

            Days = GetDays(ConsentDate.Month, ConsentDate.Year);

            AvaloniaXamlLoader.Load(this);
            DataContext = this;
        }

        /// <summary>
        /// Closes the dialog and sets the DialogResult to OK.
        /// </summary>
        protected override void Ok_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ConsentName == string.Empty)
                {
                    ErrorDialog messageBox = new()
                    {
                        DialogTitle = Localization.Exception_Name,
                        DialogText = Localization.AddConsentForm_EmptyName
                    };
                    messageBox.ShowDialog(this);
                }
                else
                    DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                Program.Error(Localization.Exception_Name, ex, this);
            }
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
    }
}