using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Threading.Tasks;
using Timotheus.Utility;

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

        private bool _Save = false;
        public bool Save
        {
            get { return _Save; }
            set { _Save = value; }
        }

        private bool OK = false;

        public PasswordDialog()
        {
            AvaloniaXamlLoader.Load(this);
            DataContext = this;
        }

        private async void Ok_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string encrypted = Cipher.Encrypt(Password);
                string decrypted = Cipher.Decrypt(encrypted);
                OK = true;
                Close();
            }
            catch (Exception)
            {
                await MessageBox.Show(this, Localization.Localization.Exception_InvalidPassword, Localization.Localization.Exception_Name, MessageBox.MessageBoxButtons.OkCancel);
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Password = string.Empty;
            OK = false;
            Close();
        }

        public new Task<bool> Show(Window parent)
        {
            TaskCompletionSource<bool> tcs = new();
            Closed += delegate
            {
                tcs.TrySetResult(OK);
            };
            if (parent != null)
                ShowDialog(parent);
            else Show();

            return tcs.Task;
        }
    }
}