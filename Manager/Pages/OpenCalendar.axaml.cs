using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Timotheus.Utility;

namespace Timotheus
{
    public partial class OpenCalendar : Dialog
    {
        internal Schedule.Calendar calendar = null;

        private string _Username = string.Empty;
        public string Username
        {
            get { return _Username; }
            set { _Username = value; }
        }

        private string _Password = string.Empty;
        public string Password
        {
            get { return _Password; }
            set { _Password = value; }
        }

        private string _URL = string.Empty;
        public string URL
        {
            get { return _URL; }
            set { _URL = value; }
        }

        private string _Path = string.Empty;
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
        public bool IsRemote
        {
            get => _IsRemote;
            set
            {
                _IsRemote = value;
                NotifyPropertyChanged(nameof(IsRemote));
            }
        }

        public OpenCalendar()
        {
            AvaloniaXamlLoader.Load(this);
            DataContext = this;
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

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
