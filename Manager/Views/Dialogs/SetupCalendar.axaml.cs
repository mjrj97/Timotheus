using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using System;
using System.Text.RegularExpressions;

namespace Timotheus.Views.Dialogs
{
    /// <summary>
    /// Dialog where the user can setup the sync settings for the calendar.
    /// </summary>
    public partial class SetupCalendar : Dialog
	{
		private string _description = string.Empty;
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

		private string _startTime;
		public string StartTime
		{
			get { return _startTime; }
			set
			{
				_startTime = value;
				NotifyPropertyChanged(nameof(StartTime));
			}
		}

		private string _endTime;
		public string EndTime
		{
			get { return _endTime; }
			set
			{
				_endTime = value;
				NotifyPropertyChanged(nameof(EndTime));
			}
		}

		private string _location;
		public string Location
		{
			get { return _location; }
			set
			{
				_location = value;
				NotifyPropertyChanged(nameof(Location));
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

        /// <summary>
        /// Loads the XAML and sets the DataContext.
        /// </summary>
        public SetupCalendar()
        {
            AvaloniaXamlLoader.Load(this);
            DataContext = this;
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