using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ReactiveUI;

namespace Timotheus
{
    public partial class EditKey : Window
    {
        internal EditKeyData data = new();

        public EditKey()
        {
            AvaloniaXamlLoader.Load(this);
            DataContext = data;
        }

        private void AddStdKeys_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public static void Show(Window parent, string text)
        {
            EditKey dialog = new();
            dialog.data.Text = text;

            if (parent != null)
                dialog.ShowDialog(parent);
            else dialog.Show();
        }
    }

    public class EditKeyData : ReactiveObject
    {
        private string _Text;
        public string Text
        {
            get => _Text;
            set => this.RaiseAndSetIfChanged(ref _Text, value);
        }
    }
}