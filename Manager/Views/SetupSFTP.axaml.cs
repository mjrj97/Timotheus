using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Timotheus.Utility;

namespace Timotheus.Views
{
    /// <summary>
    /// A dialog where the user can setup SFTP.
    /// </summary>
    public partial class SetupSFTP : Dialog
    {
        private string _host = string.Empty;
        /// <summary>
        /// Host string for SFTP.
        /// </summary>
        public string Host
        {
            get => _host;
            set 
            {
                _host = value;
                NotifyPropertyChanged(nameof(Host));
            }
        }

        private string _port = string.Empty;
        /// <summary>
        /// The port of the host.
        /// </summary>
        public string Port
        {
            get => _port;
            set
            {
                _port = value;
                NotifyPropertyChanged(nameof(Port));
            }
        }

        private string _username = string.Empty;
        /// <summary>
        /// Username on the SFTP Server.
        /// </summary>
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                NotifyPropertyChanged(nameof(Username));
            }
        }

        private string _password = string.Empty;
        /// <summary>
        /// Password to the SFTP Server.
        /// </summary>
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                NotifyPropertyChanged(nameof(Password));
            }
        }

        private string _remote = string.Empty;
        /// <summary>
        /// Remote path to sync with.
        /// </summary>
        public string Remote
        {
            get => _remote;
            set
            {
                _remote = value;
                NotifyPropertyChanged(nameof(Remote));
            }
        }

        private string _local = string.Empty;
        /// <summary>
        /// Local path to sync with.
        /// </summary>
        public string Local
        {
            get => _local;
            set
            {
                _local = value;
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
            string path = await openFolder.ShowAsync(this);
            if (path != string.Empty && path != null)
                Local = path;
        }

        /// <summary>
        /// Closes the dialog and sets the DialogResult to OK.
        /// </summary>
        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Closes the dialog and sets the DialogResult to Cancel.
        /// </summary>
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}