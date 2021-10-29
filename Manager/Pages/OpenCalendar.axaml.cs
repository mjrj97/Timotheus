using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;
using System.IO;
using System.Threading.Tasks;
using ReactiveUI;

namespace Timotheus
{
    public partial class OpenCalendar : Window
    {
        private CalendarData data;
        internal Schedule.Calendar calendar = null;

        public OpenCalendar()
        {
            data = new CalendarData();
            InitializeComponent();
            DataContext = data;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (data.IsRemote)
                    calendar = new(data.Username, data.Password, data.URL);
                else
                {
                    string[] lines = File.ReadAllLines(data.Path);
                    calendar = new(lines);
                }
                Close();
            }
            catch (Exception ex)
            {
                TextBlock error = this.Find<TextBlock>("Error");
                error.Text = ex.Message;
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void Browse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            string[] result = await openFileDialog.ShowAsync(this);
            if (result != null)
            {
                TextBox textBox = this.Find<TextBox>("BrowseField");
                textBox.Text = result[0];
            }
        }

        public new static Task<Schedule.Calendar> Show(Window parent)
        {
            OpenCalendar dialog = new();

            TaskCompletionSource<Schedule.Calendar> tcs = new();
            dialog.Closed += delegate
            {
                tcs.TrySetResult(dialog.calendar);
            };
            if (parent != null)
                dialog.ShowDialog(parent);
            else dialog.Show();

            return tcs.Task;
        }
    }

    internal class CalendarData : ReactiveObject
    {
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
            set { _Path = value; }
        }

        private bool _IsRemote = true;
        public bool IsRemote
        {
            get => _IsRemote;
            set => this.RaiseAndSetIfChanged(ref _IsRemote, value);
        }
    }
}
