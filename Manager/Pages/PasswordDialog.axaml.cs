using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Threading.Tasks;

namespace Timotheus
{
    public partial class PasswordDialog : Window
    {
        private string _Password = string.Empty;
        public string Password
        {
            get { return _Password; }
            set { _Password = value; }
        }

        public PasswordDialog()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Password = string.Empty;
            Close();
        }

        public new static Task<string> Show(Window parent)
        {
            PasswordDialog dialog = new();

            TaskCompletionSource<string> tcs = new();
            dialog.Closed += delegate
            {
                tcs.TrySetResult(dialog.Password);
            };
            if (parent != null)
                dialog.ShowDialog(parent);
            else dialog.Show();

            return tcs.Task;
        }
    }
}
