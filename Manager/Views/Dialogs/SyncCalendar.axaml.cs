﻿using Avalonia.Markup.Xaml;
using System;

namespace Timotheus.Views.Dialogs
{
    /// <summary>
    /// Dialog where the user can setup the sync settings for the calendar.
    /// </summary>
    public partial class SyncCalendar : Dialog
    {
        private string _Period = string.Empty;
        /// <summary>
        /// Period text. ie. Spring 2022.
        /// </summary>
        public string Period
        {
            get => Localization.SyncCalendar_PeriodCalendarButton + ": " + _Period;
            set
            {
                _Period = value;
                NotifyPropertyChanged(nameof(Period));
            }
        }

        private bool _SyncAll = false;
        /// <summary>
        /// Whether the calendar should be sync in its entirety.
        /// </summary>
        public bool SyncAll
        {
            get => _SyncAll;
            set
            {
                _SyncAll = value;
                NotifyPropertyChanged(nameof(SyncAll));
                NotifyPropertyChanged(nameof(SyncCustomPeriod));
            }
        }

        private bool _SyncPeriod = true;
        /// <summary>
        /// Whether the calendar should only be synced in the period shown in the program.
        /// </summary>
        public bool SyncPeriod
        {
            get => _SyncPeriod;
            set
            {
                _SyncPeriod = value;
                NotifyPropertyChanged(nameof(SyncPeriod));
                NotifyPropertyChanged(nameof(SyncCustomPeriod));
            }
        }

        private string _URL = string.Empty;
        /// <summary>
        /// New URL to sync with.
        /// </summary>
        public string URL
        {
            get { return _URL; }
            set
            {
                _URL = value;
                NotifyPropertyChanged(nameof(URL));
            }
        }

        private string _Username = string.Empty;
        /// <summary>
        /// New username to sync with.
        /// </summary>
        public string Username
        {
            get { return _Username; }
            set
            {
                _Username = value;
                NotifyPropertyChanged(nameof(Username));
            }
        }

        private string _Password = string.Empty;
        /// <summary>
        /// New password to sync with.
        /// </summary>
        public string Password
        {
            get { return _Password; }
            set
            {
                _Password = value;
                NotifyPropertyChanged(nameof(Password));
            }
        }

        private bool SyncCustomPeriod
        {
            get
            {
                return !(SyncPeriod || SyncAll);
            }
        }

        private DateTime _Start = DateTime.Now;
        /// <summary>
        /// The start date of the sync period.
        /// </summary>
        public DateTime Start
        {
            get { return _Start; }
            set
            {
                _Start = value;
                NotifyPropertyChanged(nameof(Start));
            }
        }

        private DateTime _End = DateTime.Now;
        /// <summary>
        /// The end date of the sync period.
        /// </summary>
        public DateTime End
        {
            get { return _End; }
            set
            {
                _End = value;
                NotifyPropertyChanged(nameof(End));
            }
        }

        /// <summary>
        /// Loads the XAML and sets the DataContext.
        /// </summary>
        public SyncCalendar()
        {
            AvaloniaXamlLoader.Load(this);
            DataContext = this;
        }
    }
}