using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;
using Timotheus.Utility;

namespace Timotheus
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

        #region Start
        private string _StartTime = DateTime.Now.ToString("t");
        /// <summary>
        /// Start time of the event. Has the format HH:mm.
        /// </summary>
        public string StartTime
        {
            get { return _StartTime; }
            set { _StartTime = value; }
        }

        private string _StartDay = DateTime.Now.Day.ToString();
        /// <summary>
        /// Start day of the event.
        /// </summary>
        public string StartDay
        {
            get { return _StartDay; }
            set { _StartDay = value; }
        }

        private int _StartMonth = DateTime.Now.Month - 1;
        /// <summary>
        /// Start month of the event.
        /// </summary>
        public int StartMonth
        {
            get { return _StartMonth; }
            set { _StartMonth = value; }
        }

        private string _StartYear = DateTime.Now.Year.ToString();
        /// <summary>
        /// Start year of the event.
        /// </summary>
        public string StartYear
        {
            get { return _StartYear; }
            set { _StartYear = value; }
        }
        #endregion

        #region End
        private string _EndTime = DateTime.Now.AddMinutes(90).ToString("t");
        /// <summary>
        /// End time of the event. Has the format HH:mm.
        /// </summary>
        public string EndTime
        {
            get { return _EndTime; }
            set { _EndTime = value; }
        }

        private string _EndDay = DateTime.Now.AddMinutes(90).Day.ToString();
        /// <summary>
        /// End day of the event.
        /// </summary>
        public string EndDay
        {
            get { return _EndDay; }
            set { _EndDay = value; }
        }

        private int _EndMonth = DateTime.Now.AddMinutes(90).Month - 1;
        /// <summary>
        /// End month of the event.
        /// </summary>
        public int EndMonth
        {
            get { return _EndMonth; }
            set { _EndMonth = value; }
        }

        private string _EndYear = DateTime.Now.AddMinutes(90).Year.ToString();
        /// <summary>
        /// End year of the event.
        /// </summary>
        public string EndYear
        {
            get { return _EndYear; }
            set { _EndYear = value; }
        }
        #endregion

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
            set { _Location = value; }
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
            AvaloniaXamlLoader.Load(this);
            DataContext = this;
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
                Close();
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
            Close();
        }
    }
}