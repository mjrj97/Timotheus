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
            Close();
        }

        public new static Task<string> Show(Window parent)
        {
            PasswordDialog dialog = new();

            TaskCompletionSource<string> tcs = new();
            dialog.Closed += delegate
            {
                tcs.TrySetResult(dialog.Password);
            };
            if (parent != null)
                dialog.ShowDialog(parent);
            else dialog.Show();

            return tcs.Task;
        }
    }
}