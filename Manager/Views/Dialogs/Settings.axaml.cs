using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using static Timotheus.Localization;

namespace Timotheus.Views.Dialogs
{
    public partial class Settings : Dialog
    {
        private Culture _selectedLanguage = 0;
        public Culture SelectedLanguage
        {
            get { return _selectedLanguage; }
            set
            {
                _selectedLanguage = value;
                NotifyPropertyChanged(nameof(SelectedLanguage));
            }
        }

        private bool _hideToSystemTray = true;
        public bool HideToSystemTray
        {
            get { return _hideToSystemTray; }
            set
            {
                _hideToSystemTray = value;
                NotifyPropertyChanged(nameof(HideToSystemTray));
            }
        }

        private bool _lookForUpdates = true;
        public bool LookForUpdates
        {
            get { return _lookForUpdates; }
            set
            {
                _lookForUpdates = value;
                NotifyPropertyChanged(nameof(LookForUpdates));
            }
        }

        private bool _openOnStartUp = true;
        public bool OpenOnStartUp
        {
            get { return _openOnStartUp; }
            set
            {
                _openOnStartUp = value;
                NotifyPropertyChanged(nameof(OpenOnStartUp));
            }
        }

        public Settings()
        {
            DataContext = this;
            AvaloniaXamlLoader.Load(this);
        }

        /// <summary>
        /// This deletes the system settings for Timotheus, but opens a warning beforehand to ensure consent.
        /// </summary>
        private async void DeleteSettings_Click(object sender, RoutedEventArgs e)
        {
            WarningDialog dialog = new()
            {
                DialogTitle = Localization.Exception_Warning,
                DialogText = Localization.Settings_DeleteSettings_Warning
            };
            await dialog.ShowDialog(this);
            if (dialog.DialogResult == DialogResult.OK)
                Timotheus.DeleteRegistry();
        }
    }
}