using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

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

        /// <summary>
        /// What happens when 'OK' is pressed on the dialog.
        /// </summary>
        protected override async void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (DontShowAgain)
            {
                WarningDialog dialog = new()
                {
                    DialogTitle = Localization.Exception_Warning,
                    DialogText = Localization.UpdateDialog_AreYouSure
                };
                await dialog.ShowDialog(this);

                if (dialog.DialogResult == DialogResult.OK)
                    DialogResult = DialogResult.OK;
            }
            else
                DialogResult = DialogResult.OK;
        }
    }
}
