using System;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Timotheus.Utility;

namespace Timotheus.Views.Dialogs
{
    public partial class PasswordDialog : Dialog
    {
        private string _Password = string.Empty;
        /// <summary>
        /// The password in the textbox.
        /// </summary>
        public string Password
        {
            get { return _Password; }
            set { _Password = value; }
        }

        private bool _Save = Timotheus.Registry.Retrieve("SavePasswordsPreference") != "False";
        /// <summary>
        /// Whether the password should be saved.
        /// </summary>
        public bool Save
        {
            get { return _Save; }
            set { _Save = value; }
        }

        private readonly Regex hasNumber = new(@"[0-9]+");
        private readonly Regex hasSpace = new(@"[ ]+");
        private readonly Regex hasUpperChar = new(@"[A-Z]+");
        private readonly Regex hasLowerChar = new(@"[a-z]+");
        private readonly Regex hasMinimum8Chars = new(@".{8,}");
        private readonly Regex hasSymbols = new(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

        public PasswordDialog()
        {
            AvaloniaXamlLoader.Load(this);
            DataContext = this;
        }

        protected override void Ok_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string error = "";
                
                if (!hasNumber.IsMatch(Password))
                    error += Localization.PasswordDialog_RegexHasNumber + "\n";
                if (!hasUpperChar.IsMatch(Password))
                    error += Localization.PasswordDialog_RegexHasUpper + "\n";
                if (!hasLowerChar.IsMatch(Password))
                    error += Localization.PasswordDialog_RegexHasLower + "\n";
                if (!hasMinimum8Chars.IsMatch(Password))
                    error += Localization.PasswordDialog_RegexHas8Char + "\n";
                if (!hasSymbols.IsMatch(Password))
                    error += Localization.PasswordDialog_RegexHasSymbol + "\n";
                if (hasSpace.IsMatch(Password))
                    error += Localization.PasswordDialog_RegexHasSpace + "\n";

                if (error.Length > 0)
                    throw new Exception(error);

                string encrypted = Cipher.Encrypt(Password);
                string decrypted = Cipher.Decrypt(encrypted);

                Timotheus.Registry.Update("SavePasswordsPreference", Save.ToString());

                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                Exception exception = ex;

                if (ex is CryptographicException)
                    exception = new Exception(Localization.Exception_InvalidPassword);

                Program.Error(Localization.Exception_Name, exception, this);
            }
        }

        protected override void KeyDown_Window(object sender, KeyEventArgs e)
        {
            IInputElement obj = ((FocusManager)FocusManager.Instance).GetFocusedElement(this);
            if (obj is Dialog || obj is TextBox)
            {
                if (e.Key == Key.Enter)
                {
                    Ok_Click(null, null);
                    e.Handled = true;
                }
                else if (e.Key == Key.Escape)
                {
                    Cancel_Click(null, null);
                    e.Handled = true;
                }
            }
        }
    }
}