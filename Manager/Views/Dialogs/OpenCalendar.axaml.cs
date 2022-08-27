using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Timotheus.Utility;

namespace Timotheus.Views.Dialogs
{
    /// <summary>
    /// Dialog where the user can input info to open a calendar on a remote server or by a local file.
    /// </summary>
    public partial class OpenCalendar : Dialog
    {
        private string _Username = string.Empty;
        /// <summary>
        /// [Remote] The username used to connect to the calendar server.
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
        /// [Remote] The password used to connect to the calendar server.
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

        private string _URL = string.Empty;
        /// <summary>
        /// [Remote] CalDAV url used to connect to the calendar server.
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

        private string _Path = string.Empty;
        /// <summary>
        /// [Local] Path to the .ics calendar file.
        /// </summary>
        public string Path
        {
            get { return _Path; }
            set
            {
                _Path = value;
                NotifyPropertyChanged(nameof(Path));
            }
        }

        private bool _IsRemote = true;
        /// <summary>
        /// Whether the [Remote] or [Local] variables should be used. 
        /// </summary>
        public bool IsRemote
        {
            get => _IsRemote;
            set
            {
                _IsRemote = value;
                NotifyPropertyChanged(nameof(IsRemote));
            }
        }

        /// <summary>
        /// Initializes the XAML and assigns the DataContext.
        /// </summary>
        public OpenCalendar()
        {
            AvaloniaXamlLoader.Load(this);
            DataContext = this;
        }

        /// <summary>
        /// Opens a dialog where the user can specify a local .ics file.
        /// </summary>
        private async void Browse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new();

            FileDialogFilter txtFilter = new();
            txtFilter.Extensions.Add("ics");
            txtFilter.Name = "Calendar files (.ics)";

            openFileDialog.Filters = new();
            openFileDialog.Filters.Add(txtFilter);

            string[] result = await openFileDialog.ShowAsync(this);
            if (result != null && result.Length > 0)
            {
                Path = result[0];
            }
        }
    }
}