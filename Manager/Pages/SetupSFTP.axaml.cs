using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ReactiveUI;

namespace Timotheus
{
    public partial class SetupSFTP : Window
    {
        public SetupSFTP()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public static void Show(Window parent)
        {
            SetupSFTP dialog = new();

            if (parent != null)
                dialog.ShowDialog(parent);
            else dialog.Show();
        }
    }

    public class SetupData : ReactiveObject
    {
        private string _Host;
        public string Host
        {
            get => _Host;
            set => this.RaiseAndSetIfChanged(ref _Host, value);
        }

        private string _Username;
        public string Username
        {
            get => _Username;
            set => this.RaiseAndSetIfChanged(ref _Username, value);
        }

        private string _Password;
        public string Password
        {
            get => _Password;
            set => this.RaiseAndSetIfChanged(ref _Password, value);
        }
    }
}