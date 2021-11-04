using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ReactiveUI;
using System.Collections.Generic;
using System.Threading.Tasks;
using Timotheus.IO;

namespace Timotheus
{
    public partial class EditKey : Window
    {
        /// <summary>
        /// The keys usually expected in a key file.
        /// </summary>
        private readonly static string[] std = {
            "Calendar-Email",
            "Calendar-Password",
            "Calendar-URL",
            "SSH-URL",
            "SSH-Username",
            "SSH-Password",
            "SSH-RemoteDirectory",
            "SSH-LocalDirectory",
            "Settings-Name",
            "Settings-Address",
            "Settings-Image"
        };

        internal class EditKeyData : ReactiveObject
        {
            private string _Text;
            public string Text
            {
                get => _Text;
                set => this.RaiseAndSetIfChanged(ref _Text, value);
            }
        }

        /// <summary>
        /// The data context of the window.
        /// </summary>
        internal EditKeyData data = new();

        bool ok = false;

        /// <summary>
        /// Constructor. Loads the XAML and assigns data context.
        /// </summary>
        public EditKey()
        {
            AvaloniaXamlLoader.Load(this);
            DataContext = data;
        }

        /// <summary>
        /// Checks if the text contains all of the standard keys, and if any are missing adds them to the text.
        /// </summary>
        private void AddStdKeys_Click(object sender, RoutedEventArgs e)
        {
            Register register = new Register(':', data.Text);
            List<Key> keys = register.Keys();

            bool firstAdded = false;
            for (int i = 0; i < std.Length; i++)
            {
                bool found = false;

                int j = 0;
                while (j < keys.Count && !found)
                {
                    if (keys[j].name == std[i])
                        found = true;
                    j++;
                }

                if (!found)
                {
                    if (firstAdded)
                        data.Text += '\n' + std[i] + ':';
                    else
                    {
                        data.Text += std[i] + ':';
                        firstAdded = true;
                    }
                }
            }
        }

        /// <summary>
        /// The dialog closes and the values are assigned.
        /// </summary>
        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            ok = true;
            Close();
        }

        /// <summary>
        /// The dialog is closed without using the values.
        /// </summary>
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Opens the edit key dialog.
        /// </summary>
        public static Task<Register> Show(Window parent, string text)
        {
            EditKey dialog = new();
            dialog.data.Text = text;

            TaskCompletionSource<Register> tcs = new();
            dialog.Closed += delegate
            {
                if (dialog.ok)
                    tcs.TrySetResult(new Register(':', dialog.data.Text));
            };

            if (parent != null)
                dialog.ShowDialog(parent);
            else dialog.Show();

            return tcs.Task;
        }
    }
}