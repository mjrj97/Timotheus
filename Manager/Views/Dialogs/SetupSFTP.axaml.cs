using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;

namespace Timotheus.Views.Dialogs
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
                _remote = value.Replace("\\", "/");
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
                _local = value.Replace("\\", "/");
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
        /// Fix the path textbox
        /// </summary>
        private void DirectoryText_KeyUp(object sender, KeyEventArgs e)
        {
            string text = ((TextBox)sender).Text;
            try
            {
                string path = text.Trim();
                path = path.Replace("\\", "/");
                ((TextBox)sender).Text = path;
            }
            catch (ArgumentException ex)
            {
                Program.Log(ex);
            }
        }
    }
}