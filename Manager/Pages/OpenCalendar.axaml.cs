using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Timotheus
{
    public partial class OpenCalendar : Window
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

        internal Schedule.Calendar calendar = null;

        private bool _IsRemote = true;
        public bool IsRemote
        {
            get { return _IsRemote; }
            set { _IsRemote = value; }
        }

        public OpenCalendar()
        {
            InitializeComponent();
            DataContext = this;
            #if DEBUG
            this.AttachDevTools();
            #endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            UpdateToggle();
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsRemote)
                    calendar = new(Username, Password, URL);
                else
                {
                    string[] lines = File.ReadAllLines(Path);
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

        private void ToggleType_Click(object sender, RoutedEventArgs e)
        {
            UpdateToggle();
        }

        private void UpdateToggle()
        {
            DockPanel localPanel = this.Find<DockPanel>("LocalPanel");
            DockPanel remotePanel = this.Find<DockPanel>("RemotePanel");
            localPanel.IsEnabled = !IsRemote;
            remotePanel.IsEnabled = IsRemote;
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
}
