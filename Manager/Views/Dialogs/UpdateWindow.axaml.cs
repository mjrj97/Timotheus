using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Timotheus.Utility;

namespace Timotheus.Views.Dialogs
{
    public partial class UpdateWindow : Dialog
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

        private bool _dontShowAgain = false;
        public bool DontShowAgain
        {
            get { return _dontShowAgain; }
            set
            {
                _dontShowAgain = value;
                NotifyPropertyChanged(nameof(DontShowAgain));
            }
        }

        public UpdateWindow()
        {
            DataContext = this;
            AvaloniaXamlLoader.Load(this);
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
