using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ReactiveUI;
using System.Threading.Tasks;

namespace Timotheus
{
    public partial class SetupSFTP : Window
    {
        private bool ok = false;
        private readonly SetupData data;

        public SetupSFTP()
        {
            data = new SetupData();
            AvaloniaXamlLoader.Load(this);
            DataContext = data;
        }

        private async void Browse_Click(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog openFolder = new();
            data.Local = await openFolder.ShowAsync(this);
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            ok = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public new static Task<IO.DirectoryManager> Show(Window parent)
        {
            SetupSFTP dialog = new();

            TaskCompletionSource<IO.DirectoryManager> tcs = new();
            dialog.Closed += delegate
            {
                if (dialog.ok)
                    tcs.TrySetResult(new IO.DirectoryManager(dialog.data.Local, dialog.data.Remote, dialog.data.Host, dialog.data.Username, dialog.data.Password));
            };

            if (parent != null)
                dialog.ShowDialog(parent);
            else dialog.Show();

            return tcs.Task;
        }
    }

    public class SetupData : ReactiveObject
    {
        private string _Host = string.Empty;
        public string Host
        {
            get => _Host;
            set => this.RaiseAndSetIfChanged(ref _Host, value);
        }

        private string _Username = string.Empty;
        public string Username
        {
            get => _Username;
            set => this.RaiseAndSetIfChanged(ref _Username, value);
        }

        private string _Password = string.Empty;
        public string Password
        {
            get => _Password;
            set => this.RaiseAndSetIfChanged(ref _Password, value);
        }

        private string _Remote = string.Empty;
        public string Remote
        {
            get => _Remote;
            set => this.RaiseAndSetIfChanged(ref _Remote, value);
        }

        private string _Local = string.Empty;
        public string Local
        {
            get => _Local;
            set => this.RaiseAndSetIfChanged(ref _Local, value);
        }
    }
}