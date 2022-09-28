using System;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Timotheus.Utility;

namespace Timotheus.Views.Dialogs
{
    public partial class PasswordDialog : Dialog
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

        public PasswordDialog()
        {
            AvaloniaXamlLoader.Load(this);
            DataContext = this;
        }

        protected override void Ok_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string encrypted = Cipher.Encrypt(Password);
                string decrypted = Cipher.Decrypt(encrypted);
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                Program.Error(Localization.Exception_Name, ex, this);
            }
        }
    }
}