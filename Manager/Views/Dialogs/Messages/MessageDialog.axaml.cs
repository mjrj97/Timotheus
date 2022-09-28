using Avalonia.Markup.Xaml;

namespace Timotheus.Views.Dialogs
{
    public partial class MessageDialog : Dialog
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

        public MessageDialog()
        {
            DataContext = this;
            AvaloniaXamlLoader.Load(this);
        }
    }
}