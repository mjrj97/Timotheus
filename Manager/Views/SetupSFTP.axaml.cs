using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Timotheus.Utility;

namespace Timotheus
{
    /// <summary>
    /// A dialog where the user can setup SFTP.
    /// </summary>
    public partial class SetupSFTP : Dialog
    {
        private string _Host = string.Empty;
        /// <summary>
        /// Host string for SFTP.
        /// </summary>
        public string Host
        {
            get => _Host;
            set 
            {
                _Host = value;
                NotifyPropertyChanged(nameof(Host));
            }
        }

        private string _Username = string.Empty;
        /// <summary>
        /// Username on the SFTP Server.
        /// </summary>
        public string Username
        {
            get => _Username;
            set
            {
                _Username = value;
                NotifyPropertyChanged(nameof(Username));
            }
        }

        private string _Password = string.Empty;
        /// <summary>
        /// Password to the SFTP Server.
        /// </summary>
        public string Password
        {
            get => _Password;
            set
            {
                _Password = value;
                NotifyPropertyChanged(nameof(Host));
            }
        }

        private string _Remote = string.Empty;
        /// <summary>
        /// Remote path to sync with.
        /// </summary>
        public string Remote
        {
            get => _Remote;
            set
            {
                _Remote = value;
                NotifyPropertyChanged(nameof(Remote));
            }
        }

        private string _Local = string.Empty;
        /// <summary>
        /// Local path to sync with.
        /// </summary>
        public string Local
        {
            get => _Local;
            set
            {
                _Local = value;
                NotifyPropertyChanged(nameof(Local));
            }
        }

        /// <summary>
        /// Loads the XAML and sets the DataContext.
        /// </summary>
        public SetupSFTP()
        {
            AvaloniaXamlLoader.Load(this);
            DataContext = this;
        }

        /// <summary>
        /// A OpenFolderDialog where the user can specify which local folder to sync the remote folder with.
        /// </summary>
        private async void Browse_Click(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog openFolder = new();
            Local = await openFolder.ShowAsync(this);
        }

        /// <summary>
        /// Closes the dialog and sets the DialogResult to OK.
        /// </summary>
        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// Closes the dialog and sets the DialogResult to Cancel.
        /// </summary>
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}