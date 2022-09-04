using Avalonia;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Timotheus.Utility;

namespace Timotheus.Views.Dialogs
{
    public partial class ErrorDialog : Dialog
    {
        private string _dialogTitle = string.Empty;
        public string DialogTitle
        {
            get { return _dialogTitle; }
            set
            {
                _dialogTitle = value;
                NotifyPropertyChanged(nameof(DialogTitle));
            }
        }

        private string _dialogText = string.Empty;
        public string DialogText
        {
            get { return _dialogText; }
            set
            {
                _dialogText = value;
                NotifyPropertyChanged(nameof(DialogText));
            }
        }

        public ErrorDialog()
        {
            DataContext = this;
            AvaloniaXamlLoader.Load(this);
        }

        public void CopyToClipboard_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Clipboard.SetTextAsync(DialogText);
        }
    }
}